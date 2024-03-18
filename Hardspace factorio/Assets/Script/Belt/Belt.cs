using System.Collections;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [Header("Gerenciamento")]
    [SerializeField] float SpeedForSeconds = 1f;
    private float mSpeed = 1f;
    private float svspeed;

    [Header("Objetos")]
    public GameObject item;
    [SerializeField] Transform lateralDeSaida;
    [Header("layer")]
    [SerializeField]LayerMask layerMask;

    [Header("Color")]
    [SerializeField] Color[] tiers;

    [Header("Debug")]
    [SerializeField] bool isSpliter = false;
    [SerializeField] private Belt NexBelt;
    [SerializeField] private bool _isNext;

    Collider2D colider;
    SpriteRenderer render;
    Animator animator;
    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();   
        svspeed = mSpeed / SpeedForSeconds;
        colider = GetComponent<Collider2D>();
        animator.speed = SpeedForSeconds;

        updatelocal();
        OnDestroy();

        if (SpeedForSeconds == 2)
        {
            render.color = tiers[1];
        }
        else if (SpeedForSeconds == 4)
        {
            render.color = tiers[2];
        }
        else
        {
            render.color = tiers[0];
        }
    }
    public void updatelocal()
    {        
        ray();
    }
    private void OnDestroy()
    {
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f), Vector2.down, 0.5F);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.left, 0.5F);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.up, 0.5F);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.right, 0.5F);

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
        if (isSpliter) return;
        if (_isNext && NexBelt.item == null && item != null)
            StartCoroutine(move());

    }

    private void ray()
    {
        RaycastHit2D m_HitDetect = Physics2D.Raycast(lateralDeSaida.position, Direction(), 0.5F, layerMask);
        if (m_HitDetect)
        {
            NexBelt = m_HitDetect.collider.GetComponent<Belt>();         

            if (NexBelt != null)
            {
                if(NexBelt.gameObject == gameObject)
                {
                    _isNext = false;
                    return;
                }

                _isNext = true;
                return;
            }
        }
        _isNext = false;
    }

    private Vector2 Direction()
    {
        return (lateralDeSaida.position - colider.bounds.center).normalized;
    }

    private IEnumerator move()
    {
        var distanceDelt = SpeedForSeconds * Time.deltaTime;
        item.transform.position = Vector3.MoveTowards(item.transform.position, NexBelt.transform.position, distanceDelt);
        yield return new WaitForSeconds(svspeed);
        if (item != null)
            NexBelt.item = item;
        if(NexBelt.item != null)
            item = null;
    }

}
