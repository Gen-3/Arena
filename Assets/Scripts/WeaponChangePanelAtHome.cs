﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangePanelAtHome : MonoBehaviour
{
    
    [SerializeField] PlayerStatusSO playerStatusSO = default;
    public DropArea[] dropAreas = new DropArea[5];


    public void SetPlayerEquip()
    {
        for (int i = 0; i < dropAreas.Length; i++)//①PlayerStatusSOの装備をドラッグオブジェクトに応じて変更する
        {
            DragObjAtHome dragObjAtHome = dropAreas[i].GetComponentInChildren<DragObjAtHome>();
            if (dragObjAtHome != null)
            {
                if (dragObjAtHome.equipDataSO == null)
                {
                    continue;
                }
                if (i == 0)
                {
                    playerManager.weapon = dragObjAtHome.equipDataSO;
                }
                if (i == 1)
                {
                    playerManager.subWeapon1 = dragObjAtHome.equipDataSO;
                }
                if (i == 2)
                {
                    playerManager.subWeapon2 = dragObjAtHome.equipDataSO;
                }
            }
        }

    }

    public void DisplayPlayersWeapon()
    {
        for (int i = 0; i < dropAreas.Length; i++)//②PlayerManagerに反映された引数WeaponSOをドラッグオブジェクトのGameObjectの中身としてセットする。textの表記も変える。
        {
            DragObj dragObj = dropAreas[i].GetComponentInChildren<DragObj>();

            if (dragObj != null)
            {

                if (i == 0)
                {
                    dragObj.Set(playerManager.weapon);
                }
                if (i == 1)
                {
                    dragObj.Set(playerManager.subWeapon1);
                }
                if (i == 2)
                {
                    dragObj.Set(playerManager.subWeapon2);
                }
            }
            else
            {
                Debug.Log(i + "dragObjがsnullらしい");
            }
        }

    }
}
