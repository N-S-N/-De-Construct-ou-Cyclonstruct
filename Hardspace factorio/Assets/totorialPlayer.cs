using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class totorialPlayer : MonoBehaviour
{
    [SerializeField] GameObject UiInicio;
    [SerializeField] PlayerControler controler;
    [SerializeField] Inventary inventario;
    [SerializeField] GameObject demolicao;

    [SerializeField] float RayCastDistance = 5;

    [SerializeField] LayerMask coletavel;
    [SerializeField] LayerMask porta;
    [SerializeField] TMP_Text textoUi;
    [SerializeField] GameObject portafeixada;
    [SerializeField] Animator animacaoportaanimitor;
    [SerializeField] Camera cam;

    [SerializeField] GameObject mause;

    bool ismouseposition;
    bool finish = false;
    void Update()
    {
        if (finish) return;
        //inicio
        if (UiInicio.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UiInicio.SetActive(false);
                controler.enabled = true;
                inventario.enabled = true;
            }
            return;
        }

        if (ismouseposition)
        {
            mause.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mause.SetActive(false);
                ismouseposition = false;
                return;
            }

            //Vector2 mauseposition = Input.mousePosition;
            // mauseposition = cam.WorldToViewportPoint(mauseposition);
            Vector2 mauseposition = cam.ScreenToWorldPoint(Input.mousePosition);

            mause.transform.position = mauseposition;
            RaycastHit2D m_HitDetectinChest2 = Physics2D.CircleCast(mauseposition, 0.1f, Vector2.down, 0, porta);
            if (m_HitDetectinChest2)
            {
                if (m_HitDetectinChest2.collider != null)
                {
                    saveisrayu = m_HitDetectinChest2.collider.gameObject.GetComponent<SpriteRenderer>();
                    saveisrayu.color = Color.red;
                    if (Input.GetMouseButtonDown(0))
                    {
                        animacaoportaanimitor.SetBool("destroir", true);
                        Invoke("portaspawm", 0.7f);
                        finish = true;
                        mause.SetActive(false);
                        ismouseposition = false;
                        saveisrayu.color = Color.white;
                        return;
                    }
                }
            }
            else
            {
                if (saveisrayu != null)
                    saveisrayu.color = Color.white;
            }
        }


        //pegar a feramenta
        RaycastHit2D m_HitDetectinChest = Physics2D.CircleCast(transform.position, RayCastDistance, Vector2.down, 1, coletavel);

        if (m_HitDetectinChest)
        {
            if (m_HitDetectinChest.collider != null)
            {
                textoUi.text = "Aperte (E) para pegar a furadeira";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    demolicao.SetActive(true);
                    Destroy(m_HitDetectinChest.collider.gameObject);
                    textoUi.text = "";
                }
            }
        }
        else
        {
            textoUi.text = "";
        }



        //apria porta
    }
    private SpriteRenderer saveisrayu;
    public void animacaoporta()
    {
        ismouseposition = true;
    }

    void portaspawm()
    {
        Instantiate(portafeixada);
    }
}
