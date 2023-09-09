using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseAuthController : MonoBehaviour
{
    public DatabaseManager databaseManager;

    public GameObject loginPanel, signupPanel, profilePanel, forgetPasswordPanel, notificationPanel;
    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUserName, forgetPassEmail;
    public TextMeshProUGUI notifTitleText, notifMessageText, profileUserNameText, profileUserEmailText;
    
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    bool isSignIn = false;
    bool isSigned = false;

    public void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    private void Update()
    {
        if (isSignIn && !isSigned)
        {
            isSigned = true;
            profileUserNameText.text = "" + user.DisplayName;
            profileUserEmailText.text = "" + user.Email;
            OpenProfilePanel();
        }
    }

    public void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenForgetPasswordPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
    }

    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        SignInUser(loginEmail.text, loginPassword.text);
        loginEmail.text = "";
        loginPassword.text = "";
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupEmail.text) && string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text) && string.IsNullOrEmpty(signupUserName.text))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        CreateUser(signupEmail.text, signupPassword.text, signupUserName.text);
    }

    public void ForgetPass()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        forgetPasswordSubmit(forgetPassEmail.text);
    }

    public void CloseNotifPanel()
    {
        notifTitleText.text = "";
        notifMessageText.text = "";

        notificationPanel.SetActive(false);
    }

    public void LogOut()
    {
        auth.SignOut();
        profileUserNameText.text = "";
        profileUserEmailText.text = "";
        OpenLoginPanel();
    }

    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError) firebaseEx.ErrorCode;
                        ShowNotificationMessage("Error", GetErrorMessage(errorCode));
                    }
                }

                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            profileUserNameText.text = "" + result.User.DisplayName;
            profileUserEmailText.text = "" + result.User.Email;
            OpenProfilePanel();
        });
    }

    private void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;
            }
        }
    }

    private void CreateUser(string email, string password, string username) {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => { 
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError) firebaseEx.ErrorCode;
                        ShowNotificationMessage("Error", GetErrorMessage(errorCode));
                    }
                }

                return;
            }
            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", username, result.User.UserId);

            UpdateUserProfile(username);

            profileUserNameText.text = username;
            profileUserEmailText.text = result.User.Email;
            OpenProfilePanel();
            ShowNotificationMessage("Alert", "Account Successfully Created");
            databaseManager.CreateNewUser(username, result.User.Email, result.User.UserId);
        });
    }

    private void UpdateUserProfile(string userName)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = userName,
                PhotoUrl = new System.Uri("https://picsum.photos/200"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }

    private void ShowNotificationMessage(string title, string message)
    {
        notifTitleText.text = "" + title;
        notifMessageText.text = "" + message;

        notificationPanel.SetActive(true);
    }

    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Account does not exist";
                break;
            case AuthError.MissingPassword:
                message = "Missing password";
                break;
            case AuthError.WeakPassword:
                message = "Password is weak";
                break;
            case AuthError.WrongPassword:
                message = "Wrong password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Your email is already in use";
                break;
            case AuthError.InvalidEmail:
                message = "Your email is invalid";
                break;
            case AuthError.MissingEmail:
                message = "Your email is missing";
                break;
            default:
                message = "Invalid error";
                break;
        }
        return message;
    }

    private void forgetPasswordSubmit(string forgetPasswordEmail)
    {
        auth.SendPasswordResetEmailAsync(forgetPasswordEmail).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled");
            }

            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError) firebaseEx.ErrorCode;
                        ShowNotificationMessage("Error", GetErrorMessage(errorCode));
                    }
                }
            }
            OpenLoginPanel();
            forgetPassEmail.text = "";
            ShowNotificationMessage("Alert", "Successfully sent email for resetting your password");
        });
    } 
}
