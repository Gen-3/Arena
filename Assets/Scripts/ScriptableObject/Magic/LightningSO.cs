using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LightningSO : MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        Debug.Log("ライトニング！");
    }
}
