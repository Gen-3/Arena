using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FireStormSO: MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        Debug.Log("ファイアストーム！");
    }
}
