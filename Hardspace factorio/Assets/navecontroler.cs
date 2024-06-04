using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class navecontroler : MonoBehaviour
{
    [SerializeField] List<ParteFufete> componets = new List<ParteFufete>();
    private Inventary inventory;
    public GameObject player;
    int index = 0;

    public void setactiveteGameobj()
    {
        for (int i = 0; i < componets.Count; i++)
        {
            componets[i].parte.SetActive(true);  
        }
    }
    public float coletar()
    {
        if(inventory == null) inventory = player.GetComponent<Inventary>();

        for (int i = 0; i< componets[index].inv.Count; i++)
        {
            GameObject Iteminst = Instantiate(componets[index].inv[i].item);

            Item item = Iteminst.GetComponent<Item>();
            item.currentQuantity = componets[index].inv[i].quantidade;

            inventory.addItemInventory(item);

        }

        float time = componets[index].timedemolição;
        
        index++;
        Invoke("desativar", time);
        return time;
    }
    void desativar()
    {
        componets[index-1].parte.SetActive(false);
        if (index == componets.Count)
        {
            gameObject.SetActive(false);
        }
    }


}
[System.Serializable]
public class ParteFufete
{
    public GameObject parte;
    public List<Inventorypart> inv;
    public float timedemolição;
}

[System.Serializable]
public class Inventorypart
{
    public GameObject item;
    public int quantidade;
}

