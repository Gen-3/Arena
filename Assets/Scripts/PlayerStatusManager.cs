using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] Text subWeaponText = default;
    [SerializeField] Text shieldText = default;
    [SerializeField] Text armorText = default;
    [SerializeField] Text goldText = default;
    [SerializeField] Text expText = default;
    [SerializeField] Text fameText = default;
    [SerializeField] Text maxFameText = default;

    public void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "Home_FirstTime")
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
            weaponText.text = playerStatusSO.runtimeWeapon.equipName;
            subWeaponText.text = playerStatusSO.runtimeSubWeapon.equipName;
            shieldText.text = playerStatusSO.runtimeShield.equipName;
            armorText.text = playerStatusSO.runtimeArmor.equipName;
            goldText.text = playerStatusSO.runtimeGold.ToString();
            expText.text = playerStatusSO.runtimeExp.ToString();
            fameText.text = playerStatusSO.runtimeFame.ToString();
            maxFameText.text = playerStatusSO.runtimeMaxFame.ToString();
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
        usablePoint -= 10;
        usablePointText.text = $"残りポイント:{usablePoint}";
        playerStatusSO.SetStatus(type, 10);
    }
    public void DownStatus(PlayerStatusSO.Status type)
    {
        // ひく
        int runtimeStatus = GetStatus(type);
        if (usablePoint >= 200 || runtimeStatus <= 0)
        {
            return;
        }
        usablePoint += 10 ;
        usablePointText.text = $"残りポイント:{usablePoint}";
        playerStatusSO.SetStatus(type, -10);
    }

    public int GetStatus(PlayerStatusSO.Status type)
    {
        return playerStatusSO.GetStatus(type);
    }


}