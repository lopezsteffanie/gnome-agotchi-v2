using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameGnomePanelController : MonoBehaviour
{
    public TextMeshProUGUI displayGnomeName;
    public TMP_InputField inputGnomeName;
    public GameObject inputField, gamePanel;
    public Button startGameButton, confirmNameButton, resetNameButton;
    public FirebaseAuthController firebaseAuthController;
    public GnomeColorController gnomeColorController;
    public DatabaseManager databaseManager;

    private string currentGnomeName = "";

    private void Initialize()
    {
        displayGnomeName.text = "";
    }

    private void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClick);
        confirmNameButton.onClick.AddListener(OnConfirmNameButtonClick);
        resetNameButton.onClick.AddListener(OnResetNameButtonClick);
    }

    public string getGnomeName()
    {
        return currentGnomeName;
    }

    private void OnConfirmNameButtonClick()
    {
        currentGnomeName = inputGnomeName.text;
        displayGnomeName.text = "Your gnome's name: " + currentGnomeName;
        inputField.SetActive(false);
        startGameButton.interactable = true;
    }

    private void OnResetNameButtonClick()
    {
        displayGnomeName.text = "";
        inputField.SetActive(true);
        startGameButton.interactable = false;
    }

    private void OnStartGameButtonClick()
    {
        int colorIndex = gnomeColorController.GetSelectedGnomeIndex();
        string currentUser = firebaseAuthController.GetCurrentUserId();
        if (currentUser != null)
        {
            Debug.Log("Before OpenGamePanel");
            firebaseAuthController.OpenPanel(gamePanel.name);
            Debug.Log("After OpenGamePanel");
            
            databaseManager.CreateNewGnome(currentGnomeName, colorIndex, (gnomeId) =>
            {
                databaseManager.UpdateUser(currentUser, gnomeId);
            });
        }
        else
        {
            Debug.LogWarning("User is not logged in.");
            // TODO: Handle error
        }
    }
}
