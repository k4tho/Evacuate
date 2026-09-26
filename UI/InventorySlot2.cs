using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot2 : MonoBehaviour
{
    private string inventoryTag;
    private List<GameObject> inventoryList;

    public Image itemImageUI;      
    public Text quantityText;  

    void Awake()
    {
        inventoryList = new List<GameObject>();
        ResetInventory();
    }

    // Get first item in list unless there is nothing; They are all of the same type
    public GameObject GetItem()
    {
        if (inventoryList.Count == 0)
        {
            return null;
        }
        else
        {
            return inventoryList[0];
        }
    }

    public string GetInventoryTag()
    {
        return inventoryTag;
    }

    public void SetItem(GameObject item)
    {
        if (item == null)
        {
            ResetInventory();
        }
        else
        {
            inventoryList.Add(item);
            inventoryTag = item.tag;

            // Set item image from Collectible
            Sprite sprite = item.GetComponent<Collectible>()?.itemImage;
            if (sprite != null)
            {
                itemImageUI.sprite = sprite;
                itemImageUI.enabled = true;
            }

            SetItemQuantityText(inventoryList.Count.ToString());
        }
    }

    public void AddMoreItems(GameObject item)
    {
        inventoryList.Add(item);
        SetItemQuantityText(inventoryList.Count.ToString());
    }

    public void RemoveItems(int count)
    {
        for (int i = count; i > 0; i--)
        {
            GameObject target = inventoryList[i - 1];
            inventoryList.RemoveAt(i - 1);
            Destroy(target);
        }
        UpdateSlotInfo();
    }

    public List<GameObject> GetListOfItemsInSlot()
    {
        return inventoryList;
    }

    private void ResetInventory()
    {
        inventoryTag = "";
        inventoryList.Clear();

        itemImageUI.sprite = null;
        itemImageUI.enabled = false;

        SetItemQuantityText("");
    }

    private void SetItemQuantityText(string input)
    {
        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out _))
        {
            quantityText.text = "x" + input;
        }
        else
        {
            quantityText.text = "";
        }
    }

    public void UpdateSlotInfo()
    {
        if (inventoryList.Count == 0)
        {
            ResetInventory();
        }
        else
        {
            SetItemQuantityText(inventoryList.Count.ToString());
        }
    }
}
