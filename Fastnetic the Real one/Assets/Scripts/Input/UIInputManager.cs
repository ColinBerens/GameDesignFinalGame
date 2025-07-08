using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject firstSelectedUI;

    void Start()
    {
        // Force control scheme to Gamepad
        playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);

        // Set first UI selected object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedUI);
    }
}
