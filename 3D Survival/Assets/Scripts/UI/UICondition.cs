using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{

    public Condition health;
    public Condition hunger;
    public Condition thirsty;
    public Condition stamina;


    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    
    void Update()
    {
        
    }
}
