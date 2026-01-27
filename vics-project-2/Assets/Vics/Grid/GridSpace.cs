using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GridSpace
{
	private int xOrigin;
	private int yOrigin;
	public int CellSize { get; private set; }

	public static EventHandler<OnGridSpaceFocusedEventArgs> OnGridSpaceFocused;

	public class OnGridSpaceFocusedEventArgs : EventArgs
	{
		public GridSpace gridSpace;
	}

	public void FocusOn()
	{
		OnGridSpaceFocusedEventArgs eventArgs =
			new OnGridSpaceFocusedEventArgs() { gridSpace = this };

		OnGridSpaceFocused?.Invoke(this, eventArgs);
	}

	public GridSpace(int xOrigin, int yOrigin, int cellSize)
	{
		this.xOrigin = xOrigin;
		this.yOrigin = yOrigin;
		this.CellSize = cellSize;

		this.debugDraw();
	}

	public override string ToString()
	{
		return "(" + xOrigin + ", " + yOrigin + ")";
	}

	public void debugDraw()
	{
		this.debugDrawText();
		this.debugDrawOutline();
	}


	public void debugDrawOutline()
	{

		Color lineColor = Color.chartreuse;

		Vector3 botLeft = new Vector3(this.xOrigin, this.yOrigin) * this.CellSize;
		Vector3 topLeft = new Vector3(this.xOrigin, this.yOrigin + 1) * this.CellSize;
		Vector3 topRight = new Vector3(this.xOrigin + 1, this.yOrigin + 1) * this.CellSize;
		Vector3 botRight = new Vector3(this.xOrigin + 1, this.yOrigin) * this.CellSize;

		Debug.DrawLine(botLeft, topLeft, lineColor, 100f);
		Debug.DrawLine(topLeft, topRight, lineColor, 100f);
		Debug.DrawLine(topRight, botRight, lineColor, 100f);
		Debug.DrawLine(botRight, botLeft, lineColor, 100f);
	}


	public void debugDrawText()
	{
		// The second new Vector3 here is just to offset the text by half a cellsize on the x and y axes.
		Vector3 textWorldPosition = this.GetWorldPosition() + new Vector3(this.CellSize, this.CellSize) * 0.5f;
		string labelString = this.ToString();
		int fontSize = 20;
		Color textColor = Color.white;
		TextAnchor textAnchor = TextAnchor.MiddleCenter;
		TextMesh textMesh = UtilsClass.CreateWorldText(labelString, null, textWorldPosition, fontSize, textColor, textAnchor);
		textMesh.transform.eulerAngles = new Vector3(0f, 180f, 45f);
	}

	public Vector3 GetWorldPosition()
	{
		return new Vector3(this.xOrigin, this.yOrigin) * this.CellSize;
	}
}
