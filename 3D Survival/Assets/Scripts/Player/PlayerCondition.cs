using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}


public class PlayerCondition : MonoBehaviour, IDamagable
{

    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition thirsty { get { return uiCondition.thirsty; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    public bool playSpeedBuff = false;

    public GameObject neckBuff;

    private Vector3 startPos;

    void Start()
    { 
        neckBuff.SetActive(false);
        startPos = transform.position;
    }

 
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        thirsty.Subtract(thirsty.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (thirsty.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }   
    public void Drink(float amount)
    {
        thirsty.Add(amount);
    }


    public void Die()
    {
        Debug.Log("Die!!");
        transform.position = startPos;
        health.MakeMaxValue();
        hunger.MakeMaxValue();
        thirsty.MakeMaxValue();
        stamina.MakeMaxValue();
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;

        }

        stamina.Subtract(amount);
        return true;
    }

    public void StartBuff(ConsumableType Type, float value)
    {
        Debug.Log("StartBuff!!");
        StartCoroutine(BuffItem(Type, value));
    }



    public IEnumerator BuffItem(ConsumableType Type, float value)
    {
        float time = 0f;
        float timeThreshold = 0.1f;
        float totalTime = 10f;
        //Debug.Log("InCoroutine");
        while (time < totalTime)
        {
            time += timeThreshold;
            switch (Type)
            {

                case ConsumableType.Health:

                    //Debug.Log("Health Buff Restore!");
                    Heal(value / (totalTime / timeThreshold));

                    break;
                case ConsumableType.Hunger:

                    //Debug.Log("Hunger Buff Restore!");
                    Eat(value / (totalTime / timeThreshold));

                    break;
                case ConsumableType.Stamina:

                    //Debug.Log("Stamina Buff Restore!");
                    UseStamina(-value / (totalTime / timeThreshold)); // ���׹̳� ä���

                    break;
                case ConsumableType.Speed:

                    if (!playSpeedBuff)
                    {
                        GetComponent<PlayerController>().SpeedBuff = true;
                        playSpeedBuff = true;
                    }

                    break;
            }
            yield return new WaitForSeconds(timeThreshold);

        }
        //RemoveSelectedItem();
        if (playSpeedBuff) // if it was Speed Buff make buff End
        { 
            playSpeedBuff = false;
            GetComponent<PlayerController>().SpeedBuff = false; 
        }
        Debug.Log("Buff End!!");
    }

    public void NecklaceBuff(ref bool buffon)
    {
        if (!buffon)
        {
            playSpeedBuff = true;
        }
        else
        {
            playSpeedBuff = false;
        }

        playSpeedBuff = !buffon;
        neckBuff.SetActive(!buffon);
        buffon = !buffon;

        
    }



}
