using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

	[SerializeField] private int xAxisCellCount;
	[SerializeField] private int yAxisCellCount;
	[SerializeField] private int zAxisCellCount;
	[SerializeField] private int cellSize;
	private BoxCollider boxCollider;
	private Dictionary<Vector3Int, List<Entity>> entityMap = new Dictionary<Vector3Int, List<Entity>>();


	public int GetCellSize()
	{
		return this.cellSize;
	}


	private void Start()
	{
		this.InitBoxCollider();
	}


	private void OnEnable()
	{
		GameInput.OnRightMouseClick += this.HandleRightMouseClick;
		Entity.OnEntityPositionChange += this.HandleEntityPositionChange;
		Entity.OnEntityDestroy += this.HandleEntityDestroy;
	}


	private void OnDisable()
	{
		GameInput.OnRightMouseClick -= this.HandleRightMouseClick;
		Entity.OnEntityPositionChange -= this.HandleEntityPositionChange;
		Entity.OnEntityDestroy -= this.HandleEntityDestroy;
	}


	private void InitBoxCollider()
	{
		if (this.boxCollider == null)
		{
			this.boxCollider = this.gameObject.AddComponent<BoxCollider>();
			Vector3 colliderWorldSize = new Vector3(this.xAxisCellCount * cellSize, this.yAxisCellCount * cellSize, 0.01f);
			this.boxCollider.size = colliderWorldSize;
			this.boxCollider.center = this.GetCenterPoint();
			this.boxCollider.isTrigger = true;
			this.gameObject.AddComponent<ColliderDebugDraw>();
		}
	}


	private Vector3 GetCenterPoint()
	{
		Vector3 gridSpaceCenterPoint = new Vector3(this.xAxisCellCount * cellSize / 2, this.yAxisCellCount * cellSize / 2, 0f);
		return gridSpaceCenterPoint;
	}


	private void HandleRightMouseClick(object sender, GameInput.MousePositionEventArgs mousePosEventArgs)
	{
		RaycastHit[] raycastHits = mousePosEventArgs.rayCastHits;

		int rayCastHitsCount = raycastHits.Length;

		for (int rayCastHitIndex = 0; rayCastHitIndex < rayCastHitsCount; rayCastHitIndex++)
		{
			RaycastHit raycastHit = raycastHits[rayCastHitIndex];
			GameObject hitObject = raycastHit.transform.gameObject;

			// Use transform.InverseTransformPoint to convert world hit to local hit. If this GridSystem is positioned
			// at (0,0,0), this wouldn't be necessary. But if we move the transform of this object, we need to adjust
			// the raycastHit.point, which is in world co-ordinates, to match our GridSystem's position.
			Vector3 localHitPoint = this.transform.InverseTransformPoint(raycastHit.point);
			// -------------------------------------------------------------------------------------------------------

			if (hitObject == this.gameObject)
			{
				int x = Mathf.FloorToInt(localHitPoint.x / this.cellSize);
				int y = Mathf.FloorToInt(localHitPoint.y / this.cellSize);
				int z = Mathf.FloorToInt(localHitPoint.z / this.cellSize);

				if (IsGridSpaceInRange(x, y, z))
				{
					GridSpace gridSpace = this.GetGridSpaceData(x, y);
					Vector3 gridSpaceWorldPosition = gridSpace.GetWorldPosition(this);
					float halfCellSize = this.cellSize / 2f;
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
	}


	private void HandleEntityPositionChange(object sender, Entity.EntityPositionChangeEventArgs eventArgs)
	{
		GridSystem gridSystemToChange = eventArgs.gridSystem;
		Entity entity = sender as Entity;

		if (entity != null) // if the cast was successfull
		{

			if (gridSystemToChange == this)
			{
				Vector3Int toPosition = eventArgs.toPosition;
				Vector3Int fromPosition = eventArgs.fromPosition;

				// ----------------------------------------------------------------------
				// If this entity previously occupied a space on this grid-system, remove 
				// that reference since it has now moved. We want to remove-first so we
				// don't "add it twice" and then remove it if for whatever reason the to
				// and from cells are the same cell.
				if (this.entityMap.ContainsKey(fromPosition))
				{
					bool isRemoved = this.entityMap[fromPosition].Remove(entity);
					if (isRemoved)
					{
						if (this.entityMap[fromPosition].Count == 0)
						{
							this.entityMap.Remove(fromPosition); // clean up the map if it contains nothing now
						}
					}
				}
				// ---------------------------------------------------------------------

				if (this.IsGridSpaceInRange(toPosition))
				{
					//----------------------------------------------------------------------
					// Add the entity to a List of entities stored at the "to" position...
					if (!this.entityMap.ContainsKey(toPosition))
						this.entityMap[toPosition] = new List<Entity>();

					List<Entity> newEntityList = this.entityMap[toPosition];

					newEntityList.Add(entity);
					// --------------------------------------------------------------------
				}
				else
					throw new System.Exception("Tried to move an Entity to an invalid space");
			}
		}
		else
			throw new System.Exception("Non-Entity object used onEntityPositionChange!");
	}


	private void HandleEntityDestroy(object sender, Entity.EntityDestroyEventArgs eventArgs)
	{
		Entity entity = sender as Entity;

		if (entity != null) // if the cast was successfull
		{
			Vector3Int entityPosition = eventArgs.position;

			if (this.entityMap.ContainsKey(entityPosition))
			{
				List<Entity> entityList = this.entityMap[entityPosition];
				entityList.Remove(entity);

				// Clean up the key if the cell is now empty
				if (this.entityMap[entityPosition].Count == 0)
				{
					this.entityMap.Remove(entityPosition);
				}
			}
		}
	}


	/* This conditional is to save against extreme rare edge cases that could potentially cause massive bugs that
	would be incredibly hard to find. That is because if the user clicks on a very edge, floating point Imprecision
	might return a value like 10.000000001 instead of 10, which could potentially cause all sorts of weirdness
	according to Google Gemini. */
	private bool IsGridSpaceInRange(float x, float y, float z)
	{
		return x >= 0 && x < xAxisCellCount && y >= 0 && y < yAxisCellCount && z >= 0 && z < zAxisCellCount;
	}


	private bool IsGridSpaceInRange(Vector3Int gridPosition)
	{
		return this.IsGridSpaceInRange(gridPosition.x, gridPosition.y, gridPosition.z);
	}


	private GridSpace GetGridSpaceData(int x, int y, int z = 0)
	{
		return new GridSpace(x, y, z);
	}

}
