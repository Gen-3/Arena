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
    [SerializeField] WeaponSO subWeapon = default;
    [SerializeField] ShieldSO shield = default;
    [SerializeField] ArmorSO armor = default;
    [SerializeField] int gold = default;
    [SerializeField] int exp = default;
    [SerializeField] int fame = default;
    [SerializeField] int maxFame = default;

    /*publicの分はruntimeHogehogeでしか取得しないから、元の変数はpublicじゃなくてserializeFieldでいいっぽい？？
    public WeaponSO weapon = default;
    public WeaponSO subweapon = default;
    public ShieldSO shield = default;
    public ArmorSO armor = default;
    */

    public string runtimePlayerName;
    public int runtimeStr;
    public int runtimeDex;
    public int runtimeAgi;
    public int runtimeVit;
    public int runtimeMen;
    public int runtimeHp;
    public int runtimeMagicLevel;
    public WeaponSO runtimeWeapon;
    public WeaponSO runtimeSubWeapon;
    public ShieldSO runtimeShield;
    public ArmorSO runtimeArmor;
    public int runtimeGold;
    public int runtimeExp;
    public int runtimeFame;
    public int runtimeMaxFame;


    public enum Status
    {
        str,
        dex,
        agi,
        vit,
        men,
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
        runtimeSubWeapon = subWeapon;
        runtimeShield = shield;
        runtimeArmor = armor;
        runtimeGold = gold;
        runtimeExp = exp;
        runtimeFame = fame;
        runtimeMaxFame = maxFame;
    }

    public void OnBeforeSerialize()
    {
    }

}



