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
    [SerializeField] PlayerStatusSO playerStatusSO = default;

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
            playerStatusSO.runtimeHp = playerStatusSO.runtimeVit * 33 / 40 + playerStatusSO.runtimeMen * 7 / 40;
            playerNameText.text = playerStatusSO.runtimePlayerName;
            strText.text = playerStatusSO.runtimeStr.ToString();
            dexText.text = playerStatusSO.runtimeDex.ToString();
            agiText.text = playerStatusSO.runtimeAgi.ToString();
            vitText.text = playerStatusSO.runtimeVit.ToString();
            menText.text = playerStatusSO.runtimeMen.ToString();
            hpText.text = playerStatusSO.runtimeHp.ToString();
            MagicLevelText.text = playerStatusSO.runtimeMagicLevel.ToString();
            
            if (playerStatusSO.runtimeWeapon == null)
            {
                Debug.Log($"playerStatusSO.runtimeWeaponがnullだったので0番をセットしました");
                playerStatusSO.runtimeWeapon = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            }
            if (playerStatusSO.runtimeSubWeapon1 == null)
            {
                Debug.Log($"playerStatusSO.runtimeSubWeapon1がnullだったので0番をセットしました");
                playerStatusSO.runtimeSubWeapon1 = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            }
            if (playerStatusSO.runtimeSubWeapon2 == null)
            {
                Debug.Log($"playerStatusSO.runtimeSubWeapon2がnullだったので0番をセットしました");
                playerStatusSO.runtimeSubWeapon2 = weaponShopItemDatabaseSO.EquipList[0] as WeaponSO;
            }
            if (playerStatusSO.runtimeShield == null)
            {
                Debug.Log($"playerStatusSO.runtimeShieldがnullだったので0番をセットしました");
                playerStatusSO.runtimeShield = shieldShopItemDatabaseSO.EquipList[0] as ShieldSO;
            }
            if (playerStatusSO.runtimeArmor == null)
            {
                Debug.Log($"playerStatusSO.runtimeArmorがnullだったので0番をセットしました");
                playerStatusSO.runtimeArmor = armorShopItemDatabaseSO.EquipList[0] as ArmorSO;
            }

            weaponText.text = playerStatusSO.runtimeWeapon.equipName;
            subWeapon1Text.text = playerStatusSO.runtimeSubWeapon1.equipName;
            subWeapon2Text.text = playerStatusSO.runtimeSubWeapon2.equipName;
            shieldText.text = playerStatusSO.runtimeShield.equipName;
            armorText.text = playerStatusSO.runtimeArmor.equipName;

            goldText.text = playerStatusSO.runtimeGold.ToString();
            expText.text = playerStatusSO.runtimeExp.ToString();
            fameText.text = playerStatusSO.runtimeFame.ToString();
            maxFameText.text = playerStatusSO.runtimeMaxFame.ToString();
        }
        else
        {
            playerStatusSO.Initialization();
        }
    }

    private void Update()//押下したキーボードの名前を調べるためのコード、開発用。
    {

        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftBracket))
        {
            playerStatusSO.runtimeStr += 5;
            playerStatusSO.runtimeDex += 5;
            playerStatusSO.runtimeAgi += 5;
            playerStatusSO.runtimeVit += 5;
            playerStatusSO.runtimeMen += 5;
            Debug.Log("ステータス＋５");
            Start();
            return;
        }
        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.RightBracket))
        {
            playerStatusSO.runtimeStr -= 5;
            playerStatusSO.runtimeDex -= 5;
            playerStatusSO.runtimeAgi -= 5;
            playerStatusSO.runtimeVit -= 5;
            playerStatusSO.runtimeMen -= 5;
            Debug.Log("ステータス−５");
            Start();
            return;
        }

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
        playerStatusSO.SetStatus(type, 5);
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
        playerStatusSO.SetStatus(type, -5);
        SetMagicLevel();
    }

    public int GetStatus(PlayerStatusSO.Status type)
    {
        return playerStatusSO.GetStatus(type);
    }

    public void SetMagicLevel()
    {
        if (playerStatusSO.runtimeMen < 20)
        {
            playerStatusSO.runtimeMagicLevel = 0;
        }
        else if (playerStatusSO.runtimeMen < 30)
            playerStatusSO.runtimeMagicLevel = 1;
        else if (playerStatusSO.runtimeMen < 40)
            playerStatusSO.runtimeMagicLevel = 5;
        else if (playerStatusSO.runtimeMen < 50)
            playerStatusSO.runtimeMagicLevel = 10;
        else if (playerStatusSO.runtimeMen < 65)
            playerStatusSO.runtimeMagicLevel = 15;
        else if (playerStatusSO.runtimeMen < 80)
            playerStatusSO.runtimeMagicLevel = 20;
        else if (playerStatusSO.runtimeMen < 100)
            playerStatusSO.runtimeMagicLevel = 25;
        magicLevelTextFirstTime.text = playerStatusSO.runtimeMagicLevel.ToString();
    }

}