using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void CreateNewUser(string name, string email, string userId)
    {
        User newUser = new User(name, email);
        string userJson = JsonUtility.ToJson(newUser);

        try
        {
            await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(userJson);
            Debug.Log("User saved successfully!");
        }
        catch (AggregateException ae)
        {
            Debug.LogError($"Error creating user: {ae.InnerException}");
        }
    }

    public async void CreateNewGnome(string name, int colorIndex, System.Action<string> onGnomeCreated)
    {
        // Create a new Needs object and associate it with the Gnome
        Needs newNeeds = new Needs();
        string needsJson = JsonUtility.ToJson(newNeeds);
        DatabaseReference needsReference = dbReference.Child("needs").Push();

        try
        {
            await needsReference.SetRawJsonValueAsync(needsJson);
            string needsId = needsReference.Key;

            // Create the Gnome object with the associated Needs Id
            Gnome newGnome = new Gnome(name, colorIndex, needsId);
            string gnomeJson = JsonUtility.ToJson(newGnome);

            DatabaseReference gnomeReference = dbReference.Child("gnome").Push();
            await gnomeReference.SetRawJsonValueAsync(gnomeJson);

            Debug.Log("Gnome saved successfully!");

            // Invoke the callback with the gnomeId
            onGnomeCreated?.Invoke(gnomeReference.Key);
        }
        catch (AggregateException ae)
        {
            Debug.LogError($"Error creating Gnome: {ae.InnerException}");
        }
    }

    public async void UpdateUser(string userId, string gnomeId)
    {
        var updates = new Dictionary<string, object>
        {
            { "gnome", gnomeId },
            { "startGameStatus", true }
        };

        try
        {
            await dbReference.Child("users").Child(userId).UpdateChildrenAsync(updates);
            Debug.Log("User data updated successfully!");
        }
        catch (AggregateException ae)
        {
            Debug.LogError($"Error updating user data: {ae.InnerException}");
        }
    }

    public bool GetStartGameStatus(string userId)
    {
        var task = dbReference.Child("users").Child(userId).Child("startGameStatus").GetValueAsync();
        
        if (task.IsFaulted)
        {
            Debug.LogError($"Error fetching startGameStatus: {task.Exception}");
            return false;
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
            
            if (snapshot.Exists)
            {
                string startGameStatusStr = snapshot.GetRawJsonValue();
                bool startGameStatus;
                
                if (bool.TryParse(startGameStatusStr, out startGameStatus))
                {
                    return startGameStatus;
                }
                else
                {
                    Debug.LogWarning("Invalid startGameStatus value in the database");
                    // TODO: Handle the case where startGameStatus is not a valid boolean
                    return false; // Default to false
                }
            }
            else
            {
                Debug.LogWarning("startGameStatus not found in the database");
                // TODO: Handle the case where startGameStatus is not found
                return false; // Default to false
            }
        }
        
        // Default to false if none of the conditions above are met
        return false;
    }
}
