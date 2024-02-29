using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool Havered;
    public Item heldItem;

    private Color apaque = new Color(1, 1, 1, 1);
    private Color transparent = new Color(1, 1, 1, 0);

    private Image thisSlotImage;

    private TMP_Text thisSlotQunatityText;
    public void inistialiseSlot()
    {
        thisSlotImage = GetComponent<Image>();
        thisSlotQunatityText = GetComponentInChildren<TMP_Text>();
        thisSlotImage.sprite = null;
        thisSlotImage.color = transparent;
        SetItem(null);
    }

    public void SetItem(Item item)
    {
        heldItem = item;

        if(item != null)
        {
            thisSlotImage.sprite = heldItem.icone;
            thisSlotImage.color = apaque;
            UpdateData();
        }
        else
        {
            thisSlotImage.sprite = null;
            thisSlotImage.color = transparent;
            UpdateData();
        }
    }

    public void UpdateData()
    {
        if (heldItem != null)
            thisSlotQunatityText.text = heldItem.currentQuantity.ToString();
        else
            thisSlotQunatityText.text = "";

    }


    public Item getItem()
    {
        return heldItem;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Havered = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Havered = false;
    }
}
