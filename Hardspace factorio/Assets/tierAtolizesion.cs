
using UnityEngine;

public class tierAtolizesion : MonoBehaviour
{
    ListmissonUI liost;
    private void Start()
    {
        liost = GetComponentInChildren<ListmissonUI>();
    }
    void Update()
    {
        liost.Update();
    }
}
