using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private bool lockOnTarget;
    [SerializeField] private Bola _bola;
    
    void Update()
    {
        if (lockOnTarget)
        {
            transform.LookAt(target);
        }
    }

    public void CallBall()
    {
        _bola.Comenzar();
    }
}
