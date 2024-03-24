using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class objColliderPrevay : MonoBehaviour
{
    public List<Collider2D> ColaderDesativete  = new List<Collider2D>();
    public List<GameObject> isPrevayAtivet = new List<GameObject>();
    public List<UnityEvent> EventIsPrevay = new List<UnityEvent>();
}
