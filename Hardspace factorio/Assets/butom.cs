using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butom : MonoBehaviour
{
   public void passo()
    {
        FindObjectOfType<totorialGmae>().proximo(8);
        Destroy(this);
    }
}
