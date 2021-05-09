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

    //状態異常のbool変数
    public bool flash;
    public bool protect;
    public bool slow;
    public bool sleep;
    public bool power;
    public bool quick;
    public bool silence;


    public virtual void Damage(int amount)
    {
        hp -= amount;
        if (sleep)
        {
            sleep = false;
            Debug.Log($"{unitName}のSleep状態が解除");
        }
    }

    public virtual void ExecuteDirectAttack(Battler attacker, Battler target)
    {
        int hit = 70 + attacker.dex - target.agi;
        if (flash)
        {
            hit /= 2;
        }

        int protectCoefficient;
        if (target.protect)
        {
            protectCoefficient = 1;
            Debug.Log($"{target}はプロテクトなう");
        }
        else
        {
            protectCoefficient = 0;
        }

        int powerCoefficient;
        if (attacker.power)
        {
            powerCoefficient = 1;
        }
        else
        {
            powerCoefficient = 0;
        }

        int damageMin = ((attacker.atk + powerCoefficient * attacker.atk) * attacker.dex / 100 - target.def - protectCoefficient * target.def) / 10;
        int damageMax = (attacker.atk + powerCoefficient * attacker.atk - target.def - protectCoefficient * target.def) / 10;
        if (damageMax < 1) { damageMax = 1; }

        float rundomNumber = Random.Range(0f, 100f);
        if (rundomNumber < hit)
        {
            int damage = Random.Range(damageMin, damageMax);
            if (damage <= 0) { damage = 0; };
            target.Damage(damage);
            Debug.Log($"{attacker.unitName}の攻撃で{target.unitName}に{damage}のダメージ！({damageMin}-{damageMax}/{hit}%)(残りHPは{target.hp})");
            Pronpter.instance.UpdateConsole($"{attacker.unitName}の攻撃で{target.unitName}に{damage}のダメージ");
        }
        else
        {
            Debug.Log($"{attacker.unitName}の攻撃を{target.unitName}が回避した({damageMin}-{damageMax}/{hit}%)");
            Pronpter.instance.UpdateConsole($"{attacker.unitName}の攻撃を{target.unitName}が回避した");
        }
    }

    /*
    public virtual void ExecuteMagic(Battler attacker, Battler target)
    {

    }
    */
    
}