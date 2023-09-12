using UnityEngine;

public class GnomeColorController : MonoBehaviour
{
    public GameObject[] gnomePrefabs;
    
    private bool hasSelectedGnome = false;
    private int selectedGnomeIndex = 0;

    private void Start()
    {
        if (!hasSelectedGnome)
        {
            SelectRandomGnome();
        }
        else
        {
            foreach (GameObject gnome in gnomePrefabs)
            {
                if (gnome != gnomePrefabs[GetSelectedGnomeIndex()])
                {
                    gnome.SetActive(false);
                }
            }
        }
    }

    private void SelectRandomGnome()
    {
        int randomIndex = Random.Range(0, gnomePrefabs.Length);
        GameObject selectedGnome = gnomePrefabs[randomIndex];
        selectedGnome.SetActive(true);
        hasSelectedGnome = true;
        selectedGnomeIndex = randomIndex;
    }

    public int GetSelectedGnomeIndex()
    {
        return selectedGnomeIndex;
    }
}
