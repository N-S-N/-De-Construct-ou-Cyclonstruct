using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventData : MonoBehaviour
{
    [Header("missao")]
    public List<UnityEvent> EvenosMissao = new List<UnityEvent>();
    public List<UnityEvent> EvenosMissaoTier = new List<UnityEvent>();

    #region Events
    public void NivelDoTierDesbloquiado(int i)
    {
        PlayerPrefs.SetInt("tier", i);
    }

    #endregion
}
