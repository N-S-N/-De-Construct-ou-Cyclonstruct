using UnityEngine;

public interface IBuildingState
{
    void EndState();
    int Idcus();
    void OnAction(Vector3Int gridPosition, float rotation);
    void UpdateState(Vector3Int gridPosition, float rotation);
}