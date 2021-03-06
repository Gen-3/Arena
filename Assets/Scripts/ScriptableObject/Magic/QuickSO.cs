using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuickSO : MagicBaseSO
{
    [SerializeField] GameObject effect;

    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        user.quick = true;

        target.MagicEffectNoDamage(user, target, effect, 2f);
        TextManager.instance.UpdateConsole($"{user.unitName}はクイックを唱えた");
    }
}
