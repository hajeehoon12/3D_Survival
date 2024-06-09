using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    Equipable,
    Consumable,
    Resource,
    Buff,
    Permanent,
    Construct,
    Upgrade
}

public enum ConsumableType
{ 
    Health,
    Hunger,
    Drink,
    Stamina,
    Speed
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[System.Serializable]
public class CraftDict
{
    public string name;
    public int amount;
}



[CreateAssetMenu(fileName = "Item" , menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Construct")]
    public GameObject constructObject;
    public GameObject virtualObjectGreen;
    public float heightOfBuilding;
    public int woodNeedAmount;
    public int rockNeedAmount;

    [Header("Craft")]
    public bool canCraft;
    public CraftDict[] craftIngredient;


}
