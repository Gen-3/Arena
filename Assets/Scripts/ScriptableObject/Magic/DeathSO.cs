using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DeathSO: MagicBaseSO
{
    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);

        int successRate = user.men - target.men;
        if (successRate < 5) { successRate = 5; }
        int rundomNumber = Random.Range(0, 101);

        if (rundomNumber <= successRate)
        {
            int damage = target.hp;
            target.Damage(damage);
            Pronpter.instance.UpdateConsole($"{user.unitName}のデスで{target.unitName}に{damage}のダメージ");
        }
        //ダメージ後の処理（BattleManager内でforEachを使って撃破処理をしようとしたが仕様でできないらしく、こちらに記述）
        if (target is EnemyManager)
        {
            ((EnemyManager)target).CheckHP();
        }
        //敵の攻撃の場合のプレイヤーノックアウト判定やゲーオーバー処理はBattleManagerに記述
    }
}
