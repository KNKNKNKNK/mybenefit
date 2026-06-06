using System;
using LogData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogMgr : MonoBehaviour
{
    public static LogMgr Instance { get; private set; }

    [SerializeField]
    ScrollRect scrollRect;

    [SerializeField]
    TextMeshProUGUI logText;

    [SerializeField]
    RectTransform logContent;

    float contentPaddingVertical = 100f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (scrollRect == null)
            scrollRect = GetComponentInChildren<ScrollRect>();

        if (logContent == null && logText != null)
            logContent = logText.rectTransform.parent as RectTransform;

        if (logContent != null)
        {
            var layout = logContent.GetComponent<VerticalLayoutGroup>();
            if (layout != null)
                contentPaddingVertical = layout.padding.vertical;
        }

        if (logText != null)
            logText.text = string.Empty;
    }

    public void AddLog(LogEventType eventType, string detail = "")
    {
        if (eventType == LogEventType.None || logText == null)
            return;

        var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {LogMessages.Format(eventType, detail)}";
        logText.text = string.IsNullOrEmpty(logText.text)
            ? message
            : message + "\n" + logText.text;

        UpdateLogContentSize();
        ScrollToTop();
    }

    public void AddLog(LogEventType eventType, int amount)
    {
        string formattedAmount = amount >= 1000
            ? amount.ToString("#,##0")
            : amount.ToString();

        AddLog(eventType, formattedAmount);
    }

    public void AddInactiveBlockedLog(LogEventType blockedEventType, string detail = "")
    {
        if (logText == null)
            return;

        var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {LogMessages.FormatInactiveBlocked(blockedEventType, detail)}";
        logText.text = string.IsNullOrEmpty(logText.text)
            ? message
            : message + "\n" + logText.text;

        UpdateLogContentSize();
        ScrollToTop();
    }

    public void AddInactiveBlockedLog(LogEventType blockedEventType, int amount)
    {
        string formattedAmount = amount >= 1000
            ? amount.ToString("#,##0")
            : amount.ToString();

        AddInactiveBlockedLog(blockedEventType, formattedAmount);
    }

    void UpdateLogContentSize()
    {
        if (logText == null || logContent == null)
            return;

        logText.ForceMeshUpdate();

        float textHeight = logText.preferredHeight;
        logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.sizeDelta.x, textHeight);
        logContent.sizeDelta = new Vector2(logContent.sizeDelta.x, textHeight + contentPaddingVertical);

        LayoutRebuilder.ForceRebuildLayoutImmediate(logContent);
    }

    void ScrollToTop()
    {
        if (scrollRect == null)
            return;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
