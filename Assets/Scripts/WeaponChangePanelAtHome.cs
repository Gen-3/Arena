using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangePanelAtHome : MonoBehaviour
{
    
    [SerializeField] PlayerStatusSO playerStatusSO = default;
    public DropAreaAtHome[] dropAreasAtHome = new DropAreaAtHome[3];

    private void Start()
    {
        DisplayPlayersWeapon();
    }

    public void SetPlayerWeapon()
    {
        for (int i = 0; i < dropAreasAtHome.Length; i++)//①playerStatusSOの装備をドラッグオブジェクトに応じて変更する
        {
            DragObjAtHome dragObjAtHome = dropAreasAtHome[i].GetComponentInChildren<DragObjAtHome>();
            if (dragObjAtHome != null)
            {
                if (dragObjAtHome.weaponSO == null)
                {
                    continue;
                }
                if (i == 0)
                {
                    playerStatusSO.runtimeWeapon = dragObjAtHome.weaponSO;
                }
                if (i == 1)
                {
                    playerStatusSO.runtimeSubWeapon1 = dragObjAtHome.weaponSO;
                }
                if (i == 2)
                {
                    playerStatusSO.runtimeSubWeapon2 = dragObjAtHome.weaponSO;
                }
            }
        }

    }


    [SerializeField] Text shieldText = default;
    [SerializeField] Text armorText = default;
    public void DisplayPlayersWeapon()
    {
        for (int i = 0; i < dropAreasAtHome.Length; i++)//②playerStatusSOに反映された引数WeaponSOをドラッグオブジェクトのGameObjectの中身としてセットする。textの表記も変える。
        {
            DragObjAtHome dragObjAtHome = dropAreasAtHome[i].GetComponentInChildren<DragObjAtHome>();

            if (dragObjAtHome != null)
            {

                if (i == 0)
                {
                    dragObjAtHome.Set(playerStatusSO.runtimeWeapon);
                }
                if (i == 1)
                {
                    dragObjAtHome.Set(playerStatusSO.runtimeSubWeapon1);
                }
                if (i == 2)
                {
                    dragObjAtHome.Set(playerStatusSO.runtimeSubWeapon2);
                }
            }
            else
            {
                Debug.Log(i + "dragObjがsnullらしい");
            }

            if (playerStatusSO.runtimeShield != null)
            {
                shieldText.text = playerStatusSO.runtimeShield.equipName;
            }
            else
            {
                shieldText.text = "";
            }

            if (playerStatusSO.runtimeArmor != null)
            {
                armorText.text = playerStatusSO.runtimeArmor.equipName;
            }
            else
            {
                armorText.text = "";
            }
        }
    }
}
