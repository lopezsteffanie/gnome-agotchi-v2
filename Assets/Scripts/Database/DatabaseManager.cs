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

    public async void CreateNewGnome(string name, int colorIndex, int personalityIndex, System.Action<string> onGnomeCreated)
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
            Gnome newGnome = new Gnome(name, colorIndex, needsId, personalityIndex);
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

    public async Task<bool> GetStartGameStatus(string userId)
    {
        var task = dbReference.Child("users").Child(userId).Child("startGameStatus").GetValueAsync();
        
        // Await the Firebase task
        await task;
        
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
                if (startGameStatusStr == "true")
                {
                    return true;
                }
                else if (startGameStatusStr == "false")
                {
                    return false;
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
        Debug.LogWarning("No conditions met to check startGameStatus, defaulting to false");
        return false;
    }

    public async Task<int> GetCurrentGnomeColorIndex(string userId)
    {
        try
        {
            // Get the gnomeId associated with the user
            var gnomeIdSnapshot = await dbReference.Child("users").Child(userId).Child("gnome").GetValueAsync();
            if (!gnomeIdSnapshot.Exists)
            {
                Debug.LogWarning("Gnome ID not found for the user.");
                // TODO: Update to handle errors
                return 0; // Default to index 0
            }

            string gnomeId = gnomeIdSnapshot.GetRawJsonValue().Trim('"'); // Trim double quotes
            Debug.Log($"gnomeId: {gnomeId}");

            // Get the gnome's data
            string gnomeDataPath = string.Format("gnome/{0}", gnomeId);
            Debug.Log($"gnome datapath: {gnomeDataPath}");
            var gnomeSnapshot = await dbReference.Child(gnomeDataPath).GetValueAsync();
            if (!gnomeSnapshot.Exists)
            {
                Debug.LogWarning("Gnome data not found.");
                // TODO: Update to handle errors
                return 0; // Default to index 0
            }
            Debug.Log("Gnome Data:");
            Debug.Log(gnomeSnapshot.GetRawJsonValue());

            // Check if "colorIndex" exists in the gnome's data
            if (gnomeSnapshot.HasChild("colorIndex"))
            {
                // Retrieve the colorIndex
                var colorIndexSnapshot = gnomeSnapshot.Child("colorIndex");
                string colorIndexStr = colorIndexSnapshot.GetRawJsonValue();
                Debug.Log($"ColorIndexStr: {colorIndexStr}");
                if (int.TryParse(colorIndexStr, out int colorIndex))
                {
                    return colorIndex;
                }
                else
                {
                    Debug.LogWarning("Invalid colorIndex value in the database");
                    // TODO: Handle the case where colorIndex is not a valid integer
                    return 0; // Default to index 0
                }
            }
            else
            {
                Debug.LogWarning("Color index not found for the gnome.");
                // TODO: Update to handle errors
                return 0; // Default to index 0
            }
        }
        catch (AggregateException e)
        {
            Debug.LogError($"Error fetching gnome color index: {e.InnerException}");
            // TODO: Handle error;
            return 0; // Default to index 0
        }
    }

    public async Task<int> GetCurrentGnomePersonalityIndex(string userId)
    {
        try
        {
            // Get the gnomeId associated with the user
            var gnomeIdSnapshot = await dbReference.Child("users").Child(userId).Child("gnome").GetValueAsync();
            if (!gnomeIdSnapshot.Exists)
            {
                Debug.LogWarning("Gnome ID not found for the user.");
                // TODO: Update to handle errors
                return 0; // Default to index 0
            }

            string gnomeId = gnomeIdSnapshot.GetRawJsonValue().Trim('"'); // Trim double quotes
            Debug.Log($"gnomeId: {gnomeId}");

            // Get the gnome's data
            string gnomeDataPath = string.Format("gnome/{0}", gnomeId);
            Debug.Log($"gnome datapath: {gnomeDataPath}");
            var gnomeSnapshot = await dbReference.Child(gnomeDataPath).GetValueAsync();
            if (!gnomeSnapshot.Exists)
            {
                Debug.LogWarning("Gnome data not found.");
                // TODO: Update to handle errors
                return 0; // Default to index 0
            }
            Debug.Log("Gnome Data:");
            Debug.Log(gnomeSnapshot.GetRawJsonValue());

            // Check if "personalityIndex" exists in the gnome's data
            if (gnomeSnapshot.HasChild("personalityIndex"))
            {
                // Retrieve the personalityIndex
                var personalityIndexSnapshot = gnomeSnapshot.Child("personalityIndex");
                string personalityIndexStr = personalityIndexSnapshot.GetRawJsonValue();
                Debug.Log($"ColorIndexStr: {personalityIndexStr}");
                if (int.TryParse(personalityIndexStr, out int personalityIndex))
                {
                    return personalityIndex;
                }
                else
                {
                    Debug.LogWarning("Invalid personalityIndex value in the database");
                    // TODO: Handle the case where colorIndex is not a valid integer
                    return 0; // Default to index 0
                }
            }
            else
            {
                Debug.LogWarning("Personality index not found for the gnome.");
                // TODO: Update to handle errors
                return 0; // Default to index 0
            }
        }
        catch (AggregateException e)
        {
            Debug.LogError($"Error fetching gnome personality index: {e.InnerException}");
            // TODO: Handle error;
            return 0; // Default to index 0
        }
    }
}
