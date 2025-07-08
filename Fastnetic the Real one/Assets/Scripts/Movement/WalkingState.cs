using UnityEngine;

public class WalkingState : IPlayerState
{
	private readonly MovementScript _controller;

	public WalkingState(MovementScript controller)
	{
		_controller = controller;
	}

	public void Enter()
	{
		_controller.Animator.SetBool("Walking", true);
		if (_controller.IsGrounded && !_controller.IsSoundPlaying)
		{
			_controller.SoundManager.Play("Walking");
			_controller.IsSoundPlaying = true;
		}
	}

	public void Update()
	{
		if (!_controller.IsGrounded)
		{
			_controller.TransitionToState(_controller.FallingState);
			return;
		}

		if (Mathf.Abs(_controller.HorizontalInput) < 0.01f)
		{
			_controller.TransitionToState(_controller.IdleState);
		}

		if (_controller.InputScript.JumpInput)
		{
			_controller.TransitionToState(_controller.JumpingState);
		}
		if (!_controller.IsGrounded && _controller.RB.linearVelocity.y < 0f)
		{
			_controller.TransitionToState(_controller.FallingState);
			return;
		}
	}

	public void FixedUpdate()
	{
		_controller.Move();
	}

	public void Exit()
	{
		_controller.SoundManager.Stop("Walking");
		_controller.IsSoundPlaying = false;
	}
}


