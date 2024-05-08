using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Threading;





public class next : MonoBehaviour
{
   
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    public GameObject loginPanel, registerPanel,forgetpasswordPanel, notificationPanel;
    public TMPro.TMP_InputField loginEmail, loginPassword, registerEmail, registerPassword, registerUsername, registerAge, forgetpasswordEmail;
    public TMPro.TMP_Text notifTitle, notifMessage;


    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    public void openLoginpanel() {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        forgetpasswordPanel.SetActive(false);


    }
    public void openregisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        forgetpasswordPanel.SetActive(false);



    }
    public void openforgetpasswordPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        forgetpasswordPanel.SetActive(true);



    }

    public void loginUser() { 
    if(string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text)){
            showNotificationMessage("Error Message", "Fields Empty: please fill all fields");
            return;
        }

        //do login
        loginuser(loginEmail.text,loginPassword.text);
    }
    public void RegisterUser()
    {
        if (string.IsNullOrEmpty(registerEmail.text) || string.IsNullOrEmpty(registerPassword.text) || string.IsNullOrEmpty(registerUsername.text) || string.IsNullOrEmpty(registerAge.text))
        {
            showNotificationMessage("Error Message", "Fields Empty: please fill all fields");
            return;
        }
        
        //do register
        createUser(registerEmail.text, registerPassword.text);
   

    }

    public void forgetpassword()
    {
        if (string.IsNullOrEmpty(forgetpasswordEmail.text) )
        {
            showNotificationMessage("Error Message", "Fields Empty: please fill all fields");
            return;
        }

        forgetPasswordSubmit(forgetpasswordEmail.text);
        openLoginpanel();



    }
    private void showNotificationMessage(string title,string message)
    {
        notifTitle.text = "" + title;
        notifMessage.text = "" + message;
        notificationPanel.SetActive(true);

    }
    public void closeNotifPanel()
    {
        notifTitle.text = "";
        notifMessage.text = "";
        notificationPanel.SetActive(false);

    }
    void createUser(string email,string password) {


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
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        showNotificationMessage("Error message", GetErrorMessage(errorCode));
                    }

                }
                return;
            }
            string errorMessage = IsPasswordValid(password);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Show notification message about weak password
                showNotificationMessage("Error", errorMessage);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            showNotificationMessage("Alert", "You Have registered Successfully");

            openLoginpanel();

        });


    }


    public void Next(string sceneName)
    {
       
        SceneManager.LoadScene(sceneName);
    }
    public void loginuser(string email, string password)
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
               foreach(Exception exception in task.Exception.Flatten().InnerExceptions) {

                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        showNotificationMessage("Error message",GetErrorMessage(errorCode));
                    }

                }
                
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            Next("Charcters");
        });
    }



    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
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
               
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }


    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Acxount Not Exist";
                break;
            case AuthError.MissingPassword:
                message = "Missing Password";
                break;
            case AuthError.WeakPassword:
                message = "Password must be 6 characters or longer. ";
                break;
            case AuthError.WrongPassword:
                message = "Wrong password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Email Already In Use";
                break;
            case AuthError.InvalidEmail:
                message = "Email Invalid";
                break;
            case AuthError.MissingEmail:
                message = "Email Missing";
                break;
            default:
                message = "Invalid Error";
                break;
        }
        return message;
    }
    void forgetPasswordSubmit(string forgetpasswordEmail)
    {
        auth.SendPasswordResetEmailAsync(forgetpasswordEmail).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
               
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {

                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        showNotificationMessage("Error message", GetErrorMessage(errorCode));
                    }

                }

               
            }

            showNotificationMessage("Alert", "Successfully Send Email For Reset Password");

            openLoginpanel();

        });

    }
    private string IsPasswordValid(string password)
    {
        string errorMessage = "";

        // Check if password contains at least one uppercase letter
        if (!password.Any(char.IsUpper))
        {
            errorMessage += "Include at least one uppercase letter. ";
        }

        // Check if password contains at least one lowercase letter
        if (!password.Any(char.IsLower))
        {
            errorMessage += "Include at least one lowercase letter. ";
        }

        // Check if password contains at least one digit
        if (!password.Any(char.IsDigit))
        {
            errorMessage += "Include at least one digit. ";
        }

        // Check if password contains at least one special character
        // You can define your own set of special characters
        string specialCharacters = @"!@#$%^&*()-_=+[]{}|;:'"",.<>/?";
        if (!password.Any(specialCharacters.Contains))
        {
            errorMessage += "Include at least one special character. ";
        }

        return errorMessage.Trim(); // Trim any leading or trailing whitespace
    }



}
