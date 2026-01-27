using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

	private InputActions inputActions;
	public static event EventHandler<MousePositionEventArgs> OnRightMouseClick;

	public class MousePositionEventArgs : EventArgs
	{
		public Vector2 MousePosition;
	}

	void Awake()
	{
		inputActions = new InputActions();
		inputActions.Enable();
		inputActions.UI.RightClick.performed += this.RightMouseButton_performed;

		InputSystem.EnableDevice(Mouse.current); // No idea why this is required - Mouse not detected without it.
	}

	private void RightMouseButton_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		MousePositionEventArgs mousePosArgs = new MousePositionEventArgs() { MousePosition = mousePosition };
		OnRightMouseClick?.Invoke(this, mousePosArgs);
	}

}
