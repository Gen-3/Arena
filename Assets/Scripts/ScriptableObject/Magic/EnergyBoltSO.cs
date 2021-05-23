﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnergyBoltSO: MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);
        int damage = (int)Random.Range((user.men - target.men) / 4 * (1 - target.resistanceMagic / 100), ((user.men - target.men) / 3 + 10) * (1 - target.resistanceMagic / 100));
        if (damage < 0) { damage = 0; }
        target.Damage(damage);
        Debug.Log($"{user.name}のエナジーボルトで{target.name}に{damage}のダメージ(最小値は{(user.men - target.men) / 4 * (1 - target.resistanceMagic / 100)}、最大値は{((user.men - target.men) / 3 + 10) * (1 - target.resistanceMagic / 100)})");
        Pronpter.instance.UpdateConsole($"{user.unitName}のエナジーボルトで{target.unitName}に{damage}のダメージ");

        //ダメージ後の処理（BattleManager内でforEachを使って撃破処理をしようとしたが仕様でできないらしく、こちらに記述）
        if (target is EnemyManager)
        {
            ((EnemyManager)target).CheckHP();
        }
        //敵の攻撃の場合のプレイヤーノックアウト判定やゲーオーバー処理はBattleManagerに記述

    }


}
