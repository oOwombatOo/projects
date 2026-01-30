using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

	[SerializeField] InputEventChannel inputEventChannel;
	private InputActions inputActions;

	void Awake()
	{
		inputActions = new InputActions();
		inputActions.Enable();
		InputSystem.EnableDevice(Mouse.current); // No idea why this is required - Mouse not detected without it.
	}

	private void HandleRightClick(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();

		/* This creates a ray from the center of the camera to the given position */
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);

		/* RaycastAll returns an array of all RaycastHits. Raycast function will return a boolean (use an out parameter to get the object)
		and stops after it hits the first object. */
		RaycastHit[] rayCastHits = Physics.RaycastAll(ray);
		int hitCount = rayCastHits.Length;
		RightClickEventArgs rightClickEventArgs = new RightClickEventArgs() { rayCastHits = rayCastHits };
		inputEventChannel.FireEvent(this, rightClickEventArgs);

	}

	private void OnEnable()
	{
		inputActions.UI.RightClick.performed += this.HandleRightClick;
	}

	private void OnDisable()
	{
		inputActions.UI.RightClick.performed -= this.HandleRightClick;
	}

}
