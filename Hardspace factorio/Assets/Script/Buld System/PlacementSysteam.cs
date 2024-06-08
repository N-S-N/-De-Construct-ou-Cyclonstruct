using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;

public class PlacementSysteam : MonoBehaviour
{

    [SerializeField] private InputManager _inputManager;

    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectsDataBaseSO database;

    [SerializeField] private GameObject _gridVisualization;

    [SerializeField] private GameObject ButomSelect;

    private GridData floorData, furnitureData; 

    [SerializeField] private PreviwSystem previw;

    private Vector3Int lastDectedPosition = Vector3Int.zero;

    [SerializeField] ObjectPlacer objectPlacer;

    IBuildingState buldingState;

    private float currentRotation;

    [SerializeField] UnityEvent direita, esquerda;

    [SerializeField] List<int> IdRotesionValid = new List<int>();

    [SerializeField] List<int> missionValidesion = new List<int>();

    [SerializeField] UnityEvent tier, mission,verificacao;

    [SerializeField]public controleUIInventario controle;

    private int IdCusto =-1;

    private int Id = -1;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new();
    }
    public void IdCUSTO(int i) 
    {
        IdCusto = i;
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
                                          objectPlacer,
                                          this,
                                          IdCusto);
        _inputManager.Onclicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
                   
        Id = ID;
        if (-1 == IdRotesionValid.IndexOf(Id))
            currentRotation = 0;
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

        if (-1 != missionValidesion.IndexOf(Id))
        {
            if(Id == 8)
                tier.Invoke();
            if (Id == 7)  
                mission.Invoke();

            //StopPlacement();
        }
        
    }

    //para o load
    public void PlaceStructureLoad(Vector3Int GridPossision, float currentRotationLoad)
    {
        buldingState.OnAction(GridPossision, currentRotationLoad);
    }

    public void Rotate(bool dereita)
    {
        //todos que pode muydar de rotação
        if (-1 != IdRotesionValid.IndexOf(Id))
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

    public void StopPlacement()
    {    
        if (buldingState == null) return;        
        _gridVisualization.SetActive(false);
        buldingState.EndState();
        _inputManager.Onclicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        lastDectedPosition = Vector3Int.zero;
        buldingState = null;
        Id = -1;
        ButomSelect.SetActive(false);
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
