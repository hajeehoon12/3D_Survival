using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon; // (Vector 90 0 0 ) �ش� X Rotation ���� ����

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

    public GameObject LightOff;


    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

   
    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        if(time > 0.75f || time <0.25f) LightOff.SetActive(false);
        else LightOff.SetActive(true);

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); // Environment Light
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // Reflection Light


    }


    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    { 
        float intensity = intensityCurve.Evaluate(time);
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        // �ش� 360 *0.25 = 90���̱⿡ ���� ���� 90���� ���ְ� moon �� 270���̱⿡ 0.75f ( noon�� 90���̱⿡ 360��������� *4
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
