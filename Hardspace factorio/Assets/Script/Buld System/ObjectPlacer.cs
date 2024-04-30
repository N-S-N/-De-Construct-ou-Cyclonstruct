using System.Collections.Generic;
using UnityEngine;


public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> placedGameObject = new();
    [HideInInspector] public List<float> rotacao = new();
    [HideInInspector] public List<Vector3> positionOBJ = new();
    [HideInInspector] public List<int> ID = new();
    public int PlaceObject(GameObject prefab, Vector3 position, Vector3 rotation, Vector2 size, int id)
    {
        GameObject newObject = Instantiate(prefab);
        positionOBJ.Add(position);

        if (rotation.z > 0 && rotation.z <= 180)
        {
            position.x += size.x;
        }
        if (rotation.z >= 180)
        {
            position.y += size.y;
        }
        ID.Add(id);
        newObject.transform.position = position;
        newObject.transform.rotation = Quaternion.Euler(rotation);
        placedGameObject.Add(newObject);
        rotacao.Add(rotation.z);
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
