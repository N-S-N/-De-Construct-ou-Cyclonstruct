
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class controleUIInventario : MonoBehaviour
{
    [SerializeField]List<Button> construcao = new List<Button>();

    [SerializeField]GameObject UiButtom;

    [SerializeField] List<requisitor> material = new List<requisitor>();

    Inventary Inventary;

    [SerializeField] PlacementSysteam pla;

    private void Start()
    {
        Inventary = GetComponent<Inventary>();

        for (int k = 0; k < material.Count; k++)
        {
            for (int j = 0; j < material[k].material.Count; j++)
            {
                GameObject a = Instantiate(material[k].material[j].obj);
                material[k].material[j].item = a.GetComponent<Item>();
                a.SetActive(false);
            }
        }     
        verificacao(-1);
    }

    public void verificacao(int p)
    {
       
        for (int k = 0; k < material.Count; k++)
        {                  
            for (int j = 0; j < material[k].material.Count; j++)                  
            {
                for (int i = 0; i < Inventary.inventorySlot.Count; i++)
                {
                    Item heldItem = Inventary.inventorySlot[i].getItem();
                    if (heldItem != null)
                    {
                        if (material[k].material[j].item.ID == heldItem.ID
                            && heldItem.currentQuantity >= material[k].material[j].quantidade)
                        {
                            material[k].material[j].temItem = true;
                            i = Inventary.inventorySlot.Count;
                            break;
                        }
                        else
                        {
                            material[k].material[j].temItem = false;
                        }
                    }
                }
            }
        }

        for (int i = 0;i < material.Count; i++)
        {
            construcao[i].interactable = true;
        }

        for (int k = 0; k < material.Count; k++)
        {
            for (int j = 0; j < material[k].material.Count; j++)
            {
                if (!material[k].material[j].temItem)
                {
                    construcao[k].interactable = false;
                    if (p == k) pla.StopPlacement();
                    break;
                }
            }
        }
    }

    public void tiraitem(int i)
    {
        for (int k = 0; k < material[i].material.Count; k++)
        {
            for (int j = 0; j < Inventary.inventorySlot.Count; j++)
            {
                Item heldItem = Inventary.inventorySlot[j].getItem();
                if (heldItem != null)
                {
                    if (heldItem.ID == material[i].material[k].item.ID &&
                        heldItem.currentQuantity >= material[i].material[k].quantidade)
                    {
                        heldItem.currentQuantity -= material[i].material[k].quantidade;

                        if (heldItem.currentQuantity <= 0)
                            Inventary.inventorySlot[j].SetItem(null);

                        break;
                    }

                }
            }
        }
        for (int j = 0; j < Inventary.inventorySlot.Count; j++)
        {
            Inventary.inventorySlot[j].UpdateData();
        }
    }

}
[System.Serializable]
public class Inventorypartrec
{
    public GameObject obj;
    public Item item = null;
    public bool temItem; 
    public int quantidade;
}

[System.Serializable]
public class requisitor
{
    public List<Inventorypartrec> material;
}
