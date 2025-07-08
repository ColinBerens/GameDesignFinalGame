using UnityEngine;

public class IdleState : IPlayerState
{
	private readonly MovementScript _controller;

	public IdleState(MovementScript controller)
	{
		_controller = controller;
	}

	public void Enter()
	{
		_controller.Animator.SetBool("Walking", false);
		_controller.Animator.SetBool("Jump", false);
		_controller.Animator.SetBool("Idle", true);
		_controller.SoundManager.Stop("Walking");
	}

	public void Update()
	{
		if (!_controller.IsGrounded)
		{
			_controller.TransitionToState(_controller.FallingState);
			return;
		}

		if (Mathf.Abs(_controller.HorizontalInput) > 0.01f)
		{
			_controller.TransitionToState(_controller.WalkingState);
		}

		if (_controller.InputScript.JumpInput)
		{
			_controller.TransitionToState(_controller.JumpingState);
		}
	}

	public void FixedUpdate() { }

	public void Exit() 
	{
		_controller.Animator.SetBool("Idle", false);
	}
}


