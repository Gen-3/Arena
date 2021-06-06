using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SleepSO : MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        float successRate = user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        float rundomNumber = Random.Range(0, 101);

        if( rundomNumber <= successRate)
        {
            target.sleep = true;
            Debug.Log($"{target}のスリープが成功({successRate}%/R={rundomNumber})");
        }
        else
        {
            Debug.Log($"{target}のスリープが失敗({successRate}%/R={rundomNumber})");
        }
    }
}
