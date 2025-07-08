using System;
using System.Collections;
using UnityEngine;
public class MovementScript : MonoBehaviour
{
	[Header("InputManager")]
	public InputScript InputScript;

	[Header("Movement")]
	public float MoveSpeed = 5f;
	public float JumpForce = 10f;

	[Header("Ground Check")]
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private float _groundRadius = 0.2f;
	[SerializeField] private LayerMask _groundLayer;

	[Header("Wall Check")]
	[SerializeField] private Transform _leftCheck;
	[SerializeField] private Transform _rightCheck;

	[SerializeField] private GameObject _smoke;

	public SoundManager SoundManager;
	public Animator Animator { get; private set; }
	public bool IsSoundPlaying { get; set; }

	public Rigidbody2D RB;
	public float HorizontalInput { get; private set; }

	public bool IsGrounded { get; private set; }
	private bool _isCollidingleft;
	private bool _isCollidingright;

	private IPlayerState _currentState;

	// States
	public IdleState IdleState { get; private set; }
	public WalkingState WalkingState { get; private set; }
	public JumpingState JumpingState { get; private set; }
	public FallingState FallingState { get; private set; }

	private void Start()
	{
        SoundManager = FindFirstObjectByType<SoundManager>();

        RB = GetComponent<Rigidbody2D>();
		Animator = GetComponent<Animator>();

		IdleState = new IdleState(this);
		WalkingState = new WalkingState(this);
		JumpingState = new JumpingState(this);
		FallingState = new FallingState(this);

		TransitionToState(IdleState);
	}

	private void Update()
	{
		HorizontalInput = InputScript.MoveInput.x;
		FlipSprite();
		_currentState.Update();
	}
	public void CreateSmoke()
	{
		GameObject obj = Instantiate(_smoke, transform.position, Quaternion.identity);
		StartCoroutine(DestroySmoke(obj));
	}

	private IEnumerator DestroySmoke(GameObject obj)
	{
		yield return new WaitForSeconds(0.33f);
		Destroy(obj);
	}

	private void FixedUpdate()
	{
		CheckEnvironment();
		_currentState.FixedUpdate();
	}

	public void TransitionToState(IPlayerState newState)
	{
		_currentState?.Exit();
		_currentState = newState;
		_currentState.Enter();
	}

	public void Move()
	{
		float clampedInput = HorizontalInput;
		if (_isCollidingleft) clampedInput = Mathf.Clamp(HorizontalInput, 0, 1);
		if (_isCollidingright) clampedInput = Mathf.Clamp(HorizontalInput, -1, 0);
		RB.linearVelocity = new Vector2(clampedInput * MoveSpeed, RB.linearVelocity.y);
	}

	public void Jump()
	{
		RB.linearVelocity = new Vector2(RB.linearVelocity.x, JumpForce);
		SoundManager.Play("Jump");
	}

	private void FlipSprite()
	{
		var spriteRenderer = GetComponent<SpriteRenderer>();
		if (HorizontalInput < 0f) spriteRenderer.flipX = true;
		else if (HorizontalInput > 0f) spriteRenderer.flipX = false;
	}

    private void CheckEnvironment()
    {
        IsGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer);

        // Cast a short ray from left and right check positions
        float rayDistance = 0.1f; // make sure this is enough to touch the wall

        RaycastHit2D hitLeft = Physics2D.Raycast(_leftCheck.position, Vector2.left, rayDistance, _groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(_rightCheck.position, Vector2.right, rayDistance, _groundLayer);

        _isCollidingleft = hitLeft.collider != null;
        _isCollidingright = hitRight.collider != null;

        // Debug rays
        Debug.DrawRay(_leftCheck.position, Vector2.left * rayDistance, Color.red);
        Debug.DrawRay(_rightCheck.position, Vector2.right * rayDistance, Color.green);
    }
    private void OnDrawGizmosSelected()
	{
		if (_groundCheck != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
		}
	}
}


