using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnergyBoltSO: MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        Debug.Log("エナジーボルト開始");
        int damage = (int)Random.Range((user.men - target.men) / 4, (user.men - target.men) / 3 + 10);
        if (damage < 0) { damage = 0; }
        //Debug.Log($"damageは{damage}で、最小値は{(user.men - target.men) / 4}、最大値は{(user.men - target.men) / 3 + 10}");
        target.Damage(damage);
        Debug.Log($"{user.name}のエナジーボルトで{target.name}に{damage}のダメージ");

        //ダメージ後の処理（BattleManager内でforEachを使って撃破処理をしようとしたが仕様でできないらしく、こちらに記述）
        if (target is EnemyManager)
        {
            ((EnemyManager)target).CheckHP();
        }
        //敵の攻撃の場合のプレイヤーノックアウト判定やゲーオーバー処理はBattleManagerに記述

    }


}
