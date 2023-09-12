using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationMessagePanelController : MonoBehaviour
{
    public Button closeButton;
    public GameObject notifMessagePanel;

    private void Start()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnCloseButtonClick()
    {
        notifMessagePanel.SetActive(false);
    }
}
