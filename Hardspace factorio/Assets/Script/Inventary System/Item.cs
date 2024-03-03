using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public int ID = 0;
    public string Name = "new name";
    public string Description = "new Description";
    public Sprite icone;
    public int currentQuantity = 1;
    public int MaxQuabttity = 100;

    public int equippableItemItex = -1;

    [Header("Item use")]
    public UnityEvent MyEvent;
    public bool removeOnUse = false;

    public void UseItem()
    {
        if(MyEvent.GetPersistentEventCount() > 0)
        {
            MyEvent.Invoke();

            if (removeOnUse)
                currentQuantity--;
        }
    }
}
