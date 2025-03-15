using UnityEngine;
using System.Collections.Generic;


public abstract class EnemyAttackBank : MonoBehaviour
{
    public abstract Dictionary<string, Attack> attacks { get; set; }
}
