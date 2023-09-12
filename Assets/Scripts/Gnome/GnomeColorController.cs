using UnityEngine;

public class GnomeColorController : MonoBehaviour
{
    public DatabaseManager databaseManager;
    public FirebaseAuthController firebaseAuthController;

    public GameObject[] gnomePrefabs;
    
    private int selectedGnomeIndex = 0;

    private async void Start()
    {
        // Check the startGameStatus for the current user
        string userId = firebaseAuthController.GetCurrentUserId();
        bool startGameStatus = await databaseManager.GetStartGameStatus(userId);

        if (startGameStatus)
        {
            // Use the saved gnome's color index
            int colorIndex = await databaseManager.GetCurrentGnomeColorIndex(userId);
            SelectGnomeByColorIndex(colorIndex);
        }
        else
        {
            // Generate a random gnome
            SelectRandomGnome();
        }

    }

    public int GetSelectedGnomeIndex()
    {
        return selectedGnomeIndex;
    }

    private void SelectRandomGnome()
    {
        int randomIndex = Random.Range(0, gnomePrefabs.Length);
        GameObject selectedGnome = gnomePrefabs[randomIndex];
        selectedGnome.SetActive(true);
        selectedGnomeIndex = randomIndex;
    }

    private void SelectGnomeByColorIndex(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < gnomePrefabs.Length)
        {
            GameObject selectedGnome = gnomePrefabs[colorIndex];
            selectedGnome.SetActive(true);
            selectedGnomeIndex = colorIndex;
        }
        else
        {
            Debug.LogWarning("Invalid gnome color index.");
            // TODO: Handle the case where color index is invalid
            // Default to gnome 0
            GameObject selectedGnome = gnomePrefabs[0];
            selectedGnome.SetActive(true);
            selectedGnomeIndex = 0;
        }
    }
}
