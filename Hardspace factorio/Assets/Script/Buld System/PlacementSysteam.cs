using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlacementSysteam : MonoBehaviour
{

    [SerializeField] private InputManager _inputManager;

    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectsDataBaseSO database;

    [SerializeField] private GameObject _gridVisualization;

    private GridData floorData, furnitureData; 

    [SerializeField] private PreviwSystem previw;

    private Vector3Int lastDectedPosition = Vector3Int.zero;

    [SerializeField] ObjectPlacer objectPlacer;

    IBuildingState buldingState;

    private float currentRotation;

    [SerializeField] UnityEvent direita, esquerda;

    private int Id = -1;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        
        StopPlacement();
        _gridVisualization.SetActive(true);

        buldingState = new PlacementState(ID,
                                          _grid,
                                          previw,
                                          database,
                                          floorData,
                                          furnitureData,
                                          objectPlacer);
        _inputManager.Onclicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
        currentRotation = 0;
        Id = ID;
    }

    public void StartRemoving()
    {
        StopPlacement();
        _gridVisualization.SetActive(true);
        buldingState = new RemovingState(_grid, previw, floorData, furnitureData, objectPlacer);

        _inputManager.Onclicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosision = _inputManager.GetSelectedMapPosition();
        Vector3Int GridPossision = _grid.WorldToCell(mousePosision);

        buldingState.OnAction(GridPossision, currentRotation);

    }

    public void Rotate(bool dereita)
    {
        Debug.Log(Id);
        //todos que pode muydar de rotação
        if (Id == 1)
        {
            if (dereita) {
                currentRotation += 90;
                if (currentRotation >= 360)
                {
                    currentRotation = 0;
                }
            }
            else
            {                
                if (currentRotation <= 0)
                {
                    currentRotation = 360;
                }
                currentRotation -= 90;
            }
            Vector3 mousePosision = _inputManager.GetSelectedMapPosition();
            Vector3Int GridPossision = _grid.WorldToCell(mousePosision);
            buldingState.UpdateState(GridPossision, currentRotation);
        }
        else
        {
            currentRotation = 0;
        }     
    }

    //private bool checkplacementValidity(Vector3Int gridPossision, int _selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[_selectedObjectIndex].ID == 0 ? 
    //        floorData : 
    //        furnitureData;

    //    return selectedData.CanPlaceObejctAt(gridPossision, database.objectsData[_selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {    
        if (buldingState == null) return;        
        _gridVisualization.SetActive(false);
        buldingState.EndState();
        _inputManager.Onclicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        lastDectedPosition = Vector3Int.zero;
        buldingState = null;
        Id = -1;
    }

    private void Update()
    {
        if (buldingState == null ) return;

        if (Input.GetKeyDown(KeyCode.Q))
            direita.Invoke();
        if (Input.GetKeyDown(KeyCode.R))
            esquerda.Invoke();

        Vector3 mousePosision = _inputManager.GetSelectedMapPosition();
        Vector3Int GridPossision = _grid.WorldToCell(mousePosision);
        if (lastDectedPosition != GridPossision)
        {
            buldingState.UpdateState(GridPossision, currentRotation);
            lastDectedPosition = GridPossision;
        }
        
       // _cellIndicator.transform.position = new Vector2(_cellIndicator.transform.position.x + 0.5F, _cellIndicator.transform.position.y + 0.5F) ;
    }
}
