using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class totorialGmae : MonoBehaviour
{
    [Header("objto torial")]
    [SerializeField] TMP_Text textoTotorial;
    [SerializeField] Image imagemTotorial;
    [Header("etapas do totorial")]
    [SerializeField]List<string> TextosDoTotorial = new List<string>();
    [SerializeField] List<Sprite> DesenhoDoTotorial = new List<Sprite>();
    int idex = -1;

    public void proximo(int isex)
    {
        if (idex >= isex) return;
        if (isex > DesenhoDoTotorial.Count) Destroy(gameObject);

        textoTotorial.text = TextosDoTotorial[isex];
        imagemTotorial.sprite = DesenhoDoTotorial[isex];
        idex = isex;
        

    } 
}
