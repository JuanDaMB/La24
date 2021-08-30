using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public event Action<string> OnCompleteMove;

    public ForceMode forceMode = ForceMode.Force;
    public Force_Direction direction = Force_Direction.None;

    [SerializeField]
    private bool logBallMagnitude;

    [SerializeField]
    private List<AudioClip> soundClips;

    public bool initForce = false;

    private Vector3 directionVector;
    private Rigidbody rb;

    private float forceMultiplier;

    private bool isActive;
    private bool isOnNumber;

    public Rigidbody Body
    {
        get { return rb; }
    }

    private float BallSpeed
    {
        get { return rb.velocity.magnitude; }
    }

    public bool IsSearchingNumber
    {
        get;
        private set;
    }

    [SerializeField]
    private ForceMode searchForceMode;

    [SerializeField]
    [Range(0, 10f)]
    private float ballForceMuptiplier = 3;

    private float timeInNumberSelected;

    [SerializeField]
    [Range(0, 4)]
    private float timeWithoutTouchPoint = 2.5f;

    [SerializeField]
    [Range(0, 0.5f)]
    private float minMagnitudeToForce = 0.1f;

    [SerializeField]
    [Range(0, 0.5f)]
    private float maxMagnitudeToForce = 0.1f;

    private List<Transform> numbersPath;

    private AudioSource audioSource;

    public bool FoundNumber
    {
        get;
        private set;
    }

    public int GNANumberValue
    {
        get;
        set;
    }

    public Number GnaNumber
    {
        get;
        private set;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        isActive = false;
        isOnNumber = false;
    }

    public void TouchAPoint(Vector3 force, ForceMode mode)
    {
        if (!isActive || BallSpeed > maxMagnitudeToForce || BallSpeed < minMagnitudeToForce)
        {
            if (Application.isEditor && logBallMagnitude)
            {
                string message = BallSpeed > maxMagnitudeToForce ? "Not apply force in major: " : "Not apply force in minior: ";
                Debug.LogWarning(message + BallSpeed);
            }
            return;
        }

        if (IsInvoking("RandomForce"))
            CancelInvoke("RandomForce");
        Invoke("RandomForce", timeWithoutTouchPoint);

        rb.AddForce(force * rb.velocity.magnitude, mode);
    }

    private IEnumerator PLayBallSound()
    {
        while (isActive)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = soundClips[UnityEngine.Random.Range(0, soundClips.Count)];
                audioSource.Play();
            }
            yield return 0;
        }
    }

    private void RandomForce()
    {
        Vector3 randForce = rb.velocity + (transform.right * UnityEngine.Random.Range(3, -3));
        TouchAPoint(randForce, ForceMode.Force);
    }

    public void StartBall(List<Number> numbersTable)
    {
        IsSearchingNumber = false;
        FoundNumber = false;

        GnaNumber = numbersTable[UnityEngine.Random.Range(0, numbersTable.Count)];
        foreach (Number n in numbersTable)
        {
            if (n.id == GNANumberValue.ToString())
            {
                GnaNumber = n;
                Debug.Log("Found number: " + n.id);
                break;
            }
        }
        Debug.Log("GNA number is: " + GnaNumber.name);

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.WakeUp();

        SelectForce();

        Vector3 force = directionVector * forceMultiplier;
        rb.AddForce(force, forceMode);

        Invoke("ActiveMove", 0.1f);
    }

    private void ActiveMove()
    {
        isActive = true;

        StartCoroutine(PLayBallSound());
        Invoke("SearchGNANumber", 8f);
    }

    private void SearchGNANumber()
    {
        IsSearchingNumber = true;
        Debug.Log("Time To Search Number");

        TryMove();
    }

    public void TryMove()
    {
        if (FoundNumber)
            return;

        LeanTween.cancel(gameObject);
        MoveToPoint();
    }

    private void MoveToPoint()
    {
        if (BallSpeed < maxMagnitudeToForce)
        {
            transform.LookAt(GnaNumber.transform);

            Vector3 direction = GnaNumber.transform.position - transform.position;
            Vector3 force = direction.normalized * (ballForceMuptiplier);

            Body.AddForceAtPosition(force, GnaNumber.transform.position, searchForceMode);

            LeanTween.delayedCall(gameObject, 3, TryMove);
        }
        else
            LeanTween.delayedCall(gameObject, 0.25f, TryMove);
    }

    private void SelectForce()
    {
        forceMultiplier = 1000;
        switch (direction)
        {
            case Force_Direction.Forward:
                directionVector = transform.forward;
                break;

            case Force_Direction.Forward_Left:
                directionVector = transform.forward - transform.right;
                break;

            case Force_Direction.Forward_Right:
                directionVector = transform.forward + transform.right;
                break;

            case Force_Direction.Backward:
                directionVector = -transform.forward;
                break;

            case Force_Direction.Backward_Left:
                directionVector = -transform.forward - transform.right;
                break;

            case Force_Direction.Backward_Right:
                directionVector = -transform.forward + transform.right;
                break;

            case Force_Direction.Left:
                directionVector = -transform.right;
                break;

            case Force_Direction.Right:
                directionVector = transform.right;
                break;
        }
    }

    public void ActiveAnimationBallNumberSelected()
    {
        FoundNumber = true;
        rb.isKinematic = true;

        Vector3 toPos = GnaNumber.transform.position;
        toPos.y = transform.position.y;

        LeanTween.move(gameObject, toPos, 1).setOnComplete(CompleteFinalAnimation).setEase(LeanTweenType.easeInOutBack);
    }

    private void CompleteFinalAnimation()
    {
        isActive = false;

        if (OnCompleteMove != null)
            OnCompleteMove(GnaNumber.name);
    }

    public void ActiveBall()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Number")
        {
            if (!isOnNumber)
                LeanTween.delayedCall(gameObject, 1f, TryOutOfNumber);

            isOnNumber = true;
            collision.GetComponent<Number>().ActiveHighLight();
        }
    }

    private void TryOutOfNumber()
    {
        if (BallSpeed > maxMagnitudeToForce)
            return;

        Vector3 direction = GnaNumber.transform.position - transform.position;
        Vector3 force = direction.normalized * (ballForceMuptiplier * 2f);

        Body.AddForceAtPosition(force, GnaNumber.transform.position, searchForceMode);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Number" && isActive)
        {
            if (isOnNumber && !FoundNumber)
                LeanTween.cancel(gameObject);

            isOnNumber = false;
            collision.GetComponent<Number>().Default();
        }
    }

    private void LogBallMagnitude()
    {
        if (Application.isEditor && logBallMagnitude)
            Debug.Log("Ball speed magnitude: " + BallSpeed);
    }
}