using System.Collections;
using System.Collections.Generic;
using NewThings.Scripts.GameAndUI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;
    private CoinData _coinData;
    private int _index;
    public void Clicked()
    {
        _image.sprite = _coinData.spriteMarcado;
    }

    public void DeClicked()
    {
        _image.sprite = _coinData.sprite;
    }

    public void SetData(CoinData data, int index)
    {
        _coinData = data;
        _index = index;
        ChangeText();
        _image.sprite = data.sprite;
    }

    public void Suscribe(UnityAction<int,int> coinChanged)
    {
        _button.onClick.AddListener(()=>coinChanged(_coinData.Value, _index));
    }

    public void ChangeText()
    {
        _text.text = (_coinData.Value).ToString();
    }
}
