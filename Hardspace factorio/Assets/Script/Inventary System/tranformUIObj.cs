using UnityEngine;
using UnityEngine.Rendering;

public class tranformUIObj : MonoBehaviour
{
    public Transform tranformobj;
    public Transform tranformobjMission;
    public void trasfom(Transform tras)
    {
        tranformobj = tras;
    }
    public void misionUiTradform(Transform tras)
    {
        tranformobjMission = tras;
    } 
}
