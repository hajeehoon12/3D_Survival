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
        return craftWindow.activeInHierarchy; // 켜져있는지 확인
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
    public void SelectItem(int index) // 아이템 선택 시
    {
        if (slots[index].item == null) return; // 아이템이 없는 슬롯을 클릭했을 경우

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
        //제작 시, 재료 아이템이 사라지고 인벤에 제작한 아이템이 들어오도록 해야함
        Craft(selectedItem);
    }

    void Craft(ItemData data)
    {
        Instantiate(data.dropPrefab,dropPosition.position, Quaternion.identity);
    }

}
