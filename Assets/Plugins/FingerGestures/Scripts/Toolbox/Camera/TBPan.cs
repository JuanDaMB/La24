using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Camera/Pan")]
[RequireComponent(typeof(DragRecognizer))]
public class TBPan : MonoBehaviour
{
	private Transform cachedTransform;

	public float sensitivity = 1.0f;
	public float smoothSpeed = 10;
	public BoxCollider moveArea;    // the area to constrain camera movement to

	private Vector3 idealPos;
	private DragGesture dragGesture;

	public delegate void PanEventHandler(TBPan source, Vector3 move);

	public event PanEventHandler OnPan;

	private void Awake()
	{
		cachedTransform = this.transform;
	}

	private void Start()
	{
		idealPos = cachedTransform.position;

		// sanity check
		if (!GetComponent<DragRecognizer>())
		{
			Debug.LogWarning("No drag recognizer found on " + this.name + ". Disabling TBPan.");
			enabled = false;
		}
	}

	private void OnDrag(DragGesture gesture)
	{
		dragGesture = (gesture.State == GestureRecognitionState.Ended) ? null : gesture;
	}

	private void Update()
	{
		if (dragGesture != null)
		{
			if (dragGesture.DeltaMove.SqrMagnitude() > 0)
			{
				Vector2 screenSpaceMove = sensitivity * dragGesture.DeltaMove;
				Vector3 worldSpaceMove = screenSpaceMove.x * cachedTransform.right + screenSpaceMove.y * cachedTransform.up;
				idealPos -= worldSpaceMove;

				if (OnPan != null)
					OnPan(this, worldSpaceMove);
			}
		}

		idealPos = ConstrainToMoveArea(idealPos);

		if (smoothSpeed > 0)
			cachedTransform.position = Vector3.Lerp(cachedTransform.position, idealPos, Time.deltaTime * smoothSpeed);
		else
			cachedTransform.position = idealPos;
	}

	// project point on panning plane
	public Vector3 ConstrainToPanningPlane(Vector3 p)
	{
		Vector3 lp = cachedTransform.InverseTransformPoint(p);
		lp.z = 0;
		return cachedTransform.TransformPoint(lp);
	}

	public void TeleportTo(Vector3 worldPos)
	{
		cachedTransform.position = idealPos = ConstrainToPanningPlane(worldPos);
	}

	public void FlyTo(Vector3 worldPos)
	{
		idealPos = ConstrainToPanningPlane(worldPos);
	}

	public Vector3 ConstrainToMoveArea(Vector3 p)
	{
		if (moveArea)
		{
			Vector3 min = moveArea.bounds.min;
			Vector3 max = moveArea.bounds.max;

			p.x = Mathf.Clamp(p.x, min.x, max.x);
			p.y = Mathf.Clamp(p.y, min.y, max.y);
			p.z = Mathf.Clamp(p.z, min.z, max.z);
		}

		return p;
	}
}