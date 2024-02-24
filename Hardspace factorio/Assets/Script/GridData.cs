using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new(); 
    public void AddObjectAt(Vector3Int gridPossision,
                            Vector2Int objectSize,
                            int ID,
                            int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPossision, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosision, Vector2Int objectSize)
    {
        List<Vector3Int> returbVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returbVal.Add(gridPosision + new Vector3Int(x, y, 0));
            }
        }
        return returbVal;
    }

    public bool CanPlaceObejctAt(Vector3Int gridPosision, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosision, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }
}

public class PlacementData 
{ 
    public List<Vector3Int> occupiedPosition;
    public int ID {  get;private set; }
    public int PlacedObjectIndex {  get; private set; }

    public PlacementData(List<Vector3Int> occupiedPosition, int iD, int placedObjectIndex)
    {
        this.occupiedPosition = occupiedPosition;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
