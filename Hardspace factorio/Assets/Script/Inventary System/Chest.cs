using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject chestUIPrefab;
    [SerializeField] private Transform chestUIparent;

    [HideInInspector] public List<Slot> allChestSlot = new List<Slot>();
    [HideInInspector] public GameObject chestInstantiatedParent;

    // Lost table

    private void Start()
    {
        GameObject chestSlot = Instantiate(chestUIPrefab,chestUIparent.position,chestUIparent.rotation, chestUIparent);

        foreach(Transform childSlot in chestSlot.transform.GetChild(1))
        {
            Slot childSlotScript = childSlot.GetComponent<Slot>();
            allChestSlot.Add(childSlotScript);

            childSlotScript.inistialiseSlot();
        }

        chestInstantiatedParent = chestSlot;
        chestInstantiatedParent.SetActive(false);
    }
}
