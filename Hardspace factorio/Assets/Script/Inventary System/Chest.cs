using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] List<Inventorypart> material = new List<Inventorypart>();

    [SerializeField] private GameObject chestUIPrefab;
    [SerializeField] private Transform chestUIparent;

    [HideInInspector] public List<Slot> allChestSlot = new List<Slot>();
    [HideInInspector] public GameObject chestInstantiatedParent;

    [Header("Layer")]
    [SerializeField] LayerMask anyon;

    GameObject chestSlot;

    Animator animator;
    // Lost table 
    //opem
    void updatedata()
    {
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f), Vector2.down, 0.5F, anyon);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.left, 0.5F,anyon);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.up, 0.5F, anyon);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.right, 0.5F, anyon);

        if (down.collider)
            if (down.collider.CompareTag("garra"))
                down.collider.GetComponent<garaScript>().updatelocal();
        if (lesft.collider)
            if (lesft.collider.CompareTag("garra"))
                lesft.collider.GetComponent<garaScript>().updatelocal();
        if (up.collider)
            if (up.collider.CompareTag("garra"))
                up.collider.GetComponent<garaScript>().updatelocal();
        if (right.collider)
            if (right.collider.CompareTag("garra"))
                right.collider.GetComponent<garaScript>().updatelocal();
    }

    public void Start()
    {
        Invoke("delaystart",0.5f);
    }
    void delaystart()
    {

        if (GetComponent<Collider2D>().enabled == false) return;
        animator = GetComponent<Animator>();
        chestUIparent = GetComponentInParent<tranformUIObj>().tranformobj;

        chestSlot = Instantiate(chestUIPrefab, chestUIparent.position, chestUIparent.rotation, chestUIparent);

        foreach (Transform childSlot in chestSlot.transform.GetChild(2))
        {
            Slot childSlotScript = childSlot.GetComponent<Slot>();
            allChestSlot.Add(childSlotScript);

            childSlotScript.inistialiseSlot();
        }

        chestInstantiatedParent = chestSlot;
        chestInstantiatedParent.SetActive(false);
        updatedata();
    }

    private void OnDestroy()
    {
        updatedata();
        Destroy(chestSlot);

        if (GetComponent<Collider2D>().enabled == true)
        {
            for (int i = 0; i < material.Count; i++)
            {
                GameObject drop = Instantiate(material[i].item.gameObject, transform.position, transform.rotation);

                Item dropItem = drop.GetComponent<Item>();

                dropItem.currentQuantity = material[i].quantidade;

                drop.transform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            }
        }

    }

    private void Update()
    {
        if (chestInstantiatedParent == null) return;
        if (chestInstantiatedParent.activeSelf)
        {
            animator.SetBool("opem", true);
        }
        else
        {
            animator.SetBool("opem", false);
        }
    }
}
