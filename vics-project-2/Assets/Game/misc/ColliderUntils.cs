using UnityEngine;

public class ColliderDebugDraw : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		BoxCollider boxCollider = GetComponent<BoxCollider>();
		if (boxCollider != null)
		{
			Gizmos.color = Color.red;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
		}
	}
}

public class ColliderUntils
{

}
