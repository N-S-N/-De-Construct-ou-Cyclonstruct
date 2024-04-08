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
    public List<int> requiredQuantity = new List<int>();

    [HideInInspector]public GameObject chestInstantiatedParent;
    [SerializeField] private float _internofloatTime;

    private GameObject chestSlot;
    [Header("Layer")]
    [SerializeField] LayerMask anyon;

    Collider2D coll;
    // Lost table 
    private void Start()
    {
        coll = GetComponent<Collider2D>();
        chestUIparent = GetComponentInParent<tranformUIObj>().tranformobj;

        chestSlot = Instantiate(chestUIPrefab, chestUIparent.position, chestUIparent.rotation, chestUIparent);
        chestSlot.GetComponentInChildren<ListInventoryMachine>().Industril = this;
        chestInstantiatedParent = chestSlot;
        chestInstantiatedParent.SetActive(false);
        Invoke("uodatedataradio",0.2f);
    }

    private void Update()
    {
        if (inputintrustriSlot.Count == 0) return;
        //veriricação de contidade
        for (int i = 0; i < inputintrustriSlot.Count; i++)
        {
            if (inputintrustriSlot[i].getItem() == null) break;
            Item holdItem = inputintrustriSlot[i].getItem();
            if (requiredQuantity[i] > holdItem.currentQuantity) return;                 
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
                inputintrustriSlot[i].UpdateData();
            }
            for (int i = 0;i < outputtrustriSlot.Count;i++)
            {
                if (outputtrustriSlot[i].getItem() == null) break;
                Item holdItem = outputtrustriSlot[i].getItem();
                holdItem.currentQuantity += quantityProduced[i];
                outputtrustriSlot[i].UpdateData();
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
        uodatedataradio();
    }
    public void uodatedataradio()
    {
        RaycastHit2D up1 = Physics2D.Raycast(new Vector2(coll.bounds.max.x, coll.bounds.max.y), Vector2.up, 0.5F, anyon);
        RaycastHit2D right1 = Physics2D.Raycast(new Vector2(coll.bounds.max.x, coll.bounds.max.y), Vector2.right, 0.5F, anyon);
        RaycastHit2D down1 = Physics2D.Raycast(new Vector2(coll.bounds.max.x, coll.bounds.min.y), Vector2.down, 0.5F, anyon);
        RaycastHit2D right2 = Physics2D.Raycast(new Vector2(coll.bounds.max.x, coll.bounds.min.y), Vector2.right, 0.5F, anyon);
        RaycastHit2D down2 = Physics2D.Raycast(new Vector2(coll.bounds.min.x, coll.bounds.min.y), Vector2.down, 0.5F, anyon);
        RaycastHit2D left1 = Physics2D.Raycast(new Vector2(coll.bounds.min.x, coll.bounds.min.y), Vector2.left, 0.5F, anyon);
        RaycastHit2D up2 = Physics2D.Raycast(new Vector2(coll.bounds.min.x, coll.bounds.max.y), Vector2.up, 0.5F, anyon);
        RaycastHit2D left2 = Physics2D.Raycast(new Vector2(coll.bounds.min.x, coll.bounds.max.y), Vector2.left, 0.5F, anyon);


        if (down1.collider)
            if (down1.collider.CompareTag("garra"))
                down1.collider.GetComponent<garaScript>().updatelocal();
        if (left1.collider)
            if (left1.collider.CompareTag("garra"))
                left1.collider.GetComponent<garaScript>().updatelocal();
        if (up1.collider)
            if (up1.collider.CompareTag("garra"))
                up1.collider.GetComponent<garaScript>().updatelocal();
        if (right1.collider)
            if (right1.collider.CompareTag("garra"))
                right1.collider.GetComponent<garaScript>().updatelocal();
        if (down2.collider)
            if (down2.collider.CompareTag("garra"))
                down2.collider.GetComponent<garaScript>().updatelocal();
        if(left2.collider)
            if (left2.collider.CompareTag("garra"))
                left2.collider.GetComponent<garaScript>().updatelocal();
        if(up2.collider)
            if (up2.collider.CompareTag("garra"))
                up2.collider.GetComponent<garaScript>().updatelocal();
        if(right2.collider)
            if (right2.collider.CompareTag("garra"))
                right2.collider.GetComponent<garaScript>().updatelocal();

    }

}
