using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ItemOnSlot : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,ISaveable,IPointerClickHandler
{
    [SerializeField]
    private Item item;
    public Image icon;

    public Item Holder
    {
        get { return item; }
        set
        {
            item = value;
            if(icon == null) icon = GetComponent<Image>();
            icon.sprite = value != null ? value.icon : null;
        }
    }
    [SerializeField]
    private int count;

    public TextMeshProUGUI countText;
    private Transform parentTransform;
    public int Count
    {
        get { return count; }
        set 
        { 
            count = value; 
            if(countText == null) countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>(); 
            countText.SetText(value > 1 ? value.ToString() : "");
        }
    }

    private void Start()
    {
        icon = GetComponent<Image>();
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        parentTransform = transform.parent;
    }

    public bool IsEmpty()
    {
        return item == null;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTransform.position = icon.transform.position;
        transform.SetParent(parentTransform.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if(item == null) return;
        icon.transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentTransform);
        icon.transform.position = parentTransform.transform.position;
        Slot slot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>();

        //交换slot 位置
        if (slot != null)
        {        
            ItemOnSlot target = slot.itemOnSlot;
            Slot slotSource = GetComponentInParent<Slot>();
            
            InventoryType thisSlotType = GetComponentInParent<Slot>().GetInventoryType();
            InventoryType targetSlotType = slot.GetInventoryType();
        
            InventorySlotsManager ismSource = GetComponentInParent<Slot>().GetInventorySlotsManager();
            InventorySlotsManager ismDest = slot.GetInventorySlotsManager(); 

            int newIndex = target.transform.GetSiblingIndex();
            int oldIndex = parentTransform.GetSiblingIndex();
            
            
            //同槽内的交换
            if(thisSlotType == targetSlotType)
                Swap(target);
            
            //从其他槽拖拽消耗品到QuickUse
            else if (item.GetItemType() == ItemType.Consumable && targetSlotType == InventoryType.QuickUseSlots)
            {
                ismDest.inventoryUI.im.AddQuickUse(item.GetID(),slot.GetIndex());
                ismDest.inventoryUI.allItemPos.Remove(item);
                slotSource.Clear();
                //清楚消耗品索引
            }
            
            //拖着装备Item
            else if (item.GetItemType() == ItemType.Weapon && targetSlotType == InventoryType.RWeaponsSlots)
            {
                ismDest.inventoryUI.im.am.wm.EquipWeapon((WeaponItem)item,slot.GetIndex(),true);
                ismDest.inventoryUI.allItemPos.Remove(item);
                slotSource.Clear();
            }
            
            else if (item.GetItemType() == ItemType.Weapon && targetSlotType == InventoryType.LWeaponsSlots)
            {
                ismDest.inventoryUI.im.am.wm.EquipWeapon((WeaponItem)item,slot.GetIndex(),false);
                ismDest.inventoryUI.allItemPos.Remove(item);
                slotSource.Clear();
            }
            

            // InventoryUI inventoryUI = GetComponentInParent<Slot>().gameObject.GetComponentInParent<InventoryUI>();
            // if (inventoryUI.allItemToggle.isOn)
            // {
            //     inventoryUI.SwapPos(inventoryUI.allItemPos,target.itemOnSlot.Holder,item,newIndex,oldIndex);
            // }
            // else if (inventoryUI.weaponToggle.isOn)
            // {
            //     inventoryUI.SwapPos(inventoryUI.weaponItemPos,target.itemOnSlot.Holder,item,newIndex,oldIndex);
            // }
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void Swap(ItemOnSlot target)
    {
        Item targetItem = target.Holder;
        int count = target.Count;
        target.Holder = Holder;
        target.Count = Count;
        Holder = targetItem;
        Count = count;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemDetailPanel.GetInstance().SetItem(this.item);
        }
    }

    public void PopulateSaveData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(item == null) return;
        ItemClickMenu itemClickMenu  = ItemClickMenu.GetInstance();
        itemClickMenu.transform.position = eventData.position;
        itemClickMenu.onPactItem = this;
        itemClickMenu.gameObject.SetActive(true);
        switch (item.GetItemType())
        {
            case ItemType.Weapon:
                itemClickMenu.ShowWeapon();
                break;
            case ItemType.Consumable:
                itemClickMenu.ShowConsumable();
                break;
            default:
                Debug.LogError("faild to read ItemType");
                break;
        }
    }
}
