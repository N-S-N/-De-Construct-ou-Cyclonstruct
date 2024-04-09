using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class missaoScripter : MonoBehaviour
{
    #region variaves

    [Header("tipo de missao")]
    [SerializeField] bool _isMisaonOfTier;
    public List<missaoDataList> missao = new List<missaoDataList>();

    [Header("Debug")]
    [SerializeField] private missaoDataList missaoSelect = null;
    private missaoDataList missaoSelectdesativete = null;
    [SerializeField] private List<InveltoryMissaoSloySave> SaveSlot = new List<InveltoryMissaoSloySave>();
    [HideInInspector] public Button butom;
    [HideInInspector] public GameObject ItemMenu;
    [HideInInspector] public GameObject selectionMenu;
    private List<GameObject> bleckgrand = new List<GameObject>();
    [SerializeField] private GameObject chestUIPrefab;
    [HideInInspector] private Transform chestUIparent;

    [Header("Inventory Lists")]
    public List<Slot> allintrustriSlot = new List<Slot>();
    public List<Slot> inputintrustriSlot = new List<Slot>();

    [HideInInspector] public GameObject chestInstantiatedParent;
    Collider2D coll;

    private GameObject chestSlot;
    [Header("Layer")]
    [SerializeField] LayerMask anyon;

    #endregion

    #region start and update Data
    private void Start()
    {

        upadateEventSysteam();
        for (int i = 0; i < missao.Count; i++)
        {
            SaveSlot.Add(null);
        }

        coll = GetComponent<Collider2D>();
        //Debug.Log(new Vector2(coll.bounds.max.x - coll.bounds.min.x, coll.bounds.max.y - coll.bounds.min.y));
        chestUIparent = GetComponentInParent<tranformUIObj>().tranformobj;

        chestSlot = Instantiate(chestUIPrefab, chestUIparent.position, chestUIparent.rotation, chestUIparent);
        chestSlot.GetComponentInChildren<ListmissonUI>().mission = this;
        chestInstantiatedParent = chestSlot;
        chestInstantiatedParent.SetActive(false);
        Invoke("uodatedataradio", 0.2f);

    }
    private void OnDestroy()
    {
        Destroy(chestSlot);
        uodatedataradio();
    }
    public void uodatedataradio()
    {
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
            inputintrustriSlot[u].gameObject.SetActive(false);
            BlackImage[u].SetActive(false);
        }

        for (int u = 0; u < input.Count; u++)
        {
            if (missaoSelect.renquedrentes.Count > u)
            {
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
        if (!_isMisaonOfTier)
        {
            EventData EventData = FindAnyObjectByType<EventData>();
            for (int i = 0; i < missao.Count; i++)
            {
                missao[i].Eventos = EventData.EvenosMissao[i];
            }
        }
        else
        {
            EventData EventData = FindAnyObjectByType<EventData>();
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
            Item holdItem = inputintrustriSlot[i].getItem();
            if (missaoSelect.renquedrentes[i].requiredQuantity > holdItem.currentQuantity)
                return;
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

