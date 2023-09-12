using System.Collections;
using UnityEngine;
using TMPro;

public class AuthButtonsController : MonoBehaviour
{
    public Animator registerAccount, forgotPassword, login, signUp, registerBack, forgetPasswordBack, forgetPasswordSubmit, logout;
    public FirebaseAuthController firebaseAuthController;
    public TMP_InputField loginEmail, loginPassword, signUpEmail, signUpPassword, signUpUserName;

    public void onCreateNewAccountClick()
    {
        // Debug.Log("Create New Account button clicked");
        PlayAndStartCoroutine(registerAccount, () => OpenSignUpPanel());
    }

    public void onForgotPasswordClick()
    {
        PlayAndStartCoroutine(forgotPassword, () => OpenForgetPasswordPanel());
    }

    public void onLoginClick()
    {
        // Debug.Log("Login button clicked");
        PlayAndStartCoroutine(login, () => SignInUser());
    }
    
    public void onSignUpClick()
    {
        // Debug.Log("Sign Up button clicked");
        PlayAndStartCoroutine(signUp, () => CreateUser());
    }

    public void onRegisterBackClick()
    {
        PlayAndStartCoroutine(registerBack, () => Back());
    }

    public void onForgetPasswordBackClick()
    {
        PlayAndStartCoroutine(forgetPasswordBack, () => Back());
    }

    public void onForgetPasswordSubmitClick()
    {
        PlayAndStartCoroutine(forgetPasswordSubmit, () => ForgetPassword());
    }

    public void onLogout()
    {
        PlayAndStartCoroutine(logout, () => Logout());
    }

    private void PlayAndStartCoroutine(Animator animator, System.Func<IEnumerator> coroutineFunc)
    {
        animator.Play("Pressed");
        StartCoroutine(coroutineFunc());
    }

    private IEnumerator OpenSignUpPanel()
    {
        // Debug.Log("OpenSignUpPanel coroutine started");
        yield return new WaitForSeconds(GetAnimationLength(registerAccount));
        firebaseAuthController.OpenPanel("signupPanel");
        // Debug.Log("OpenSignUpPanel coroutine completed");
    }

    private IEnumerator OpenForgetPasswordPanel()
    {
        yield return new WaitForSeconds(GetAnimationLength(forgotPassword));
        firebaseAuthController.OpenPanel("forgetPassPanel");
    }

    private IEnumerator SignInUser()
    {
        Debug.Log("SignInUser coroutine started");
        yield return new WaitForSeconds(GetAnimationLength(login));
        // Debug.Log("Before calling firebaseAuthController.SignInUser()");

        string email = loginEmail.text;
        string password = loginPassword.text;

        // Call the SignInUser method in FirebaseAuthController and pass a callback function
        firebaseAuthController.SignInUser(email, password, () => {
            // This code will execute when authentication is successful
            // Debug.Log("After calling firebaseAuthController.SignInUser()");
            Debug.Log("SignInUser coroutine completed");
        });
    }

    private IEnumerator CreateUser()
    {
        // Debug.Log("CreateUser coroutine started");
        yield return new WaitForSeconds(GetAnimationLength(signUp));
        firebaseAuthController.CreateUser(signUpEmail.text, signUpPassword.text, signUpUserName.text);
        // Debug.Log("CreateUser coroutine completed");
    }

    private IEnumerator Back()
    {
        yield return new WaitForSeconds(GetAnimationLength(registerBack));
        firebaseAuthController.OpenPanel("loginPanel");
    }

    private IEnumerator ForgetPassword()
    {
        yield return new WaitForSeconds(GetAnimationLength(forgetPasswordSubmit));
        firebaseAuthController.ForgetPass();
    }

    private IEnumerator Logout()
    {
        yield return new WaitForSeconds(GetAnimationLength(logout));
        firebaseAuthController.LogOut();
    }

    private float GetAnimationLength(Animator animator)
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
}
