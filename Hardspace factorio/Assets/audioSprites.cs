using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSprites : MonoBehaviour
{
    [SerializeField] AudioSource musicamenu, intromenu;
    private void Start()
    {
        musicamenu.PlayDelayed(1f);
    }

}
