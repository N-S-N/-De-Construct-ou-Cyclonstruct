using UnityEngine;


public class Belt : MonoBehaviour
{
    [Header("Gerenciamento")]
    [SerializeField] float SpeedForSeconds = 1f;
    private float mSpeed = 1f;
    private float svspeed;
    [HideInInspector] public float time;

    [Header("Objetos")]
    public GameObject item;
    [SerializeField]public Transform lateralDeSaida;

    [Header("layer")]
    [SerializeField]LayerMask layerMask;
    [SerializeField] LayerMask update;
    [SerializeField] LayerMask Iteam;

    [Header("Color")]
    [SerializeField] Color[] tiers;

    [Header("Debug")]
    [SerializeField] public bool isSpliter = false;
    [SerializeField] private Belt NexBelt;
    [SerializeField] private bool _isNext;
    [HideInInspector] public bool colidio;
    [HideInInspector] public bool selecionado = false;


    [SerializeField]private Collider2D Iteamcolider;
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
        time = svspeed;


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
        Invoke("ray",0.3f);
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
            if(down.collider.CompareTag("Tunio"))
                down.collider.GetComponent<tunioScript>().updatelocal();
        }
        if (lesft.collider)
        {
            if (lesft.collider.CompareTag("garra"))
                lesft.collider.GetComponent<garaScript>().updatelocal();
            if (lesft.collider.CompareTag("belt"))
                lesft.collider.GetComponent<Belt>().updatelocal();
            if (lesft.collider.CompareTag("spliter"))
                lesft.collider.GetComponent<Spliter>().updatelocal();
            if (lesft.collider.CompareTag("Tunio"))
                lesft.collider.GetComponent<tunioScript>().updatelocal();
        }
        if (up.collider)
        {
            if (up.collider.CompareTag("garra"))
                up.collider.GetComponent<garaScript>().updatelocal();
            if (up.collider.CompareTag("belt"))
                up.collider.GetComponent<Belt>().updatelocal();
            if (up.collider.CompareTag("spliter"))
                up.collider.GetComponent<Spliter>().updatelocal();
            if (up.collider.CompareTag("Tunio"))
                up.collider.GetComponent<tunioScript>().updatelocal();
        }
        if (right.collider)
        {
            if (right.collider.CompareTag("garra"))
                right.collider.GetComponent<garaScript>().updatelocal();
            if (right.collider.CompareTag("belt"))
                right.collider.GetComponent<Belt>().updatelocal();
            if (right.collider.CompareTag("spliter"))
                right.collider.GetComponent<Spliter>().updatelocal();
            if (right.collider.CompareTag("Tunio"))
                right.collider.GetComponent<tunioScript>().updatelocal();
        }
    }
    private void Update()
    {
        if (isSpliter) return;
        if (item != null && Iteamcolider == null)
            Iteamcolider = item.GetComponent<Collider2D>();
            if(item != null)
                item.GetComponent<menuBelt>().Belt = this;
        if (item == null && Iteamcolider != null)        
            Iteamcolider = null;
        if (!_isNext && item != null)
            animator.speed = 0;
        if (_isNext && item != null)
            moveIteam();
        else if (_isNext)
            animator.speed = SpeedForSeconds;
        
    }
    private void ray()
    {
        RaycastHit2D m_HitDetect = Physics2D.Raycast(lateralDeSaida.position, Direction(), 0.5F, layerMask);
        if (m_HitDetect)
        {
            if (!m_HitDetect.collider.GetComponent<Collider2D>().enabled)
            {
                ray();
                return;
            }
            NexBelt = m_HitDetect.collider.GetComponent<Belt>();         

            if (NexBelt != null)
            {
                if(NexBelt.gameObject == gameObject)
                {
                    _isNext = false;
                    NexBelt = null;
                    return;
                }

                _isNext = true;
                return;
            }
        }
        NexBelt = null;
        _isNext = false;

    }
    private Vector2 Direction()
    {
        return (lateralDeSaida.position - colider.bounds.center).normalized;
    }
    void moveIteam()
    {
        //RaycastHit2D m_HitDetect = Physics2D.Raycast(item.transform.position, Direction(), 0.5F, Iteam);
        if (Iteamcolider == null) return; 

        RaycastHit2D m_HitDetect1 = Physics2D.BoxCast(new Vector2(item.transform.position.x, item.transform.position.y) + (Direction() * (Iteamcolider.bounds.size.x))
            , Iteamcolider.bounds.size/2,0f ,Direction(), 0 , Iteam);

        if (!m_HitDetect1 || selecionado)
        {
            animator.speed = SpeedForSeconds;
            colidio = false;
            /*if (svspeed / 2 >= time)
            {
                if (NexBelt.item != null) return;
            }*/

            time -= Time.deltaTime;
            var distanceDelt = SpeedForSeconds * Time.deltaTime;
            item.transform.position = Vector3.MoveTowards(item.transform.position, NexBelt.transform.position, distanceDelt);
            
            if (time <= 0)
            {
                if (NexBelt.item != null) return;
                item.GetComponent<menuBelt>().Belt = null;
                NexBelt.item = item;
                item = null;
                Iteamcolider = null;
                time = svspeed;
                selecionado = false;
            }
        }
        else
        {
            animator.speed = 0;
            if (NexBelt.item != null) return;

            Belt outro = m_HitDetect1.collider.GetComponent<menuBelt>().Belt;
            
            colidio = true;
            if (outro == null) return;
            if (NexBelt.item == null && outro.colidio && !outro.selecionado)
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
