using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

	private PlayerInputActions playerInputActions;
	public event EventHandler OnInteractAction;



	private void Awake()
	{
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();

		/* This is adding an event handler to the Interact.performed event that was
		generated automatically be Unity when we created the Interact input in the
		PlayerInputActions input object in the UI */
		playerInputActions.Player.Interact.performed += Interact_performed;
	}

	private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		/* The ? here is basically a null check. Invoke is required simply because we can't
		follow the null check with parenthesis, that's all it's doing.

		Why are we checking if it's null? It will be null if there are no subscribers to this
		event for whatever reasons. So this is basically just checking if there are
		subscribers to our Event, and only firing it if yes. */
		OnInteractAction?.Invoke(this, EventArgs.Empty);
	}

	public Vector2 GetMovementVectorNormalized()
	{
		/*
		A Vector represents a straight line. A Vector2 is therefor a straight line
		on a flat surface. A Vector2 of (2,3) would mean, starting at origin (0,0),
		move 2 units right and 3 units up, and imagine a line going from 0,0 to that
		point. The length of the vector is the length of the line. In this case,
		it would be the square root of 4+9 (3.6)
		*/
		// This value is getting read from the "new" Input System, which is largely setup in the GUI.
		Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

		/*
		Normalizing a Vector converts it to have a length of 1, and adjusts the
		numbers so the line is still "pointing" in the same direction.
		For example, a Vector2 of (5,10) - length:11.18 - would become (0.447, 0.894).
		This is useful for when we care about direction but not length.
		*/
		inputVector.Normalize();
		return inputVector;
	}

}
