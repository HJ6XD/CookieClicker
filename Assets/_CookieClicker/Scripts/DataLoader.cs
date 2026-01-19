using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;


public class DataLoader : MonoBehaviour
{
    DatabaseReference databaseReference;
    FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        if(auth.CurrentUser == null)
        {
            Debug.LogError("No auth user. Sing in first");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        LoadMyData(uid);
    }

    public void LoadMyData(string uid)
    {
        databaseReference.Child("users").Child(uid).GetValueAsync().
            ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Load failed: " + task.Exception);
                }

                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    Debug.Log(json);
                }

                else
                {
                    Debug.LogError($"{uid} data was not found.");
                }
            });
    }

}
