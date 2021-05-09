using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DispelSO : MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        int successRate = user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        int rundomNumber = Random.Range(0, 101);

        if( rundomNumber <= successRate)
        {
            target.flash = false;
            target.protect = false;
            target.slow = false;
            target.sleep = false;
            target.power = false;
            target.quick = false;
            target.silence = false;
            Debug.Log($"{target}へのディスペルが成功({successRate}%/R={rundomNumber})");
        }
        else
        {
            Debug.Log($"{target}へのディスペルが失敗({successRate}%/R={rundomNumber})");
        }
    }
}
