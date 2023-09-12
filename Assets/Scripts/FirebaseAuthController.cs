using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseAuthController : MonoBehaviour
{
    public DatabaseManager databaseManager;

    public GameObject[] panels;
    public TMP_InputField[] loginInputFields, signupInputFields, forgetPassInputFields;
    public TextMeshProUGUI notifTitleText, notifMessageText;
    public GameObject notificationPanel, gnome;
    public Button logoutButton;
    
    private FirebaseAuth auth;
    private FirebaseUser user;
    private bool isSignIn = false;
    private bool isSigned = false;

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
            Debug.Log("User has signed in successfully.");
            isSigned = true;
            
            string userId = GetCurrentUserId();
            databaseManager.GetStartGameStatus(userId, startGameStatus => {
                if (startGameStatus)
                {
                    Debug.Log("Before opening gamePanel");
                    // Open the gamePanel
                    OpenPanel("gamePanel");
                    Debug.Log("After opening gamePanel");
                }
                else
                {
                    Debug.Log("Before opening nameGnomePanel");
                    // Open the nameGnomePanel
                    OpenPanel("nameGnomePanel");
                    Debug.Log("After opening nameGnomePanel");
                }
            });

            gnome.SetActive(true);
            logoutButton.interactable = true;
        }
        else if (!isSignIn && isSigned)
        {
            Debug.Log("User has signed out.");
            isSigned = false;

            gnome.SetActive(false);
            logoutButton.interactable = false;
        }
    }

    public void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void OpenPanel(string panelName)
    {
        foreach (GameObject panel in panels)
        {
            Debug.Log($"Checking panel: {panel.name}");
            Debug.Log($"Panel {panel.name} is active? {panel.name == panelName}");
            panel.SetActive(panel.name == panelName);
        }
    }

    public void LoginUser()
    {
        Debug.Log("Before calling LoginUser");
        if (AreInputFieldsEmpty(loginInputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        SignInUser(loginInputFields[0].text, loginInputFields[1].text); // loginEmail, loginPassword
        ClearInputFields(loginInputFields);
        Debug.Log("After calling LoginUser");
    }

    public void SignUpUser()
    {
        if (AreInputFieldsEmpty(signupInputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        CreateUser(signupInputFields[1].text, signupInputFields[2].text, signupInputFields[0].text); // signupEmail, signupPassword, signupUserName
    }

    public void ForgetPass()
    {
        if (AreInputFieldsEmpty(forgetPassInputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        ForgetPasswordSubmit(forgetPassInputFields[0].text); // forgetPassEmail
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
        OpenPanel("loginPanel");
        gnome.SetActive(false);
        logoutButton.interactable = false;
        isSignIn = false;
        isSigned = false;
    }

    public string GetCurrentUserId()
    {
        user = auth.CurrentUser;
        if (user != null)
        {
            return user.UserId;
        }
        // TODO: Update for error handling
        return null;
    }

    private void SignInUser(string email, string password)
    {
        Debug.Log("Before calling SignInUser");
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was faulted");
                HandleAuthError(task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");

            string userId = result.User.UserId;
            databaseManager.GetStartGameStatus(userId, startGameStatus => {
                if (startGameStatus)
                {
                    Debug.Log("Before opening gamePanel");
                    // Open the gamePanel
                    OpenPanel("gamePanel");
                    Debug.Log("After opening gamePanel");
                }
                else
                {
                    Debug.Log("Before opening nameGnomePanel");
                    // Open the nameGnomePanel
                    OpenPanel("nameGnomePanel");
                    Debug.Log("After opening nameGnomePanel");
                }
            });

            gnome.SetActive(true);
            logoutButton.interactable = true;
        });
        Debug.Log("After sign in user called");
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log($"Signed out {user.UserId}");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log($"Signed in {user.UserId}");
                isSignIn = true;
            }
            else
            {
                Debug.Log("User is not signed in or authentication state changed.");
            }
        }
    }

    private void CreateUser(string email, string password, string username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                HandleAuthError(task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"Firebase user created successfully: {username} ({result.User.UserId})");

            UpdateUserProfile(username);

            Debug.Log("Before opening nameGnomePanel");
            OpenPanel("nameGnomePanel");
            Debug.Log("After opening nameGnomePanel");
            gnome.SetActive(true);
            logoutButton.interactable = true;
            ShowNotificationMessage("Alert", "Account Successfully Created");
            databaseManager.CreateNewUser(username, result.User.Email, result.User.UserId);
        });
    }

    private void UpdateUserProfile(string userName)
    {
        FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = userName,
                PhotoUrl = new Uri("https://picsum.photos/200"),
            };
            currentUser.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"UpdateUserProfileAsync encountered an error: {task.Exception}");
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

    private void HandleAuthError(AggregateException exception)
    {
        foreach (Exception innerException in exception.Flatten().InnerExceptions)
        {
            if (innerException is FirebaseException firebaseEx)
            {
                AuthError errorCode = (AuthError) firebaseEx.ErrorCode;
                ShowNotificationMessage("Error", GetErrorMessage(errorCode));
            }
        }
    }

    private static string GetErrorMessage(AuthError errorCode)
    {
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                return "Account does not exist";
            case AuthError.MissingPassword:
                return "Missing password";
            case AuthError.WeakPassword:
                return "Password is weak";
            case AuthError.WrongPassword:
                return "Wrong password";
            case AuthError.EmailAlreadyInUse:
                return "Your email is already in use";
            case AuthError.InvalidEmail:
                return "Your email is invalid";
            case AuthError.MissingEmail:
                return "Your email is missing";
            default:
                return "Invalid error";
        }
    }

    private void ForgetPasswordSubmit(string forgetPasswordEmail)
    {
        auth.SendPasswordResetEmailAsync(forgetPasswordEmail).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled");
            }

            if (task.IsFaulted)
            {
                HandleAuthError(task.Exception);
            }
            
            OpenPanel("loginPanel");
            forgetPassInputFields[0].text = ""; // forgetPassEmail
            ShowNotificationMessage("Alert", "Successfully sent email for resetting your password");
        });
    }

    private static bool AreInputFieldsEmpty(TMP_InputField[] fields)
    {
        foreach (TMP_InputField field in fields)
        {
            if (string.IsNullOrEmpty(field.text))
            {
                return true;
            }
        }
        return false;
    }

    private static void ClearInputFields(TMP_InputField[] fields)
    {
        foreach (TMP_InputField field in fields)
        {
            field.text = "";
        }
    }
}
