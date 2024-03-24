using TMPro;
using UnityEngine;




public class UiMeneger : MonoBehaviour
{
    [SerializeField] TMP_Text atalho;

    private void Update()
    {
        if (atalho.gameObject.activeSelf)
        {
            Vector2 mause = Input.mousePosition;
            atalho.gameObject.transform.position = mause;
        }
    }
    public void selectionButon(string atanhoButon)
    {
        Vector2 mause = Input.mousePosition;        
        atalho.gameObject.SetActive(true);
        atalho.gameObject.transform.position = mause;
        atalho.text = atanhoButon;
    }
    public void DeselectselectionButon()
    {      
        atalho.gameObject.SetActive(false);
        atalho.text = "";
    }

}


