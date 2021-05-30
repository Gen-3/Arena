using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnergyBoltSO: MagicBaseSO
{
    [SerializeField] GameObject boltEffect;

    public override void Execute(Battler user, Battler target)
    {
        base.Execute(user, target);

        float damageMin = (user.men - target.men) / 10 * (1 - target.resistanceMagic / 100);
        if (damageMin < -9)
        {
            damageMin = -9;
        }
        float damageMax = ((user.men - target.men) / 10 + 10) * (1 - target.resistanceMagic / 100);
        if (damageMax < 0)
        {
            damageMax = 0;
        }
        float damage = Random.Range(damageMin, damageMax);
        if (damage < 0) { damage = 0; }
        target.Damage(damage,user,target);

        //Instantiate(boltEffect, target.transform.position,Quaternion.identity);

        Debug.Log($"{user.name}のエナジーボルトで{target.name}に{damage}のダメージ({(user.men - target.men) / 10 * (1 - target.resistanceMagic / 100)}~{((user.men - target.men) / 10 + 10) * (1 - target.resistanceMagic / 100)})(残りHPは{target.hp})");
        TextManager.instance.UpdateConsole($"{user.unitName}のエナジーボルトで{target.unitName}に{(int)damage}のダメージ");

        Instantiate(boltEffect, target.transform.transform.position, Quaternion.identity);

        if (target is EnemyManager)
        {
            ((EnemyManager)target).CheckHP();
        }
        //敵の攻撃の場合のプレイヤーノックアウト判定やゲーオーバー処理はBattleManagerに記述
    }
}
