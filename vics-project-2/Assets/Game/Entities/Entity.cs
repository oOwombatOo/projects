using System;
using UnityEngine;

public class Entity : MonoBehaviour
{

	private Vector3Int position;
	[SerializeField] private GridSystem gridSystem;



	public static event EventHandler<EntityPositionChangeEventArgs> OnEntityPositionChange;

	public class EntityPositionChangeEventArgs : EventArgs
	{
		public GridSystem gridSystem;
		public Vector3Int fromPosition;
		public Vector3Int toPosition;
	}

	public static event EventHandler<EntityDestroyEventArgs> OnEntityDestroy;

	public class EntityDestroyEventArgs : EventArgs
	{
		public Vector3Int position;
	}


	private void Start()
	{
		if (this.gridSystem == null) Debug.LogError("Needs a currentGridSystem Set!");
		this.UpdatePosition(new Vector3Int(2, 2, 0));
	}


	private void OnDestroy()
	{
		EntityDestroyEventArgs eventArgs = new EntityDestroyEventArgs()
		{
			position = this.position
		};

		OnEntityDestroy?.Invoke(this, eventArgs);
	}


	public void UpdatePosition(Vector3Int updateDelta)
	{
		Vector3Int updatedPosition = this.position + updateDelta;

		EntityPositionChangeEventArgs eventArgs = new EntityPositionChangeEventArgs()
		{
			gridSystem = this.gridSystem,
			toPosition = updatedPosition,
			fromPosition = this.position,
		};

		this.position = updatedPosition;

		OnEntityPositionChange?.Invoke(this, eventArgs);
	}

}
