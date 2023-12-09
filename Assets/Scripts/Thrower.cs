using UnityEngine;
public class Thrower : MonoBehaviour
{

	[SerializeField] Rigidbody rigidBody;
	[SerializeField] Vector3 pointOfAction;
	[SerializeField] float velocityMultiplier = 10;
	[SerializeField] float maxDistance = 5;
	[SerializeField][Min(0)] float gizmoSize;

	Vector3 _startPos;

	Vector3 _dragPos;

	Plane _startPlane;

	Plane ObjectPlane => new(transform.position, Vector3.forward);

	Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);
	Vector3 PointOfActionLocal => rigidBody.centerOfMass + pointOfAction;
	Vector3 PointOfActionWorld => rigidBody.transform.TransformPoint(PointOfActionLocal);

	Vector3 ThrowVector
	{
		get
		{
			string[,] matrix2 = new string[4, 2];       // 2D tömb  Mérete: 4*2
			int lengt = matrix2.Length;                 // 8
			int rank = matrix2.Rank;                    // 2
			int length0 = matrix2.GetLength(0);         // 4
			int length1 = matrix2.GetLength(1);         // 2
			int lowerBound0 = matrix2.GetLowerBound(0); // 0

			for(int i = 0; i < matrix2.GetLength(0); i++)
			{
				for(int j = 0; j < matrix2.GetLength(1); j++)
				{
					matrix2[i, j] = i + " " + j;
				}
			}




			Vector2 A = Vector2.right;
			Vector2 B = Vector2.up;

			float angle1 = Vector2.Angle(A, B);
			// result is 90 degrees
			float angle2 = Vector2.Angle(B, A);
			// result is 90 degrees


			float signedAngle1 = Vector2.SignedAngle(A, B);
			// result is 90 degrees
			float signedAngle2 = Vector2.SignedAngle(B, A);
			// result is -90 degrees






			Vector3 dir = _startPos - _dragPos;
			float magnitude = dir.magnitude;
			if (magnitude == 0)
				return Vector3.zero;
			float distanceNormalized = Mathf.InverseLerp(0, maxDistance, dir.magnitude);
			Debug.Log(dir.magnitude + "  " + distanceNormalized);
			Vector3 velocity = distanceNormalized * velocityMultiplier * dir.normalized;
			return velocity;
		}
	}


	void OnMouseDown()
	{
		_startPlane = ObjectPlane;
		_startPos = IntersectPlane(MouseRay);

		_dragPos = _startPos;
	}

	void OnMouseDrag()
	{
		_dragPos = IntersectPlane(MouseRay);
	}

	Vector3 IntersectPlane(Ray ray)
	{
		if (_startPlane.Raycast(ray, out float enter))
			return ray.origin + ray.direction.normalized * enter;
		return Vector3.zero;
	}

	void OnMouseUp()
	{
		ApplyVelocity();
		_startPos = Vector3.zero;
		_dragPos = Vector3.zero;
	}

	bool Valid => Application.isPlaying && rigidBody != null;

	void Stop()
	{
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
	}

	void OnValidate()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void ApplyVelocity()
	{
		rigidBody
			.AddForceAtPosition(ThrowVector,
			PointOfActionWorld,
			ForceMode.VelocityChange);
	}
}
