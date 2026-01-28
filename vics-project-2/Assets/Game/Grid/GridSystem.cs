using Unity.VisualScripting;
using CodeMonkey.Utils;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

	[SerializeField] private int xAxisCellCount;
	[SerializeField] private int yAxisCellCount;
	[SerializeField] private int cellSize;
	private BoxCollider boxCollider;


	private struct GridSpace
	{
		public int xPivot;
		public int yPivot;

		public GridSpace(int xPivot, int yPivot)
		{
			this.xPivot = xPivot;
			this.yPivot = yPivot;
		}

		public Vector3 GetLocalPosition(GridSystem parent)
		{
			Vector3 localPosition = new Vector3(this.xPivot * parent.cellSize, this.yPivot * parent.cellSize, 0f);
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
			Vector3 tl = worldPos + new Vector3(0, parent.cellSize);
			Vector3 tr = worldPos + new Vector3(parent.cellSize, parent.cellSize);
			Vector3 br = worldPos + new Vector3(parent.cellSize, 0);

			Debug.DrawLine(worldPos, tl, lineColor, secondsAliveTime);
			Debug.DrawLine(tl, tr, lineColor, secondsAliveTime);
			Debug.DrawLine(tr, br, lineColor, secondsAliveTime);
			Debug.DrawLine(br, worldPos, lineColor, secondsAliveTime);
		}


		public void DebugDrawText(GridSystem parent, float secondsAliveTime = 1f)
		{
			// The second new Vector3 here is just to offset the text by half a cellsize on the x and y axes.
			Vector3 textWorldPosition = this.GetWorldPosition(parent) + new Vector3(parent.cellSize, parent.cellSize) * 0.5f;
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


	private void Start()
	{
		this.InitBoxCollider();
		GameInput.OnRightMouseClick += this.HandleRightMouseClick;
	}

	private void OnEnable()
	{
		GameInput.OnRightMouseClick += this.HandleRightMouseClick;
	}

	private void OnDisable()
	{
		GameInput.OnRightMouseClick -= this.HandleRightMouseClick;
	}


	private void InitBoxCollider()
	{
		if (this.boxCollider == null)
		{
			this.boxCollider = this.gameObject.AddComponent<BoxCollider>();
			Vector3 colliderWorldSize = new Vector3(this.xAxisCellCount * cellSize, this.xAxisCellCount * cellSize, 0.01f);
			this.boxCollider.size = colliderWorldSize;
			this.boxCollider.center = this.GetCenterPoint();
			this.boxCollider.isTrigger = true;
			this.gameObject.AddComponent<ColliderDebugDraw>();
		}
	}


	private Vector3 GetCenterPoint()
	{
		Vector3 gridSpaceCenterPoint = new Vector3(this.xAxisCellCount * cellSize / 2, this.xAxisCellCount * cellSize / 2, 0f);
		return gridSpaceCenterPoint;
	}


	private void HandleRightMouseClick(object sender, GameInput.MousePositionEventArgs mousePosEventArgs)
	{
		RaycastHit[] raycastHits = mousePosEventArgs.rayCastHits;

		int rayCastHitsCount = raycastHits.Length;

		for (int rayCastHitIndex = 0; rayCastHitIndex < rayCastHitsCount; rayCastHitIndex++)
		{
			RaycastHit raycastHit = raycastHits[rayCastHitIndex];
			GameObject hitObject = raycastHit.transform.GameObject();

			if (hitObject == this.gameObject)
			{
				int x = Mathf.FloorToInt(raycastHit.point.x / this.cellSize);
				int y = Mathf.FloorToInt(raycastHit.point.y / this.cellSize);
				GridSpace gridSpace = this.GetGridSpaceData(x, y);
				Vector3 gridSpaceWorldPosition = gridSpace.GetWorldPosition(this);
				float halfCellSize = this.cellSize / 2;
				float xRequestFocus = gridSpaceWorldPosition.x + halfCellSize;
				float yRequestFocus = gridSpaceWorldPosition.y + halfCellSize;
				float zRequestFocus = 0f;
				Vector3 requestedFocusPosition = new Vector3(xRequestFocus, yRequestFocus, zRequestFocus);

				gridSpace.DebugDraw(this);

				CameraController.RequestCameraFocusEventArgs eventArgs
					= new CameraController.RequestCameraFocusEventArgs { RequestedFocusWorldPosition = requestedFocusPosition };

				CameraController.OnRequestCameraFocus?.Invoke(this, eventArgs);
			}
		}
	}

	private GridSpace GetGridSpaceData(int x, int y)
	{
		return new GridSpace(x, y);
	}

}
