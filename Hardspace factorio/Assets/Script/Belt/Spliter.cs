using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

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
    [SerializeField] LayerMask update;

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
        updatelocal();
        OnDestroy();
    }

    public void updatelocal()
    {
        for (int i = 0; i < NexBelt.Length; i++)
        {
            ray(i);
        }
        for (int i = 0; i < NexBelt.Length; i++)
        {
            ray(i);
        }
    }

    private void OnDestroy()
    {
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f), Vector2.down, 0.5F, update);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.left, 0.5F, update);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.up, 0.5F, update);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.right, 0.5F, update);

        if (down.collider)
        {
            if (down.collider.CompareTag("garra"))
                down.collider.GetComponent<garaScript>().updatelocal();
            if (down.collider.CompareTag("belt"))
                down.collider.GetComponent<Belt>().updatelocal();
            if (down.collider.CompareTag("spliter"))
                down.collider.GetComponent<Spliter>().updatelocal();
        }
        if (lesft.collider)
        {
            if (lesft.collider.CompareTag("garra"))
                lesft.collider.GetComponent<garaScript>().updatelocal();
            if (lesft.collider.CompareTag("belt"))
                lesft.collider.GetComponent<Belt>().updatelocal();
            if (lesft.collider.CompareTag("spliter"))
                lesft.collider.GetComponent<Spliter>().updatelocal();
        }
        if (up.collider)
        {
            if (up.collider.CompareTag("garra"))
                up.collider.GetComponent<garaScript>().updatelocal();
            if (up.collider.CompareTag("belt"))
                up.collider.GetComponent<Belt>().updatelocal();
            if (up.collider.CompareTag("spliter"))
                up.collider.GetComponent<Spliter>().updatelocal();
        }
        if (right.collider)
        {
            if (right.collider.CompareTag("garra"))
                right.collider.GetComponent<garaScript>().updatelocal();
            if (right.collider.CompareTag("belt"))
                right.collider.GetComponent<Belt>().updatelocal();
            if (right.collider.CompareTag("spliter"))
                right.collider.GetComponent<Spliter>().updatelocal();
        }
    }

    private void Update()
    {
        if (Belt.item != null && item == null)
        {
            item = Belt.item;
            
        }

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
