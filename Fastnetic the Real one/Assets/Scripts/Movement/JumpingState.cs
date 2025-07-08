using UnityEngine;

public class JumpingState : IPlayerState
{
	private readonly MovementScript _controller;

	public JumpingState(MovementScript controller)
	{
		_controller = controller;
	}

	public void Enter()
	{
		_controller.Animator.SetBool("Jump", true);
        _controller.Animator.SetBool("Walking", false);

        _controller.Jump();
		_controller.CreateSmoke();
	}

	public void Update()
	{
		if (_controller.RB.linearVelocity.y < 0f)
		{
			_controller.TransitionToState(_controller.FallingState);
		}
	}

	public void FixedUpdate()
	{
		_controller.Move();
	}

	public void Exit()
	{
	}
}


