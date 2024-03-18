using System.Collections.Generic;
using UnityEngine;

public class IndustrialScripts : MonoBehaviour
{
    [SerializeField] private GameObject chestUIPrefab;
    [HideInInspector] private Transform chestUIparent;

    [Header("Inventory Lists")]
    public List<Slot> allintrustriSlot = new List<Slot>();
    public List<Slot> inputintrustriSlot = new List<Slot>();
    public List<Slot> outputtrustriSlot = new List<Slot>();

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
        uodatedataradio();
    }

    private void Update()
    {
        //veririca��o de contidade
        for (int i = 0; i < inputintrustriSlot.Count; i++)
        {
            if (inputintrustriSlot[i].getItem() == null) break;
            Item holdItem = inputintrustriSlot[i].getItem();

            if (requiredQuantity[i] > holdItem.currentQuantity)return;
        }

        //verificar se tem espaso para produ��o
        for (int i = 0; i < outputtrustriSlot.Count; i++)
        {
            if (outputtrustriSlot[i].getItem() == null) break;
            Item holdItem = outputtrustriSlot[i].getItem();
            if (holdItem.currentQuantity + quantityProduced[i] == holdItem.MaxQuabttity) return;
        }

        // tempo de produ��o
        _internofloatTime -= Time.deltaTime;

        if (_internofloatTime <= 0)
        {           
            for (int i = 0; i < inputintrustriSlot.Count; i++)
            {
                if (inputintrustriSlot[i].getItem() == null) break;
                Item holdItem = inputintrustriSlot[i].getItem();
                holdItem.currentQuantity -= requiredQuantity[i];
                inputintrustriSlot[i].UpdateData();
            }
            for (int i = 0;i < outputtrustriSlot.Count;i++)
            {
                if (outputtrustriSlot[i].getItem() == null) break;
                Item holdItem = outputtrustriSlot[i].getItem();
                holdItem.currentQuantity += quantityProduced[i];
                Debug.Log(holdItem.currentQuantity);
                outputtrustriSlot[i].UpdateData();
            }
            _internofloatTime = TimeProduction;
        }

    }

    //update de informa��o
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
        uodatedataradio();
    }
    public void uodatedataradio()
    {
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0, -1.5f), Vector2.down, 0.5F);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-1.5f, 0), Vector2.left, 0.5F);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 1.5f), Vector2.up, 0.5F);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(1.5f, 0), Vector2.right, 0.5F);

        if(down.collider)
            if (down.collider.CompareTag("garra"))
                down.collider.GetComponent<garaScript>().updatelocal();
        if(lesft.collider)
            if (lesft.collider.CompareTag("garra"))
                lesft.collider.GetComponent<garaScript>().updatelocal();
        if(up.collider)
            if (up.collider.CompareTag("garra"))
                up.collider.GetComponent<garaScript>().updatelocal();
        if(right.collider)
            if (right.collider.CompareTag("garra"))
                right.collider.GetComponent<garaScript>().updatelocal();
    }

}
