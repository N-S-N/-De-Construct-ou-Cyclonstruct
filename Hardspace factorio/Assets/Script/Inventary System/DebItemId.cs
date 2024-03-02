using System.Collections.Generic;
using UnityEngine;

public class DebItemId : MonoBehaviour
{
    [Header("coloca todos os item nesta lista")]
    public List<Item> prefb = new List<Item>();
    [Header("quando inicilizar o jogo olha esta lista para saber as informação de item\n" +
        "não esquece de dechar o numero do lado iqual a do prefb")]
    public List<DebItem> deb =new List<DebItem>();
    
    private void Start()
    {
        
        for (int i = 0; i < prefb.Count; i++) 
        {
            deb[i].neme = prefb[i].name;
            deb[i].IdItem = prefb[i].ID;
            deb[i].itemPrefab = prefb[i];
        }
    }
}

[System.Serializable]
public class DebItem
{
    public Item itemPrefab ;
    public int IdItem  ;
    public string neme  ;
}


