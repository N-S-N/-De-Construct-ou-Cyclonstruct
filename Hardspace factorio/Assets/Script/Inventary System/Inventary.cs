using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventary : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject Inventory;
    private List<Slot> allInventorySlot =  new List<Slot>();
    public List<Slot> inventorySlot =  new List<Slot>();
    public List<Slot> hotbarSlots = new List<Slot>();

    [SerializeField] TMP_Text ItemHoverText;

    [Header("Raycast")]
    [SerializeField] float RayCastDistance = 5;
    [SerializeField] LayerMask itemLayer;
    public Transform dropLocation;

    [Header("Drog and Drop")]
    public Image dragInconImage;
    private Item currentDraggedItem;
    private int currentDragSlotIndex = -1;

    [Header("EquippableItems")]
    public List<GameObject> eqippableItems = new List<GameObject>();
    public Transform selectrsItemImage;

    PlayerControler playerControler;
    public void Start()
    {
        playerControler = GetComponent<PlayerControler>();
        toggleInventory(false);

        allInventorySlot.AddRange(inventorySlot);
        allInventorySlot.AddRange(hotbarSlots);
        
        foreach (Slot uislot in allInventorySlot)
        {
            uislot.inistialiseSlot();
        }
    }
    public void Update()
    {
        itemRaycast(Input.GetKeyDown(KeyCode.E));

        if (Input.GetKeyDown(KeyCode.I))
            toggleInventory(!Inventory.activeInHierarchy);
        if(Input.GetKeyDown(KeyCode.Escape))
            toggleInventory(false);

        if(Inventory.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            dragInventoryIcon();
        }
        else if (currentDragSlotIndex != -1 && Input.GetMouseButtonUp(0) || currentDragSlotIndex != -1 && !Inventory.activeInHierarchy)
        {
            dropInventoryIcon();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            dropItem();

        for(int i = 1; i< hotbarSlots.Count+1; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                enableHotBarItem(i-1);
            }
        }

        dragInconImage.transform.position = Input.mousePosition;

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
                        playerControler.interection = true;
                    }
                }
                else //Get the Name
                {
                    if (newItem)
                    {
                        ItemHoverText.text = newItem.Name;
                    }
                    playerControler.interection = false;
                }
            }
        }
        else
        {
            playerControler.interection = false;
        }
    }

    private void addItemInventory(Item itemToAdd)
    {
        int leftoverQuantity = itemToAdd.currentQuantity;
        Slot openSholt = null;
        for(int i = 0; i < allInventorySlot.Count; i++)
        {
            Item heldItem = allInventorySlot[i].getItem();

            if (heldItem != null && itemToAdd.ID == heldItem.ID)
            {
                int freeSpaceInSlot = heldItem.MaxQuabttity - heldItem.currentQuantity;
                if (freeSpaceInSlot >= leftoverQuantity)
                {
                    heldItem.currentQuantity += leftoverQuantity;
                    Destroy(itemToAdd.gameObject);
                    allInventorySlot[i].UpdateData();
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
                    openSholt = allInventorySlot[i];
            }

            allInventorySlot[i].UpdateData();
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

    private void dragInventoryIcon()
    {
        for (int i = 0; i < allInventorySlot.Count; i++)
        {
            Slot curSlot = allInventorySlot[i];
            if(curSlot.Havered && curSlot.hasItem())
            {
                currentDragSlotIndex = i;

                currentDraggedItem = curSlot.getItem();
                dragInconImage.sprite = currentDraggedItem.icone;
                dragInconImage.color = new Color(1,1,1,1);

                curSlot.SetItem(null);
            }
        }
    }

    private void dropItem()
    {
        for (int i = 0; i < allInventorySlot.Count; i++)
        {
            Slot curSlot = allInventorySlot[i];
            if(curSlot.Havered && curSlot.hasItem())
            {
                curSlot.getItem().gameObject.SetActive(true);
                curSlot.getItem().transform.position = dropLocation.position;
                curSlot.SetItem(null);
                break;
            }
        }
    }
    private void dropInventoryIcon()
    {
        dragInconImage.sprite = null;
        dragInconImage.color = new Color(1, 1, 1, 0);

        for (int i = 0; i < allInventorySlot.Count; i++)
        {
            Slot curSlot = allInventorySlot[i];
            if (curSlot.Havered)
            {
                if (curSlot.hasItem())
                {
                    Item itemToSlot = curSlot.getItem();

                    curSlot.SetItem(currentDraggedItem);

                    allInventorySlot[currentDragSlotIndex].SetItem(itemToSlot);

                    resetDragVariables();
                    return;
                }
                else
                {
                    curSlot.SetItem(currentDraggedItem);
                    resetDragVariables();
                    return;
                }
            }
        }

        allInventorySlot[currentDragSlotIndex].SetItem(currentDraggedItem);
        resetDragVariables();

    }

    private void resetDragVariables()
    {
        currentDraggedItem = null;
        currentDragSlotIndex = -1;
    }

    private void enableHotBarItem(int hotbarItex)
    {
        foreach (GameObject a in eqippableItems) 
        {
            a.SetActive(false);
        }

        Slot hotbarSlot = hotbarSlots[hotbarItex];
        selectrsItemImage.transform.position = hotbarSlots[hotbarItex].transform.position;

        if (hotbarSlot.hasItem())
        {
            if (hotbarSlot.getItem().equippableItemItex != -1)
            {
                eqippableItems[hotbarSlot.getItem().equippableItemItex].SetActive(true);
            }
        }
    }
}
