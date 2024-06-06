using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventData : MonoBehaviour
{

    [Header("missao")]
    public List<UnityEvent> EvenosMissao = new List<UnityEvent>();
    public List<UnityEvent> EvenosMissaoTier = new List<UnityEvent>();
    public UnityEvent destoryTier, destoynormal;

    [Header("OBJ")]
    [SerializeField] GameObject tier, missiom;

    #region Events
    public void NivelDoTierDesbloquiado(int i)
    {
        PlayerPrefs.SetInt("tier", i);
    }
    private void Start()
    {
        PlayerPrefs.SetInt("tier", 0);
    }

    public void activetetier()
    {
        tier.SetActive(true);

    }
    public void activetenormal()
    {
        missiom.SetActive(true);

    }

    #endregion
}
