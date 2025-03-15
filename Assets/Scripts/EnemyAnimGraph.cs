#nullable enable

using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class AnimTransition
{
    public string animNode;
    public float likelyhood;

    public AnimTransition(float likelyhood_, string animNode_)
    {
        likelyhood = likelyhood_;
        animNode = animNode_;
    }
}

public class AnimNode
{
    public string animName;
    public int animId;

    public float minRange;
    public float maxRange;
    public float rangePreference; // willingness to rellocate to perform this attack. [0,1]

    public float minDegree;
    public float maxDegree;
    public float degreePreference; // willingness to rotate to perform this attack. [0, 1]

    public delegate string TransitionOverride();
    public TransitionOverride? transitionOverride;

    public List<AnimTransition> transitions;

    public AnimNode(string animName, int animId,
        float minRange, float maxRange, float rangePreference,
        float minDegree, float maxDegree, float degreePreference,
        List<AnimTransition> transitions,
        TransitionOverride? transitionOverride = null)
    {
        this.animName = animName;
        this.animId = animId;
        this.minRange = minRange;
        this.maxRange = maxRange;
        this.rangePreference = rangePreference;
        this.minDegree = minDegree;
        this.maxDegree = maxDegree;
        this.degreePreference = degreePreference;
        this.transitionOverride = transitionOverride;
        this.transitions = transitions;
    }
}

public class RotateNode
{
    public string animName;
    public int animId;

    public float minDegree;
    public float maxDegree;

    public float likelyhood;

    public RotateNode(string animName, int animId, float minDegree, float maxDegree, float likelyhood)
    {
        this.animName = animName;
        this.animId = animId;
        this.minDegree = minDegree;
        this.maxDegree = maxDegree;
        this.likelyhood = likelyhood;
    }
}

public class MoveNode
{
    public string animName;
    public int animId;
    public bool doesAnimLoop;
    public float manualMoveSpeed;

    public float minDistance;
    public float maxDistance;

    public float likelyhood;

    public MoveNode(string animName, int animId, bool doesAnimLoop, float minDistance, float maxDistance, float likelyhood, float manualMoveSpeed = 0)
    {
        this.animName = animName;
        this.animId = animId;
        this.doesAnimLoop = doesAnimLoop;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
        this.likelyhood = likelyhood;
        this.manualMoveSpeed = manualMoveSpeed;
    }
}

public class CounterNode : AnimNode
{
    public float minAttackDegree;
    public float maxAttackDegree;
    public string fromStance;

    public float likelyhood;

    public CounterNode(
       string animName,
       float minAttackDegree, float maxAttackDegree, string fromStance, float minDegree, float maxDegree, float likelyhood,
       List<AnimTransition> transitions) : base(animName, -1, 0, 0, 0, minDegree, maxDegree, 0, transitions)
    {
        this.fromStance = fromStance;
        this.minAttackDegree = minAttackDegree;
        this.maxAttackDegree = maxAttackDegree;
        this.likelyhood = likelyhood;
    }
}

public class EnemyAnimGraph : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform target;
    public Animator animator;

    Dictionary<string, AnimNode> animNodes;
    List<RotateNode> rotateNodes;
    List<MoveNode> moveNodes;
    List<CounterNode> counterNodes;

    AnimNode currentAnimNode;
    RotateNode defaultRotateLeft;
    RotateNode defaultRotateRight;

    enum Substate
    {
        MainAnim,
        Rotate,
        Move,
    }
    Substate animNodeSubstate = Substate.MainAnim;


    public delegate bool ManualNextClipCondition(Transform transform_);
    ManualNextClipCondition? manualNextClipCondition = null;

    EnemyRootMove rootMove;

    string parryStance;

    void Start()
    {
        EnemyAnims enemyAnims = GetComponent<EnemyAnims>();
        animNodes = enemyAnims.animNodes;
        rotateNodes = enemyAnims.rotateNodes;
        moveNodes = enemyAnims.moveNodes;
        counterNodes = enemyAnims.counterNodes;
        currentAnimNode = animNodes[enemyAnims.startAnim];
        defaultRotateLeft = enemyAnims.defaultRotateLeft;
        defaultRotateRight = enemyAnims.defaultRotateRight;

        rootMove = GetComponent<EnemyRootMove>();
        animator.speed = 1f;
        parryStance = enemyAnims.startParryStance;


        
    }

    void Update()
    {
        if (manualNextClipCondition != null)
        {
            if (manualNextClipCondition(transform))
            {
                manualNextClipCondition = null;
                DetermineNextAnimClip();
            }
        }
    }

    public void Stagger()
    {
        animator.Play("Armature_staggerUp");
        animator.SetInteger("transition", 0);
        animNodeSubstate = Substate.MainAnim;
        currentAnimNode = animNodes[GetComponent<EnemyAnims>().startAnim];
    }

    public bool Parry(float attackAngle)
    {
        
        (float distance, float angleOffset) = GetDistanceAndAngleToTarget();
        List<float> likelyhoods = new List<float>();
        bool foundParryNode = false;
        foreach (CounterNode counterNode in counterNodes)
        {
            bool match = IsAngleInArc(attackAngle, counterNode.minAttackDegree, counterNode.maxAttackDegree)
                && IsAngleInArc(angleOffset, counterNode.minDegree, counterNode.maxDegree)
                && counterNode.fromStance == parryStance;
            likelyhoods.Add(match ? counterNode.likelyhood : 0);
            if (match)
            {
                foundParryNode = true;
            }
        }
        if (!foundParryNode)
        {
            return false;
        }
        likelyhoods = likelyhoods.Select(i => i / likelyhoods.Sum()).ToList();
        currentAnimNode = counterNodes[SampleIndex(likelyhoods)];
        animator.CrossFade(currentAnimNode.animName, 0.08f);
        animNodeSubstate = Substate.MainAnim;
        rootMove.speed = 0;
        return true;
    }

    public void DetermineNextAnimClip()
    {
        rootMove.speed = 0;

        (float distance, float angleOffset) = GetDistanceAndAngleToTarget();
        
        if (animNodeSubstate == Substate.MainAnim)
        {
            currentAnimNode = GetNextNode(distance, angleOffset);
        }
        bool needsMove = distance < currentAnimNode.minRange || distance > currentAnimNode.maxRange;
        bool needsRotate = angleOffset < currentAnimNode.minDegree || angleOffset > currentAnimNode.maxDegree;

        if (needsRotate && needsMove)
        {
            RotateNode rotateNode = GetRotateNode(angleOffset, true);
            animator.SetInteger("transition", rotateNode.animId);
            animNodeSubstate = Substate.Rotate;
        }
        else if (needsRotate)
        {
            RotateNode rotateNode = GetRotateNode(angleOffset, false);
            animator.SetInteger("transition", rotateNode.animId);
            animNodeSubstate = Substate.Rotate;
        }
        else if (needsMove)
        {
            MoveNode moveNode = GetMoveNode(distance, currentAnimNode.minRange, currentAnimNode.maxRange);
            animator.SetInteger("transition", moveNode.animId);
            rootMove.speed = moveNode.manualMoveSpeed;
            animNodeSubstate = Substate.Move;
            if (moveNode.doesAnimLoop)
            {
                bool StopLoopMove(Transform transform_)
                {
                    Vector3 targetVector = target.position - transform_.position;
                    targetVector.y = 0;
                    if (currentAnimNode.minRange <= targetVector.magnitude && currentAnimNode.maxRange >= targetVector.magnitude)
                    {
                        return true;
                    }
                    return false;
                }

                manualNextClipCondition = StopLoopMove;
            }

        }
        else
        {
            animator.SetInteger("transition", currentAnimNode.animId);
            animNodeSubstate = Substate.MainAnim;
        }
    }

    MoveNode GetMoveNode(float currentDistance, float targetMinDistance, float targetMaxDistance)
    {
        List<float> likelyhoods = new List<float>();
        foreach (MoveNode moveNode in moveNodes)
        {
            float minPossible = currentDistance + moveNode.minDistance;
            float maxPossible = currentDistance + moveNode.maxDistance;
            bool match = targetMinDistance <= maxPossible && minPossible <= targetMaxDistance;
            likelyhoods.Add(match ? moveNode.likelyhood : 0);
        }
        likelyhoods = likelyhoods.Select(i => i / likelyhoods.Sum()).ToList();
        return moveNodes[SampleIndex(likelyhoods)];
    }

    RotateNode GetRotateNode(float angleOffset, bool onlyDefault)
    {
        if (onlyDefault)
        {
            if (angleOffset < 0)
            {
                return defaultRotateLeft;
            }
            return defaultRotateRight;
        }
        List<float> likelyhoods = new List<float>();
        foreach (RotateNode rotateNode in rotateNodes)
        {
            bool match = IsAngleInArc(angleOffset, rotateNode.minDegree, rotateNode.maxDegree);
            likelyhoods.Add(match ? rotateNode.likelyhood : 0);
        }
        likelyhoods = likelyhoods.Select(i => i / likelyhoods.Sum()).ToList();
        return rotateNodes[SampleIndex(likelyhoods)];
    }


    AnimNode GetNextNode(float distance, float angleOffset)
    {
        
        if (currentAnimNode.transitionOverride != null)
        {
            return animNodes[currentAnimNode.transitionOverride()];
        }

        List<float> likelyhoods = new List<float>();
        foreach (AnimTransition transition in currentAnimNode.transitions)
        {
            AnimNode node = animNodes[transition.animNode];
            bool needsMove = distance < node.minRange || distance > node.maxRange;
            bool needsRotate = !IsAngleInArc(angleOffset, node.minDegree, node.maxDegree);

            float moveWeight = needsMove ? node.rangePreference : 0;
            float angleWeight = needsRotate ? node.degreePreference : 0;

            float likelyhood = transition.likelyhood * (1 - moveWeight) * (1 - angleWeight);
            likelyhoods.Add(likelyhood);
        }
        likelyhoods = likelyhoods.Select(i => i / likelyhoods.Sum()).ToList();
        return animNodes[currentAnimNode.transitions[SampleIndex(likelyhoods)].animNode];

    }

    static int SampleIndex(List<float> probabilities)
    {
        float randomValue = Random.Range(0f,1f); // Random value between 0 and 1
        float cumulativeSum = 0.0f;

        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulativeSum += probabilities[i];
            if (randomValue < cumulativeSum)
            {
                return i;
            }
        }

        return probabilities.Count - 1; // Fallback in case of rounding issues
    }

    (float, float) GetDistanceAndAngleToTarget()
    {
        Vector3 targetVector = target.position - transform.position;
        targetVector.y = 0;
        float angle = Vector3.SignedAngle(transform.forward, targetVector, Vector3.up);
        return (targetVector.magnitude, angle);
    }

    bool IsAngleInArc(float angle, float startAngle, float endAngle)
    {
        if (startAngle == -180 && endAngle == 180)
        {
            return true;
        }

        angle = Mathf.Repeat(angle, 360);
        startAngle = Mathf.Repeat(startAngle, 360);
        endAngle = Mathf.Repeat(endAngle, 360);

        if (startAngle <= endAngle)
        {
            // Normal case: arc does not wrap around 360
            return angle >= startAngle && angle <= endAngle;
        }
        else
        {
            // Arc wraps around 360 (e.g., 350° to 10°)
            return angle >= startAngle || angle <= endAngle;
        }
    }

}
