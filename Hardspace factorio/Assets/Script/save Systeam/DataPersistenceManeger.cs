using System.Collections.Generic;
using UnityEngine;
using System;
using static missaoScripter;
using System.IO;

public class DataPersistenceManeger : MonoBehaviour
{
    [Header("espaso de save")]
    public int saveSlot;

    [Header("nome do arguivo")]
    [SerializeField] string saveFileName;

    [Header("variaves e scripts")]
    [SerializeField] PlacementSysteam placemente;

    [SerializeField] ObjectPlacer objplamente;

    [SerializeField] GameObject player;

    [SerializeField] ObjectsDataBaseSO DataBaseSO;

    [Header("grid")]
    [SerializeField] Grid _grid;

    [Header("save")]
    public SaveInGame SaveGameVariavel;

    [Header("Debug")]
    [SerializeField] constucao debagconstru;

    

    private void Awake()
    {
        saveSlot = PlayerPrefs.GetInt("saveSlot");
        LoadGame();
    }

    public void LoadGame()
    {
        if (File.Exists(saveFileName + saveSlot))
        {
            //pegando do json e passaando para lista
            string jsonData = File.ReadAllText(saveFileName + saveSlot);

            SaveGameVariavel = JsonUtility.FromJson<SaveInGame>(jsonData);

            //usando a lista para o inventario e obj

        }
    }

    public void SaveGame()
    {
        //limpando o seve
        SaveGameVariavel.positionPlayer = Vector3.zero;
        SaveGameVariavel.constucao.Clear();
        //colocando o save
        SaveGameVariavel.positionPlayer = player.transform.position;

        //Debug.Log(Save.constucao.Count);

        for (int i = 0; i < objplamente.placedGameObject.Count; i++)
        {
            SaveGameVariavel.constucao.Add(debagconstru);
            Vector3Int GridPossision = _grid.WorldToCell(objplamente.positionOBJ[i]);

            SaveGameVariavel.constucao[i].position = GridPossision;
            SaveGameVariavel.constucao[i].rotacion = objplamente.rotacao[i];
            SaveGameVariavel.constucao[i].ID = objplamente.ID[i];

            //inventario da coisas
            Chest chest = objplamente.placedGameObject[i].GetComponentInChildren<Chest>();
            IndustrialScripts IndustrialScripts = objplamente.placedGameObject[i].GetComponentInChildren<IndustrialScripts>();
            missaoScripter missaoScripter = objplamente.placedGameObject[i].GetComponentInChildren<missaoScripter>();
            Belt Belt = objplamente.placedGameObject[i].GetComponentInChildren<Belt>();
            Spliter Spliter = objplamente.placedGameObject[i].GetComponentInChildren<Spliter>();
            tunioScript tunioScript = objplamente.placedGameObject[i].GetComponentInChildren<tunioScript>();
            garaScript garaScript = objplamente.placedGameObject[i].GetComponentInChildren<garaScript>();

            if (chest != null)
            {
                SaveGameVariavel.constucao[i].inventario.allintrustriSlot = chest.allChestSlot;
            }
            else if(IndustrialScripts != null )
            {
                SaveGameVariavel.constucao[i].inventario.inputintrustriSlot = IndustrialScripts.inputintrustriSlot;
                SaveGameVariavel.constucao[i].inventario.outputtrustriSlot = IndustrialScripts.outputtrustriSlot;
                SaveGameVariavel.constucao[i].inventario.Time = IndustrialScripts.TimeProduction;
                SaveGameVariavel.constucao[i].inventario.quatity = IndustrialScripts.quantityProduced;
                SaveGameVariavel.constucao[i].inventario.requiry = IndustrialScripts.requiredQuantity;
            }
            if (missaoScripter != null)
            {
                SaveGameVariavel.constucao[i].inventario.SaveSlot = missaoScripter.SaveSlot;
                SaveGameVariavel.constucao[i].inventario.missao = missaoScripter.missaoSelect;
            }
            if (Belt != null)
            {
                SaveGameVariavel.constucao[i].inventario.Iteam.Add(Belt.item);
            }
            if (Spliter != null)
            {
                SaveGameVariavel.constucao[i].inventario.Iteam.Add(Spliter.item);
            }
            if (tunioScript != null)
            {
                SaveGameVariavel.constucao[i].inventario.Iteam = tunioScript.Iteam;
            }
            if (garaScript != null)
            {
                SaveGameVariavel.constucao[i].inventario.Iteam.Add(garaScript.isRuning);
            }
        }
        //converter em tojson
        string jsonData = JsonUtility.ToJson( SaveGameVariavel,true);

        File.WriteAllText(saveFileName + saveSlot, jsonData);
    }
    
    public void NewGame()
    {
       
    }
    //debug

    private void OnApplicationQuit()
    {
        SaveGame(); // Debug.Log(GridPossision);
    }



}


#region list

[Serializable]
public class SaveInGame
{ 
    public Vector3 positionPlayer;
    public List<constucao> constucao = new List<constucao>();
}

[Serializable]
public class constucao
{
    public int ID;
    public Vector3Int position;
    public float rotacion;
    public inventario inventario;

}
[Serializable]
public class inventario 
{
    public List<Slot> allintrustriSlot = new List<Slot>(); // inventario
    public List<Slot> inputintrustriSlot = new List<Slot>(); //inventario
    public List<Slot> outputtrustriSlot = new List<Slot>(); //invntario
    public float Time; //produção
    public int[] quatity; //produção
    public List<int> requiry; //produção
    public List<InveltoryMissaoSloySave> SaveSlot = new List<InveltoryMissaoSloySave>(); //missaom
    public missaoDataList missao; //missao
    public List<GameObject> Iteam; //inventario
}

#endregion
