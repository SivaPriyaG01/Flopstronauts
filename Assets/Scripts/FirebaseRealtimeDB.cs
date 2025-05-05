using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseRealtimeDB : MonoBehaviour
{
    DatabaseReference dbRef;
    string firebaseURL="https://flopstronauts-default-rtdb.asia-southeast1.firebasedatabase.app/";


    [System.Serializable]
    public class User {
    public string username;
    //public string playerID;

    public User() {
    }

        public User(string username)
        {
            this.username = username;
        }

    //     public User(string username, string playerID) {
    //     this.username = username;
    //     // //this.playerID = playerID;
    //     // public User(string username)
    //     // {
    //     //     this.username = username;
    //     // }
    // }
}
    
    void Awake()
    {
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Initialize Firebase
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Database URL
                app.Options.DatabaseUrl = new System.Uri(firebaseURL);

                // Initialize FirebaseAuth and DatabaseReference
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;

                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    public void WriteNewUser(string username,string playerID) 
    {
    //User user = new User(username, playerID);
    User user = new User(username);
    string json = JsonUtility.ToJson(user);

    dbRef.Child("users").Child(playerID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task=>
    {
        if(task.IsCompleted &&!task.IsFaulted)
        {
            Debug.Log("Player Username Saved");
        }
        else
        {
            Debug.Log("Save failed");
        }
    });
    }


}
