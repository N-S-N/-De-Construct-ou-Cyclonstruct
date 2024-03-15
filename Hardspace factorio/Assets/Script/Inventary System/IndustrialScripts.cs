using System.Collections.Generic;
using UnityEngine;

public class IndustrialScripts : MonoBehaviour
{
    [SerializeField] private GameObject chestUIPrefab;
    [HideInInspector] private Transform chestUIparent;

    [Header("Inventory Lists")]
    [HideInInspector] public List<Slot> allintrustriSlot = new List<Slot>();
    [HideInInspector] public List<Slot> inputintrustriSlot = new List<Slot>();
    [HideInInspector] public List<Slot> outputtrustriSlot = new List<Slot>();

    [Header("Time and Production")]
    [HideInInspector] public float TimeProduction;
    [HideInInspector] public int[] quantityProduced;
    [HideInInspector] public List<int> requiredQuantity = new List<int>();

    [HideInInspector]public GameObject chestInstantiatedParent;
    private float _internofloatTime;

    private GameObject chestSlot;

    // Lost table 
    private void Start()
    {
        chestUIparent = GetComponentInParent<tranformUIObj>().tranformobj;

        chestSlot = Instantiate(chestUIPrefab, chestUIparent.position, chestUIparent.rotation, chestUIparent);
        chestSlot.GetComponentInChildren<ListInventoryMachine>().Industril = this;
        chestInstantiatedParent = chestSlot;
        chestInstantiatedParent.SetActive(false);
        
    }

    private void Update()
    {
        //veriricação de contidade
        for (int i = 0; i < inputintrustriSlot.Count; i++)
        {
            if (inputintrustriSlot[i].getItem() == null) break;
            Item holdItem = inputintrustriSlot[i].getItem();
            if (requiredQuantity[i] < holdItem.currentQuantity)return;
        }

        //verificar se tem espaso para produção
        for (int i = 0; i < outputtrustriSlot.Count; i++)
        {
            if (outputtrustriSlot[i].getItem() == null) break;
            Item holdItem = outputtrustriSlot[i].getItem();
            if (holdItem.currentQuantity + quantityProduced[i] == holdItem.MaxQuabttity) return;
        }

        // tempo de produção
        _internofloatTime -= Time.deltaTime;
        if (_internofloatTime <= 0)
        {           
            for (int i = 0; i < inputintrustriSlot.Count; i++)
            {
                if (inputintrustriSlot[i].getItem() == null) break;
                Item holdItem = inputintrustriSlot[i].getItem();
                holdItem.currentQuantity -= requiredQuantity[i];
            }
            for (int i = 0;i < outputtrustriSlot.Count;i++)
            {
                if (outputtrustriSlot[i].getItem() == null) break;
                Item holdItem = outputtrustriSlot[i].getItem();
                holdItem.currentQuantity += quantityProduced[i];
            }
            _internofloatTime = TimeProduction;
        }      
    }

    //update de informação
    public void UpdateLists(List<Slot> input, List<Slot> output, float Time, int[] quatity, List<int> requiry)
    {
        inputintrustriSlot = input;
        outputtrustriSlot = output;
        allintrustriSlot.AddRange(inputintrustriSlot);
        allintrustriSlot.AddRange(outputtrustriSlot);

        TimeProduction = Time;
        quantityProduced = quatity;
        requiredQuantity = requiry;
        _internofloatTime = Time;
    }

    //deletado
    private void OnDestroy()
    {
        Destroy(chestSlot);
    }

}
