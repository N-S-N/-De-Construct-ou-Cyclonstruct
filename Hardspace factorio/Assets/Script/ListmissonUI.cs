using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ListmissonUI : MonoBehaviour
{
    [HideInInspector] public missaoScripter mission;

    [Header("Maneger")]
    [SerializeField] tierButon butom;
    [SerializeField] GameObject item;
    [SerializeField] GameObject SelectionMenu;
    [Header("Debug")]
    [SerializeField] int _tierAtual = 0;
    private int _tier;

    [Header("Inventory Lists")]
    public List<Slot> allintrustriSlot = new List<Slot>();
    public List<Slot> inputintrustriSlot = new List<Slot>();
    [Header("BackGrand Lists")]
    public List<GameObject> BackGrandImager = new List<GameObject>();


    private void Start()
    {       
        _tier = _tierAtual;
        allintrustriSlot.Clear();
        allintrustriSlot.AddRange(inputintrustriSlot);

        for (int i = 0; i < allintrustriSlot.Count; i++)
        {
            allintrustriSlot[i].inistialiseSlot();
        }
    }

    public void Update()
    {
        _tierAtual = PlayerPrefs.GetInt("tier");
        if (_tierAtual != _tier)
            tierUpdate();
    }
    void tierUpdate()
    {
        for (int i = 0; i < butom.tier[_tierAtual].Butom.Count; i++)
        {
            Debug.Log("mudando tier");
            butom.tier[_tierAtual].Butom[i].gameObject.SetActive(true);
        }

        _tier = _tierAtual;
    }

    public void UpdateDataMission(int MissionInEvents)
    {
        mission.updateDatainFucion(MissionInEvents, inputintrustriSlot, BackGrandImager);
    }

    public void UpdateDataMission(Button thisButom)
    {
        mission.butom = thisButom; //item
        mission.ItemMenu = item;
        mission.selectionMenu = SelectionMenu;
    }

}
[System.Serializable]
public class tierButon
{
    public List<tierList> tier = new List<tierList>();
}

[System.Serializable]
public class tierList
{
    public List<Button> Butom = new List<Button>();

}