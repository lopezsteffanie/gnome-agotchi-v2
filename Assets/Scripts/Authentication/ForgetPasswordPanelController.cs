using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgetPasswordPanelController : MonoBehaviour
{
    public FirebaseAuthController firebaseAuthController;

    public Button backButton, submitButton;
    public GameObject loginPanel;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }

    private void OnBackButtonClick()
    {
        firebaseAuthController.OpenPanel(loginPanel.name);
    }

    private void OnSubmitButtonClick()
    {
        firebaseAuthController.ForgetPass();
    }
}
