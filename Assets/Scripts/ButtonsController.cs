using System.Collections;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    public Animator registerAccount, forgotPassword, login, signUp, registerBack, forgetPasswordBack, forgetPasswordSubmit, logout;
    public FirebaseAuthController firebaseAuthController;
    
    public void onCreateNewAccountClick()
    {
        registerAccount.Play("Pressed");
        StartCoroutine(OpenSignUpPanel(registerAccount));
    }

    public void onForgotPasswordClick()
    {
        forgotPassword.Play("Pressed");
        StartCoroutine(OpenForgetPasswordPanel(forgotPassword));
    }

    public void onLoginClick()
    {
        login.Play("Pressed");
        StartCoroutine(LoginUser(login));
    }

    public void onSignUpClick()
    {
        signUp.Play("Pressed");
        StartCoroutine(SignUpUser(signUp));
    }

    public void onRegisterBackClick()
    {
        registerBack.Play("Pressed");
        StartCoroutine(Back(registerBack));
    }

    public void onForgetPasswordBackClick()
    {
        forgetPasswordBack.Play("Pressed");
        StartCoroutine(Back(forgetPasswordBack));
    }

    public void onForgetPasswordSubmitClick()
    {
        forgetPasswordSubmit.Play("Pressed");
        StartCoroutine(ForgetPassword(forgetPasswordSubmit));
    }

    public void onLogout()
    {
        logout.Play("Pressed");
        StartCoroutine(Logout(logout));
    }

    private IEnumerator OpenSignUpPanel(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        firebaseAuthController.OpenSignUpPanel();
    }

    private IEnumerator OpenForgetPasswordPanel(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        firebaseAuthController.OpenForgetPasswordPanel();
    }

    private IEnumerator LoginUser(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        firebaseAuthController.LoginUser();
    }

    private IEnumerator SignUpUser(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        firebaseAuthController.SignUpUser();
    }

    private IEnumerator Back(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        firebaseAuthController.OpenLoginPanel();
    }

    private IEnumerator ForgetPassword(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        firebaseAuthController.ForgetPass();
    }

    private IEnumerator Logout(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength - 0.5f);
        firebaseAuthController.LogOut();
    }
}
