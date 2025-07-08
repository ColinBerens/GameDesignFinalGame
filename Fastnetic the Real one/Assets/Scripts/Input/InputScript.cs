using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputScript : MonoBehaviour
{
	public Vector2 MoveInput {get; private set; }= Vector2.zero;
	public Vector2 ViewInput {get; private set; }= Vector2.zero;
    public bool JumpInput {get; private set; }= false;
    public bool MagneticInput { get; private set; } = false;
	public bool MenuInput { get; private set; } = false;
	public bool ReplayInput { get; private set; } = false;
	public bool RepelInput { get; private set; } = false;
    InputMap _input = null;
	private void OnEnable()
	{
		_input = new InputMap();
		_input.Movement.Enable();

		_input.Movement.Movement.performed += SetMovement;
		_input.Movement.Movement.canceled += SetMovement;

        _input.Movement.Jump.started += SetJump;
        _input.Movement.Jump.canceled += SetJump;

		_input.Movement.Magnetic.started += SetMagnetic;
		_input.Movement.Magnetic.canceled += SetMagnetic;

        _input.Movement.Repel.started += SetRepel;
        _input.Movement.Repel.canceled += SetRepel;

		_input.Movement.Direction.performed += SetView;
        _input.Movement.Direction.canceled += SetView;


        _input.Movement.Enable();
	}


    private void OnDisable()
	{
		_input.Movement.Movement.performed -= SetMovement;
		_input.Movement.Movement.canceled -= SetMovement;

        _input.Movement.Jump.started -= SetJump;
        _input.Movement.Jump.canceled -= SetJump;

		_input.Movement.Magnetic.started -= SetMagnetic;
        _input.Movement.Magnetic.canceled -= SetMagnetic;

        _input.Movement.Repel.started -= SetRepel;
        _input.Movement.Repel.canceled -= SetRepel;

        _input.Movement.Direction.performed -= SetView;
        _input.Movement.Direction.canceled -= SetView;

        _input.Movement.Disable();
	}
	private void Update()
	{
		if (RepelInput)
		{
			RumbleManager.Instance.StartRumble(0.3f, 0.3f/4);
		}
		else if (MagneticInput)
		{
			RumbleManager.Instance.StartRumble(0.3f / 4, 0.3f);
		}
		else
		{
			RumbleManager.Instance.StopRumble();
		}
		MenuInput = _input.Movement.MenuInput.WasPressedThisFrame();
		if (MenuInput)
		{
			Application.Quit();
		}
		ReplayInput = _input.Movement.ReplayInput.WasPressedThisFrame();
		if (ReplayInput)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
	private void SetMovement(InputAction.CallbackContext ctx)
	{
		MoveInput = ctx.ReadValue<Vector2>();
	}
	private void SetView(InputAction.CallbackContext ctx)
	{
		ViewInput = ctx.ReadValue<Vector2>();
	}
	private void SetJump(InputAction.CallbackContext ctx)
    {
        JumpInput = ctx.started;
    }
    private void SetMagnetic(InputAction.CallbackContext ctx)
    {
        MagneticInput = ctx.started;
    }
    private void SetRepel(InputAction.CallbackContext ctx)
    {
		RepelInput = ctx.started;
    }
}
