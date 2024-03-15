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

        //output
        for (int i = 0; i < outputtrustriSlot.Count; i++)
        {
            if (recipe.createdItemPrefab[i] != null)
            {
                addItemInventoryoutput(recipe.createdItemPrefab[i].GetComponent<Item>());
                outputtrustriSlot[i].gameObject.SetActive(true);
                backgrandOutput[i].gameObject.SetActive(true);
            }
            else
            {
                outputtrustriSlot[i].SetItem(null);
                outputtrustriSlot[i].gameObject.SetActive(false);
                backgrandOutput[i].gameObject.SetActive(false);
            }
        }
        //input
        for (int i = 0; i < inputintrustriSlot.Count; i++)
        {
            if (recipe.requiredIngredients[i] != null)
            {
                addItemInput(recipe.requiredIngredients[i].IdItem);
                requiredQuantity.Add(recipe.requiredIngredients[i].IdItem);
                inputintrustriSlot[i].gameObject.SetActive(true);
                backgrandInput[i].gameObject.SetActive(true);
            }
            else
            {
                inputintrustriSlot[i].gameObject.SetActive(false);
                backgrandInput[i].gameObject.SetActive(false);
            }
        }

        allintrustriSlot.AddRange(inputintrustriSlot);
        allintrustriSlot.AddRange(outputtrustriSlot);

        Industril.UpdateLists(inputintrustriSlot, outputtrustriSlot, TimeProduction, quantityProduced, requiredQuantity);
    }

    private void addItemInventoryoutput(Item itemToAdd, int overideIndex = 0)
    {
        int leftoverQuantity = overideIndex;

        Slot openSholt = null;

        for (int i = 0; i < outputtrustriSlot.Count; i++)
        {
            Item heldItem = outputtrustriSlot[i].getItem();

            if (heldItem != null)
            {
                return;
            }
            else if (heldItem == null)
            {
                if (!openSholt)
                    openSholt = outputtrustriSlot[i];
            }

            outputtrustriSlot[i].UpdateData();
        }

        if (openSholt)
        {
            openSholt.SetItem(itemToAdd);
            itemToAdd.currentQuantity = leftoverQuantity;
        }
    }
    private void addItemInventoryinput(Item itemToAdd, int overideIndex = 0)
    {
        int leftoverQuantity = overideIndex;

        Slot openSholt = null;

        for (int i = 0; i < inputintrustriSlot.Count; i++)
        {
            Item heldItem = inputintrustriSlot[i].getItem();

            if (heldItem != null)
            {
                return;
            }
            else if (heldItem == null)
            {
                if (!openSholt)
                    openSholt = inputintrustriSlot[i];
            }

            inputintrustriSlot[i].UpdateData();
        }

        if (openSholt)
        {
            openSholt.SetItem(itemToAdd);        
            itemToAdd.currentQuantity = leftoverQuantity;
            itemToAdd.gameObject.SetActive(false);
        }
        
    }

    void addItemInput(int Id)
    {
        GameObject itemPrefab = allItemPrefabs.Find(prefab => prefab.GetComponent<Item>().ID == Id);

        if (itemPrefab != null)
        {
            GameObject createdItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Item item = createdItem.GetComponent<Item>();

            item.currentQuantity = 0;

            addItemInventoryinput(item);
        }
    }
}
