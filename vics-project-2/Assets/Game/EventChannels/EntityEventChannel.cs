using System;
using UnityEditor;
using UnityEngine;


public class PositionChangeEventArgs : EventArgs
{
	public GridSystem gridSystem;
	public Vector3Int fromPosition;
	public Vector3Int toPosition;
	public Entity entity;
}

public class DestroyEventArgs : EventArgs
{
	public Vector3Int position;
	public Entity entity;
}


[CreateAssetMenu(fileName = "EntityEventChannel", menuName = "Scriptable Objects/EntityEventChannel")]
public class EntityEventChannel : ScriptableObject
{
	// ------------------------------------------------------------------------

	public event EventHandler<PositionChangeEventArgs> OnPositionChange;

	public void FireEvent(object sender, PositionChangeEventArgs e)
	{
		OnPositionChange?.Invoke(sender, e);
	}

	// ------------------------------------------------------------------------

	public event EventHandler<DestroyEventArgs> OnDestroy;

	public void FireEvent(object sender, DestroyEventArgs e)
	{
		OnDestroy?.Invoke(sender, e);
	}

	// ------------------------------------------------------------------------
}
