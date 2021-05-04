using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ProtectSO : MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        user.protect = true;
    }
}
