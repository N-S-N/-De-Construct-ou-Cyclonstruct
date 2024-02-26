using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int GameObjectIndex = -1;
    Grid grid;
    PreviwSystem previousSystem;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid,
                         PreviwSystem previousSystem,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previousSystem = previousSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        previousSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previousSystem.StopShowPreaview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (!furnitureData.CanPlaceObejctAt(gridPosition,Vector2Int.one))
        {
            selectedData = furnitureData;
        }
        else if(!floorData.CanPlaceObejctAt(gridPosition, Vector2Int.one))
        {
            selectedData = floorData;
        }

        if (selectedData == null) 
        {
            //sound

        }
        else
        {
            GameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (GameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(GameObjectIndex);
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previousSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObejctAt(gridPosition, Vector2Int.one) && 
            floorData.CanPlaceObejctAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previousSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
