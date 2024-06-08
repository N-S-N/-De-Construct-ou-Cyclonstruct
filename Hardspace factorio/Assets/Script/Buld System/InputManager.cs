using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _scenaCamera;

    private Vector3 _lastPosition;

    [SerializeField] LayerMask _placementLayermask;

    public event Action Onclicked, OnExit;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
            InvokeRepeating("butom",0,0.1f);
        if (Input.GetMouseButtonUp(0))
            CancelInvoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }
    void butom()
    {
        Onclicked?.Invoke();
    }
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject(); 

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
