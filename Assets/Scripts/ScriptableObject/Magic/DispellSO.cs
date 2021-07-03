using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DispellSO : MagicBaseSO
{
    [SerializeField] GameObject effect;

    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        float successRate = 50 + user.men - target.men ;
        if (successRate < 5) { successRate = 5; }
        float rundomNumber = Random.Range(0, 100);

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
            TextManager.instance.UpdateConsole($"{target.unitName}にかかっていた魔法の気配が消え去った");
        }
        else
        {
            Debug.Log($"{target}へのディスペルが失敗({successRate}%/R={rundomNumber})");
            TextManager.instance.UpdateConsole($"{user.unitName}のディスペルは失敗した");
        }

        target.MagicEffectNoDamage(user, target, effect, 2f);
        TextManager.instance.UpdateConsole($"{user.unitName}はディスペルを唱えた");
    }
}
