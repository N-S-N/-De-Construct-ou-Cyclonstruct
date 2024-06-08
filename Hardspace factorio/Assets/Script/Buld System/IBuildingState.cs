using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void Idcus(int custo);
    void OnAction(Vector3Int gridPosition, float rotation);
    void UpdateState(Vector3Int gridPosition, float rotation);
}