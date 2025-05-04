using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseRealtimeDB : MonoBehaviour
{
    string firebaseURL="https://flopstronauts-default-rtdb.asia-southeast1.firebasedatabase.app/";


    public class User {
    public string username;
    public string playerID;

    public User() {
    }

    public User(string username, string playerID) {
        this.username = username;
        this.playerID = playerID;
    }
}
    
    void Awake()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void WriteNewUser(string userId, string playerID) 
    {
    User user = new User(name, playerID);
    string json = JsonUtility.ToJson(user);

    //mDatabaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }


}
