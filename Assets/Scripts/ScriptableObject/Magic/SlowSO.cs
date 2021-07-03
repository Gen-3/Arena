using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SlowSO : MagicBaseSO
{
    [SerializeField] GameObject effect;

    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        float successRate = 20 + user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        float rundomNumber = Random.Range(0, 101);

        if( rundomNumber <= successRate)
        {
            target.slow = true;
            Debug.Log($"{target}のスロウが成功({successRate}%/R={rundomNumber})");
            TextManager.instance.UpdateConsole($"{target.unitName}の動きが遅くなった");
        }
        else
        {
            Debug.Log($"{target}のスロウが失敗({successRate}%/R={rundomNumber})");
            TextManager.instance.UpdateConsole($"{user.unitName}のスロウは失敗した");
        }
        target.MagicEffectNoDamage(user, target, effect, 2f);
    }
}
