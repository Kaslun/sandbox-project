using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC/NPC")]
public class NPC : Controller
{
    [Header("AI")]
    public bool canInteract;
}
