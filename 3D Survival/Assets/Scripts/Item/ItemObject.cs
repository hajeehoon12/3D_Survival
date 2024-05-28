using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public (string,string) GetInteractPrompt();
    public void OnInteract();
}


public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public (string, string) GetInteractPrompt()
    {
        string str = $"{data.displayName}";
        string str1 = $"{data.description}";
        return (str,str1);
    }

    public void OnInteract() // 아이템과 상호작용시 습득
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
