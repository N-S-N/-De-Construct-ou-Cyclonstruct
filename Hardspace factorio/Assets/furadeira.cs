using UnityEngine;

public class furadeira : MonoBehaviour
{
    [SerializeField] float alcance;

    [SerializeField] LayerMask porta;
    private SpriteRenderer saveisrayu;
    private SpriteRenderer cirlo;
    [SerializeField] GameObject aria;

    PlayerControler PlayerControler;
    Inventary inv;

    public bool furadeiraactivete;

    private void Start()
    {
        cirlo = aria.GetComponent<SpriteRenderer>();
        PlayerControler = GetComponent<PlayerControler>();
        inv = GetComponent<Inventary>();
    }

    public void Ui()
    {
        furadeiraactivete = true;
    }

    private void Update()
    {
        if (furadeiraactivete) 
        {
            inv.enteracaofuradeira = true;

            if (PlayerControler.unterageFuredaira == false) 
            {
                coletar();
            }
            aria.SetActive(true);

            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                furadeiraactivete = false;
                inv.enteracaofuradeira = false;
                aria.SetActive(false);
            }
        }
        else
        {
            if (saveisrayu != null)
                saveisrayu.color = Color.white;
        }
    }

    public void coletar()
    {

        RaycastHit2D m_HitDetectinChest2 = Physics2D.CircleCast(transform.position, alcance, Vector2.down, 1, porta);
        if (m_HitDetectinChest2)
        {
            if (m_HitDetectinChest2.collider != null)
            {
                Color red = Color.red;
                red.a = 0.5f;
                cirlo.color = red;
                

                saveisrayu = m_HitDetectinChest2.collider.gameObject.GetComponent<SpriteRenderer>();
                if(saveisrayu != null)
                    saveisrayu.color = Color.red;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerControler.interection = true;
                    PlayerControler.unterageFuredaira = true;

                    m_HitDetectinChest2.collider.gameObject.GetComponent<navecontroler>().player = gameObject;

                    Invoke("stopinterect", m_HitDetectinChest2.collider.gameObject.GetComponent<navecontroler>().coletar());
                    return;
                }
            }
        }
        else
        {
            Color whaiter = Color.white;
            whaiter.a = 0.5f;
            cirlo.color = whaiter;
            if (saveisrayu != null)
                saveisrayu.color = Color.white;
        }
    }

    void stopinterect()
    {
        CancelInvoke();
        PlayerControler.interection = false;
        PlayerControler.unterageFuredaira = false;
        inv.enteracaofuradeira = false;
    }

}
