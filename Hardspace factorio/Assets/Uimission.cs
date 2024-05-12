using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Uimission : MonoBehaviour
{
    public List<SpacoMissaon> SpacoMissaon = new List<SpacoMissaon>();
}

[Serializable]
public class SpacoMissaon
{
    public Image UIImageIconir;
    public TMP_Text TextMissionMissao;
}
