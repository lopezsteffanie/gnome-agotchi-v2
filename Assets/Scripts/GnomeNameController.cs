using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GnomeNameController : MonoBehaviour
{
    public TextMeshProUGUI displayGnomeName;
    public TMP_InputField inputGnomeName;
    public GameObject confirmNameButton, inputField, resetNameButton, nameGnomePanel, gamePanel;
    public Button startGameButton;
    public Animator confirmNameAnimator, resetNameAnimator, startGameAnimator;
    public FirebaseAuthController firebaseAuthController;
    public GnomeColorController gnomeColorController;
    public DatabaseManager databaseManager;

    private string currentGnomeName = "";

    public void Initialize()
    {
        displayGnomeName.text = "";
    }

    public void OnSetName()
    {
        currentGnomeName = inputGnomeName.text;
        displayGnomeName.text = "Your gnome's name: " + currentGnomeName;
        inputField.SetActive(false);
        startGameButton.interactable = true;
        confirmNameAnimator.Play("Pressed");
        StartCoroutine(SetName(confirmNameAnimator));
    }

    public void OnResetName()
    {
        displayGnomeName.text = "";
        inputField.SetActive(true);
        startGameButton.interactable = false;
        resetNameAnimator.Play("Pressed");
        StartCoroutine(ResetName(resetNameAnimator));
    }

    public void OnStartGame()
    {
        int colorIndex = gnomeColorController.GetSelectedGnomeIndex();
        string currentUser = firebaseAuthController.GetCurrentUserId();
        if (currentUser != null)
        {
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
        startGameAnimator.Play("Pressed");
        StartCoroutine(StartGame(startGameAnimator));
    }

    public string getGnomeName()
    {
        return currentGnomeName;
    }

    private IEnumerator SetName(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength);
        confirmNameButton.SetActive(false);
        resetNameButton.SetActive(true);
    }

    private IEnumerator ResetName(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength);
        confirmNameButton.SetActive(true);
        resetNameButton.SetActive(false);
    }
    
    private IEnumerator StartGame(Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animationLength);
        firebaseAuthController.OpenPanel("gamePanel");
    }
}
