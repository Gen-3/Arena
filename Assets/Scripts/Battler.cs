using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Battler : MonoBehaviour
{
    public bool done = false; 

    public string unitName;
    //ユニット自身のパラメータ
    public float str;
    public float dex;
    public float agi;
    public float vit;
    public float men;

    //装備の影響等を計算して得られるパラメータ
    public float wt;
    public float hp;
    public float atk;
    public float def;
    public float mob;

    //敵は既定値、自身は装備依存のパラメータ
    public float resistanceFire;//減衰割合を0-100で表す
    public float resistanceMagic;
    public float weight;

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

    //[SerializeField] DamageUI damageUI;
    [SerializeField] DamageUI DamageUICanvas = default;

    [SerializeField] GameObject attackEffect;
//    [SerializeField] GameObject boltEffect;
    [SerializeField] GameObject fireEffect;
//    [SerializeField] GameObject lightningEffect;
//    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject debuffEffect;

    public virtual void DamageAndEffect(float amount, Battler attacker, Battler target, GameObject effectPrefab, float delayTime)
    {
        StartCoroutine(DamageAndEffectCor(amount, attacker, target, effectPrefab, delayTime));
    }
    public virtual IEnumerator DamageAndEffectCor(float amount, Battler attacker, Battler target, GameObject effectPrefab, float delayTime)
    {
        Vector3 effectPosition = new Vector3(target.transform.transform.position.x, target.transform.transform.position.y, target.transform.transform.position.z - 1);
        Instantiate(effectPrefab, effectPosition, Quaternion.identity);
        yield return new WaitForSeconds(delayTime);
        Damage(amount,attacker,target);//ダメージ処理→Sleep解除処理→テキスト処理
    }
    public virtual void Damage(float amount, Battler attacker, Battler target)
    {
        StartCoroutine(DamageCoroutine(amount,attacker,target));
    }

    IEnumerator DamageCoroutine(float amount, Battler attacker, Battler target)
    {
        //ここに攻撃エフェクト呼び出しを入れる
        hp -= amount;

        //damageUI.ShowDamage(amount,attacker,target);

        if (sleep)
        {
            sleep = false;
            Debug.Log($"{unitName}のSleep状態が解除");
        }
        DamageUI damageUICanvus = Instantiate(DamageUICanvas, target.transform.transform.position, Quaternion.identity);
        damageUICanvus.ShowDamage(amount);

        yield return new WaitForSeconds(0.5f);

        if (target is EnemyManager)
        {
            ((EnemyManager)target).CheckHP();
        }

        attacker.done = true;
    }



    public void MagicEffectNoDamage(Battler attacker, Battler target, GameObject effectPrefab, float delayTime)
    {
        StartCoroutine(MagicEffectNoDamageCoroutine(attacker, target, effectPrefab, delayTime));
    }
    IEnumerator MagicEffectNoDamageCoroutine(Battler attacker, Battler target, GameObject effectPrefab, float delayTime)
    {
        Vector3 effectPosition = new Vector3(target.transform.transform.position.x, target.transform.transform.position.y, target.transform.transform.position.z - 1);
        Instantiate(effectPrefab, effectPosition, Quaternion.identity);
        yield return new WaitForSeconds(delayTime);
        attacker.done = true;
    }



    public virtual void ExecuteDirectAttack(Battler attacker, Battler target)
    {
        StartCoroutine(ExecuteDirectAttackCoroutine(attacker, target));
    }
    IEnumerator ExecuteDirectAttackCoroutine(Battler attacker, Battler target)
    {
        float hit = 70 + attacker.dex - target.agi;
        if (flash)
        {
            hit /= 2;
        }
        if (hit < 5)
        {
            hit = 5;
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

        float damageMin = (attacker.atk + powerCoefficient * attacker.str / 2 - target.def - protectCoefficient * target.vit / 2) / 5 - (1 - attacker.dex / 100) * 10 - 10;
        if (damageMin < -5) { damageMin = -5; }
        float damageMax = (attacker.atk + powerCoefficient * attacker.str / 2 - target.def - protectCoefficient * target.vit / 2) / 5;
        if (damageMax < 1) { damageMax = 1; }

        float rundomNumber = Random.Range(0f, 100f);
        if (rundomNumber < hit)
        {
            float damage = Random.Range(damageMin, damageMax);
            if (damage < 0) { damage = 0; };

            Instantiate(attackEffect, target.transform.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);

            target.Damage(damage, attacker, target);
            Debug.Log($"{attacker.unitName}の攻撃で{target.unitName}に{damage}のダメージ！({damageMin}~{damageMax}/{hit}%)(残りHPは{target.hp})");
            Debug.Log($"atk{attacker.atk} + Pc{powerCoefficient} * str{attacker.str} - def{target.def} - Pc{protectCoefficient} * vit{target.vit}");
            TextManager.instance.UpdateConsole($"{attacker.unitName}の攻撃で{target.unitName}に{(int)damage}のダメージ");
        }
        else
        {
            Debug.Log($"{attacker.unitName}の攻撃を{target.unitName}が回避した({damageMin}-{damageMax}/{hit}%)");
            TextManager.instance.UpdateConsole($"{attacker.unitName}の攻撃を{target.unitName}が回避した");

            DamageUI damageUICanvus = Instantiate(DamageUICanvas, target.transform.position, Quaternion.identity);
            damageUICanvus.ShowMiss();
            done = true;
        }
    }

    private GameObject fireBreathEffect;
    public virtual void ExecuteFireAttack(Battler attacker, Battler target)
    {
        StartCoroutine(ExecuteFireAttackCoroutine(attacker, target));
    }

    IEnumerator ExecuteFireAttackCoroutine(Battler attacker, Battler target)
    {
        float damageMin;
        float damageMax;
        if (attacker.atk > target.def)
        {
            damageMin = (attacker.atk - target.def) / 5 * (1 - target.resistanceFire / 100)-10;
            damageMax = (attacker.atk - target.def) / 5 * (1 - target.resistanceFire / 100);
            if (damageMax < 1) { damageMax = 1; }
        }
        else
        {
            damageMin = (attacker.atk - target.def) / 5;
            if (damageMin < -9) { damageMin = -9; }
            damageMax = (attacker.atk - target.def) / 5;
            if (damageMax < 1) { damageMax = 1; }
        }

        float damage = Random.Range(damageMin, damageMax);
        if (damage < 0) { damage = 0; };

        Vector3 effectPos = new Vector3(attacker.transform.position.x, attacker.transform.position.y, attacker.transform.position.z - 2);
        fireBreathEffect = Instantiate(fireEffect, effectPos, Quaternion.identity);
        Vector3 effectTargetPosition = (target.transform.position - new Vector3(0, 0, 1));
        fireBreathEffect.transform.DOMove(effectTargetPosition, 0.3f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.3f);

        target.Damage(damage, attacker, target);
        Debug.Log($"{attacker.unitName}の炎で{target.unitName}に{damage}のダメージ！({damageMin}-{damageMax})(残りHPは{target.hp})");
        TextManager.instance.UpdateConsole($"{attacker.unitName}の炎で{target.unitName}に{(int)damage}のダメージ");

        Destroy(fireBreathEffect.gameObject);
    }

    public virtual void ExecuteBowAttack(Battler attacker, Battler target)
    {
        float hit = 70 + attacker.dex - target.agi;
        if (flash)
        {
            hit /= 2;
        }
        if (hit < 5)
        {
            hit = 5;
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

        float damageMin = ((attacker.atk + powerCoefficient * attacker.str) * attacker.dex / 100 - target.def - protectCoefficient * target.vit) / 5;
        if (damageMin < -9) { damageMin = -9; }
        float damageMax = (attacker.atk + powerCoefficient * attacker.str - target.def - protectCoefficient * target.vit) / 5;
        if (damageMax < 1) { damageMax = 1; }

        float rundomNumber = Random.Range(0f, 100f);
        if (rundomNumber < hit)
        {
            int damage = (int)Random.Range(damageMin, damageMax);
            if (damage <= 0) { damage = 0; };
            target.Damage(damage,attacker,target);
            Debug.Log($"{attacker.unitName}の攻撃で{target.unitName}に{damage}のダメージ！({damageMin}-{damageMax}/{hit}%)(残りHPは{target.hp})");
            TextManager.instance.UpdateConsole($"{attacker.unitName}の矢で{target.unitName}に{(int)damage}のダメージ");
            Instantiate(attackEffect, target.transform.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log($"{attacker.unitName}の攻撃を{target.unitName}が回避した({damageMin}-{damageMax}/{hit}%)(残りHPは{target.hp})");
            TextManager.instance.UpdateConsole($"{attacker.unitName}の矢を{target.unitName}が回避した");

            DamageUI damageUICanvus = Instantiate(DamageUICanvas, target.transform.position, Quaternion.identity);
            damageUICanvus.ShowMiss();
            done = true;
        }

    }
}