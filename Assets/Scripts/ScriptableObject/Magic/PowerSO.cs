using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PowerSO : MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        user.power = true;
    }
}
