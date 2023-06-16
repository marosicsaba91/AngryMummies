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

	Plane ObjectPlane => new Plane(transform.position, Vector3.forward);

	Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);
	Vector3 PointOfActionLocal => rigidBody.centerOfMass + pointOfAction;
	Vector3 PointOfActionWorld => rigidBody.transform.TransformPoint(PointOfActionLocal);

	Vector3 ThrowVector
	{
		get
		{
			Vector3 dir = _startPos - _dragPos;
			float magnitude = dir.magnitude;
			if (magnitude == 0)
				return Vector3.zero;
			float distanceNormalized = Mathf.InverseLerp(0, maxDistance, dir.magnitude);
			Debug.Log(dir.magnitude + "  " + distanceNormalized);
			Vector3 velocity = dir.normalized * velocityMultiplier * distanceNormalized;
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
