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
        float successRate = user.men - target.men;
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
        }
        else
        {
            Debug.Log($"{target}へのディスペルが失敗({successRate}%/R={rundomNumber})");
        }

        target.MagicEffectNoDamage(user, target, effect, 2f);
        TextManager.instance.UpdateConsole($"{user.unitName}はディスペルを唱えた");
    }
}
