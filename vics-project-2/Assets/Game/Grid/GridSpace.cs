using UnityEngine;
using CodeMonkey.Utils;

public struct GridSpace
{
	public int xPivot;
	public int yPivot;
	public int zPivot;

	public GridSpace(int xPivot, int yPivot, int zPivot = 0)
	{
		this.xPivot = xPivot;
		this.yPivot = yPivot;
		this.zPivot = zPivot;
	}

	public Vector3 GetLocalPosition(GridSystem parent)
	{
		Vector3 localPosition = new Vector3(this.xPivot * parent.GetCellSize(), this.yPivot * parent.GetCellSize(), 0f);
		return localPosition;
	}

	public Vector3 GetWorldPosition(GridSystem parent)
	{
		Vector3 worldPosition = this.GetLocalPosition(parent);
		worldPosition += parent.transform.position;
		return worldPosition;
	}

	public void DebugDraw(GridSystem parent, float secondsAliveTime = 1f)
	{
		this.DebugDrawText(parent, secondsAliveTime);
		this.DebugDrawOutline(parent, secondsAliveTime);
	}

	public void DebugDrawOutline(GridSystem parent, float secondsAliveTime = 1f)
	{
		Color lineColor = Color.chartreuse;

		// Get the base position
		Vector3 worldPos = this.GetWorldPosition(parent);

		// Calculate relative to base
		Vector3 tl = worldPos + new Vector3(0, parent.GetCellSize());
		Vector3 tr = worldPos + new Vector3(parent.GetCellSize(), parent.GetCellSize());
		Vector3 br = worldPos + new Vector3(parent.GetCellSize(), 0);

		Debug.DrawLine(worldPos, tl, lineColor, secondsAliveTime);
		Debug.DrawLine(tl, tr, lineColor, secondsAliveTime);
		Debug.DrawLine(tr, br, lineColor, secondsAliveTime);
		Debug.DrawLine(br, worldPos, lineColor, secondsAliveTime);
	}

	public void DebugDrawText(GridSystem parent, float secondsAliveTime = 1f)
	{
		// The second new Vector3 here is just to offset the text by half a cellsize on the x and y axes.
		Vector3 textWorldPosition = this.GetWorldPosition(parent) + new Vector3(parent.GetCellSize(), parent.GetCellSize()) * 0.5f;
		string labelString = "(" + this.xPivot + "," + this.yPivot + ")";
		int fontSize = 50;
		Color textColor = Color.blue;
		TextAnchor textAnchor = TextAnchor.MiddleCenter;
		TextMesh textMesh = UtilsClass.CreateWorldText(labelString, null, textWorldPosition, fontSize, textColor, textAnchor);
		textMesh.characterSize = 0.2f;
		textMesh.transform.localEulerAngles = new Vector3(0f, 180f, 45f);

		Object.Destroy(textMesh.gameObject, secondsAliveTime);
	}
}