using UnityEngine;
using System;

public class RightClickEventArgs : EventArgs
{
	public RaycastHit[] rayCastHits;
}

[CreateAssetMenu(fileName = "InputEventChannel", menuName = "Scriptable Objects/InputEventChannel")]
public class InputEventChannel : ScriptableObject
{

	public event EventHandler<RightClickEventArgs> OnRightClick;

	public void FireEvent(object sender, RightClickEventArgs eventArgs)
	{
		OnRightClick?.Invoke(sender, eventArgs);
	}

}
