using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSky : MonoBehaviour
{
    public Material dayMat;
    public Material nightMat;

    public GameObject dayLight;
    public GameObject dayEffect;
    public GameObject nightLight;
    public GameObject nightEffect;

    public Material snowyMat;
    public Material rainyMat;

    public GameObject snowyLight;
    public GameObject snowyEffect;
    public GameObject rainyLight;
    public GameObject rainyEffect;

    public Material sunsetMat;
    public Material sunriseMat;


    public GameObject sunsetLight;
    public GameObject sunsetEffect;
    public GameObject sunriseLight;
    public GameObject sunriseEffect;

    public Color dayFog;
    public Color nightFog;

    public Color snowyFog;
    public Color rainyFog;

    public Color sunsetFog;
    public Color sunriseFog;

    void Start()
    {
        RenderSettings.skybox = dayMat;
        RenderSettings.fogColor = dayFog;


        dayLight.SetActive(true);
        nightLight.SetActive(false);
        snowyLight.SetActive(false);
        rainyLight.SetActive(false);
        sunsetLight.SetActive(false);
        sunriseLight.SetActive(false);

        dayEffect.SetActive(true);
        nightEffect.SetActive(false);
        snowyEffect.SetActive(false);
        rainyEffect.SetActive(false);
        sunsetEffect.SetActive(false);
        sunriseEffect.SetActive(false);

    }
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 2.0f);
    }

    void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }

    public void IfDay()
    {
        RenderSettings.skybox = dayMat;
        RenderSettings.fogColor = dayFog;
        dayLight.SetActive(true);
        nightLight.SetActive(false);
        snowyLight.SetActive(false);
        rainyLight.SetActive(false);
        sunsetLight.SetActive(false);
        sunriseLight.SetActive(false);

        dayEffect.SetActive(true);
        nightEffect.SetActive(false);
        snowyEffect.SetActive(false);
        rainyEffect.SetActive(false);
        sunsetEffect.SetActive(false);
        sunriseEffect.SetActive(false);
        AudioManager.instance.StopBGM2();
    }

    public void IfNight()
    {
        RenderSettings.skybox = nightMat;
        RenderSettings.fogColor = nightFog;
        dayLight.SetActive(false);
        nightLight.SetActive(true);
        snowyLight.SetActive(false);
        rainyLight.SetActive(false);
        sunsetLight.SetActive(false);
        sunriseLight.SetActive(false);

        dayEffect.SetActive(false);
        nightEffect.SetActive(true);
        snowyEffect.SetActive(false);
        rainyEffect.SetActive(false);
        sunsetEffect.SetActive(false);
        sunriseEffect.SetActive(false);
        AudioManager.instance.StopBGM2();
    }

    public void IfSnowy()
    {
        RenderSettings.skybox = snowyMat;
        RenderSettings.fogColor = snowyFog;
        dayLight.SetActive(false);
        nightLight.SetActive(false);
        snowyLight.SetActive(true);
        rainyLight.SetActive(false);
        sunsetLight.SetActive(false);
        sunriseLight.SetActive(false);

        dayEffect.SetActive(false);
        nightEffect.SetActive(false);
        snowyEffect.SetActive(true);
        rainyEffect.SetActive(false);
        sunsetEffect.SetActive(false);
        sunriseEffect.SetActive(false);
        AudioManager.instance.StopBGM2();
    }

    public void IfRainy()
    {
        RenderSettings.skybox = rainyMat;
        RenderSettings.fogColor = rainyFog;
        dayLight.SetActive(false);
        nightLight.SetActive(false);
        snowyLight.SetActive(false);
        rainyLight.SetActive(true);
        sunsetLight.SetActive(false);
        sunriseLight.SetActive(false);

        dayEffect.SetActive(false);
        nightEffect.SetActive(false);
        snowyEffect.SetActive(false);
        rainyEffect.SetActive(true);
        sunsetEffect.SetActive(false);
        sunriseEffect.SetActive(false);
        AudioManager.instance.StopBGM2();
        AudioManager.instance.PlayBGM2("RainThunder", 0.5f);
    }
    public void IfSunset()
    {
        RenderSettings.skybox = sunsetMat;
        RenderSettings.fogColor = sunsetFog;
        dayLight.SetActive(false);
        nightLight.SetActive(false);
        snowyLight.SetActive(false);
        rainyLight.SetActive(false);
        sunsetLight.SetActive(true);
        sunriseLight.SetActive(false);

        dayEffect.SetActive(false);
        nightEffect.SetActive(false);
        snowyEffect.SetActive(false);
        rainyEffect.SetActive(false);
        sunsetEffect.SetActive(true);
        sunriseEffect.SetActive(false);
        AudioManager.instance.StopBGM2();
    }

    public void IfSunRise()
    {
        RenderSettings.skybox = sunriseMat;
        RenderSettings.fogColor = sunriseFog;
        dayLight.SetActive(false);
        nightLight.SetActive(false);
        snowyLight.SetActive(false);
        rainyLight.SetActive(false);
        sunsetLight.SetActive(false);
        sunriseLight.SetActive(true);

        dayEffect.SetActive(true);
        nightEffect.SetActive(false);
        snowyEffect.SetActive(false);
        rainyEffect.SetActive(false);
        sunsetEffect.SetActive(false);
        sunriseEffect.SetActive(true);
        AudioManager.instance.StopBGM2();
    }

    void OnGUI()
    {

        if (GUI.Button(new Rect(5, 5, 80, 20), "Day")) 
        {
            IfDay();
        }

        if (GUI.Button(new Rect(5, 35, 80, 20), "Night"))
        {
            IfNight();
        }

        if (GUI.Button(new Rect(5, 65, 80, 20), "Snowy"))
        {
            IfSnowy();
        }
        if (GUI.Button(new Rect(5, 95, 80, 20), "Rainy"))
        {
            IfRainy();
        }

        if (GUI.Button(new Rect(5, 125, 80, 20), "Sunset"))
        {
            IfSunset();
        }
        if (GUI.Button(new Rect(5, 155, 80, 20), "Sunrise"))
        {
            IfSunRise();
        }
    }

    
}
