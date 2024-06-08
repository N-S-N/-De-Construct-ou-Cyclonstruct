using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class chamandoNaves : MonoBehaviour
{
    [SerializeField] List<Inventorypart> material = new List<Inventorypart>();
    [SerializeField] GameObject prefabNave;
    [SerializeField] float TimeToNave;
    private GameObject insta;
    Collider2D coll;

    private void OnDestroy()
    {
        if (coll == false) return;
        if(coll.enabled == false) return;
        if (material.Count > 0)
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


    void Start()
    {
        Invoke("daley",0.5f);
        Invoke("spawm", TimeToNave);
    }

    void Update()
    {
        if (insta == null) return;
        if (insta.activeInHierarchy == false)
        {
            Invoke("spawm", TimeToNave);
        }
    }
    void daley()
    {
        coll = GetComponent<Collider2D>();
    }

    void ajust()
    {
        insta.transform.position = new Vector3(insta.transform.position.x-1.2f, insta.transform.position.y-0.3f,2f);
    }
    void spawm()
    {
        if (insta == null)
        {
            insta = Instantiate(prefabNave, transform.position, transform.rotation);
            ajust();
            return;
        }
        if (insta.activeInHierarchy == true)
        {
            CancelInvoke();
            return;
        }   
        insta.SetActive(true);
        insta.GetComponent<navecontroler>().setactiveteGameobj();
        

    }
}//
