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

    private void Start()
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
        if (isSignIn)
        {
            // Debug.Log("User has signed in successfully.");
            string userId = GetCurrentUserId();
            bool startGameStatus = databaseManager.GetStartGameStatus(userId);
            string panelName = startGameStatus ? "gamePanel" : "nameGnomePanel";
            OpenPanel(panelName);

            gnome.SetActive(true);
            logoutButton.interactable = true;
        }
        else
        {
            // Debug.Log("User has signed out.");
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
            panel.SetActive(panel.name == panelName);
        }
    }

    public void AuthenticateUser(bool isSignUp)
    {
        TMP_InputField[] inputFields = isSignUp ? signupInputFields : loginInputFields;
        if (AreInputFieldsEmpty(inputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        Action onSignInSuccess = () =>
        {
            string userId = GetCurrentUserId();
            bool startGameStatus = databaseManager.GetStartGameStatus(userId);
            string panelName = startGameStatus ? "gamePanel" : "nameGnomePanel";
            OpenPanel(panelName);
        };

        if (isSignUp)
        {
            CreateUser(inputFields[1].text, inputFields[2].text, inputFields[0].text);
        }
        else
        {
            SignInUser(inputFields[0].text, inputFields[1].text, onSignInSuccess);
        }

        ClearInputFields(inputFields);
    }

    public void ForgetPass()
    {
        if (AreInputFieldsEmpty(forgetPassInputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        ForgetPasswordSubmit(forgetPassInputFields[0].text);
    }

    public void LogOut()
    {
        auth.SignOut();
        OpenPanel("loginPanel");
        isSignIn = false;
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        user = auth.CurrentUser;
        isSignIn = user != null;
    }

    public void CreateUser(string email, string password, string username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                HandleAuthError(task.Exception);
                return;
            }

            AuthResult result = task.Result;
            UpdateUserProfile(username);

            OpenPanel("nameGnomePanel");
            gnome.SetActive(true);
            logoutButton.interactable = true;
            // ShowNotificationMessage("Alert", "Account Successfully Created");
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
                if (task.IsFaulted)
                {
                    Debug.LogError($"UpdateUserProfileAsync encountered an error: {task.Exception}");
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }

    public void SignInUser(string email, string password, Action onSignInSuccess)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                HandleAuthError(task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
            string userId = GetCurrentUserId();

            bool startGameStatus = databaseManager.GetStartGameStatus(userId);
            string panelName = startGameStatus ? "gamePanel" : "nameGnomePanel";
            Debug.Log($"Before opening panel: {panelName}");
            OpenPanel(panelName);
            Debug.Log($"After opening panel: {panelName}");
            
            // Call the callback function to notify the coroutine
            onSignInSuccess?.Invoke();
        });
    }

    private void ShowNotificationMessage(string title, string message)
    {
        notifTitleText.text = title;
        notifMessageText.text = message;
        notificationPanel.SetActive(true);
    }

    private void HandleAuthError(AggregateException exception)
    {
        foreach (Exception innerException in exception.Flatten().InnerExceptions)
        {
            if (innerException is FirebaseException firebaseEx)
            {
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
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
}
