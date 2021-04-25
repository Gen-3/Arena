using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnergyBoltSO: MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        Debug.Log("エナジーボルト！");
    }
}
