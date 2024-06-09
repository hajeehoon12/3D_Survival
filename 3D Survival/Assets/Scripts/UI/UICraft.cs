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
    public UIInventory inventory;
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
        UpdateUI();
        ClearSelectedCraftWindow();
        

        craftWindow.SetActive(false);
    }

    void Update()
    {

    }

    void ClearSelectedCraftWindow()
    {
        selectedItemName.text = string.Empty;
        selectedDescription.text = string.Empty;
        //selectedMaterialName.text = string.Empty;
        //selectedMaterialValue.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        CraftButton.SetActive(false);
    }

    public void Toggle()
    {
        if (inventory.gameObject.activeSelf) inventory.gameObject.SetActive(false);

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

        selectedMaterialName.text = "Need";
        selectedMaterialValue.text = "Amount";
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.craftIngredient.Length; i++)
        {
            selectedStatName.text += selectedItem.craftIngredient[i].name + "\n";
            selectedStatValue.text += selectedItem.craftIngredient[i].amount + "\n";
        }
        CraftButton.SetActive(true);
    }

    public void OnCraftButton() // if you click CraftButton
    {
        Craft(selectedItem);
    }

    void Craft(ItemData data)
    {
        for (int i = 0; i < selectedItem.craftIngredient.Length; i++)
        {
            if (!GameManager.instance.CanConsumeItem(selectedItem.craftIngredient[i].name, selectedItem.craftIngredient[i].amount))
            {
                AudioManager.instance.PlaySFX("Cant", 0.5f);
                return;
            }
        }
        for (int i = 0; i < selectedItem.craftIngredient.Length; i++)
        {
            GameManager.instance.ConsumeItem(selectedItem.craftIngredient[i].name, selectedItem.craftIngredient[i].amount);
        }

        AudioManager.instance.PlaySFX("Craft", 0.5f);
        inventory.UpdateUI();

        GameObject craftedItem = Instantiate(data.dropPrefab,dropPosition.position, Quaternion.identity);
        craftedItem.GetComponent<Rigidbody>().AddForce((Vector3.forward * 2f  + Vector3.up * 2f) * craftedItem.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
        Toggle();
    }

}
