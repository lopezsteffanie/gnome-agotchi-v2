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

    private void Start()
    {
        // Check and fix Firebase dependencies asynchronously
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Initialize Firebase once dependencies are resolved
                InitializeFirebase();
            }
            else
            {
                // Log an error if Firebase dependencies cannot be resolved
                // TODO: Handle error
                UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    private async void Update()
    {
        if (isSignIn && !isSigned)
        {
            isSigned = true;
            string userId = GetCurrentUserId();

            // Get the startGameStatus and open gamePanel if status is true or nameGnomePanel is status is false;
            bool startGameStatus = await databaseManager.GetStartGameStatus(userId);
            string panelName = startGameStatus ? panels[4].name : panels[3].name;
            OpenPanel(panelName);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the auth state changed event and clean up
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    // Opens a specified panel among the available panels
    public void OpenPanel(string panelName)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel.name == panelName);
        }
    }

    // Get the current user's unique identifier
    public string GetCurrentUserId()
    {
        if (auth != null)
        {
            user = auth.CurrentUser;
            if (user != null)
            {
                return user.UserId;
            }
        }
        // TODO: Update for error handling
        return null;
    }

    // Initiates the password reset process
    public void ForgetPass()
    {
        if (AreInputFieldsEmpty(forgetPassInputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }

        ForgetPasswordSubmit(forgetPassInputFields[0].text);
    }

    // Logs the user out and returns to the login panel
    public void LogOut()
    {
        auth.SignOut();
        OpenPanel(panels[0].name);
        gnome.SetActive(false);
        logoutButton.interactable = false;
        isSignIn = false;
        isSigned = false;
    }

    // Creates a new user account with the provided email, password, and username
    public void CreateUser(TMP_InputField email, TMP_InputField password, TMP_InputField username)
    {
        TMP_InputField[] inputFields = { email, password, username };
        if (AreInputFieldsEmpty(inputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
            return;
        }
        
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                HandleAuthError(task.Exception);
                return;
            }

            AuthResult result = task.Result;
            UpdateUserProfile(username.text);

            // open the nameGnomePanel after successful user creation
            OpenPanel(panels[3].name);
            gnome.SetActive(true);
            logoutButton.interactable = true;
            databaseManager.CreateNewUser(username.text, result.User.Email, result.User.UserId);
            
            // Clear input fields after account creation
            ClearInputFields(inputFields);
        });
    }

    // Sign in a user with the provided email and password
    public void SignInUser(TMP_InputField email, TMP_InputField password, Action onSignInSuccess)
    {
        TMP_InputField[] inputFields = { email, password };
        if (AreInputFieldsEmpty(inputFields))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details in All Fields");
        }
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(async task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                HandleAuthError(task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
            string userId = GetCurrentUserId();

            logoutButton.interactable = true;
            gnome.SetActive(true);
            bool startGameStatus = await databaseManager.GetStartGameStatus(userId);
            string panelName = startGameStatus ? panels[4].name : panels[3].name;
            OpenPanel(panelName);
            ClearInputFields(inputFields);
            
            // Call the callback function to notify the coroutine
            onSignInSuccess?.Invoke();
        });
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

    private void UpdateUserProfile(string userName)
    {
        FirebaseUser currentUser = auth.CurrentUser;
        if (currentUser != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = userName,
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
        notifTitleText.text = "";
        notifMessageText.text = "";
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

            OpenPanel(panels[0].name);
            ClearInputFields(forgetPassInputFields);
            ShowNotificationMessage("Alert", "Successfully sent email for resetting your password");
        });
    }
}
