using System;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{


	public static Player Instance { get; private set; }

	public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

	public class OnSelectedCounterChangedEventArgs : EventArgs
	{
		public ClearCounter selectedCounter;
	}

	[SerializeField] private float moveSpeed = 7f;
	[SerializeField] private float interactDistance = 2f;
	[SerializeField] private GameInput gameInput;
	[SerializeField] private LayerMask countersLayerMask;

	private bool isWalking = false;

	/** This is used in HandleInteractions so that the RayCast that detirmines if we're in interactable distance
	still works even if we have stopped within that distance. */
	private Vector3 lastMoveDirVector;

	private ClearCounter selectedCounter;

	private void Awake()
	{
		if (Instance != null) Debug.LogError("There is more than one player instance!");
		Player.Instance = this;
	}

	private void Start()
	{
		gameInput.OnInteractAction += GameInput_OnInteractAction;
	}

	private void GameInput_OnInteractAction(object sender, System.EventArgs e)
	{
		this.HandleInteractions();

		if (selectedCounter != null) selectedCounter.Interact();
		else Debug.Log("NULL");
	}

	public bool IsWalking()
	{
		return isWalking;
	}
	// Update is called once per frame
	private void Update()
	{
		HandleMovement();
		//HandleInteractions();
	}

	private void HandleInteractions()
	{
		Vector3 moveDirVector = this.GetMovementDirVector();

		if (moveDirVector != Vector3.zero)
			lastMoveDirVector = moveDirVector;

		RaycastHit raycastHit;
		bool didRayCastHit =
			Physics.Raycast(transform.position,
			lastMoveDirVector,
			out raycastHit,
			this.interactDistance,
			this.countersLayerMask);

		if (didRayCastHit)
		{
			/*
			 This is kind of confusing to me. Why are we using a transform to get a component?
			 The way Claude A.I. explained it: it's a legacy design decision. Every GameObject
			 is required to have a Transform component, so Unity returns that as a convenient
			 "handle" to the object that was hit. From there you can access the GameObject itself
			 or any other components. It would make more sense to just return the GameObject
			 directly, but here we are.

			Gemini backs this up: In Unity’s architecture, the Transform component is much more
			than just a data holder for coordinates. Every GameObject must have a Transform. Because
			of this, the Transform acts as the central hub for the object's identity in the scene
			hierarchy.
			*/
			bool didRayCastHitClearCounter =
				raycastHit.transform.TryGetComponent(out ClearCounter clearCounter);

			if (didRayCastHitClearCounter)
				if (this.selectedCounter != clearCounter)
					this.SetSelectedCounter(clearCounter);
				else this.SetSelectedCounter(null);
		}
		else this.SetSelectedCounter(null);
	}

	private void HandleMovement()
	{

		Vector3 moveDirVector = GetMovementDirVector();
		float rotateSpeed = 7f;
		float moveDistance = moveSpeed * Time.deltaTime;

		if (IsMovementBlocked(moveDirVector, moveDistance))
		{
			// If movement is blocked, assume we're trying to move diagonaly along two axes, and try again just moving
			// along one axis. This will result in the player "sliding" along the wall instead of just stopping if diagonal
			// movement is held down against the wall.

			// Note, these must be normalized, which will reset the length of the Vector to 1, where it would have been
			// shorter before if we were moving on a diagonal.
			Vector3 moveDirX = new Vector3(moveDirVector.x, 0, 0).normalized;
			Vector3 moveDirZ = new Vector3(0, 0, moveDirVector.z).normalized;

			if (!IsMovementBlocked(moveDirX, moveDistance)) moveDirVector = moveDirX;
			else if (!IsMovementBlocked(moveDirZ, moveDistance)) moveDirVector = moveDirZ;
			else moveDirVector = Vector3.zero;
		}

		transform.position += moveDirVector * moveSpeed * Time.deltaTime;
		transform.forward = Vector3.Slerp(transform.forward, moveDirVector, Time.deltaTime * rotateSpeed);
		isWalking = moveDirVector != Vector3.zero;
	}


	private bool IsMovementBlocked(Vector3 moveDirection, float moveDistance)
	{
		float playerHeight = 2f;
		Vector3 capsuleOrigin = transform.position;
		Vector3 capsuleHeight = transform.position + Vector3.up * playerHeight;
		float capsuleRadius = 0.7f;
		bool isBlocked = Physics.CapsuleCast(capsuleOrigin, capsuleHeight, capsuleRadius, moveDirection, moveDistance);

		return isBlocked;
	}

	private Vector3 GetMovementDirVector()
	{
		Vector2 inputVector = gameInput.GetMovementVectorNormalized();
		Vector3 moveDirVector = new Vector3(inputVector.x, 0f, inputVector.y);
		return moveDirVector;
	}

	private void SetSelectedCounter(ClearCounter selectedCounter)
	{

		this.selectedCounter = selectedCounter;

		OnSelectedCounterChangedEventArgs eventArgs =
			new OnSelectedCounterChangedEventArgs { selectedCounter = this.selectedCounter };

		OnSelectedCounterChanged?.Invoke(this, eventArgs);
	}

}

