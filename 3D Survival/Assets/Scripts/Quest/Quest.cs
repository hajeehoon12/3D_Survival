using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Quest : MonoBehaviour, IInteractable
{

    public static Quest Instance;

    public GameObject[] _Tombs;
    public GameObject Tombs;
    public GameObject _Zombie;
    public ItemData _rewardItem;
    private int targetNum = 0;
    private int MaxTargetNum;

    private void Awake()
    {
        Instance = this;
    }


    public void QuestStart()
    {
        MaxTargetNum = _Tombs.Length;
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

    public void RewardItem()
    {
        CharacterManager.Instance.Player.itemData = _rewardItem;
        CharacterManager.Instance.Player.controller.uiInventory.AddItem();
    }

    private void CoffinDown()
    {
        AudioManager.instance.PlaySFX("ScaryBird", 0.5f);
        transform.DOMoveY(transform.position.y - 4 , 3f).onComplete += StartTombAnimation;
    }

    private void StartTombAnimation()
    {
        AudioManager.instance.PlaySFX("ScaryTitle", 0.5f);
        AudioManager.instance.PlayBGM("Tapdance", 0.3f);
        
        Tombs.transform.DOScale(2, 5f);
        Tombs.transform.DOMoveY(-2.5f, 5f);
        Tombs.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 180 , 0), 5f).OnComplete(() =>
        {
            for (int i = 0; i < _Tombs.Length; i++)
            {
                GameObject zombie = Instantiate(_Zombie, _Tombs[i].transform.position, Quaternion.identity);
                zombie.transform.position += zombie.transform.forward * 0.5f + new Vector3(0, -2, 0);
                zombie.transform.DOMoveY(0.6f, 3f);
                
            }
            AudioManager.instance.PlaySFX("ScaryBell",0.5f);
        }
        );

       
    }

    private void EndTombAnimation()
    {
        Tombs.transform.DOScale(0.33f, 5f);
        Tombs.transform.DOMoveY(-2.37f, 5f);
        Tombs.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 180, 0), 5f);

    }

    public void TargetMission()
    {
        targetNum += 1;
        if (targetNum == MaxTargetNum) EndQuest();

    }

    private void EndQuest()
    {
        EndTombAnimation();
        AudioManager.instance.PlayBGM("Peace", 0.2f);
        RewardItem();
    }

    public void OnInteract()
    {
        QuestStart();
    }
}
