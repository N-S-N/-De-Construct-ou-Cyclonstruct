using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject chestUIPrefab;
    [SerializeField] private Transform chestUIparent;

    [HideInInspector] public List<Slot> allChestSlot = new List<Slot>();
    [HideInInspector] public GameObject chestInstantiatedParent;

    GameObject chestSlot;
    // Lost table 

    void updatedata()
    {
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f), Vector2.down, 0.5F);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.left, 0.5F);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.up, 0.5F);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.right, 0.5F);

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

    private void Start()
    {
        updatedata();
        chestUIparent = GetComponentInParent<tranformUIObj>().tranformobj;

        chestSlot = Instantiate(chestUIPrefab,chestUIparent.position,chestUIparent.rotation, chestUIparent);

        foreach(Transform childSlot in chestSlot.transform.GetChild(1))
        {
            Slot childSlotScript = childSlot.GetComponent<Slot>();
            allChestSlot.Add(childSlotScript);

            childSlotScript.inistialiseSlot();
        }

        chestInstantiatedParent = chestSlot;
        chestInstantiatedParent.SetActive(false);
    }

    private void OnDestroy()
    {
        updatedata();
        Destroy(chestSlot);
    }
}
