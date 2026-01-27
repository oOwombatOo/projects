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


}
