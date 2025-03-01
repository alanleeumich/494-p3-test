using UnityEngine;
using System.Collections.Generic;

public abstract class EnemyAnims : MonoBehaviour
{
    public abstract Dictionary<string, AnimNode> animNodes { get; set; }
    public abstract List<RotateNode> rotateNodes { get; set; }
    public abstract List<MoveNode> moveNodes { get; set; }
    public abstract List<CounterNode> counterNodes { get; set; }
    public abstract string startAnim { get; set; }

    public abstract RotateNode defaultRotateLeft { get; set; }
    public abstract RotateNode defaultRotateRight { get; set; }

    public abstract string startParryStance { get; set; }
}
