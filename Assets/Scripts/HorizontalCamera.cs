using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class HorizontalCamera : MonoBehaviour
{
	private Camera _mCamera;
	private float _lastAspect;

#pragma warning disable 0649
	[FormerlySerializedAs("m_fieldOfView")]
	[SerializeField]
	private float mFieldOfView = 60f;
	public float FieldOfView
	{
		get { return mFieldOfView; }
		set
		{
			if (mFieldOfView != value)
			{
				mFieldOfView = value;
				RefreshCamera();
			}
		}
	}

	[FormerlySerializedAs("m_orthographicSize")]
	[SerializeField]
	private float mOrthographicSize = 5f;
	public float OrthographicSize
	{
		get { return mOrthographicSize; }
		set
		{
			if (mOrthographicSize != value)
			{
				mOrthographicSize = value;
				RefreshCamera();
			}
		}
	}
#pragma warning restore 0649

	private void OnEnable()
	{
		RefreshCamera();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.update -= Update;
		UnityEditor.EditorApplication.update += Update;
#endif
	}

	private void Update()
	{
		float aspect = _mCamera.aspect;
		if (aspect != _lastAspect)
			AdjustCamera(aspect);
	}

	public void RefreshCamera()
	{
		if (!_mCamera)
			_mCamera = GetComponent<Camera>();

		AdjustCamera(_mCamera.aspect);
	}

	private void AdjustCamera(float aspect)
	{
		_lastAspect = aspect;

		// Credit: https://forum.unity.com/threads/how-to-calculate-horizontal-field-of-view.16114/#post-2961964
		float _1OverAspect = 1f / aspect;
		_mCamera.fieldOfView = 2f * Mathf.Atan(Mathf.Tan(mFieldOfView * Mathf.Deg2Rad * 0.5f) * _1OverAspect) * Mathf.Rad2Deg;
		_mCamera.orthographicSize = mOrthographicSize * _1OverAspect;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		RefreshCamera();
	}

	private void OnDisable()
	{
		UnityEditor.EditorApplication.update -= Update;
	}
#endif
}