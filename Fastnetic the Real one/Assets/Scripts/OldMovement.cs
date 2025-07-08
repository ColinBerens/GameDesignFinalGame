using UnityEngine;

public class OldMovement : MonoBehaviour
{
	[Header("InputManager")]
	[SerializeField] InputScript _inputScript;

	[Header("Movement")]
	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private float _jumpForce = 10f;

	[Header("Ground Check")]
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private float _groundRadius = 0.2f;
	[SerializeField] private LayerMask _groundLayer;

	[Header("Wall Check")]
	[SerializeField] private Transform _leftCheck;
	[SerializeField] private Transform _rightCheck;
	bool _isCollidingleft;
	bool _isCollidingright;

	private Rigidbody2D _rb;
	private bool _isGrounded;
	private float _horizontalInput;

	[SerializeField] private SoundManager _soundManager;
	private bool _isPlayed;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		_horizontalInput = _inputScript.MoveInput.x;
		Jump();
		Flip();
		if (_horizontalInput >= 0.01f || _horizontalInput <= -0.01f)
		{
			GetComponent<Animator>().SetBool("Walking", true);
			if (!_isPlayed && _isGrounded)
			{
				_isPlayed = true;
				_soundManager.Play("Walking");
			}
		}
		else
		{
			_isPlayed = false;
			GetComponent<Animator>().SetBool("Walking", false);
			_soundManager.Stop("Walking");
		}
		if (!_isGrounded)
		{
			_soundManager.Stop("Walking");
			_isPlayed = false;
		}
	}

	private void Flip()
	{
		if (_horizontalInput < 0f)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		}
		else if (_horizontalInput > 0f)
		{
			GetComponent<SpriteRenderer>().flipX = false;
		}
	}

	private void Jump()
	{
		if (_isGrounded)
		{
			GetComponent<Animator>().SetBool("Jump", false);
		}
		if (_inputScript.JumpInput && _isGrounded)
		{
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
			_soundManager.Play("Jump");
			GetComponent<Animator>().SetBool("Jump", true);
		}
	}
	private void FixedUpdate()
	{
		if (_isCollidingleft)
		{
			// Move the character
			_horizontalInput = Mathf.Clamp(_horizontalInput, 0, 1);
		}
		else if (_isCollidingright)
		{
			// Move the character
			_horizontalInput = Mathf.Clamp(_horizontalInput, -1, 0);

		}

		// Move the character
		_rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rb.linearVelocity.y);

		// Ground check
		_isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer);
		_isCollidingleft = Physics2D.OverlapCircle(_leftCheck.position, _groundRadius, _groundLayer);
		_isCollidingright = Physics2D.OverlapCircle(_rightCheck.position, _groundRadius, _groundLayer);
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
