using System.Collections.Generic;
using UnityEngine;


public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObject = new();

    public int PlaceObject(GameObject prefab, Vector3 position, Vector3 rotation, Vector2 size)
    {
        GameObject newObject = Instantiate(prefab);

        if (rotation.z > 0 && rotation.z <= 180)
        {
            position.x += size.x;
        }
        if (rotation.z >= 180)
        {
            position.y += size.y;
        }

        newObject.transform.position = position;
        newObject.transform.rotation = Quaternion.Euler(rotation);
        placedGameObject.Add(newObject);
        return placedGameObject.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObject.Count <= gameObjectIndex || placedGameObject[gameObjectIndex] == null)
            return;
        Destroy(placedGameObject[gameObjectIndex]);
        placedGameObject[gameObjectIndex] = null;
    }
}
