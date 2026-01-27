using UnityEngine;

public class GridSystem
{

	private int width;
	private int height;
	private int cellSize;
	private GridSpace[,] gridSpaces;

	public GridSystem(int width, int height, int cellSize)
	{
		this.gridSpaces = new GridSpace[width, height];
		this.width = width;
		this.height = height;
		this.cellSize = cellSize;

		GameInput.OnRightMouseClick += this.gameInput_OnRightMouseClick;

		for (int xOrigin = 0; xOrigin < width; xOrigin++)
		{
			for (int yOrigin = 0; yOrigin < height; yOrigin++)
			{
				GridSpace gridSpace = new GridSpace(xOrigin, yOrigin, this.cellSize);
				gridSpaces[xOrigin, yOrigin] = gridSpace;
			}
		}

		gridSpaces[5, 5].FocusOn();
	}

	private void gameInput_OnRightMouseClick(object sender, GameInput.MousePositionEventArgs mousePosEventArgs)
	{
		Vector2 mousePos = mousePosEventArgs.MousePosition;
		Debug.Log("Clicked at x:" + mousePos.x + ", y:" + mousePos.y);

		/* This creates a ray from the center of the camera to the given position */
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit[] rayCastHits = Physics.RaycastAll(ray);

		if (rayCastHits.Length > 0)
		{
			foreach (RaycastHit rayHitInfo in rayCastHits)
				Debug.Log("Raycast hit: " + rayHitInfo.collider.gameObject.name + " at distance: " + rayHitInfo.distance);

			Debug.Log("FIRST HIT: " + rayCastHits[rayCastHits.Length - 1].collider.gameObject.name);
		}
		else
		{
			Debug.Log("nothing hit");
		}
	}

}
