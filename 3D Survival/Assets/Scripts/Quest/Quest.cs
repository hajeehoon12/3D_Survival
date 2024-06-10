using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Quest : MonoBehaviour, IInteractable
{

    public GameObject[] _Tombs;
    public GameObject Tombs;

    private void Start()
    {
        //QuestStart();
    }


    public void QuestStart()
    {
        CoffinDown();
        DayNightCycle.instance._controlSky.IfRainy();
        DayNightCycle.instance.totalTime = 0f;
        Debug.Log("Quest Start!!");
    
    }

    public (string, string) GetInteractPrompt()
    {
        string str = $"꺼림찍한 관";
        string str1 = $"불길한 기운이 스며나오고 있는 기분나쁜 관이다.";
        return (str, str1);
    }

    private void CoffinDown()
    {
        transform.DOMoveY(transform.position.y - 4 , 3f).onComplete += TombAnimation;
    }

    private void TombAnimation()
    {
        Tombs.transform.DOScale(2, 5f);
        Tombs.transform.DOMoveY(-2.5f, 5f);
        Tombs.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 180 , 0), 5f);
    }



    public void OnInteract()
    {
        QuestStart();
    }
}
