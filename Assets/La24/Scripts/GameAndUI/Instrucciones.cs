using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrucciones : MonoBehaviour
{
    public void EnableInstructions()
    {
        if (GlobalObjects.State == GameState.Playing)
        {
            return;
        }
        GlobalObjects.State = GameState.Instructions;
        gameObject.SetActive(true);
    }

    public void DisableInstructions()
    {
        gameObject.SetActive(false);
    }
}
