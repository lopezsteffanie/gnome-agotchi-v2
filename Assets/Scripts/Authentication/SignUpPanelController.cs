using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUpPanelController : MonoBehaviour
{
    public FirebaseAuthController firebaseAuthController;

    public Button backButton, signUpButton;
    public TMP_InputField signUpEmail, signUpPassword, signUpUsername;
    public GameObject loginPanel;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
        signUpButton.onClick.AddListener(OnSignUpButtonClick);
    }

    private void OnBackButtonClick()
    {
        firebaseAuthController.OpenPanel(loginPanel.name);
    }

    private void OnSignUpButtonClick()
    {
        firebaseAuthController.CreateUser(signUpEmail, signUpPassword, signUpUsername);
    }
}
