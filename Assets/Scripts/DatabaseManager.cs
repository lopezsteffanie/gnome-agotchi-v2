using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateNewUser(string name, string email, string userId)
    {
        User newUser = new User(name, email);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
}
