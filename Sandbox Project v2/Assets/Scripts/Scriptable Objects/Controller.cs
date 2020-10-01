using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Controller", menuName = "AI/Controller")]
public class Controller : ScriptableObject
{
    [Header("Identity")]
    public new string name;
    public GameObject prefab;

    [Header("Attributes")]
    public int health;
    public int damage, speed, reach;
}
