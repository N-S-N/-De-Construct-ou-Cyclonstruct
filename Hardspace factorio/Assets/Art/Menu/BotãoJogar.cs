using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Control : MonoBehaviour
{
    [SerializeField] VideoPlayer vidio;
    private int sala = -1;
    public void CenaTestAndBuildSystem(int salaLOad)
    {
        //SceneManager.LoadScene("Test and Buld System");
        if (salaLOad+1 == 2)
        {
            SceneManager.LoadSceneAsync(salaLOad);
            return;
        }

        vidio.Play();
        sala = salaLOad;
        float timevidio = vidio.clip.frameCount/ (float)vidio.clip.frameRate;
       // Debug.Log(timevidio);
        Invoke("LoadSene",timevidio);
    }

    void LoadSene()
    {
        SceneManager.LoadSceneAsync(sala);
    }
}