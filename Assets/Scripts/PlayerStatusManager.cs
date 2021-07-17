using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Linq;

public class PlayerStatusManager : MonoBehaviour
{

    //未使用？    [SerializeField] GameObject Home = default;
    //未使用？    [SerializeField] GameObject StatusEditUI = default;

    /*
    [SerializeField] WeaponSO weapon;
    [SerializeField] WeaponSO subWeapon;
    [SerializeField] ShieldSO shield;
    [SerializeField] ArmorSO armor;
    */
    [SerializeField] SceneTransitionManager sceneTransitionManager = default;
    int usablePoint = 200;
    [SerializeField] Text usablePointText = default;
    [SerializeField] Text magicLevelTextFirstTime = default;
    //[SerializeField] PlayerStatusSO playerStatusSO = default;

    //UI関連
    [SerializeField] Text playerNameText = default;
    [SerializeField] Text strText = default;
    [SerializeField] Text dexText = default;
    [SerializeField] Text agiText = default;
    [SerializeField] Text vitText = default;
    [SerializeField] Text menText = default;
    [SerializeField] Text hpText = default;
    [SerializeField] Text MagicLevelText = default;
    [SerializeField] Text weaponText = default;
    [SerializeField] Text subWeapon1Text = default;
    [SerializeField] Text subWeapon2Text = default;
    [SerializeField] Text shieldText = default;
    [SerializeField] Text armorText = default;
    [SerializeField] Text goldText = default;
    [SerializeField] Text expText = default;
    [SerializeField] Text fameText = default;
    [SerializeField] Text maxFameText = default;

    //武器nullバグ対策
    [SerializeField] ShopItemDatabaseSO weaponShopItemDatabaseSO = default;
    [SerializeField] ShopItemDatabaseSO shieldShopItemDatabaseSO = default;
    [SerializeField] ShopItemDatabaseSO armorShopItemDatabaseSO = default;

    public void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Home")
        {
            PlayerStatusSO.Entity.runtimeHp = PlayerStatusSO.Entity.runtimeVit * 33 / 40 + PlayerStatusSO.Entity.runtimeMen * 7 / 40;

            if(PlayerStatusSO.Entity.runtimePlayerName == "")//名前入力できないUnityRoom用の設定
            {
                PlayerStatusSO.Entity.runtimePlayerName = "あなた";
            }

            playerNameText.text = PlayerStatusSO.Entity.runtimePlayerName;
            strText.text = PlayerStatusSO.Entity.runtimeStr.ToString();
            dexText.text = PlayerStatusSO.Entity.runtimeDex.ToString();
            agiText.text = PlayerStatusSO.Entity.runtimeAgi.ToString();
            vitText.text = PlayerStatusSO.Entity.runtimeVit.ToString();
            menText.text = PlayerStatusSO.Entity.runtimeMen.ToString();
            hpText.text = PlayerStatusSO.Entity.runtimeHp.ToString();
            MagicLevelText.text = PlayerStatusSO.Entity.runtimeMagicLevel.ToString();
            
            if (PlayerStatusSO.Entity.runtimeWeapon == null)
            {
                Debug.Log($"PlayerStatusSO.Entity.runtimeWeaponがnullだったので0番をセットしました");
                PlayerStatusSO.Entity.runtimeWeapon = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            }
            if (PlayerStatusSO.Entity.runtimeSubWeapon1 == null)
            {
                Debug.Log($"PlayerStatusSO.Entity.runtimeSubWeapon1がnullだったので0番をセットしました");
                PlayerStatusSO.Entity.runtimeSubWeapon1 = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            }
            if (PlayerStatusSO.Entity.runtimeSubWeapon2 == null)
            {
                Debug.Log($"PlayerStatusSO.Entity.runtimeSubWeapon2がnullだったので0番をセットしました");
                PlayerStatusSO.Entity.runtimeSubWeapon2 = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            }
            if (PlayerStatusSO.Entity.runtimeShield == null)
            {
                Debug.Log($"PlayerStatusSO.Entity.runtimeShieldがnullだったので0番をセットしました");
                PlayerStatusSO.Entity.runtimeShield = shieldShopItemDatabaseSO.EquipList[0] as ShieldSO;
            }
            if (PlayerStatusSO.Entity.runtimeArmor == null)
            {
                Debug.Log($"PlayerStatusSO.Entity.runtimeArmorがnullだったので0番をセットしました");
                PlayerStatusSO.Entity.runtimeArmor = armorShopItemDatabaseSO.EquipList[0] as ArmorSO;
            }

            weaponText.text = PlayerStatusSO.Entity.runtimeWeapon.equipName;
            subWeapon1Text.text = PlayerStatusSO.Entity.runtimeSubWeapon1.equipName;
            subWeapon2Text.text = PlayerStatusSO.Entity.runtimeSubWeapon2.equipName;
            shieldText.text = PlayerStatusSO.Entity.runtimeShield.equipName;
            armorText.text = PlayerStatusSO.Entity.runtimeArmor.equipName;

            goldText.text = PlayerStatusSO.Entity.runtimeGold.ToString();
            expText.text = PlayerStatusSO.Entity.runtimeExp.ToString();
            fameText.text = PlayerStatusSO.Entity.runtimeFame.ToString();
            maxFameText.text = "("+PlayerStatusSO.Entity.runtimeMaxFame.ToString()+")";
        }
        else
        {
            PlayerStatusSO.Entity.Initialization();
        }
    }

    private void Update()//押下したキーボードの名前を調べるためのコード、開発用。
    {

        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftBracket))
        {
            PlayerStatusSO.Entity.runtimeStr += 5;
            PlayerStatusSO.Entity.runtimeDex += 5;
            PlayerStatusSO.Entity.runtimeAgi += 5;
            PlayerStatusSO.Entity.runtimeVit += 5;
            PlayerStatusSO.Entity.runtimeMen += 5;
            Debug.Log("ステータス＋５");
            Start();
            return;
        }
        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.RightBracket))
        {
            PlayerStatusSO.Entity.runtimeStr -= 5;
            PlayerStatusSO.Entity.runtimeDex -= 5;
            PlayerStatusSO.Entity.runtimeAgi -= 5;
            PlayerStatusSO.Entity.runtimeVit -= 5;
            PlayerStatusSO.Entity.runtimeMen -= 5;
            Debug.Log("ステータス−５");
            Start();
            return;
        }

        /*
        KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();
        if (Input.anyKeyDown) //※KeyDown のみ
        {
            foreach (var key in keyCodes)
            {
                if (Input.GetKeyDown(key))
                {
                    Debug.Log(key);
                    break;
                }
            }
        }
        */
    }

    public void DicideEdit(string sceneName)
    {
        if (usablePoint == 0)
        {
            sceneTransitionManager.LoadTo("Home");
        }
    }

    public void UpStatus(PlayerStatusSO.Status type)
    {
        // たす
        int runtimeStatus = GetStatus(type);
        if (usablePoint <= 0 || runtimeStatus >= 80) 
        {
            return;
        }
        usablePoint -= 5;
        usablePointText.text = $"残りポイント:{usablePoint}";
        PlayerStatusSO.Entity.SetStatus(type, 5);
        SetMagicLevel();
    }
    public void DownStatus(PlayerStatusSO.Status type)
    {
        // ひく
        int runtimeStatus = GetStatus(type);
        if (usablePoint >= 200 || runtimeStatus <= 0)
        {
            return;
        }
        usablePoint += 5 ;
        usablePointText.text = $"残りポイント:{usablePoint}";
        PlayerStatusSO.Entity.SetStatus(type, -5);
        SetMagicLevel();
    }

    public int GetStatus(PlayerStatusSO.Status type)
    {
        return PlayerStatusSO.Entity.GetStatus(type);
    }

    public void SetMagicLevel()
    {
        if (PlayerStatusSO.Entity.runtimeMen < 20)
        {
            PlayerStatusSO.Entity.runtimeMagicLevel = 0;
        }
        else if (PlayerStatusSO.Entity.runtimeMen < 30)
            PlayerStatusSO.Entity.runtimeMagicLevel = 1;
        else if (PlayerStatusSO.Entity.runtimeMen < 40)
            PlayerStatusSO.Entity.runtimeMagicLevel = 5;
        else if (PlayerStatusSO.Entity.runtimeMen < 50)
            PlayerStatusSO.Entity.runtimeMagicLevel = 10;
        else if (PlayerStatusSO.Entity.runtimeMen < 65)
            PlayerStatusSO.Entity.runtimeMagicLevel = 15;
        else if (PlayerStatusSO.Entity.runtimeMen < 80)
            PlayerStatusSO.Entity.runtimeMagicLevel = 20;
        else if (PlayerStatusSO.Entity.runtimeMen < 100)
            PlayerStatusSO.Entity.runtimeMagicLevel = 25;
        magicLevelTextFirstTime.text = PlayerStatusSO.Entity.runtimeMagicLevel.ToString();
    }

}