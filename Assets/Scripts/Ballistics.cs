using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Ballistics : MonoBehaviour
{
	[SerializeField] LineRenderer lineRenderer;

	[Space]

	[SerializeField] Vector3 gravity = new (0, -9.81f, 0);
	[SerializeField] float speed = 5f;
	[SerializeField] float drag = 0.1f;
	[SerializeField] float timeStep = 0.1f;
	[SerializeField] float maxTime = 2f;

	void Update()
	{
		if (lineRenderer == null)
			return;

		Transform t = transform;
		List<Vector3> calculatedPoints = GetBallisticPath(t.position, t.forward * speed, gravity, drag, timeStep, maxTime);
		lineRenderer.positionCount = calculatedPoints.Count;
		lineRenderer.SetPositions(calculatedPoints.ToArray());
	}

	List<Vector3> GetBallisticPath(Vector3 position, Vector3 velocity, Vector3 gravity, float drag, float timeStep, float maxTime)
	{
		List<Vector3> points = new() { position };

		float time = 0f;
		while (time < maxTime)
		{
			time += timeStep;
			velocity += gravity * timeStep - velocity * (drag * timeStep);
			position += velocity * timeStep;
			points.Add(position);
		}

		return points;
	}
}
