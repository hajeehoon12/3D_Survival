using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    Equipable,
    Consumable,
    Resource,
    Buff,
    Permanent
}

public enum ConsumableType
{ 
    Health,
    Hunger,
    Stamina,
    Speed
}

public enum MaterialType
{
    Wood,
    Rock,
    Carrot,
    Ham,
    Water
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

public class ItemDataMaterial
{
    public MaterialType type;
    public float value;
}




[CreateAssetMenu(fileName = "Item" , menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public string materialName;
    public string materialValue;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Material")]
    public ItemDataMaterial[] materials;

    [Header("Equip")]
    public GameObject equipPrefab;





}
