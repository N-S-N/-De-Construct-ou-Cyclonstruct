using System;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering;

public class PreviwSystem : MonoBehaviour
{
    [SerializeField] 
    float previwYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObjects;

    //[SerializeField]
    //private SpriteRenderer previewMatarialsPrefab;
    //private SpriteRenderer previewMatarialInstance;

    private SpriteRenderer cellIndicatorRender;

    private void Start()
    {
        //previewMatarialInstance = previewMatarialsPrefab;
        cellIndicator.SetActive(false);
        cellIndicatorRender = cellIndicator.GetComponentInChildren<SpriteRenderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObjects = Instantiate(prefab);
        PreparePreaview(previewObjects);
        PrepareCursoe(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursoe(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector2(size.x, size.y);
            cellIndicatorRender.size = size;
        }
    }

    private void PreparePreaview(GameObject previewObjects)
    {
        SpriteRenderer renderers = previewObjects.GetComponentInChildren<SpriteRenderer>();
        Color c = Color.white;
        c.a = 0.2f;
        renderers.color = c;
    }

    public void StopShowPreaview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObjects);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovoPreview(position);
        MoveCursosr(position);
        Applyfeedback(validity);

    }

    private void Applyfeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRender.color = c;
        //previewMatarialInstance.color = c;
    }

    private void MoveCursosr(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovoPreview(Vector3 position)
    {
        previewObjects.transform.position = new Vector3(position.x, position.y + previwYOffset,0);
    }
}
