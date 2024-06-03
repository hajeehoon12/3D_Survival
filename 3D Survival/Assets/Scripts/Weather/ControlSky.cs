using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSky : MonoBehaviour
{
    public Material dayMat;
    public Material nightMat;
    public GameObject dayLight;
    public GameObject nightLight;

    public Material snowyMat;
    public Material rainyMat;
    public GameObject snowyLight;
    public GameObject rainyLight;

    public Material sunsetMat;
    public Material sunriseMat;
    public GameObject sunsetLight;
    public GameObject sunriseLight;

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
    }
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 2.0f);
    }

    void OnGUI()
    {



        if (GUI.Button(new Rect(5, 5, 80, 20), "Day")) {
            RenderSettings.skybox = dayMat;
            RenderSettings.fogColor = dayFog;
            dayLight.SetActive(true);
            nightLight.SetActive(false);
            snowyLight.SetActive(false);
            rainyLight.SetActive(false);
            sunsetLight.SetActive(false);
            sunriseLight.SetActive(false);
        }

        if (GUI.Button(new Rect(5, 35, 80, 20), "Night"))
        {
            RenderSettings.skybox = nightMat;
            RenderSettings.fogColor = nightFog;
            dayLight.SetActive(false);
            nightLight.SetActive(true);
            snowyLight.SetActive(false);
            rainyLight.SetActive(false);
            sunsetLight.SetActive(false);
            sunriseLight.SetActive(false);
        }

        if (GUI.Button(new Rect(5, 65, 80, 20), "Snowy"))
        {
            RenderSettings.skybox = snowyMat;
            RenderSettings.fogColor = snowyFog;
            dayLight.SetActive(false);
            nightLight.SetActive(false);
            snowyLight.SetActive(true);
            rainyLight.SetActive(false);
            sunsetLight.SetActive(false);
            sunriseLight.SetActive(false);
        }
        if (GUI.Button(new Rect(5, 95, 80, 20), "Rainy"))
        {
            RenderSettings.skybox = rainyMat;
            RenderSettings.fogColor = rainyFog;
            dayLight.SetActive(false);
            nightLight.SetActive(false);
            snowyLight.SetActive(false);
            rainyLight.SetActive(true);
            sunsetLight.SetActive(false);
            sunriseLight.SetActive(false);
        }

        if (GUI.Button(new Rect(5, 125, 80, 20), "Sunset"))
        {
            RenderSettings.skybox = sunsetMat;
            RenderSettings.fogColor = sunsetFog;
            dayLight.SetActive(false);
            nightLight.SetActive(false);
            snowyLight.SetActive(false);
            rainyLight.SetActive(false);
            sunsetLight.SetActive(true);
            sunriseLight.SetActive(false);
        }
        if (GUI.Button(new Rect(5, 155, 80, 20), "Sunrise"))
        {
            RenderSettings.skybox = sunriseMat;
            RenderSettings.fogColor = sunriseFog;
            dayLight.SetActive(false);
            nightLight.SetActive(false);
            snowyLight.SetActive(false);
            rainyLight.SetActive(false);
            sunsetLight.SetActive(false);
            sunriseLight.SetActive(true);
        }
    }

    
}
