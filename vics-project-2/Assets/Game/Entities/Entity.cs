using System;
using UnityEngine;

public class Entity : MonoBehaviour
{

	private Vector3Int position;
	[SerializeField] private GridSystem gridSystem;
	[SerializeField] private EntityEventChannel entityEventChannel;


	private void Start()
	{
		if (this.gridSystem == null) Debug.LogError("Needs a currentGridSystem Set!");
		this.UpdatePosition(new Vector3Int(2, 2, 0));
	}


	private void OnDestroy()
	{
		DestroyEventArgs eventArgs = new DestroyEventArgs()
		{
			position = this.position,
			entity = this
		};

		entityEventChannel.FireEvent(this, eventArgs);
	}


	public void UpdatePosition(Vector3Int updateDelta)
	{
		Vector3Int updatedPosition = this.position + updateDelta;
		Vector3 moveDirection = ((Vector3)updateDelta).normalized;


		PositionChangeEventArgs eventArgs = new PositionChangeEventArgs()
		{
			gridSystem = this.gridSystem,
			toPosition = updatedPosition,
			fromPosition = this.position,
			entity = this,
		};

		entityEventChannel.FireEvent(this, eventArgs);

		transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * 5f);

		this.position = updatedPosition;
		Debug.Log(moveDirection);
		Debug.Log("Position Updated");
	}

}
