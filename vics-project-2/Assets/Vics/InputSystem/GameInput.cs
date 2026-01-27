using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

	private InputActions inputActions;
	public event EventHandler OnRightMouseClick;



	void Awake()
	{
		Debug.Log("AWAKE");
		inputActions = new InputActions();
		inputActions.Enable();
		inputActions.UI.Enable();
		inputActions.UI.RightClick.Enable();
		inputActions.UI.RightClick.performed += this.RightMouseButton_performed;

		Debug.Log("Action enabled: " + inputActions.UI.RightClick.enabled);
		Debug.Log("Bindings count: " + inputActions.UI.RightClick.bindings.Count);
		Debug.Log("Binding path: " + inputActions.UI.RightClick.bindings[0].effectivePath);
		inputActions.UI.RightClick.started += (ctx) => Debug.Log("STARTED");
		inputActions.UI.RightClick.canceled += (ctx) => Debug.Log("CANCELED");
	}

	private void RightMouseButton_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		Debug.Log("RIGHT MOUSE CLICK CAUGHT");
		Debug.Log(obj.ToString());
		OnRightMouseClick?.Invoke(this, EventArgs.Empty);
	}

	void Update()
	{    // Direct mouse check
		if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
		{
			Debug.Log("Mouse.current detected right click");
		}
	}

}
