using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject dawnDusk, morning, noon, sunset, night;

    private void Start()
    {
        UpdateBackground();
    }

    private void UpdateBackground()
    {
        int currentHour = System.DateTime.Now.Hour;

        if (currentHour >= 5 && currentHour < 7)
        {
            EnableBackground(dawnDusk);
        }
        else if (currentHour >= 7 && currentHour < 11)
        {
            EnableBackground(morning);
        }
        else if (currentHour >= 11 && currentHour < 16)
        {
            EnableBackground(noon);
        }
        else if (currentHour >= 16 && currentHour < 19)
        {
            EnableBackground(sunset);
        }
        else if (currentHour >= 29 && currentHour < 21)
        {
            EnableBackground(dawnDusk);
        }
        else
        {
            EnableBackground(night);
        }
    }

    private void EnableBackground(GameObject background)
    {
        // Disable all backgrounds
        dawnDusk.SetActive(false);
        morning.SetActive(false);
        noon.SetActive(false);
        sunset.SetActive(false);
        night.SetActive(false);

        // Enable the specified background
        background.SetActive(true);
    }
}
