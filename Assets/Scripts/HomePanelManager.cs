using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePanelManager : MonoBehaviour
{
    [SerializeField] GameObject changeEquipPanelAtHome = default;
//    [SerializeField] PlayerStatusSO playerStatusSO = default;
    [SerializeField] WeaponSO W00_None = default;
    [SerializeField] ShieldSO S00_None = default;
    [SerializeField] ArmorSO A00_None = default;
    [SerializeField] GameObject confirmThrowAwayPanel = default;
    [SerializeField] WeaponChangePanelAtHome weaponChangePanelAtHome = default;
    public int n = default;
    [SerializeField] PlayerStatusManager playerStatusManager = default;

    public void OpenPanel()
    {
        weaponChangePanelAtHome.DisplayPlayersWeapon();
        changeEquipPanelAtHome.SetActive(true);
    }

    public void ClosePanel()
    {
        changeEquipPanelAtHome.SetActive(false);
    }

    public void ThrowAwayWeapon()//各ボタンにアタッチ
    {
        n = 1;
        if (PlayerStatusSO.Entity.runtimeWeapon != W00_None)
        {
            confirmThrowAwayPanel.SetActive(true);
        }
    }
    public void ThrowAwaySubWeapon1()//各ボタンにアタッチ
    {
        n = 2;
        if (PlayerStatusSO.Entity.runtimeSubWeapon1 != W00_None)
        {
            confirmThrowAwayPanel.SetActive(true);
        }
    }
    public void ThrowAwaySubWeapon2()//各ボタンにアタッチ
    {
        n = 3;
        if (PlayerStatusSO.Entity.runtimeSubWeapon2 != W00_None)
        {
            confirmThrowAwayPanel.SetActive(true);
        }
    }
    public void ThrowAwayShield()//各ボタンにアタッチ
    {
        n = 4;
        if (PlayerStatusSO.Entity.runtimeShield != S00_None)
        {
            confirmThrowAwayPanel.SetActive(true);
        }
    }
    public void ThrowAwayArmor()//各ボタンにアタッチ
    {
        n = 5;
        if (PlayerStatusSO.Entity.runtimeArmor != A00_None)
        {
            confirmThrowAwayPanel.SetActive(true);
        }
    }
    public void ConfirmThrowAway()//確認パネルで「はい」を選択時
    {
        switch (n)
        {
            case 1:
                PlayerStatusSO.Entity.runtimeWeapon = W00_None;
                break;
            case 2:
                PlayerStatusSO.Entity.runtimeSubWeapon1 = W00_None;
                break;
            case 3:
                PlayerStatusSO.Entity.runtimeSubWeapon2 = W00_None;
                break;
            case 4:
                PlayerStatusSO.Entity.runtimeShield = S00_None;
                break;
            case 5:
                PlayerStatusSO.Entity.runtimeArmor = A00_None;
                break;
            default:
                break;
        }
        n = default;
        weaponChangePanelAtHome.DisplayPlayersWeapon();
        confirmThrowAwayPanel.SetActive(false);
        playerStatusManager.Start();

    }

    public void CancelThrowAway()
    {
        n = default;
        confirmThrowAwayPanel.SetActive(false);
    }
}