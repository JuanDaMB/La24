using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private static float DELAY_ROTATION_ANIMATION = 1;

	//Here is a private reference only this class can access
	private static CameraController _instance;

	//This is the public reference that other classes will use
	public static CameraController instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<CameraController>();
			return _instance;
		}
	}

	public enum PanMode
	{
		Disabled,
		OneFinger,
		TwoFingers
	}

	/// <summary>
	/// El objeto que la camara orbitara
	/// </summary>
	public Transform target;

	/// <summary>
	/// Distancia inicial de la camara al objeto
	/// </summary>
	public float initialCameraDistance = 150;

	/// <summary>
	/// Distancia minima entre la camara y el objeto
	/// </summary>
	public float minCameraDistance = 100;

	/// <summary>
	/// Distancia maxima entre la camara y el objeto
	/// </summary>
	public float maxCameraDistance = 200;

	/// <summary>
	/// Afecta la velocidad de rotacion horizontal (grados por centimetro)
	/// </summary>
	public float horizontalRotationSensitivity = 45.0f;

	/// <summary>
	/// Afecta la velocidad de rotacion vertical (grados por centimetro)
	/// </summary>
	public float verticalRotationSensitivity = 45.0f;

	/// <summary>
	/// Limitar Rotacion Horizontal
	/// </summary>
	public bool clampHorizontalRotationAngle = false;

	public float minHorizontalRotation = -75;
	public float maxHorizontalRotation = 75;

	/// <summary>
	/// Limitar Rotacion Vertical
	/// </summary>
	public bool clampVerticalRotationAngle = false;

	public float minVerticalRotation = -75;
	public float maxVerticalRotation = 75;

	/// <summary>
	/// Permitir Pitch Zoom
	/// </summary>
	public bool allowPinchZoom = true;

	/// <summary>
	/// Afecta la velocidad del Pitch Zoom
	/// </summary>
	public float pinchZoomSensitivity = 5.0f;

	/// <summary>
	/// Configuracion Pitch Zoom
	/// </summary>
	public bool smoothMotion = true;

	public float smoothZoomSpeed = 5.0f;
	public float smoothOrbitSpeed = 10.0f;

	/// <summary>
	/// Two-Finger camera panning.
	/// Panning will apply an offset to the pivot/camera target point
	/// </summary>
	public bool allowPanning = false;

	public bool invertPanningDirections = false;
	public float panningSensitivity = 1.0f;
	public Transform panningPlane;  // reference transform used to apply the panning translation (using panningPlane.right and panningPlane.up vectors)
	public bool smoothPanning = true;
	public float smoothPanningSpeed = 12.0f;

	// collision test
	public LayerMask collisionLayerMask;

	private float distance = 10.0f;
	private float rotationHorizontal = 0;
	private float rotationVertical = 0;

	private float idealDistance = 0;
	private float idealRotationHorizontal = 0;
	private float idealRotationVertical = 0;

	private Vector3 idealPanOffset = Vector3.zero;
	private Vector3 panOffset = Vector3.zero;

	private float initDistance;
	private float initRotationHorizontal;
	private float initRotationVertical;

	private PinchRecognizer pinchRecognizer;

	public float Distance
	{
		get { return distance; }
	}

	public float IdealDistance
	{
		get { return idealDistance; }
		set { idealDistance = Mathf.Clamp(value, minCameraDistance, maxCameraDistance); }
	}

	public float RotationHorizontal
	{
		get { return rotationHorizontal; }
	}

	public float IdealRotationHorizontal
	{
		get { return idealRotationHorizontal; }
		set { idealRotationHorizontal = clampHorizontalRotationAngle ? ClampAngle(value, minHorizontalRotation, maxHorizontalRotation) : value; }
	}

	public float RotationVertical
	{
		get { return rotationVertical; }
	}

	public float IdealRotationVertical
	{
		get { return idealRotationVertical; }
		set { idealRotationVertical = clampVerticalRotationAngle ? ClampAngle(value, minVerticalRotation, maxVerticalRotation) : value; }
	}

	public Vector3 IdealPanOffset
	{
		get { return idealPanOffset; }
		set { idealPanOffset = value; }
	}

	public Vector3 PanOffset
	{
		get { return panOffset; }
	}

	private void InstallGestureRecognizers()
	{
		List<GestureRecognizer> recogniers = new List<GestureRecognizer>(GetComponents<GestureRecognizer>());
		DragRecognizer drag = recogniers.Find(r => r.EventMessageName == "OnDrag") as DragRecognizer;
		DragRecognizer twoFingerDrag = recogniers.Find(r => r.EventMessageName == "OnTwoFingerDrag") as DragRecognizer;
		PinchRecognizer pinch = recogniers.Find(r => r.EventMessageName == "OnPinch") as PinchRecognizer;

		// check if we need to automatically add a screenraycaster
		if (OnlyRotateWhenDragStartsOnObject)
		{
			ScreenRaycaster raycaster = gameObject.GetComponent<ScreenRaycaster>();

			if (!raycaster)
				raycaster = gameObject.AddComponent<ScreenRaycaster>();
		}

		if (!drag)
		{
			drag = gameObject.AddComponent<DragRecognizer>();
			drag.RequiredFingerCount = 1;
			drag.IsExclusive = true;
			drag.MaxSimultaneousGestures = 1;
			drag.SendMessageToSelection = GestureRecognizer.SelectionType.None;
		}

		if (!pinch)
			pinch = gameObject.AddComponent<PinchRecognizer>();

		if (!twoFingerDrag)
		{
			twoFingerDrag = gameObject.AddComponent<DragRecognizer>();
			twoFingerDrag.RequiredFingerCount = 2;
			twoFingerDrag.IsExclusive = true;
			twoFingerDrag.MaxSimultaneousGestures = 1;
			twoFingerDrag.ApplySameDirectionConstraint = true;
			twoFingerDrag.EventMessageName = "OnTwoFingerDrag";
		}
	}

	private void Start()
	{
		InstallGestureRecognizers();

		if (!panningPlane)
			panningPlane = this.transform;

		Vector3 angles = transform.eulerAngles;

		initDistance = IdealDistance = initialCameraDistance;
		initRotationHorizontal = IdealRotationHorizontal = angles.y;
		initRotationVertical = IdealRotationVertical = angles.x;

		distance = IdealDistance = initialCameraDistance;
		rotationHorizontal = IdealRotationHorizontal = angles.y;
		rotationVertical = IdealRotationVertical = angles.x;

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

		Apply();
	}

	public void DefaultCamera()
	{
		IdealDistance = initDistance;
		IdealRotationHorizontal = initRotationHorizontal;
		IdealRotationVertical = initRotationVertical;
		Apply();
	}

	public void ChangeCameraTarget(GameObject _target)
	{
		target = _target.transform;
		DefaultCamera();
	}

	public void MoveCamera(float moveX, float moveY)
	{
		if (target)
		{
			IdealRotationHorizontal = moveY;
			IdealRotationVertical = moveX;
		}

		StartCoroutine(RotationAnimation());
	}

	#region Gesture Event Messages

	private float nextDragTime = 0;

	public bool OnlyRotateWhenDragStartsOnObject = false;

	private void OnDrag(DragGesture gesture)
	{
		//Debug.Log ("Drag: " + Constants.isDragAction + " Text: " + LevelText.isInitPresentationActive + " WIN: " + WinMenuBehaviour.isWinMenuActive + " Tutorial: " + TutorialBaseMenuBehaviour.isTutorialActive);

		// don't rotate unless the drag started on our target object
		if (OnlyRotateWhenDragStartsOnObject)
		{
			if (gesture.Phase == ContinuousGesturePhase.Started)
			{
				if (!gesture.Recognizer.Raycaster)
				{
					Debug.LogWarning("The drag recognizer on " + gesture.Recognizer.name + " has no ScreenRaycaster component set. This will prevent OnlyRotateWhenDragStartsOnObject flag from working.");
					OnlyRotateWhenDragStartsOnObject = false;
					return;
				}

				if (target && !target.GetComponent<Collider>())
				{
					Debug.LogWarning("The target object has no collider set. OnlyRotateWhenDragStartsOnObject won't work.");
					OnlyRotateWhenDragStartsOnObject = false;
					return;
				}
			}

			if (!target || gesture.StartSelection != target.gameObject)
				return;
		}

		// wait for drag cooldown timer to wear off
		//  used to avoid dragging right after a pinch or pan, when lifting off one finger but the other one is still on screen
		if (Time.time < nextDragTime)
			return;

		if (target)
		{
			IdealRotationHorizontal += gesture.DeltaMove.x.Centimeters() * horizontalRotationSensitivity;
			IdealRotationVertical -= gesture.DeltaMove.y.Centimeters() * verticalRotationSensitivity;
		}
		Apply();
	}

	private IEnumerator RotationAnimation()
	{
		float timerLookAnimation = 0;
		while (timerLookAnimation < DELAY_ROTATION_ANIMATION)
		{
			Apply();
			timerLookAnimation += Time.deltaTime;
			yield return null;
		}
		OnCompleteRotation();
	}

	private void OnCompleteRotation()
	{
		Debug.Log("OnCompleteRotation");
	}

	private void OnPinch(PinchGesture gesture)
	{
		if (allowPinchZoom)
		{
			IdealDistance -= gesture.Delta.Centimeters() * pinchZoomSensitivity;
			//Debug.Log("IdealDistance: "+IdealDistance);
			nextDragTime = Time.time + 0.25f;

			Apply();
		}
	}

	private void OnTwoFingerDrag(DragGesture gesture)
	{
		if (allowPanning)
		{
			Vector3 move = -panningSensitivity * (panningPlane.right * gesture.DeltaMove.x.Centimeters() + panningPlane.up * gesture.DeltaMove.y.Centimeters());

			if (invertPanningDirections)
				IdealPanOffset -= move;
			else
				IdealPanOffset += move;

			nextDragTime = Time.time + 0.25f;

			Apply();
		}
	}

	#endregion Gesture Event Messages

	private void Apply()
	{
		//Debug.Log ("Apply");
		if (smoothMotion)
		{
			distance = Mathf.Lerp(distance, IdealDistance, Time.deltaTime * smoothZoomSpeed);
			rotationHorizontal = Mathf.Lerp(rotationHorizontal, IdealRotationHorizontal, Time.deltaTime * smoothOrbitSpeed);
			rotationVertical = Mathf.LerpAngle(rotationVertical, IdealRotationVertical, Time.deltaTime * smoothOrbitSpeed);
		}
		else
		{
			distance = IdealDistance;
			rotationHorizontal = IdealRotationHorizontal;
			rotationVertical = IdealRotationVertical;
		}

		if (smoothPanning)
			panOffset = Vector3.Lerp(panOffset, idealPanOffset, Time.deltaTime * smoothPanningSpeed);
		else
			panOffset = idealPanOffset;

		transform.rotation = Quaternion.Euler(rotationVertical, rotationHorizontal, 0);

		Vector3 lookAtPos = (target.position + panOffset);
		Vector3 desiredPos = lookAtPos - distance * transform.forward;

		if (collisionLayerMask != 0)
		{
			Vector3 dir = desiredPos - lookAtPos; // from target to camera
			float dist = dir.magnitude;
			dir.Normalize();

			RaycastHit hit;
			if (Physics.Raycast(lookAtPos, dir, out hit, dist, collisionLayerMask))
			{
				desiredPos = hit.point - dir * 0.1f;
				distance = hit.distance;
			}
		}

		transform.position = desiredPos;
	}

	private void LateUpdate()
	{
		Apply();
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;

		if (angle > 360)
			angle -= 360;

		return Mathf.Clamp(angle, min, max);
	}

	// recenter the camera
	public void ResetPanning()
	{
		IdealPanOffset = Vector3.zero;
	}
}