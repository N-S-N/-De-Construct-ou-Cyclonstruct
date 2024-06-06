using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class missaoScripter : MonoBehaviour
{
    #region variaves

    [SerializeField] List<Inventorypart> material = new List<Inventorypart>();

    [Header("tipo de missao")]
    [SerializeField] bool _isMisaonOfTier;
    public List<missaoDataList> missao = new List<missaoDataList>();

    [Header("Debug")]
    [SerializeField] public missaoDataList missaoSelect = null;
    private missaoDataList missaoSelectdesativete = null;
    [SerializeField] public List<InveltoryMissaoSloySave> SaveSlot = new List<InveltoryMissaoSloySave>();
    [HideInInspector] public Button butom;
    [HideInInspector] public GameObject ItemMenu;
    [HideInInspector] public GameObject selectionMenu;
    private List<GameObject> bleckgrand = new List<GameObject>();
    [SerializeField] private GameObject chestUIPrefab;
    [SerializeField] private GameObject missiopnUIPrefab;
    [HideInInspector] private Transform chestUIparent;
    [HideInInspector] private Transform chestUIparentMission;

    [Header("Inventory Lists")]
    public List<Slot> allintrustriSlot = new List<Slot>();
    public List<Slot> inputintrustriSlot = new List<Slot>();

    [HideInInspector] public GameObject chestInstantiatedParent;
    Collider2D coll;

    private GameObject chestSlot;
    [SerializeField]private GameObject missiontSlot;
    [Header("Layer")]
    [SerializeField] LayerMask anyon;

    private Uimission UI;
    #endregion

    #region start and update Data
    public void Start()
    {
        Invoke("delaystart", 0.5f);

        
    }

    void delaystart()
    {
        if (GetComponent<Collider2D>().enabled == false) return;

        upadateEventSysteam();
        for (int i = 0; i < missao.Count; i++)
        {
            SaveSlot.Add(null);
        }

        coll = GetComponent<Collider2D>();
        //Debug.Log(new Vector2(coll.bounds.max.x - coll.bounds.min.x, coll.bounds.max.y - coll.bounds.min.y));
        chestUIparent = GetComponentInParent<tranformUIObj>().tranformobj;
        chestUIparentMission = GetComponentInParent<tranformUIObj>().tranformobjMission;

        if (coll == null) return;
        chestSlot = Instantiate(chestUIPrefab, chestUIparent.position, chestUIparent.rotation, chestUIparent);

        chestSlot.GetComponentInChildren<ListmissonUI>().mission = this;
        chestInstantiatedParent = chestSlot;
        chestInstantiatedParent.SetActive(false);
        Invoke("uodatedataradio", 0.2f);
    }


    private void OnDestroy()
    {
        Destroy(chestSlot);
        Destroy(missiontSlot);
        if (coll == false) return;
        if (material.Count > 0) 
        {
            for (int i = 0; i < material.Count; i++)
            {
                GameObject drop = Instantiate(material[i].item.gameObject, transform.position, transform.rotation);

                Item dropItem = drop.GetComponent<Item>();

                dropItem.currentQuantity = material[i].quantidade;

                drop.transform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            }
        }
        if (_isMisaonOfTier)
        {
            FindAnyObjectByType<EventData>().destoryTier.Invoke();
        }
        else
        {
            FindAnyObjectByType<EventData>().destoynormal.Invoke();
        }
        uodatedataradio();
    }
    public void uodatedataradio()
    {
        if(coll == null) Destroy(missiontSlot);
        Vector2 size = new Vector2(coll.bounds.max.x - coll.bounds.min.x, coll.bounds.max.y - coll.bounds.min.y);

        for (int i = 0; i < size.x; i++)
        {
            RaycastHit2D lateraldireita = Physics2D.Raycast(new Vector2(coll.bounds.min.x + i + 0.5f, coll.bounds.max.y), Vector2.up, 0.5F, anyon);
            RaycastHit2D lateralesquerda = Physics2D.Raycast(new Vector2(coll.bounds.min.x + i + 0.5f, coll.bounds.min.y), Vector2.down, 0.5F, anyon);

            if (lateraldireita.collider)
                if (lateraldireita.collider.CompareTag("garra"))
                    lateraldireita.collider.GetComponent<garaScript>().updatelocal();

            if (lateralesquerda.collider)
                if (lateralesquerda.collider.CompareTag("garra"))
                    lateralesquerda.collider.GetComponent<garaScript>().updatelocal();
        }
        for (int i = 0; i < size.y; i++)
        {
            RaycastHit2D horizontaldireita = Physics2D.Raycast(new Vector2(coll.bounds.max.x, coll.bounds.min.y + i + 0.5f), Vector2.right, 0.5F, anyon);
            RaycastHit2D horizontalesquerda = Physics2D.Raycast(new Vector2(coll.bounds.min.x, coll.bounds.min.y + i + 0.5f), Vector2.left, 0.5F, anyon);

            if (horizontaldireita.collider)
                if (horizontaldireita.collider.CompareTag("garra"))
                    horizontaldireita.collider.GetComponent<garaScript>().updatelocal();

            if (horizontalesquerda.collider)
                if (horizontalesquerda.collider.CompareTag("garra"))
                    horizontalesquerda.collider.GetComponent<garaScript>().updatelocal();

        }
    }
    #endregion

    #region Update missao

    public void updateDatainFucion(int i, List<Slot> input, List<GameObject> BlackImage)
    {
        if (missiontSlot == null)
        {
            missiontSlot = Instantiate(missiopnUIPrefab, chestUIparentMission.position, chestUIparentMission.rotation, chestUIparentMission);

            UI = missiontSlot.GetComponent<Uimission>();
        }
        Invoke("uodatedataradio", 0.3f);
        if (missao[i] != missaoSelect)
        {
            if (missaoSelect != null)
            {
                int p = missao.IndexOf(missaoSelect);
                if (inputintrustriSlot.Count != 0)
                {
                    SaveSlot[p].inputintrustriSlot = inputintrustriSlot;
                    SaveSlot[p].allintrustriSlot = allintrustriSlot;
                }
            }
        }

        missaoSelect = missao[i];
        if (SaveSlot[i] != null)
        {
            inputintrustriSlot = SaveSlot[i].inputintrustriSlot;
            allintrustriSlot = SaveSlot[i].allintrustriSlot;
        }
        else
        {
            inputintrustriSlot = input;

            for (int o = 0; o < missaoSelect.renquedrentes.Count; o++)
            {
                addInvenoryIteam(missaoSelect.renquedrentes[o].IdItem.GetComponent<Item>(), o);
            }
            allintrustriSlot.AddRange(inputintrustriSlot);
        }

        for (int u = 0; u < inputintrustriSlot.Count; u++)
        {
            if(u < UI.SpacoMissaon.Count)
                UI.SpacoMissaon[u].UIImageIconir.gameObject.SetActive(false);
            inputintrustriSlot[u].gameObject.SetActive(false);
            BlackImage[u].SetActive(false);
        }
        //
        for (int u = 0; u < input.Count; u++)
        {
            if (missaoSelect.renquedrentes.Count > u)
            {
                if(u <= UI.SpacoMissaon.Count){
                    UI.SpacoMissaon[u].UIImageIconir.gameObject.SetActive(true);
                    UI.SpacoMissaon[u].UIImageIconir.color = Color.white;
                    UI.SpacoMissaon[u].UIImageIconir.sprite = inputintrustriSlot[u].heldItem.icone;

                    UI.SpacoMissaon[u].TextMissionMissao.text = inputintrustriSlot[u].heldItem.currentQuantity + " / " + missaoSelect.renquedrentes[u].requiredQuantity;
                }
                inputintrustriSlot[u].gameObject.SetActive(true);
                BlackImage[u].SetActive(true);
            }
        }
        bleckgrand = BlackImage;

        
    }

    void addInvenoryIteam(Item itemToAdd, int i, int overideIndex = 0)
    {
        Slot openSholt = inputintrustriSlot[i];
        Item insta = Instantiate(itemToAdd);
        openSholt.SetItem(insta);
        insta.gameObject.SetActive(false);
        insta.currentQuantity = 0;
        inputintrustriSlot[i].UpdateData();

        insta.MaxQuabttity = missaoSelect.renquedrentes[i].requiredQuantity;

    }

    void upadateEventSysteam()
    {
        EventData EventData = FindAnyObjectByType<EventData>();
        if (!_isMisaonOfTier)
        {
            for (int i = 0; i < missao.Count; i++)
            {
                missao[i].Eventos = EventData.EvenosMissao[i];
            }

        }
        else
        {  
            for (int i = 0; i < missao.Count; i++)
            {
                missao[i].Eventos = EventData.EvenosMissaoTier[i];
            }
            
        }


    }

    #endregion

    #region update
    private void Update()
    {
        if (missaoSelect == null) return;
        if (butom == null) return;
        for (int i = 0; i < missaoSelect.renquedrentes.Count; i++)
        {
            UI.SpacoMissaon[i].TextMissionMissao.text = inputintrustriSlot[i].heldItem.currentQuantity + " / " + missaoSelect.renquedrentes[i].requiredQuantity;
            Item holdItem = inputintrustriSlot[i].getItem();
            if (missaoSelect.renquedrentes[i].requiredQuantity > holdItem.currentQuantity)
                return;
            UI.SpacoMissaon[i].UIImageIconir.color = Color.green;
        }

       
        butom.interactable = false;

        missaoSelect.Eventos.Invoke();
        //chestInstantiatedParent.GetComponentInChildren<ListmissonUI>().Update();
        missaoSelectdesativete = missaoSelect;
        missaoSelect = null;
        Invoke("terminoi",0.2f);
    }

    void terminoi()
    {
        for (int i = 0; i < missaoSelectdesativete.renquedrentes.Count; i++)
        {
            inputintrustriSlot[i].gameObject.SetActive(false);
            bleckgrand[i].SetActive(false);
        }
        ItemMenu.SetActive(false);
        selectionMenu.SetActive(true);
    }
   #endregion

        

    #region ListCreat

    [System.Serializable]
    public class missaoDataList
    {
        [Header("Eventos de Execução depos que terminar a fução")]
        [HideInInspector] public UnityEvent Eventos;
        [Header("lista de recursos nessesarios para a missão")]
        public List<renquedrentes> renquedrentes = new List<renquedrentes>();
    }

    [System.Serializable]
    public class renquedrentes
    {
        [Header("prefab o Iteam")]
        public GameObject IdItem;
        [Header("quantidade do Iteam")]
        public int requiredQuantity;
    }

    [System.Serializable]
    public class InveltoryMissaoSloySave
    {
        [Header("Inventory Lists")]
        public List<Slot> allintrustriSlot = new List<Slot>();
        public List<Slot> inputintrustriSlot = new List<Slot>();
    }

}
#endregion

