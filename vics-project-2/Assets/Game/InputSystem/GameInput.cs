using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

	private InputActions inputActions;
	public static event EventHandler<MousePositionEventArgs> OnRightMouseClick;

	public class MousePositionEventArgs : EventArgs
	{
		public RaycastHit[] rayCastHits;
	}

	void Awake()
	{
		inputActions = new InputActions();
		inputActions.Enable();
		InputSystem.EnableDevice(Mouse.current); // No idea why this is required - Mouse not detected without it.
	}

	private void RightMouseButton_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();

		Debug.Log("Clicked at x:" + mousePosition.x + ", y:" + mousePosition.y);

		/* This creates a ray from the center of the camera to the given position */
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);

		/* RaycastAll returns an array of all RaycastHits. Raycast function will return a boolean (use an out parameter to get the object)
		and stops after it hits the first object. */
		RaycastHit[] rayCastHits = Physics.RaycastAll(ray);
		int hitCount = rayCastHits.Length;
		MousePositionEventArgs eventArgs = new MousePositionEventArgs() { rayCastHits = rayCastHits };
		OnRightMouseClick?.Invoke(this, eventArgs);
	}

	private void OnEnable()
	{
		inputActions.UI.RightClick.performed += this.RightMouseButton_performed;
	}

	private void OnDisable()
	{
		inputActions.UI.RightClick.performed -= this.RightMouseButton_performed;
	}

}
