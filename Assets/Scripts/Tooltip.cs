using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;

    [SerializeField] private Camera uiCamera;

    private Text _tooltipText;
    private RectTransform _backgroundRectTransform;

    private void Awake()
    {
        instance = this;
        _backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        _tooltipText = transform.Find("text").GetComponent<Text>();

        ShowTooltip("test tooltip1111111111111");
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }

    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        _tooltipText.text = tooltipString;
        float textPaddingSize = 5f;
        Vector2 backgroundSize = new Vector2(_tooltipText.preferredWidth + textPaddingSize * 2, _tooltipText.preferredHeight + textPaddingSize * 2);
        _backgroundRectTransform.sizeDelta = backgroundSize;
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }
    private static void ShowTooltip_Static(string tooltipString)
    {
        instance.ShowTooltip(tooltipString);
    }
    private static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
