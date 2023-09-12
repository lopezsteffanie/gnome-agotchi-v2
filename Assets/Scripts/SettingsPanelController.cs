using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public FirebaseAuthController firebaseAuthController;
    public Button logoutButton, closeButton, settingsButton;
    public GameObject settingsPanel, settingsBtn;

    private void Start()
    {
        logoutButton.onClick.AddListener(OnLogoutButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    private void OnLogoutButtonClick()
    {
        firebaseAuthController.LogOut();
        settingsPanel.SetActive(false);
    }

    private void OnCloseButtonClick()
    {
        settingsPanel.SetActive(false);
        settingsBtn.SetActive(true);
    }

    private void OnSettingsButtonClick()
    {
        settingsPanel.SetActive(true);
        settingsBtn.SetActive(false);
    }
}
