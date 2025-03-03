using System.Collections.Generic;
using UnityEngine;

public class tunioScript : MonoBehaviour
{
    #region variaves
    [Header("Objetos")]
    public List<GameObject> Iteam = new List<GameObject>();
    [Header("lateralDeSaida[0] representa o ponto trazeito\n" +
        "lateralDeSaida[1] representa o ponto dianteiro")]
    [SerializeField] Transform[] lateralDeSaida = new Transform[2];
    [SerializeField] GameObject beltobj;

    [Header("Gerenciamento")]
    [Header("este sistema e o tamanha maximo do tunion \n elmbrese de coloca .5F no final para n�o dar erro")]
    [SerializeField] float sistancyMaxOftunio = 4.5f;
    [SerializeField] public float SpeedForSeconds = 1f;
    /*[HideInInspector]*/
    public float Directionmove = 0;//significa se ta positivo e a entrada e negativo a saida e 0 se n�ao tiver neste estado
    private float mSpeed = 1f;
    private float svspeed;
    [SerializeField] private float directionmoveprefab;
    [SerializeField] List<float> time = new List<float>();

    [Header("Layers")]
    [SerializeField] LayerMask update;
    [SerializeField] LayerMask tunio;

    [Header("Debug")]
    [HideInInspector] public bool colidio;
    [HideInInspector] public bool selecionado = false;
    [SerializeField] private tunioScript nextunio;
    [HideInInspector] public GameObject sicronizado = null;
    [HideInInspector] public tunioScript objtunioup;

    //previo
    [HideInInspector] GameObject sicronizadoupprevio;
    [HideInInspector] public tunioScript objtunioupprevio;
    [HideInInspector]public tunioScript nextunioprevio;
    LineRenderer linerenderer;

    Collider2D colider;
    //

    [HideInInspector] public Belt belt;

    [HideInInspector] public GameObject tunioextewrnoher;

    #endregion

    #region previo
    public void isEventPrevio()
    {
        Invoke("dalayevent", 0.2f);
        //raiprevio(1);
    }
    void dalayevent()
    {
        raiprevio(1);
    }
    void raiprevio(int i)
    {
        RaycastHit2D m_itDetect = Physics2D.Raycast(lateralDeSaida[i].position, Direction(i), sistancyMaxOftunio, tunio);
        Debug.DrawRay(lateralDeSaida[i].position, Direction(i), Color.red, sistancyMaxOftunio);
        if (m_itDetect)
        {
            nextunioprevio = m_itDetect.collider.GetComponent<tunioScript>();

            if (SpeedForSeconds != nextunioprevio.SpeedForSeconds)
            {
                nextunioprevio = null;
                Invoke("dalayevent", 0.2f);
                return;
            }
            nextunioprevio.objtunioupprevio = this;
            nextunioprevio.sicronizadoupprevio = gameObject;
            nextunioprevio.previoreyconter(0);

        }
        else
        {
            nextunioprevio = null;
            dawnPrevioinplacement();

        }
        Invoke("dalayevent", 0.2f);
    }

    public void previoreyconter(int i)
    {
        RaycastHit2D m_itDetect = Physics2D.Raycast(lateralDeSaida[i].position, Direction(i), sistancyMaxOftunio, tunio);
        Debug.DrawRay(lateralDeSaida[i].position, Direction(i), Color.red, sistancyMaxOftunio);
        if (m_itDetect)
        {
            nextunioprevio = m_itDetect.collider.GetComponent<tunioScript>();
            if (SpeedForSeconds != nextunioprevio.SpeedForSeconds)
            {
                nextunioprevio = null;
                isEventPrevio();
                return;
            }
            if (m_itDetect.collider.gameObject != sicronizadoupprevio)
            {
                objtunioupprevio.nextunioprevio = null;
                objtunioupprevio.dawnPrevioinplacement();
                return;
            }
            nextunioprevio.dawnPrevioinplacement();

        }
        else
        {
            objtunioupprevio.nextunioprevio = null;
            objtunioupprevio.dawnPrevioinplacement();
        }
    }

    public void dawnPrevioinplacement()
    {     
        if (nextunioprevio != null)
        {
            linerenderer.enabled = true;
            linerenderer.SetPosition(0, gameObject.transform.position);
            linerenderer.SetPosition(1, nextunioprevio.gameObject.transform.position);
        }
        else
        {
            linerenderer.enabled = false;

        }
    }

    #endregion

    #region sistema de atualisa��o e detec��o
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        colider = GetComponent<Collider2D>();
        svspeed = mSpeed / SpeedForSeconds;
        belt = beltobj.GetComponent<Belt>();
        UpdateDataDalay();
        OnDestroy();
    }
    private void OnDestroy()
    {
        if(nextunio != null)
        {
            nextunio.nextunio = null;
            nextunio.Directionmove = 0;
            nextunio.Iteam.Clear();
        }
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f), Vector2.down, 0.5F, update);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.left, 0.5F, update);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.up, 0.5F, update);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.right, 0.5F, update);

        RaycastHit2D m_itDetect = Physics2D.Raycast(lateralDeSaida[1].position, Direction(1), sistancyMaxOftunio, tunio);

        if(m_itDetect)
        {        
            m_itDetect.collider.GetComponent<tunioScript>().updatelocal();
        }
           

        if (down.collider)
        {
            if (down.collider.CompareTag("garra"))
                down.collider.GetComponent<garaScript>().updatelocal();
            if (down.collider.CompareTag("belt"))
                down.collider.GetComponent<Belt>().updatelocal();
            if (down.collider.CompareTag("spliter"))
                down.collider.GetComponent<Spliter>().updatelocal();
            if (down.collider.CompareTag("Tunio"))
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
    public void updatelocal()
    {
        Invoke("UpdateDataDalay", 0.2f);
        belt.updatelocal();
    }
    private void UpdateDataDalay()
    {
        ray(0);
    }
    public void updateesteira()
    {
        Invoke("updateesteiradelay", 0.3f); ;      
    }
    public void updateesteiradelay()
    {
        ray(1);
    }
    private void ray(int i)
    {
        RaycastHit2D m_itDetect = Physics2D.Raycast(lateralDeSaida[i].position, Direction(i), sistancyMaxOftunio, tunio);
        Debug.DrawRay(lateralDeSaida[i].position, Direction(i), Color.red, sistancyMaxOftunio);
        if (m_itDetect)
        {
            nextunio = m_itDetect.collider.GetComponent<tunioScript>();

            if (i == 1 && tunioextewrnoher != nextunio.gameObject)            
                return;
            
            sicronizado = nextunio.gameObject;

            
            if (SpeedForSeconds != nextunio.SpeedForSeconds)
            {
                time.Clear();
                Iteam.Clear();
                belt.isSpliter = true;
                nextunio = null;
                Directionmove = 0;
                return;
            }

            if (Directionmove != 0 ) return;

            if (i == 0)
            {
                nextunio.tunioextewrnoher = gameObject;
                nextunio.objtunioup = this;
                nextunio.updateesteira();
               
            }
            else if (i == 1)
            {
                belt.isSpliter = false;
                if (objtunioup.nextunio == null)
                    objtunioup.nextunio = this;
                objtunioup.updatedistanci();
                int negativo = -1;
                Directionmove = negativo;
            }
        }
        else if(Directionmove == 0 && nextunio != objtunioup)
        {
            if(objtunioup != null)
                objtunioup.updatedistanci();
            time.Clear();
            Iteam.Clear();
            sicronizado = null;
            belt.isSpliter = true;
            nextunio = null;
            Directionmove = 0;
        }
    }
    public void updatedistanci()
    {
        if (nextunio.sicronizado != gameObject)
        {
            Iteam.Clear();
            time.Clear();
            sicronizado = null;
            belt.isSpliter = true;
            nextunio = null;
            Directionmove = 0;
        }
        else
        {
            distanciAndItemSlot();
            int positivo = 1;
            Directionmove = positivo;
            int negativo = -1;
            nextunio.Directionmove = negativo;
            belt.isSpliter = true;
        }
    }
    #endregion

    #region sistema de eslot e dire��o/ tamanho 
    public void distanciAndItemSlot()
    {
        
        Iteam.Clear();
        float distanci = 0;
        Vector2 distanciomapa = nextunio.gameObject.transform.position - gameObject.transform.position;

        if (distanciomapa.x != 0)
        {
            distanci = distanciomapa.x;
        }
        else
        {
            distanci = distanciomapa.y;
        }

        if (distanci < 0) distanci *=  -1;
        //

        for (int i = 0; i < distanci; i++) 
        {
            Iteam.Add(null);
            time.Add(svspeed);
        }   
        nextunio.belt.lateralDeSaida = nextunio.lateralDeSaida[0];                  

        nextunio.belt.updatelocal();

    }
    private Vector2 Direction(int i)
    {
        //Debug.Log(lateralDeSaida[i].position);
        return (lateralDeSaida[i].position - transform.position).normalized;

    }
    [SerializeField]List<GameObject> nullgame = new List<GameObject>();
    #endregion

    #region Updates
    private void Update()
    {
        if (nextunio == null && Directionmove == 0) return;

        if (nextunio.tunioextewrnoher != gameObject) return;

        if (nextunio.nextunio != this) return;

        if (nullgame.Count == Iteam.Count) return;
        if (Iteam[0] == null && belt.item != null)
        {
            Iteam[0] = belt.item;

            belt.item.SetActive(false);

            belt.item = null;
        }

        for (int i = Iteam.Count; i > 0; i--) 
        {
            if (Iteam[i - 1] != null) moveTime(i);
            if (Iteam[i - 1] == null && time[i - 1] <= 0)
                time[i - 1] = svspeed;


        }


    }
    void moveTime(int i)
    {
        time[i-1] -= Time.deltaTime;
        if (time[i-1] > 0) return;

        if (i == Iteam.Count && nextunio.belt.item == null)            
        {
               
            Iteam[i - 1].gameObject.SetActive(true);
            Iteam[i - 1].transform.position = nextunio.gameObject.transform.position;
            nextunio.belt.item = Iteam[i - 1];          
            Iteam[i - 1] = null;          
        }
        if (i < Iteam.Count && Iteam[i - 1] != null && Iteam[i] == null)
        {
            Iteam[i] = Iteam[i - 1];
            Iteam[i - 1] = null;
            time[i - 1] = svspeed;
        }
        else
            return;
        
        time[i-1] = svspeed;
    }
    #endregion
}
