using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginPanelController : MonoBehaviour
{
    public FirebaseAuthController firebaseAuthController;
    
    public Button loginButton, registerAccountButton, forgotPasswordButton;
    public TMP_InputField loginEmail, loginPassword;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
        registerAccountButton.onClick.AddListener(OnRegisterAccountButtonClick);
        forgotPasswordButton.onClick.AddListener(OnForgotPasswordButtonClick);
    }

    private void OnLoginButtonClick()
    {
        firebaseAuthController.SignInUser(loginEmail, loginPassword, () =>
        {
            Debug.Log("Login successful!");
        });
    }

    private void OnRegisterAccountButtonClick()
    {
        firebaseAuthController.OpenPanel("signupPanel");
    }

    private void OnForgotPasswordButtonClick()
    {
        firebaseAuthController.OpenPanel("forgetPassPanel");
    }
}
