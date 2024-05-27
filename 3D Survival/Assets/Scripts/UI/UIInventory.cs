using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
    }

    void Update()
    {
        
    }

    void ClearSelectedItemWindow() // 인벤창 켜질 때 정보 초기화
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
        if (isOpen()) inventoryWindow.SetActive(false);
        else inventoryWindow.SetActive(true);
    }

    public bool isOpen()
    {
        return inventoryWindow.activeInHierarchy; // 켜져있는지 확인
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;



        if (data.canStack) // 스택 가능하고
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null) // 가지고 있었는 경우 +1
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null) // 스택이 가능하지만 가지고 있지 않거나 스택이 아닌 경우
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        // 슬롯이 꽉차있는 경우
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null; // 지금 아이템 정보값 초기화

    }

    void UpdateUI()
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


    ItemSlot GetItemStack(ItemData data) // 수량 아이템 인벤에 있는지 찾기
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount) // maxCount 를 넘기면 null반환해서 못먹게
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot() // 인벤 슬롯 빈공간 찾기
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
        // 게임 오브젝트, 위치, 각도
    }

    public void SelectItem(int index) // 아이템 선택 시
    {
        if (slots[index].item == null) return; // 아이템이 없는 슬롯을 클릭했을 경우

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

        // 버튼 활성화

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped ); // 장착이 안되있을 때
        unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped); // 장착이 되있을 때
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

    public void OnDropButton() // DropButton 누를시
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

    void RemoveSelectedItem() // 사용 혹은 버리기 시 아이템 수량 한개 제거 로직 실행
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

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;

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
