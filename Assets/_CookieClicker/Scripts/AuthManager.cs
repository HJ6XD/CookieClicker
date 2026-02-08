using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using TMPro;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;

    private FirebaseAuth auth;
    private DatabaseReference dbRef;

    public DataSaver dataSaver;
    void Start()
    {
        if (FirebaseInit.IsFirebaseReady)
        {
            InitializeAppFirebase();
        }
        else
            //FirebaseInit.OnFirebaseReady += InitializeAppFirebase;
            AppEventHub.OnFirebaseInitialized.AddListener(InitializeAppFirebase);
    }

    private void InitializeAppFirebase()
    {
        FirebaseApp app = FirebaseInit.AppInstance;

        Debug.Log("Database URL in AuthManager: " + app.Options.DatabaseUrl);

        auth = FirebaseAuth.GetAuth(app);
        dbRef = FirebaseDatabase.GetInstance(app).RootReference;

        Debug.Log("Firebase ready in AuthManager.");

    }

    public void RegisterUser()
    {
        if (auth == null)
        {
            Debug.LogWarning("Firebase not ready yet");
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;
        string username = usernameInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Signup failed: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("User created: " + user.Email);
            SaveUserData(user.UserId, username);
        });

    }

    public void LoginUser()
    {
        if (auth == null) 
        {
            Debug.LogWarning("Firebase not ready yet");
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Login failed: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("User logged in: " + user.Email);
        });
    }

    void SaveUserData(string uid, string username)
    {
        dbRef.Child("users").Child(uid).Child("username").SetValueAsync(username).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Username saved to database.");
            }
            else
                Debug.LogError("Failed to save username: " + task.Exception);
        });
    }
}
