using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetBoardButton : MonoBehaviour
{
    private Button _image;
    private TextMeshProUGUI _text;
    [SerializeField] private BetType _type;
    [SerializeField] private string value;
    private int _index;
    [SerializeField] private Button _button;
    [SerializeField] private List<BetBoardButton> connections;
    public string id;
    public int maxBet;
    public bool _hasBet;

    private Color _blank, _color;

    public void SetMaxBet()
    {
        switch (id)
        {
            case "Pleno":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Pleno;
                break;
            case "Medio":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Medio;
                break;
            case "Cuadrado":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Cuadrado;
                break;
            case "Octeto":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Octeto;
                break;
            case "Par_Impar":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Par_Impar;
                break;
            case "Altos_Bajos":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Altos_Bajos;
                break;
            case "Rojo_Negro":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Rojo_Negro;
                break;
            case "Fila":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Fila;
                break;
            case "Linea":
                maxBet = GlobalObjects.PrizesData.maxBetTypes.Linea;
                break;
        }
    }

    public void Initialize(Color blank, Color color, Button image, int idx, Action<int, string, BetType, int> onClick)
    {
        _image = image;
        _image.gameObject.transform.position = transform.position;
        _text = _image.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _blank = blank;
        _color = color;
        _image.image.color = blank;
        _text.color = blank;
        _image.gameObject.SetActive(false);
        _index = idx;
        _image.onClick.AddListener(_button.onClick.Invoke);
        _button.onClick.AddListener(() => onClick(_index, value, _type, maxBet));
    }
    
    public void UnblockConnections()
    {
        _hasBet = false;
        foreach (BetBoardButton connection in connections)
        {
            connection.ShouldBeBlocked();
        }
    }
    public void HasBet()
    {
        _hasBet = true;
        foreach (BetBoardButton connection in connections)
        {
            connection.ShouldBeBlocked();
        }
    }

    private void ShouldBeBlocked()
    {
        _button.interactable = !connections.TrueForAll(c => c._hasBet);
        _image.interactable = !connections.TrueForAll(c => c._hasBet);
    }

    public void ClearData()
    {
        _image.gameObject.SetActive(false);   
        _image.image.color = _blank;
        _text.color = _blank;
        _text.text = "";
    }
    
    public void SetImage(Sprite sprite, bool visible)
    {
        _image.gameObject.SetActive(true);
        _image.image.color = visible ? _color : _blank;
        _text.color = visible ? _color : _blank;
        _image.image.sprite = sprite;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}