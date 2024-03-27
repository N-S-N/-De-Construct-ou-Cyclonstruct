using UnityEngine;

public class EventExit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }
}
