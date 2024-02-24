using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSysteam : MonoBehaviour
{
    [SerializeField] GameObject _mouseIndicator;

    [SerializeField] private InputManager _inputManager;

    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectsDataBaseSO database;
    private int _selectedObjectIndex = -1;

    [SerializeField] private GameObject _gridVisualization;

    private GridData floorData, furnitureData;

    private List<GameObject> placedGameObject = new();

    [SerializeField] private PreviwSystem previw;

    private Vector3Int lastDectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        _selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (_selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        _gridVisualization.SetActive(true);
        previw.StartShowingPlacementPreview(database.objectsData[_selectedObjectIndex].Prefab,
            database.objectsData[_selectedObjectIndex].Size);
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

        bool placementValidity = checkplacementValidity(GridPossision, _selectedObjectIndex);
        if (!placementValidity)
            return;

        GameObject newObject = Instantiate(database.objectsData[_selectedObjectIndex].Prefab);
        newObject.transform.position = _grid.CellToWorld(GridPossision);
        placedGameObject.Add(newObject);
        GridData selectedData = database.objectsData[_selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;
        selectedData.AddObjectAt(GridPossision,
            database.objectsData[_selectedObjectIndex].Size,
            database.objectsData[_selectedObjectIndex].ID,
            placedGameObject.Count - 1);
        previw.UpdatePosition(_grid.CellToWorld(GridPossision), false);
    }

    private bool checkplacementValidity(Vector3Int gridPossision, int _selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[_selectedObjectIndex].ID == 0 ? 
            floorData : 
            furnitureData;

        return selectedData.CanPlaceObejctAt(gridPossision, database.objectsData[_selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        _selectedObjectIndex = -1;
        _gridVisualization.SetActive(false);
        previw.StopShowPreaview();
        _inputManager.Onclicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        lastDectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if (_selectedObjectIndex < 0) return;
        Vector3 mousePosision = _inputManager.GetSelectedMapPosition();
        Vector3Int GridPossision = _grid.WorldToCell(mousePosision);
        if (lastDectedPosition != GridPossision)
        {
            bool placementValidity = checkplacementValidity(GridPossision, _selectedObjectIndex);

            _mouseIndicator.transform.position = new Vector3(mousePosision.x, mousePosision.y, 0);
            previw.UpdatePosition(_grid.WorldToCell(GridPossision), placementValidity);
            lastDectedPosition = GridPossision;
        }
        
       // _cellIndicator.transform.position = new Vector2(_cellIndicator.transform.position.x + 0.5F, _cellIndicator.transform.position.y + 0.5F) ;
    }
}
