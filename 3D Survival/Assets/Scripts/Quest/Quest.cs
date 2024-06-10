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
        Debug.Log("Quest Start!!");
    
    }

    public (string, string) GetInteractPrompt()
    {
        string str = $"�������� ��";
        string str1 = $"�ұ��� ����� ���糪���� �ִ� ��г��� ���̴�.";
        return (str, str1);
    }

    private void CoffinDown()
    {
        transform.DOMoveY(transform.position.y - 4 , 3f).onComplete += TombAnimation;
    }

    private void TombAnimation()
    {
        Tombs.transform.DOScale(1, 10f);
        Tombs.transform.DOMoveY(-1.2f, 10f);
        Tombs.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 180 , 0), 10f);
    }

    public void OnInteract()
    {
        QuestStart();
    }
}
