using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image weaponIconL;
    public Image weaponIconR;
    public Image itemIcon;
    public Text itemName;
    public Text soulsBar;
    public GameObject itemTips;
    public Image itemOnGroundIcon;
    public Text itemOnGroundName;
    public Text itemOnGroundCount;
    public static UIManager instance;
    
    private void Awake() 
    {
        if(instance) Destroy(this);
        else
        {
            instance = this;
        }
    }

    public void UpdateWeaponIcon(Sprite icon,bool rh)
    {
        if(rh) weaponIconR.sprite = icon;
        else weaponIconL.sprite = icon;
    }

    public void UpdateItemIcon(ItemEnum id)
    {
        Item newItem = ItemFactory.GetItem(id);
        itemIcon.sprite = newItem.sprite;
        itemName.text   = newItem.itemName;
    }

    public void UpdateSoulsBar()
    {
        
    }

    public void ShowItemOnGround(ItemEnum item,int count)
    {
        StartCoroutine(ShowTheItemTips(item,count));
    }

    public void ShowItemOnGround(List<ItemEnum> items,List<int> counts)
    {
        StartCoroutine(ShowTheItemTips(items,counts));
    }

    IEnumerator ShowTheItemTips(ItemEnum item,int count)
    {
        itemOnGroundIcon.sprite = ItemFactory.GetItem(item).sprite;
        itemOnGroundName.text = ItemFactory.GetItem((int)item).itemName;
        itemOnGroundCount.text = count.ToString();
        itemTips.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        itemTips.SetActive(false);
    }

    IEnumerator ShowTheItemTips(List<ItemEnum> item,List<int> count)
    {
        for(int i=0;i<item.Count;i++)
        {
            itemOnGroundIcon.sprite = ItemFactory.GetItem(item[i]).sprite;
            itemOnGroundName.text = ItemFactory.GetItem((int)item[i]).itemName;
            itemOnGroundCount.text = count[i].ToString();
            itemTips.SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }
        itemTips.SetActive(false);
    }
}
