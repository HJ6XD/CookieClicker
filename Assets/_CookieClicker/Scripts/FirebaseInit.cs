using UnityEngine;
using Firebase;
using Firebase.Extensions;
using System;


public class FirebaseInit : MonoBehaviour
{
    public static bool IsFirebaseReady { get; private set; } = false;
    public static FirebaseApp AppInstance { get; private set; }
    public static event Action OnFirebaseReady;
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                AppInstance = FirebaseApp.DefaultInstance;

                if (Application.isEditor && AppInstance.Options.DatabaseUrl == null)
                {
                    AppInstance.Options.DatabaseUrl = new System.Uri("https://protocol-ccb3c-default-rtdb.firebaseio.com/");
                    Debug.Log("Database URL set in FirebaseInit: " + AppInstance.Options.DatabaseUrl);
                }

                IsFirebaseReady = true;
                Debug.Log("Firebase Initialized");
                OnFirebaseReady?.Invoke();
            }
            else
            {
                Debug.Log("Firebase init failed: " + task.Result);
            }
        });
    }
}