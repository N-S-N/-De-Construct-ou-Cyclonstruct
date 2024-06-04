using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chamandoNaves : MonoBehaviour
{
    [SerializeField] GameObject prefabNave;
    [SerializeField] float TimeToNave;
    private GameObject insta;

    void Start()
    {
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


    }
}//
