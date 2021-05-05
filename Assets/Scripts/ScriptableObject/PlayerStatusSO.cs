using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatusSO : ScriptableObject,ISerializationCallbackReceiver
{
    [SerializeField] string playerName = default;
    [SerializeField] int str = default;
    [SerializeField] int dex = default;
    [SerializeField] int agi = default;
    [SerializeField] int vit = default;
    [SerializeField] int men = default;
    [SerializeField] int hp = default;
    [SerializeField] int magicLevel = default;
    [SerializeField] WeaponSO weapon = default;
    [SerializeField] WeaponSO subWeapon1 = default;
    [SerializeField] WeaponSO subWeapon2 = default;
    [SerializeField] ShieldSO shield = default;
    [SerializeField] ArmorSO armor = default;
    [SerializeField] int gold = default;
    [SerializeField] int exp = default;
    [SerializeField] int fame = default;
    [SerializeField] int maxFame = default;
    [SerializeField] int matchAmount = default;
    [SerializeField] int winAmount = default;

    // 覚えている魔法:魔法レベルに応じて覚えている魔法が変わる？
    public List<MagicBaseSO> magicList = new List<MagicBaseSO>();


    public string runtimePlayerName;
    public int runtimeStr;
    public int runtimeDex;
    public int runtimeAgi;
    public int runtimeVit;
    public int runtimeMen;
    public int runtimeHp;
    public int runtimeMagicLevel;
    public WeaponSO runtimeWeapon;
    public WeaponSO runtimeSubWeapon1;
    public WeaponSO runtimeSubWeapon2;
    public ShieldSO runtimeShield;
    public ArmorSO runtimeArmor;
    public int runtimeGold = default;
    public int runtimeExp = default;
    public int runtimeFame = default;
    public int runtimeMaxFame = default;
    public int runtimeMatchAmount = default;
    public int runtimeWinAmount = default;

    public enum Status
    {
        str,
        dex,
        agi,
        vit,
        men,
    }

    public bool TrySetWeapon(WeaponSO weaponSO)
    {
        if (runtimeWeapon == null)
        {
            runtimeWeapon = weaponSO;
            return true;
        }
        if (runtimeSubWeapon1 == null)
        {
            runtimeSubWeapon1 = weaponSO;
            return true;
        }
        if (runtimeSubWeapon2 == null)
        {
            runtimeSubWeapon2 = weaponSO;
            return true;
        }

        return false;
    }
    public bool TrySetShield(ShieldSO shieldSO)
    {
        if (runtimeShield == null)
        {
            runtimeShield = shieldSO;
            return true;
        }
        return false;
    }
    public bool TrySetArmor(ArmorSO armorSO)
    {
        if (runtimeArmor == null)
        {
            runtimeArmor = armorSO;
            return true;
        }
        return false;
    }

    public int GetStatus(Status type)
    {
        switch (type)
        {
            default:
            case Status.str:
                return runtimeStr;
            case Status.dex:
                return runtimeDex;
            case Status.agi:
                return runtimeAgi;
            case Status.vit:
                return runtimeVit;
            case Status.men:
                return runtimeMen;
        }
    }

    public void SetStatus(Status type,int add)
    {
        switch (type)
        {
            default:
            case Status.str:
                runtimeStr += add;
                break;
            case Status.dex:
                runtimeDex += add;
                break;
            case Status.agi:
                runtimeAgi += add;
                break;
            case Status.vit:
                runtimeVit += add;
                break;
            case Status.men:
                runtimeMen += add;
                break;
        }
    }

    public void OnAfterDeserialize()
    {
        runtimePlayerName = playerName;
        runtimeStr = str;
        runtimeDex = dex;
        runtimeAgi = agi;
        runtimeVit = vit;
        runtimeMen = men;
        runtimeHp = hp;
        runtimeMagicLevel = magicLevel;
        runtimeWeapon = weapon;
        runtimeSubWeapon1 = subWeapon1;
        runtimeSubWeapon2 = subWeapon2;
        runtimeShield = shield;
        runtimeArmor = armor;
        runtimeGold = gold;
        runtimeExp = exp;
        runtimeFame = fame;
        runtimeMaxFame = maxFame;
        runtimeMatchAmount = matchAmount;
        runtimeWinAmount = winAmount;
    }

    public void OnBeforeSerialize()
    {
    }

}



