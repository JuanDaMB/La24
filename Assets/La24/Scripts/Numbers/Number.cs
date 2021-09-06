using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Number : MonoBehaviour
{
    public string id;

    public Material normalMaterial;
    public Material highlighMaterial;

    public List<Number> Links;

    private BallController ball;

    private MeshRenderer _mRenderer;

    private MeshRenderer mRenderer
    {
        get
        {
            if (_mRenderer == null)
                _mRenderer = GetComponent<MeshRenderer>();

            return _mRenderer;
        }
    }

    private void Awake()
    {
        transform.tag = "Number";
    }

    public void Default()
    {
        mRenderer.material = normalMaterial;
    }

    public void ActiveHighLight()
    {
        mRenderer.material = highlighMaterial;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (ball == null)
            ball = collider.GetComponent<BallController>();

        if (ball.FoundNumber)
            return;

        PlayNumberSound();

        //TODO xD
        // if (!ball.IsSearchingNumber)
        //     // ball.TouchAPoint(Vector3.up * GameplayManager.Instance.ballImpulse, ForceMode.Force);
        // else
        // {
        //     if (transform != ball.GnaNumber.transform)
        //         ball.TryMove();
        //     else
        //         ball.ActiveAnimationBallNumberSelected();
        // }
    }

    private void PlayNumberSound()
    {
        int rand = Random.Range(1, 9);
        PlaySound.audios.PlayFX("Numero_" + rand, 0.5f);
    }

    public void SetNumber(string numberName)
    {
        gameObject.name = numberName;

        int idAsInt = int.Parse(numberName.Replace("A", string.Empty).Replace("B", string.Empty).Replace("C", string.Empty));

        if (gameObject.name.Contains("A"))
            idAsInt += 24;

        id = idAsInt.ToString();

        GameObject numberObject = Resources.Load("Numbers/" + gameObject.name) as GameObject;
        MeshFilter numberMeshFilter = numberObject.GetComponent<MeshFilter>();

        Mesh numberMesh = numberMeshFilter.sharedMesh;
        GetComponent<MeshFilter>().mesh = numberMesh;
    }
}