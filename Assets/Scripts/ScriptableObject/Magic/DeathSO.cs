using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DeathSO: MagicBaseSO
{
    [SerializeField] GameObject deathEffect;

    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);

        float successRate = user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        float rundomNumber = Random.Range(0, 100);

        if (rundomNumber <= successRate)
        {
            float damage = target.hp;
            target.DamageAndEffect(damage, user, target, deathEffect, 1f);
            TextManager.instance.UpdateConsole($"{user.unitName}は息の根を止められた");
            Debug.Log($"デス成功(成功率{successRate}%)");
        }
        else
        {
            float damage = 0;
            target.DamageAndEffect(damage, user, target, deathEffect, 1f);
            TextManager.instance.UpdateConsole($"{user.unitName}のデスは失敗した");
            Debug.Log($"デス失敗(成功率{successRate}%)");
        }
        /*ダメージ後の処理（BattleManager内でforEachを使って撃破処理をしようとしたが仕様でできないらしく、こちらに記述）
        if (target is EnemyManager)
        {
            ((EnemyManager)target).CheckHP();
        }
        *///敵の攻撃の場合のプレイヤーノックアウト判定やゲーオーバー処理はBattleManagerに記述
    }
}
