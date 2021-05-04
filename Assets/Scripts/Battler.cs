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
        //Debug.Log($"{this.name}に{amount}のダメージ（Battlerのダメージ関数）");
        
    }

    public virtual void ExecuteDirectAttack(Battler attacker, Battler target)
    {
        Debug.Log(flash);
        int hit = 70 + attacker.dex - target.agi;
        Debug.Log(hit);
        if (flash) { hit /= 2; }
        Debug.Log(hit+"(flash判定後)");

        float rundomNumber = Random.Range(0f, 100f);
        int protect;
        if (target.protect)
        {
            protect = 1;
            Debug.Log($"{target}はプロテクトなう");
        }
        else
        {
            protect = 0;
        }
        int damageMin = (attacker.atk * attacker.dex / 100 - target.def-protect*target.def) / 10;
        int damageMax = (attacker.atk - target.def - protect * target.def) / 10;
        if (damageMin < 1) { damageMin = 1; }

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