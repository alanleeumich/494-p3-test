using UnityEngine;
using System.Collections.Generic;

public class EnemyOne : EnemyAnims
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override Dictionary<string, AnimNode> animNodes { get; set; } = new Dictionary<string, AnimNode>
    {
        {"rest" , new AnimNode(animName: "rest", animId: 0,
            minRange: float.NegativeInfinity, maxRange: float.PositiveInfinity, rangePreference: 0,
            minDegree: -180, maxDegree: 180, degreePreference: 0,
            transitions: new List<AnimTransition>{
                new AnimTransition(3, "stepUppercut"),
                new AnimTransition(3, "2hit"),
                new AnimTransition(3, "spinSlash"),
                new AnimTransition(1, "flipHandspring"),
                new AnimTransition(1, "flipCartwheel"),

         })},

        {"stepUppercut" , new AnimNode(animName: "stepUppercut", animId: 5,
            minRange: 1f, maxRange: 2.5f, rangePreference: 0.2f,
            minDegree: -30, maxDegree: 30, degreePreference: 0.2f,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest")
         })},

        {"2hit" , new AnimNode(animName: "2hit", animId: 6,
            minRange: 1f, maxRange: 1.3f, rangePreference: 0.2f,
            minDegree: -30, maxDegree: 30, degreePreference: 0.2f,
            transitions: new List<AnimTransition>{
                new AnimTransition(2, "2hit_vertical"),
                new AnimTransition(1, "2hit_spinVertical"),
         })},

        {"2hit_vertical" , new AnimNode(animName: "2hit_vertical", animId: 7,
            minRange: 0, maxRange: 1.3f, rangePreference: 1f,
            minDegree: -30, maxDegree: 30, degreePreference: 1f,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "2hit_vertical_reset"),
                new AnimTransition(2, "2hit_vertical_uppercut"),
         })},

        {"2hit_spinVertical" , new AnimNode(animName: "2hit_spinVertical", animId: 8,
            minRange: float.NegativeInfinity, maxRange: float.PositiveInfinity, rangePreference: 0f,
            minDegree: -180, maxDegree: 180, degreePreference: 0f,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "2hit_vertical_reset"),
                new AnimTransition(5, "2hit_vertical_uppercut"),
         })},

        {"2hit_vertical_reset" , new AnimNode(animName: "2hit_vertical_reset", animId: 9,
            minRange: float.NegativeInfinity, maxRange: float.PositiveInfinity, rangePreference: 0,
            minDegree: -180, maxDegree: 180, degreePreference: 0,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         })},

        {"2hit_vertical_uppercut" , new AnimNode(animName: "2hit_spinVertical", animId: 10,
            minRange: 0.5f, maxRange: 3f, rangePreference: 1f,
            minDegree: -50, maxDegree: 50, degreePreference: 1f,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         })},

        {"spinSlash" , new AnimNode(animName: "spinSlash", animId: 11,
            minRange: 1f, maxRange: 2.5f, rangePreference: 0.2f,
            minDegree: -60, maxDegree: 60, degreePreference: 0.2f,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest")
         })},

        {"flipCartwheel" , new AnimNode(animName: "flipCartwheel", animId: 12,
            minRange: 1f, maxRange: 4f, rangePreference: 0.8f,
            minDegree: -30, maxDegree: 30, degreePreference: 0.8f,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         })},

        {"flipHandspring" , new AnimNode(animName: "flipHandspring", animId: 13,
            minRange: 1f, maxRange: 4f, rangePreference: 0.8f,
            minDegree: -30, maxDegree: 30, degreePreference: 0.8f,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         })},

    };

    public override List<RotateNode> rotateNodes { get; set; } = new List<RotateNode>
    {
        new RotateNode(animName:"defaultRotateLeft", animId: 1,
            minDegree: -180, maxDegree: 0, 1),
        new RotateNode(animName:"defaultRotateRight", animId: 2,
            minDegree: 0, maxDegree: 180, 1),

    };

    public override List<MoveNode> moveNodes { get; set; } = new List<MoveNode>
    {
        new MoveNode(animName: "walkForward", animId: 3,
            doesAnimLoop: true,
            minDistance: float.NegativeInfinity, maxDistance: 0,
            likelyhood: 1,
            manualMoveSpeed: 2.5f),
        new MoveNode(animName: "walkBack", animId: 4,
            doesAnimLoop: true,
            minDistance: 0, maxDistance: float.PositiveInfinity,
            likelyhood: 1,
            manualMoveSpeed: -2.5f)
    };

    public override List<CounterNode> counterNodes { get; set; } = new List<CounterNode> {
        new CounterNode(animName: "Armature_parryUpCounter",
            minAttackDegree: -45f, maxAttackDegree: 45,
            fromStance: "rest",
            minDegree: -30f, maxDegree: 30f,
            likelyhood: 1,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         }),
        new CounterNode(animName: "Armature_parryLeftCounterOne",
            minAttackDegree: 45, maxAttackDegree: 135f,
            fromStance: "rest",
            minDegree: -30f, maxDegree: 30f,
            likelyhood: 1,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         }),
        new CounterNode(animName: "Armature_parryLeftCounterTwo",
            minAttackDegree: 45f, maxAttackDegree: 135f,
            fromStance: "rest",
            minDegree: -30f, maxDegree: 30f,
            likelyhood: 2,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         }),
        new CounterNode(animName: "Armature_parryDownCounterOne",
            minAttackDegree: 135f, maxAttackDegree: -135f,
            fromStance: "rest",
            minDegree: -30f, maxDegree: 30f,
            likelyhood: 2,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         }),
        new CounterNode(animName: "Armature_parryDownCounterTwo",
            minAttackDegree: 135f, maxAttackDegree: -135f,
            fromStance: "rest",
            minDegree: -30f, maxDegree: 30f,
            likelyhood: 1,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         }),
        new CounterNode(animName: "Armature_parryRightCounterOne",
            minAttackDegree: -135f, maxAttackDegree: -45f,
            fromStance: "rest",
            minDegree: -30f, maxDegree: 30f,
            likelyhood: 1,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         }),
        new CounterNode(animName: "Armature_parryRightCounterTwo",
            minAttackDegree: -135f, maxAttackDegree: -45f,
            fromStance: "rest",
            minDegree: -30f, maxDegree: 30f,
            likelyhood: 1,
            transitions: new List<AnimTransition>{
                new AnimTransition(1, "rest"),
         })
    };

    public override string startAnim { get; set; } = "rest";

    public override RotateNode defaultRotateLeft { get; set; }
    public override RotateNode defaultRotateRight { get; set; }

    public override string startParryStance { get; set; } = "rest";
    private void Start()
    {
        defaultRotateLeft = rotateNodes[0];
        defaultRotateRight = rotateNodes[1];

    }



}
