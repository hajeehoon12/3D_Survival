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
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

 
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
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


    public void Die()
    {
        Debug.Log("Die!!");
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
                    UseStamina(-value / (totalTime / timeThreshold)); // 스테미나 채우기
                    break;
            }
            yield return new WaitForSeconds(timeThreshold);
        }
        //RemoveSelectedItem();
        Debug.Log("Buff End!!");

    }

}
