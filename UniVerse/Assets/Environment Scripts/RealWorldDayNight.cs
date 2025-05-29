using UnityEngine;

public class RealWorldDayNight : MonoBehaviour
{
    public GameObject Sun;
    public float SunRiseTime = 7;
    public float SunSetTime = 19;
    
    [Header("Skybox Settings")]
    public Material daySkybox;
    public Material nightSkybox;
    public float skyboxTransitionDuration = 1.0f; // in hours
    
    private int CurrentHour;
    private float CurrentMinute;
    private Vector3 eulerRotation;
    private float TimeIntoDay;
    private float SolarDayLength;
    private float SolarNightLength;
    private bool isDayTime = true;
    private float transitionProgress = 0f;

    void Start()
    {
        // Set initial sun rotation
        eulerRotation = Sun.transform.rotation.eulerAngles;
        
        // Initialize skybox based on current time
        UpdateSkybox();
    }

    private void FixedUpdate()
    {
        // Update time once per frame
        CurrentHour = System.DateTime.Now.Hour;
        CurrentMinute = System.DateTime.Now.Minute;

        SunTime(CurrentHour, CurrentMinute);
        UpdateSkybox();
    }

    void SunTime(int Hour, float Minute)
    {
        SolarDayLength = SunSetTime - SunRiseTime;
        SolarNightLength = 24 - SolarDayLength;
        TimeIntoDay = Hour + Minute / 60;

        if (TimeIntoDay < SunRiseTime) // Before sunrise
        {
            Sun.transform.rotation = Quaternion.Euler(TimeIntoDay * 90 / SolarNightLength + 270, eulerRotation.y, eulerRotation.z);
            isDayTime = false;
        }
        else if(TimeIntoDay > SunSetTime) // After sunset
        {
            var SolarNightTime = TimeIntoDay - SunSetTime;
            Sun.transform.rotation = Quaternion.Euler(SolarNightTime * 90 / SolarNightLength + 180, eulerRotation.y, eulerRotation.z);
            isDayTime = false;
        }
        else // During daylight
        {
            var SolarDayTime = TimeIntoDay - SunRiseTime;
            Sun.transform.rotation = Quaternion.Euler(SolarDayTime * 180 / SolarDayLength, eulerRotation.y, eulerRotation.z);
            isDayTime = true;
        }
    }

    void UpdateSkybox()
    {
        // Calculate transition time (morning/evening)
        bool shouldBeDay = TimeIntoDay > (SunRiseTime - skyboxTransitionDuration/2) && 
                          TimeIntoDay < (SunSetTime + skyboxTransitionDuration/2);
        
        // Smooth transition between skyboxes
        if (shouldBeDay && !isDayTime)
        {
            // Transition to day skybox
            transitionProgress = Mathf.Clamp01(transitionProgress + Time.deltaTime / (skyboxTransitionDuration * 3600));
            RenderSettings.skybox.Lerp(nightSkybox, daySkybox, transitionProgress);
        }
        else if (!shouldBeDay && isDayTime)
        {
            // Transition to night skybox
            transitionProgress = Mathf.Clamp01(transitionProgress + Time.deltaTime / (skyboxTransitionDuration * 3600));
            RenderSettings.skybox.Lerp(daySkybox, nightSkybox, transitionProgress);
        }
        else
        {
            // Fully set the appropriate skybox
            RenderSettings.skybox = isDayTime ? daySkybox : nightSkybox;
            transitionProgress = 0f;
        }
        
        // Ensure skybox updates in real-time
        DynamicGI.UpdateEnvironment();
    }
}