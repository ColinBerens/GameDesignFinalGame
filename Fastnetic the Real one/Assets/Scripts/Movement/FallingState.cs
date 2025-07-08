using UnityEngine;

public class FallingState : IPlayerState
{
	private readonly MovementScript _controller;

	public FallingState(MovementScript controller)
	{
		_controller = controller;
	}

	public void Enter()
	{
        _controller.Animator.SetBool("Walking", false);
        _controller.Animator.SetBool("Fall", true);
		_controller.SoundManager.Stop("Walking");
		_controller.IsSoundPlaying = false;
	}

	public void Update()
	{
		if (_controller.IsGrounded)
		{
			_controller.TransitionToState(_controller.IdleState);
		}
	}

	public void FixedUpdate()
	{
		_controller.Move();
	}

	public void Exit()
	{
		_controller.Animator.SetBool("Jump", false);
		_controller.Animator.SetBool("Fall", false);
		_controller.SoundManager.Play("Land");
		_controller.CreateSmoke();
	}
}


