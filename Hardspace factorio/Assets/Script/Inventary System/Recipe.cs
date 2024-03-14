using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe",menuName ="Inventory/Recipe")]
public class Recipe : ScriptableObject
{
    public GameObject[] createdItemPrefab;
    public int[] quantityProduced;
    public float timeProducedForSeconds = 1;

    public List<requiredIngredients> requiredIngredients = new List<requiredIngredients>();
    
    float porminitis(float prminits)
    {
        return prminits * 60;
    } 

}

[System.Serializable]
public class requiredIngredients
{
    public int IdItem;
    public int requiredQuantity;
}
