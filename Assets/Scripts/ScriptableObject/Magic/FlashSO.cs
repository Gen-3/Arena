using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FlashSO : MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        int successRate = user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        int rundomNumber = Random.Range(0, 101);

        if( rundomNumber <= successRate)
        {
            target.flash = true;
            Debug.Log($"{target}のフラッシュが成功({successRate}%/R={rundomNumber})");
        }
        else
        {
            Debug.Log($"{target}のフラッシュが失敗({successRate}%/R={rundomNumber})");
        }
    }
}
