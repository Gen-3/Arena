using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SilenceSO : MagicBaseSO
{
    [SerializeField] GameObject effect;

    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        float successRate = 30 + user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        float rundomNumber = Random.Range(0, 101);

        if( rundomNumber <= successRate)
        {
            target.silence = true;
            Debug.Log($"{target}へのサイレンスが成功({successRate}%/R={rundomNumber})");
            TextManager.instance.UpdateConsole($"{target.unitName}は魔法を使えなくなった");
        }
        else
        {
            Debug.Log($"{target}へのサイレンスが失敗({successRate}%/R={rundomNumber})");
            TextManager.instance.UpdateConsole($"{user.unitName}のサイレンスは失敗した");
        }
        target.MagicEffectNoDamage(user, target, effect, 2f);
    }
}
