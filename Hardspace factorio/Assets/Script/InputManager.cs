using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _scenaCamera;

    private Vector3 _lastPosition;

    [SerializeField] LayerMask _placementLayermask;

    public Vector2 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _scenaCamera.nearClipPlane;

        Ray ray = _scenaCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _placementLayermask))
        {
            _lastPosition = hit.point;
        }
        return _lastPosition;
    }

}
