using UnityEngine;

public class PlacementSysteam : MonoBehaviour
{
    [SerializeField] GameObject _mouseIndicator, _cellIndicator;

    [SerializeField] private InputManager _inputManager;

    [SerializeField] private Grid _grid;
    private void Update()
    {
        Vector3 mousePosision = _inputManager.GetSelectedMapPosition();
        Vector3Int GridPossision = _grid.WorldToCell(mousePosision);
        _mouseIndicator.transform.position = new Vector3(mousePosision.x, mousePosision.y,0);
        //_mouseIndicator.transform.position = mousePosision;
        _cellIndicator.transform.position = _grid.CellToWorld(GridPossision);
        _cellIndicator.transform.position = new Vector2(_cellIndicator.transform.position.x + 0.5F, _cellIndicator.transform.position.y + 0.5F) ;
    }
}
