using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UICraft : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject craftWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedDescription;
    public TextMeshProUGUI selectedMaterialName;
    public TextMeshProUGUI selectedMaterialValue;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject CraftButton;


    private PlayerController controller;
    private PlayerCondition condition;
    UIInventory inventory;
    Player player;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    private void Awake()
    {
        
    }


    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].craft = this;
        }
        ClearSelectedCraftWindow();
        UpdateUI();

        craftWindow.SetActive(false);
    }

    void Update()
    {

    }

    void ClearSelectedCraftWindow()
    {
        selectedItemName.text = string.Empty;
        selectedDescription.text = string.Empty;
        selectedMaterialName.text = string.Empty;
        selectedMaterialValue.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        CraftButton.SetActive(false);
    }

    public void Toggle()
    {
        if (craftWindow.activeSelf)
        {
            craftWindow.SetActive(false);
            AudioManager.instance.PlaySFX("InventoryOff");
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            controller.canLook = true;
        }
        else
        {
            AudioManager.instance.PlaySFX("Inventory");
            craftWindow.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            controller.canLook = false;
        }
    }

    public bool isOpen()
    {
        //Debug.Log(inventoryWindow.activeInHierarchy);
        return craftWindow.activeInHierarchy; // �����ִ��� Ȯ��
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
    public void SelectItem(int index) // ������ ���� ��
    {
        if (slots[index].item == null) return; // �������� ���� ������ Ŭ������ ���

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedDescription.text = selectedItem.description;

        selectedMaterialName.text = string.Empty;
        selectedMaterialValue.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }
        CraftButton.SetActive(true);
    }

    public void OnCraftButton()
    {
        //���� ��, ��� �������� ������� �κ��� ������ �������� �������� �ؾ���
        Craft(selectedItem);
    }

    void Craft(ItemData data)
    {
        Instantiate(data.dropPrefab,dropPosition.position, Quaternion.identity);
    }

}
