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

    private void Update()
    {
        if (isSpliter) return;
        Invoke("ray",0.2f);
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
