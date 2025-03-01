using UnityEngine;
using System.Collections.Generic;
public class AttackBankOne : EnemyAttackBank
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override Dictionary<string, Attack> attacks { get; set; } = new Dictionary<string, Attack>()
    {
        {"2hit:0", new Attack(270)},
        {"2hit:1", new Attack(30, enableParryOnEnd: true)},
        {"2hit_spin:0", new Attack(45)},
        {"2hit_spin:1", new Attack(0, canStagger: true, enableParryOnEnd: true)},
        {"2hit_vertical_uppercut:0", new Attack(160, enableParryOnEnd: true)},
        {"flip_cartwheel:0", new Attack(-50)},
        {"flip_cartwheel:1", new Attack(0)},
        {"flip_cartwheel:2", new Attack(-135, enableParryOnEnd: true)},
        {"flip_handspring:0", new Attack(-50)},
        {"flip_handspring:1", new Attack(0)},
        {"flip_handspring:2", new Attack(0, canStagger: true, enableParryOnEnd: true)},
        {"spinSlash:0", new Attack(-90)},
        {"spinSlash:1", new Attack(-65, enableParryOnEnd: true)},
        {"stepUppercut:0", new Attack(-145)},
        {"stepUppercut:1", new Attack(0, canStagger: true, enableParryOnEnd: true)},

        {"upCounter:0", new Attack(45)},
        {"upCounter:1", new Attack(-145, enableParryOnEnd: true)},

        {"leftCounterOne:0", new Attack(-65,enableParryOnEnd: true)},
        {"leftCounterTwo:0", new Attack(0,canStagger: true,enableParryOnEnd: true)},

        {"downCounterOne:0", new Attack(90,enableParryOnEnd: true)},
        {"downCounterTwo:0", new Attack(-65,enableParryOnEnd: true)},
        {"rightCounterOne:0", new Attack(-160,enableParryOnEnd: true)},
        {"rightCounterTwo:0", new Attack(0,canStagger: true,enableParryOnEnd: true)},
    };
}
