using UnityEngine;

public class GnomePersonalityController : MonoBehaviour
{
    public DatabaseManager databaseManager;
    public FirebaseAuthController firebaseAuthController;

    public GameObject[] personalityPrefabs;

    private int selectedPersonalityIndex = 0;

    private async void Start()
    {
        // Check the startGameStatus for the current user
        string userId = firebaseAuthController.GetCurrentUserId();
        bool startGameStatus = await databaseManager.GetStartGameStatus(userId);

        if (startGameStatus)
        {
            // Use the saved gnome's personality index
            int personalityIndex = await databaseManager.GetCurrentGnomePersonalityIndex(userId);
            SelectPersonalityByIndex(personalityIndex);
        }

        else
        {
            // Generate a random gnome personality
            SelectRandomPersonality();
        }
    }

    public int GetSelectedPersonalityIndex()
    {
        Debug.Log($"Selected personality: {personalityPrefabs[selectedPersonalityIndex]}");
        return selectedPersonalityIndex;
    }

    private void SelectRandomPersonality()
    {
        int randomIndex = Random.Range(0, personalityPrefabs.Length);
        GameObject selectedPersonality = personalityPrefabs[randomIndex];
        selectedPersonality.SetActive(true);
        selectedPersonalityIndex = randomIndex;
        Debug.Log($"Selected personality: {personalityPrefabs[selectedPersonalityIndex]}");
    }

    private void SelectPersonalityByIndex(int personalityIndex)
    {
        if (personalityIndex >= 0 && personalityIndex < personalityPrefabs.Length)
        {
            GameObject selectedPersonality = personalityPrefabs[personalityIndex];
            selectedPersonality.SetActive(true);
            selectedPersonalityIndex = personalityIndex;
            Debug.Log($"Selected personality: {personalityPrefabs[selectedPersonalityIndex]}");
        }
        else
        {
            Debug.LogWarning("Invalid gnome personality index.");
            // TODO: Handle the case where personality index is invalid
            // Default personality to 0
            GameObject selectedPersonality = personalityPrefabs[0];
            selectedPersonality.SetActive(true);
            selectedPersonalityIndex = 0;
            Debug.Log("Defaulted personality index to 0");
        }
    }
}
