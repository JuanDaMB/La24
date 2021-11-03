using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetBoardButton : MonoBehaviour
{
    private Image _image;
    private TextMeshProUGUI _text;
    [SerializeField] private BetType _type;
    [SerializeField] private string value;
    private int index;
    [SerializeField] private Button _button;

    private Color _blank, _color;
    public void Initialize(Color blank, Color color,Image image,int idx, Action<int, string, BetType> onClick)
    {
        _image = image;
        _image.gameObject.transform.position = transform.position;
        _text = _image.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _blank = blank;
        _color = color;
        _image.color = blank;
        _text.color = blank;
        _image.gameObject.SetActive(false);
        index = idx;
        _button.onClick.AddListener(() => onClick(index, value, _type));
    }

    public void ClearData()
    {
        _image.gameObject.SetActive(false);   
        _image.color = _blank;
        _text.color = _blank;
        _text.text = "";
    }
    
    public void SetImage(Sprite sprite, bool visible)
    {
        _image.gameObject.SetActive(true);
        _image.color = visible ? _color : _blank;
        _text.color = visible ? _color : _blank;
        _image.sprite = sprite;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}