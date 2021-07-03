using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangePanel : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager = default;
    public DropArea[] dropAreas = new DropArea[3];


    public void SetPlayerWeapon()
    {
        for (int i = 0; i < dropAreas.Length; i++)//①PlayerManagerの装備をドラッグオブジェクトに応じて変更する
        {
            DragObj dragObj = dropAreas[i].GetComponentInChildren<DragObj>();
            if (dragObj != null)
            {
                if (dragObj.weaponSO == null)
                {
                    continue;
                }
                if (i == 0)
                {
                    playerManager.weapon = dragObj.weaponSO;
                }
                if (i == 1)
                {
                    playerManager.subWeapon1 = dragObj.weaponSO;
                }
                if (i == 2)
                {
                    playerManager.subWeapon2 = dragObj.weaponSO;
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
