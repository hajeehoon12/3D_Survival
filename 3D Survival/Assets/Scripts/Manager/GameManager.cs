using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    public UIInventory inventory;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //private void Update()
    //{
    //if (inventory.slots[0]?.item?.displayName != null)
    //Debug.Log(inventory.slots[0]?.item?.displayName);
    //}
    //private void Update()
    //{
        //Debug.Log(CanConsumeItem("Carrot", 1));
    //}

    
    //("carrot" , 3) , ("Wood" , 4)
        //(carrot wood) (3,4)

    public bool CanConsumeItem(string Item_Name, int Item_Amount) // if having target item? return true else false
    {

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            ItemData temp;
            //inventory.slots[i]?.item?.displayName
            temp = inventory.slots[i]?.item;
            if (temp?.displayName == null) continue;

            if (temp.displayName == Item_Name)
            {
                if (!temp.canStack)
                    Item_Amount -= 1;
                else
                    Item_Amount -= inventory.slots[i].quantity;
                if (Item_Amount <= 0) return true;
            }
        }
        if (Item_Amount <= 0) return true;
        Debug.Log("You don't have enough resource");
        return false;
    }

    public void ConsumeItem(string Item_Name, int Item_Amount) // consume target item
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (Item_Amount == 0) return;
            if (inventory.slots[i]?.item?.displayName == null) continue;

            if (inventory.slots[i].item.displayName == Item_Name)
            {
                if (!inventory.slots[i].item.canStack)
                {
                    Item_Amount -= 1;
                    inventory.slots[i].item = null;
                }
                else
                {
                    if (inventory.slots[i].quantity > Item_Amount)
                    {
                        inventory.slots[i].quantity -= Item_Amount;
                        return;
                    }
                    else
                    {
                        Item_Amount -= inventory.slots[i].quantity;
                        inventory.slots[i].item = null;
                    }
                    if (Item_Amount == 0) return;

                } 
            }
        }
    }



}
