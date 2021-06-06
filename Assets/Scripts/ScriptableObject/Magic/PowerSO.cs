using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PowerSO : MagicBaseSO
{
    [SerializeField] GameObject effect;

    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        user.power = true;

        target.MagicEffectNoDamage(user, target, effect, 2f);
        TextManager.instance.UpdateConsole($"{user.unitName}はパワーを唱えた");
    }
}
