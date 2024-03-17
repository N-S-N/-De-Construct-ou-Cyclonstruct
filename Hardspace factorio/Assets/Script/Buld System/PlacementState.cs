using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviwSystem previousSystem;
    ObjectsDataBaseSO dataBase;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviwSystem previousSystem,
                          ObjectsDataBaseSO dataBase,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previousSystem = previousSystem;
        this.dataBase = dataBase;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = dataBase.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {

            previousSystem.StartShowingPlacementPreview(
                dataBase.objectsData[selectedObjectIndex].Prefab,
                dataBase.objectsData[selectedObjectIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {iD}");

    }


    public void EndState()
    {
        previousSystem.StopShowPreaview();
    }

    public void OnAction(Vector3Int gridPosition, float rotation)
    {
        bool placementValidity = checkplacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity)
            return;

        Vector3 eulerRotation = new Vector3(0, 0, rotation);

        int index = objectPlacer.PlaceObject(dataBase.objectsData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition), eulerRotation, dataBase.objectsData[selectedObjectIndex].Size);

        GridData selectedData = dataBase.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;
        selectedData.AddObjectAt(gridPosition,
            dataBase.objectsData[selectedObjectIndex].Size,
            dataBase.objectsData[selectedObjectIndex].ID,
            index);

        previousSystem.UpdatePosition(grid.CellToWorld(gridPosition), false, rotation, dataBase.objectsData[selectedObjectIndex].Size);
    } 

    private bool checkplacementValidity(Vector3Int gridPossision, int _selectedObjectIndex)
    {
        GridData selectedData = dataBase.objectsData[_selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        return selectedData.CanPlaceObejctAt(gridPossision, dataBase.objectsData[_selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition, float rotation)
    {
        bool placementValidity = checkplacementValidity(gridPosition, selectedObjectIndex);

        previousSystem.UpdatePosition(grid.WorldToCell(gridPosition), placementValidity, rotation, dataBase.objectsData[selectedObjectIndex].Size);
    }
}
