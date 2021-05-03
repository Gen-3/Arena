using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public string unitName;
    //ユニット自身のパラメータ
    public int str;
    public int dex;
    public int agi;
    public int vit;
    public int men;

    //装備の影響等を計算して得られるパラメータ
    public int wt;
    public int hp;
    public int atk;
    public int def;
    public int mob;

    public Battler target = default;
    public Vector3Int targetPosition = default;

    public virtual void Damage(int amount)
    {
        hp -= amount;
        //Debug.Log($"{this.name}に{amount}のダメージ（Battlerのダメージ関数）");
        
    }

    public virtual void ExecuteDirectAttack(Battler attacker, Battler target)
    {
        int hit = 70 + attacker.dex - target.agi;
        float rundomNumber = Random.Range(0f, 100f);

        int damageMin = (attacker.atk * attacker.dex / 100 - target.def) / 10;
        int damageMax = (attacker.atk - target.def) / 10;
        if (damageMin < 1) { damageMin = 1; }

        if (rundomNumber < hit)
        {
            int damage = Random.Range(damageMin, damageMax);
            if (damage <= 0) { damage = 0; };
            target.Damage(damage);
            Debug.Log($"{attacker.unitName}の攻撃で{target.unitName}に{damage}のダメージ！({damageMin}-{damageMax}/{hit}%)(残りHPは{target.hp})");

        }
        else
        {
            Debug.Log($"{attacker.unitName}の攻撃を{target.unitName}が回避した({damageMin}-{damageMax}/{hit}%)");
        }
    }

    /*
    public virtual void ExecuteMagic(Battler attacker, Battler target)
    {

    }
    */
    
}