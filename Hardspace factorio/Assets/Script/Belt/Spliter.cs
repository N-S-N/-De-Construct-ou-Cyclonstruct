using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class Spliter : MonoBehaviour
{
    [Header("Gerenciamento")]
    [SerializeField] float SpeedForSeconds = 1f;
    private float mSpeed = 1f;
    private float svspeed;
    [HideInInspector] public float time;

    [Header("Objetos")]
    public GameObject item;
    [SerializeField] Transform[] lateralDeSaida;


    [Header("layer")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask update;
    [SerializeField] LayerMask Iteam;

    [Header("Debug")]
    [SerializeField] private Belt[] NexBelt;
    [SerializeField] private bool[] _isNext;
    [SerializeField] private int _nextbeltordem;
    [HideInInspector] public bool colidio;
    [HideInInspector] public bool selecionado = false;

    Collider2D Iteamcolider;
    Collider2D colider;
    Belt Belt;
    private void Start()
    {
        svspeed = mSpeed / SpeedForSeconds;
        time = svspeed;
        Belt = GetComponent<Belt>();
        colider = GetComponent<Collider2D>();
        updatelocal();
        OnDestroy();
    }

    public void updatelocal()
    {
        Invoke("delayupdate", 0.2f);
    }
    void delayupdate()
    {
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

        if (item != null && Iteamcolider == null)
            Iteamcolider = item.GetComponent<Collider2D>();

        if (item == null && Iteamcolider != null)
            Iteamcolider = null;

        if (_isNext[_nextbeltordem] && item != null && NexBelt[_nextbeltordem].item == null)
        {
            move(_nextbeltordem);
        }

        else if (NexBelt[_nextbeltordem] == null)
        {
            _nextbeltordem++;
            if (_nextbeltordem >= lateralDeSaida.Length) _nextbeltordem = 0;
        }
        else if (NexBelt[_nextbeltordem].item != null)
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

    private void move(int i)
    {
        if (Iteamcolider == null) return;

        RaycastHit2D m_HitDetect1 = Physics2D.BoxCast(new Vector2(item.transform.position.x, item.transform.position.y) + (Direction(i) * (Iteamcolider.bounds.size.x))
            , Iteamcolider.bounds.size / 2, 0f, Direction(i), 0, Iteam);

        if (!m_HitDetect1 || selecionado)
        {
            
            colidio = false;
            /*if (svspeed / 2 >= time)
            {
                if (NexBelt.item != null) return;
            }*/

            time -= Time.deltaTime;
            var distanceDelt = SpeedForSeconds * Time.deltaTime;
            item.transform.position = Vector3.MoveTowards(item.transform.position, NexBelt[i].transform.position, distanceDelt);

            if (time <= 0)
            {
                _nextbeltordem++;
                if (_nextbeltordem >= lateralDeSaida.Length) _nextbeltordem = 0;
                if (NexBelt[i].item != null) return;
                NexBelt[i].item = item;
                item = null;
                Belt.item = null;
                Iteamcolider = null;
                time = svspeed;
                selecionado = false;
            }
        }
        else
        {

            if (NexBelt[i].item != null) return;

            Belt outro = m_HitDetect1.collider.GetComponent<Belt>();

            colidio = true;
            if (NexBelt[i].item == null && outro.colidio && !outro.selecionado)
            {
                if (time < outro.time)
                {
                    selecionado = true;
                }
                else
                {
                    selecionado = false;
                }

            }
        }
    }

}
