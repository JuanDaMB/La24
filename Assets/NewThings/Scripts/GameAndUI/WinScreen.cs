using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{

    [SerializeField]
    private Sprite iconWin;

    [SerializeField]
    private Sprite iconLose;

    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI total;

    [SerializeField] private Animator animator;

    private float _ganancia;

    public void SetWinScreen(float gain)
    {
        _ganancia = gain;
    }

    public void ShowScreen()
    {
        gameObject.SetActive(true);
        animator.Play(_ganancia > 0 ? "Win" : "Lose");
        image.sprite = _ganancia > 0 ? iconWin : iconLose;
        total.text = "$ " + _ganancia.ToString("N0");
    }
}
