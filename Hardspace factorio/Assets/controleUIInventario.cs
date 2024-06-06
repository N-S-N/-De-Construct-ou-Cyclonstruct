
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
        verificacao();
    }

    public void verificacao()
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
                            Debug.Log("aaa " + k+ " " +j);
                            material[k].material[j].temItem = true;
                            i = Inventary.inventorySlot.Count;
                            break;
                        }
                        else
                        {
                            Debug.Log("bbb " + k + " " + j);
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
                    break;
                }
            }
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
