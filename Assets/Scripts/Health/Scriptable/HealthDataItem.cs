using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Health/healthDataItem", fileName = "Health Data Item")]
public class HealthDataItem : ScriptableObject
{     
    public int initialHealth;
    public int maxHealth;  
}