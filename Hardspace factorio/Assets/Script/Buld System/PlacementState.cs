using System;
using System.Reflection;
using UnityEngine;

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
    PlacementSysteam placementSysteam;
    int IdCusto;
    public PlacementState(int iD,
                          Grid grid,
                          PreviwSystem previousSystem,
                          ObjectsDataBaseSO dataBase,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer,
                          PlacementSysteam placementSysteam,
                          int idCusto)
    {
        ID = iD;
        this.grid = grid;
        this.previousSystem = previousSystem;
        this.dataBase = dataBase;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.placementSysteam = placementSysteam;
        IdCusto = idCusto;

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

    public int Idcus()
    {
        return ID;
    }

    public void OnAction(Vector3Int gridPosition, float rotation)
    {
        bool placementValidity = checkplacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity)
            return;

        placementSysteam.controle.tiraitem(IdCusto);
        placementSysteam.controle.verificacao(IdCusto);

        if (ID == 9)
        {
            Debug.Log("BBB");
            placementSysteam.totorialplata.Invoke();
        }
        if (ID == 4)
        {
            placementSysteam. garra.Invoke();
        }
        if (ID == 3)
        {
            placementSysteam.bau.Invoke();
        }
        if (ID == 8)
            placementSysteam.tier.Invoke();
        if (ID == 7)
        {
            Debug.Log("AAA");
            placementSysteam.mission.Invoke();
            placementSysteam.desativar.Invoke();
        }

        Vector3 eulerRotation = new Vector3(0, 0, rotation);

        int index = objectPlacer.PlaceObject(dataBase.objectsData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition), eulerRotation, dataBase.objectsData[selectedObjectIndex].Size, ID);

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
