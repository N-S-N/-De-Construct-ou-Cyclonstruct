using System.Collections.Generic;
using UnityEngine;

public class ListInventoryMachine : MonoBehaviour
{
    [Header("Inventory Lists")]
    public List<Slot> allintrustriSlot = new List<Slot>();
    public List<Slot> inputintrustriSlot = new List<Slot>();
    public List<Slot> outputtrustriSlot = new List<Slot>();

    [Header("BackGrand Lists")]
    [SerializeField] List<GameObject> backgrandInput = new List<GameObject>();
    [SerializeField] List<GameObject> backgrandOutput = new List<GameObject>();

    [Header("Time and Production")]
    [HideInInspector] public float TimeProduction;
    [HideInInspector] public int[] quantityProduced;
    [HideInInspector] public List<int> requiredQuantity = new List<int>();

    [Header("Prefabs List")]
    public List<GameObject> allItemPrefabs = new List<GameObject>();

    [Header("scripts")]
    public IndustrialScripts Industril;

    private void Start()
    {
        allintrustriSlot.Clear();
        allintrustriSlot.AddRange(inputintrustriSlot);
        allintrustriSlot.AddRange(outputtrustriSlot);

        for (int i = 0; i < allintrustriSlot.Count; i++)
        {
            allintrustriSlot[i].inistialiseSlot();
        }
    }

    public void setrecipe(Recipe recipe)
    {
        //apagar
        for (int i = 0; i < allintrustriSlot.Count; i++)
        {
            allintrustriSlot[i].inistialiseSlot();
        }  
        requiredQuantity.Clear();

        //quantidade e tempo
        quantityProduced = recipe.quantityProduced;
        TimeProduction = recipe.timeProducedForSeconds;

        int output = 0;
        //output
        for (int i = 0; i < recipe.createdItemPrefab.Length; i++)
        {
            outputtrustriSlot[i].gameObject.SetActive(true);
            backgrandOutput[i].gameObject.SetActive(true);
            addItemInventoryoutput(recipe.createdItemPrefab[i].GetComponent<Item>(),i);
            
            output = i;
        }
        for (int i = output+1; i < outputtrustriSlot.Count; i++)
        {
            outputtrustriSlot[i].SetItem(null);
            outputtrustriSlot[i].gameObject.SetActive(false);
            backgrandOutput[i].gameObject.SetActive(false);
        }
        //input
        int input = 0;
        for (int i = 0; i < recipe.requiredIngredients.Count; i++)
        {
            requiredQuantity.Add(recipe.requiredIngredients[i].requiredQuantity);
            inputintrustriSlot[i].gameObject.SetActive(true);
            backgrandInput[i].gameObject.SetActive(true);
            addItemInput(recipe.requiredIngredients[i].IdItem,i);     
            input = i;
        }

        for (int i = input + 1; i < inputintrustriSlot.Count; i++)
        {
            inputintrustriSlot[i].gameObject.SetActive(false);
            backgrandInput[i].gameObject.SetActive(false);
        }

        allintrustriSlot.AddRange(inputintrustriSlot);
        allintrustriSlot.AddRange(outputtrustriSlot);

        Industril.UpdateLists(inputintrustriSlot, outputtrustriSlot, TimeProduction, quantityProduced, requiredQuantity);
    }

    private void addItemInventoryoutput(Item itemToAdd,int i ,int overideIndex = 0)
    {
        int leftoverQuantity = overideIndex;

        Slot openSholt = outputtrustriSlot[i];

        openSholt.SetItem(itemToAdd);
        itemToAdd.currentQuantity = 0;      

    }
    private void addItemInventoryinput(Item itemToAdd,int i ,int overideIndex = 0)
    {
        int leftoverQuantity = overideIndex;

        Slot openSholt = inputintrustriSlot[i];

        openSholt.SetItem(itemToAdd);
        itemToAdd.currentQuantity = 0;

    }

    void addItemInput(int Id, int i)
    {
        GameObject itemPrefab = allItemPrefabs.Find(prefab => prefab.GetComponent<Item>().ID == Id);

        if (itemPrefab != null)
        {
            GameObject createdItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Item item = createdItem.GetComponent<Item>();

            item.currentQuantity = 0;
            addItemInventoryinput(item,i);
        }
    }
}
