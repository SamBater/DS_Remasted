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
    public Item item;
    public Image icon;
    [SerializeField]
    public int count;
    public TextMeshProUGUI countText;
    private Transform parentTransform;
    private void Awake()
    {
        icon = GetComponent<Image>();
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        parentTransform = transform.parent;
    }

    public void Fresh(Item _item,int _count)
    {
        item = _item;
        count = _count;
        icon.sprite = item.icon ? item.icon : null;
        if(count > 1)
            countText.text = count.ToString();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTransform.position = icon.transform.position;
        transform.parent = parentTransform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if(item == null) return;
        icon.transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        Slot slot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>();
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        transform.parent = parentTransform;
        icon.transform.position = parentTransform.transform.position;
        if (slot != null)
        {
            int newIndex = slot.transform.GetSiblingIndex();
            int oldIndex = parentTransform.GetSiblingIndex();
            
            slot.transform.SetSiblingIndex(oldIndex);
            transform.parent.SetSiblingIndex(newIndex);
            
            //改变在数组中记录的位置
        }
        else
        {

        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
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
        if (eventData.clickCount == 2)
        {

        }
    }
}
