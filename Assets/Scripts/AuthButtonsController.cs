using System.Collections;
using UnityEngine;

public class AuthButtonsController : MonoBehaviour
{
    public Animator registerAccount, forgotPassword, login, signUp, registerBack, forgetPasswordBack, forgetPasswordSubmit, logout;
    public FirebaseAuthController firebaseAuthController;

    public void onCreateNewAccountClick()
    {
        Debug.Log("Create New Account button clicked");
        PlayAndStartCoroutine(registerAccount, () => OpenSignUpPanel());
    }

    public void onForgotPasswordClick()
    {
        PlayAndStartCoroutine(forgotPassword, () => OpenForgetPasswordPanel());
    }

    public void onLoginClick()
    {
        Debug.Log("Login button clicked");
        PlayAndStartCoroutine(login, () => LoginUser());
    }
    
    public void onSignUpClick()
    {
        Debug.Log("Sign Up button clicked");
        PlayAndStartCoroutine(signUp, () => SignUpUser());
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
        Debug.Log("OpenSignUpPanel coroutine started");
        yield return new WaitForSeconds(GetAnimationLength(registerAccount));
        firebaseAuthController.OpenPanel("signupPanel");
        Debug.Log("OpenSignUpPanel coroutine completed");
    }

    private IEnumerator OpenForgetPasswordPanel()
    {
        yield return new WaitForSeconds(GetAnimationLength(forgotPassword));
        firebaseAuthController.OpenPanel("forgetPassPanel");
    }

    private IEnumerator LoginUser()
    {
        Debug.Log("LoginUser coroutine started");
        yield return new WaitForSeconds(GetAnimationLength(login));
        Debug.Log("Before calling firebaseAuthController.LoginUser()");
        firebaseAuthController.LoginUser();
        Debug.Log("After calling firebaseAuthController.LoginUser()");
        Debug.Log("LoginUser coroutine completed");
    }

    private IEnumerator SignUpUser()
    {
        Debug.Log("SignUpUser coroutine started");
        yield return new WaitForSeconds(GetAnimationLength(signUp));
        firebaseAuthController.SignUpUser();
        Debug.Log("SignUpUser coroutine completed");
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
