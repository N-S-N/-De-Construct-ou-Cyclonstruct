using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Spliter : MonoBehaviour
{
    [Header("Gerenciamento")]
    [SerializeField] float SpeedForSeconds = 1f;
    private float mSpeed = 1f;
    private float svspeed;

    [Header("Objetos")]
    public GameObject item;
    [SerializeField] Transform[] lateralDeSaida;

    [Header("layer")]
    [SerializeField] LayerMask layerMask;

    [Header("Debug")]
    [SerializeField] private Belt[] NexBelt;
    [SerializeField] private bool[] _isNext;
    [SerializeField] private int _nextbeltordem;

    private bool _isran = false;

    Collider2D colider;
    Belt Belt;
    private void Start()
    {
        svspeed = mSpeed / SpeedForSeconds;
        Belt = GetComponent<Belt>();
        colider = GetComponent<Collider2D>();

    }

    private void Update()
    {
        if (Belt.item != null && item == null)
        {
            item = Belt.item;
            
        }

        Invoke("TImeDelay", 0.5f);

        if (_isNext[_nextbeltordem] && NexBelt[_nextbeltordem].item == null && item != null)
        {
            StartCoroutine(move());
        }

        else if (NexBelt[_nextbeltordem] == null)
        {
            _nextbeltordem++;
            if (_nextbeltordem >= lateralDeSaida.Length) _nextbeltordem = 0;
        }
        else if (NexBelt[_nextbeltordem].item != null && !_isran)
        {
            _nextbeltordem++;
            if (_nextbeltordem >= lateralDeSaida.Length) _nextbeltordem = 0;
        }
    }

    void TImeDelay()
    {
        for (int i = 0; i < NexBelt.Length; i++)
        {
            ray(i);
        }
    }

    private void ray(int i)
    {
            RaycastHit2D m_itDetect = Physics2D.Raycast(lateralDeSaida[i].position, Direction(i), 0.5F, layerMask);
            Debug.DrawRay(lateralDeSaida[i].position, Direction(i) ,Color.red, 0.5F);

            if (m_itDetect)
            {
                NexBelt[i] = m_itDetect.collider.GetComponent<Belt>();

                if (NexBelt[i] != null)
                {
                    if (NexBelt[i].gameObject == gameObject)
                    {
                        _isNext[i] = false;
                        return;
                    }

                    _isNext[i] = true;
                    return;
                }
            }
            _isNext[i] = false;
        
    }

    private Vector2 Direction(int i)
    {
        return (lateralDeSaida[i].position - colider.bounds.center).normalized;

    }

    private IEnumerator move()
    {
        int next = _nextbeltordem;
        _isran = true;
        var distanceDelt = SpeedForSeconds * Time.deltaTime;
        item.transform.position = Vector3.MoveTowards(item.transform.position, NexBelt[_nextbeltordem].transform.position, distanceDelt);
        yield return new WaitForSeconds(svspeed);
        _isran = false;
        if (item != null)
            NexBelt[_nextbeltordem].item = item;
        if (NexBelt[_nextbeltordem].item != null)
        {
            item = null;
            Belt.item = null;
        }

        if (next == _nextbeltordem)
        {
            _nextbeltordem = next + 1;
            if (_nextbeltordem >= lateralDeSaida.Length) _nextbeltordem = 0;
            
        }
    }


}
