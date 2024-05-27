using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using System;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;
    public GameObject dropItem;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    private int curEquipIndex;

    private void Awake()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        
    }


    private void Start()
    {
        //controller.inventory += Toggle; ��������
        

        
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
        UpdateUI();
        //inventoryWindow.SetActive(false);
        //Toggle();
    }

    void Update()
    {
        
    }

    void ClearSelectedItemWindow() // �κ�â ���� �� ���� �ʱ�ȭ
    {
        selectedItemName.text = string.Empty;
        selectedDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);   
    }

    public void Toggle()
    {
        if (inventoryWindow.activeSelf)
        {
            inventoryWindow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            controller.canLook = true;
        }
        else
        {
            inventoryWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            
            controller.canLook = false;
        }
    }

    public bool isOpen()
    {
        //Debug.Log(inventoryWindow.activeInHierarchy);
        return inventoryWindow.activeInHierarchy; // �����ִ��� Ȯ��
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;



        if (data.canStack) // ���� �����ϰ�
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null) // ������ �־��� ��� +1
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null) // ������ ���������� ������ ���� �ʰų� ������ �ƴ� ���
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        // ������ �����ִ� ���
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null; // ���� ������ ������ �ʱ�ȭ

    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }


    ItemSlot GetItemStack(ItemData data) // ���� ������ �κ��� �ִ��� ã��
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount) // maxCount �� �ѱ�� null��ȯ�ؼ� ���԰�
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot() // �κ� ���� ����� ã��
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) return slots[i];
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        dropItem = Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
        dropItem.GetComponent<Rigidbody>().AddForce(Vector3.forward * 2f + Vector3.up * 2f, ForceMode.Impulse);
        // ���� ������Ʈ, ��ġ, ����
    }

    public void SelectItem(int index) // ������ ���� ��
    {
        if (slots[index].item == null) return; // �������� ���� ������ Ŭ������ ���

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        // ��ư Ȱ��ȭ

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped ); // ������ �ȵ����� ��
        unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped); // ������ ������ ��
        dropButton.SetActive(true);


    }

    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton() // DropButton ������
    {
        if (curEquipIndex == selectedItemIndex)
        { 
            if (slots[curEquipIndex].equipped)
            {
                UnEquip(curEquipIndex);
            }
        }
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem() // ��� Ȥ�� ������ �� ������ ���� �Ѱ� ���� ���� ����
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void OnFastEquip() // Fast Equip
    {

        for (int i = 0; i < slots.Length; i++) // First Place can Equippable armor on
        {
            selectedItem = slots[i].item;
            selectedItemIndex = i;
            if (selectedItem != null)
            {
                if (selectedItem.type == ItemType.Equipable)
                {
                    OnEquipButton();
                    AudioManager.instance.PlaySFX("SwordToHand");
                    return;
                }
            }
        }
        selectedItem = null;  // when it is no equippable object in inventory initiate
        selectedItemIndex = 0;
    }



    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;

        AudioManager.instance.PlaySFX("SwordToHand");

        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

}
