using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeTime;
    public float maxVolume;
    private float targetVolume;

    private bool monsterDie = false;

    void Start()
    {
        targetVolume = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = targetVolume;
        audioSource.Play();
        
    }


    void Update()
    {

        if (!Mathf.Approximately(audioSource.volume, targetVolume) && !monsterDie) // Fade Volume Up
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, (maxVolume / fadeTime) * Time.deltaTime);
        }
    }

    public IEnumerator VolumeDown() // Slow Volume Down
    {
        float time = 0f;

        monsterDie = true;

        while (time < 3)
        {
            time += Time.deltaTime;
            audioSource.volume -= Time.deltaTime * targetVolume / 3;
            Debug.Log(time);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        Debug.Log("Music End");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = 0f;
        }
    }


}
