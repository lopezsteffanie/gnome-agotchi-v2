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
        
        // Define time ranges and their corresponding backgrounds
        TimeRange[] timeRanges = new TimeRange[]
        {
            new TimeRange(5, 7, dawnDusk),
            new TimeRange(7, 11, morning),
            new TimeRange(11, 16, noon),
            new TimeRange(16, 19, sunset),
            new TimeRange(19, 21, dawnDusk),
            new TimeRange(21, 24, night)
        };

        // Find the matching background for the current hour
        GameObject selectedBackground = night; // Default to night if no match is found
        foreach (TimeRange range in timeRanges)
        {
            if (currentHour >= range.StartHour && currentHour < range.EndHour)
            {
                selectedBackground = range.Background;
                break; // Stop searching when a match is found
            }
        }

        EnableBackground(selectedBackground);
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

    // A simple class to represent a time range and its associated background
    private class TimeRange
    {
        public int StartHour { get; }
        public int EndHour { get; }
        public GameObject Background { get; }

        public TimeRange(int startHour, int endHour, GameObject background)
        {
            StartHour = startHour;
            EndHour = endHour;
            Background = background;
        }
    }
}
