using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SilenceSO : MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        int successRate = user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        int rundomNumber = Random.Range(0, 101);

        if( rundomNumber <= successRate)
        {
            target.silence = true;
            Debug.Log($"{target}へのサイレンスが成功({successRate}%/R={rundomNumber})");
        }
        else
        {
            Debug.Log($"{target}へのサイレンスが失敗({successRate}%/R={rundomNumber})");
        }
    }
}
