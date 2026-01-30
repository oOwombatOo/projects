using UnityEditor.Toolbars;
using UnityEngine;

public class Entity : MonoBehaviour
{

	private Vector3Int startPosition;
	private Vector3Int targetPosition;
	private float oneGameMeterMoveTimeSsec = 0.5f; // one "game meter" should be half a square. i.e. each standard grid space is 2m.
	private float currentMoveTime = 0f;
	private bool isMoving = false;
	[SerializeField] private GridSystem gridSystem;
	[SerializeField] private EntityEventChannel entityEventChannel;
	[SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


	private void Start()
	{
		if (this.gridSystem == null) Debug.LogError("Needs a currentGridSystem Set!");

		this.startPosition = Vector3Int.RoundToInt(this.transform.position);

		// testing purposes only
		Vector3Int target = new Vector3Int(8, 7, 0);
		this.MoveTo(target);
	}

	private void Update()
	{
		if (this.isMoving)
		{
			IncrementMovement();
		}
	}


	public void MoveTo(Vector3Int newTarget)
	{
		if (isMoving) return;

		startPosition = Vector3Int.RoundToInt(transform.position);
		targetPosition = newTarget;
		currentMoveTime = 0f;
		isMoving = true;
		float moveDistance = Vector3.Distance(startPosition, targetPosition);
		Debug.Log(moveDistance + "m in " + (moveDistance * oneGameMeterMoveTimeSsec) + "sec");
	}


	private void IncrementMovement()
	{

		// Add the time passed since last increment (i.e. frame) to the current move time.
		this.currentMoveTime += Time.deltaTime;

		float moveDistance = Vector3.Distance(this.startPosition, this.targetPosition);

		float currentMoveTimeSec = moveDistance * oneGameMeterMoveTimeSsec;

		// This catch block is for edge-case scenarios, for example if for some reason, the
		// start position and end position are the same, the this.currentMoveTime will be zero
		// which will result in a divide by zero in this function and other possible weirdness.
		if (currentMoveTimeSec <= Mathf.Epsilon)
		{
			CompleteMovement();
			return;
		}

		// Clamp01 simply caps the value at 1 to account for floating point rounding errors
		float percentMoveTimeComplete = Mathf.Clamp01(currentMoveTime / currentMoveTimeSec);

		float animationPercent = movementCurve.Evaluate(percentMoveTimeComplete);

		// Lerp returns the point between startPosition and targetPosition at the provided percent,
		// so if the value were 0.5, the point would be exactly half way to the destination.
		transform.position = Vector3.Lerp(this.startPosition, this.targetPosition, animationPercent);

		if (percentMoveTimeComplete == 1f)
		{
			CompleteMovement();
		}

	}


	private void CompleteMovement()
	{
		isMoving = false;
		transform.position = this.targetPosition;
		this.currentMoveTime = 0f;
		this.AnnounceNewPosition();

		// Ensure this is done after AnnounceNewPosition, we need the original startPosition there.
		this.startPosition = this.targetPosition;

		Debug.Log("Movement Complete");
	}


	private void OnDestroy()
	{
		DestroyEventArgs eventArgs = new DestroyEventArgs()
		{
			position = this.startPosition,
			entity = this
		};

		entityEventChannel.FireEvent(this, eventArgs);
	}


	private void AnnounceNewPosition()
	{
		PositionChangeEventArgs eventArgs = new PositionChangeEventArgs()
		{
			gridSystem = this.gridSystem,
			toPosition = this.targetPosition,
			fromPosition = this.startPosition,
			entity = this,
		};
		Debug.Log(this.targetPosition.ToString());

		entityEventChannel.FireEvent(this, eventArgs);
	}

}
