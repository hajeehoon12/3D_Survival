using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    public static DayNightCycle instance;

    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon; // (Vector 90 0 0 ) 해는 X Rotation 값만 적용

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    [Header("Weather Change")]
    public ControlSky _controlSky;

    public GameObject LightOff;


    public float totalTime = 0.25f;
    private float latestTime = 0f;
    public int change = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

   
    void Update()
    {
        totalTime += timeRate * Time.deltaTime;

        

        time = totalTime % 1.0f;

        if (totalTime - latestTime > (1f / 6f))
        {
            latestTime = totalTime;
            WeatherChange();
        }


        if (time > 0.75f || time <0.25f) LightOff.SetActive(false);
        else LightOff.SetActive(true);

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); // Environment Light
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // Reflection Light


    }

    private void WeatherChange()
    {
        change += 1;

        switch (change % 6)
        {
            case 0:
                _controlSky.IfRainy();
                break;
            case 1:
                _controlSky.IfSunRise();
                break;
            case 2:
                _controlSky.IfDay();
                break;
            case 3:
                _controlSky.IfSnowy();
                break;
            case 4:
                _controlSky.IfSunset();
                break;
            case 5:
                _controlSky.IfNight();
                break;
            default:
                _controlSky.IfDay();
                break;
        }
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    { 
        float intensity = intensityCurve.Evaluate(time);
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        // 해는 360 *0.25 = 90도이기에 정오 기준 90도를 뺴주고 moon 은 270도이기에 0.75f ( noon은 90도이기에 360도맞출려고 *4
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }

    }

}
