using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DenominationHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _transform;
    [SerializeField] private float minX, maxX;
    [SerializeField] private GameObject baseGO, optionsGO;
    [SerializeField] private Color baseColor, selectedColor;
    [SerializeField] private TextMeshProUGUI textBase;
    [SerializeField] private List<TextMeshProUGUI> textOptions;
    [SerializeField] private Button baseButton, returnButton;
    [SerializeField] private List<Button> options;

    public Action<int> currentDeno;
    private int index = 0;
    private float currentSpace = 0f;
    private bool isOpen = false, onBet;
    private void Awake()
    {
        currentSpace = minX;
        isOpen = false;
        baseButton.onClick.AddListener(OpenDropdown);
        returnButton.onClick.AddListener(CloseDropdown);
    }

    public void SetArray(List<int> newvalues)
    {
        foreach (Button option in options)
        {
            option.gameObject.SetActive(false);
        }
        for (int i = 0; i < newvalues.Count; i++)
        {
            textOptions[i].text = newvalues[i].ToString();
            options[i].gameObject.SetActive(true);
            int newIndex = i;
            int deno = newvalues[i];
            options[i].onClick.AddListener(()=>SetDefault(deno,newIndex));
        }
        optionsGO.SetActive(false);
        options[0].onClick?.Invoke();
    }

    public void OnBetState(bool betState)
    {
        onBet = betState;
        if (onBet && isOpen)
        {
            CloseDropdown();
        }
    }
    
    public void OpenDropdown()
    {
        if (onBet)
        {
            return;
        }
        StartCoroutine(OpencloseAnimation(true));
    }

    private void CloseDropdown()
    {
        SetDefault(GlobalObjects.Deno,index);
    }

    public void SetDefault(int value, int idx)
    {
        textOptions[index].color = baseColor;
        index = idx;
        textOptions[index].color = selectedColor;
        textBase.text = value.ToString();
        currentDeno?.Invoke(value);
        StartCoroutine(OpencloseAnimation(false));
    }
    
    
    private void SetLeft(float left)
    {
        _transform.offsetMin = new Vector2(left, _transform.offsetMin.y);
    }

    IEnumerator OpencloseAnimation(bool open)
    {
        isOpen = open;
        baseGO.SetActive(!open);
        optionsGO.SetActive(open);
        returnButton.gameObject.SetActive(open);

        baseButton.interactable = false;
        foreach (Button button in options)
        {
            button.interactable = false;
        }

        float t = 0f;
        float current = currentSpace;
        while (t <= 0.1f)
        {
            current = Mathf.Lerp(currentSpace, open ? maxX : minX, t / 0.1f);
            SetLeft(current);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        current = open ? maxX : minX;
        SetLeft(current);

        currentSpace = current;

        if (open)
        {
            foreach (Button button in options)
            {
                button.interactable = true;
            }
        }
        else
        {
            baseButton.interactable = true;
        }

        yield return null;
    }
}
