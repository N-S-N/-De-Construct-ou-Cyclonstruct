using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventary : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject Inventory;
    public List<Slot> InventorySlot =  new List<Slot>();
    [SerializeField] TMP_Text ItemHoverText;

    [Header("Raycast")]
    [SerializeField] float RayCastDistance = 5;
    [SerializeField] LayerMask itemLayer;

    public void Start()
    {
        toggleInventory(false);

        foreach (Slot uislot in InventorySlot)
        {
            uislot.inistialiseSlot();
        }
    }
    public void Update()
    {
        itemRaycast(Input.GetKeyDown(KeyCode.E));

        if (Input.GetKeyDown(KeyCode.I))
            toggleInventory(!Inventory.activeInHierarchy);
    }

    private void itemRaycast(bool hasCliced = false)
    {
        ItemHoverText.text = "";
        RaycastHit2D m_HitDetect = Physics2D.CircleCast(transform.position, RayCastDistance, Vector2.zero, 0, itemLayer);

        if (m_HitDetect) 
        {
            if (m_HitDetect.collider != null)
            {
                Item newItem = m_HitDetect.collider.GetComponent<Item>();
                if (hasCliced) //Pick up
                {
                    if (newItem)
                    {
                        addItemInventory(newItem);
                    }
                }
                else //Get the Name
                {
                    if (newItem)
                    {
                        ItemHoverText.text = newItem.Name;
                    }
                }
            }
        }
    }

    private void addItemInventory(Item itemToAdd)
    {
        int leftoverQuantity = itemToAdd.currentQuantity;
        Slot openSholt = null;
        for(int i = 0; i < InventorySlot.Count; i++)
        {
            Item heldItem = InventorySlot[i].getItem();

            if (heldItem != null && itemToAdd.ID == heldItem.ID)
            {
                int freeSpaceInSlot = heldItem.MaxQuabttity - heldItem.currentQuantity;
                if (freeSpaceInSlot >= leftoverQuantity)
                {
                    heldItem.currentQuantity += leftoverQuantity;
                    Destroy(itemToAdd.gameObject);
                    InventorySlot[i].UpdateData();
                    return;
                }
                else// DD as much as we can to the currest Slot
                {
                    heldItem.currentQuantity = heldItem.MaxQuabttity;
                    leftoverQuantity -= freeSpaceInSlot;
                }
            }
            else if (heldItem == null)
            {
                if(!openSholt)
                    openSholt = InventorySlot[i];
            }

            InventorySlot[i].UpdateData();
        }

        if(leftoverQuantity > 0 && openSholt)
        {
            openSholt.SetItem(itemToAdd);
            itemToAdd.currentQuantity = leftoverQuantity;
            itemToAdd.gameObject.SetActive(false);
        }
        else
        {
            itemToAdd.currentQuantity = leftoverQuantity;
        }
    }
    private void toggleInventory(bool enable)
    {
        Inventory.SetActive(enable);

    }
}
