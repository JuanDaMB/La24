using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bola : MonoBehaviour
{
    public event Action OnCompleteMove;
    private Rigidbody _rigidbody;
    private Vector3 velocity;

    public int timeScale = 4;
    public float MaxTime, brakeSpeed, brakeTime;
    public AnimationCurve curve;

    [SerializeField] private Animator _animator;
    public List<BallData> Datas;

    private float time, t = 0f, timeScaleR;
    private bool playing;
    public RawData actualData;
    public string ballName;
    private int initialTimeScale;
    private string currentClip;
    private void Start()
    {
        playing = false;
        // _rigidbody = GetComponent<Rigidbody>();
        // _rigidbody.isKinematic = true;
        // velocity = _rigidbody.velocity;
        actualData = new RawData();
        initialTimeScale = timeScale;
    }

    public void SetNumber(int n)
    {
        ballName = Datas[n - 1].number;
        actualData = Datas[n - 1].GetNumber();
        currentClip = actualData.clip;
        _animator.enabled = false;
        transform.position = actualData.startPos;
        MaxTime = actualData.totalTime;
    }

    public void SetNumberExact(int n, int x)
    {
        ballName = Datas[n-1].number;
        actualData = Datas[n-1].GetNumberExact(x);
        transform.position = actualData.startPos;
        MaxTime = actualData.totalTime;
        EstablecerMovimiento();
    }

    public void EstablecerMovimiento()
    {
        time = 0f;
        t = 0f;
        timeScale = initialTimeScale;
        Time.timeScale = 1;
        // _rigidbody.velocity = Vector3.zero;
        // _rigidbody.drag = 0.005f;
        transform.eulerAngles = Vector3.zero;
    }

    public void Comenzar()
    {
        Time.timeScale = timeScale;
        _animator.enabled = true;
        _animator.Play(currentClip);
        
        // _rigidbody.isKinematic = false;
        // _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        playing = true;
    }
    private void TerminarMovimiento()
    {
        // _rigidbody.drag = 0.005f;
        // _rigidbody.velocity = Vector3.zero;
        // _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        // _rigidbody.isKinematic = true;
        playing = false;
        OnCompleteMove?.Invoke();
    }

    public void Accelerar()
    {
        timeScale = 20;
    }

    private void Update()
    {
        if (!playing)
        {
            return;
        }
        if (time >= MaxTime)
        {
            if (t <= brakeTime)
            {
                // _rigidbody.drag = curve.Evaluate(t / brakeTime) * (1) + 0.005f;
                timeScaleR = (1 - curve.Evaluate(t / brakeTime)) * (timeScale - 1) + 1;
                Time.timeScale = timeScaleR;
                t += Time.unscaledDeltaTime;
            }
            else
            {
                Time.timeScale = 1;
                TerminarMovimiento();
            }
        }
        else
        {
            Time.timeScale = timeScale;
        }
        time += Time.deltaTime;
    }
}

[Serializable]
public class BallData
{
    public string number;
    [SerializeField]
    private List<RawData> data;

    public RawData GetNumber()
    {
        int r = Random.Range(0, data.Count);
        data[r].clip = number + " " + r;
        return data[r];
    }
    
    public RawData GetNumberExact(int n)
    {
        return data[n];
    }

    public int GetArraySize()
    {
        return data.Count;
    }
}

[Serializable]
public class RawData
{
    public Vector3 startPos;
    public float totalTime;
    public string clip;
}