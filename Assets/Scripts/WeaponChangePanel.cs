using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangePanel : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager = default;
    // [SerializeField] Text[] weaponList = new Text[3];
    public DropArea[] dropAreas = new DropArea[3];

    // PlayerManagerの武器を表示する

    //public static WeaponChangePanel instance;
    //private void Awake()
    //{
    //    instance = this;
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ShowWeaponList();
        }
    }


    public void ShowWeaponList()
    {
        Debug.Log("ShowWeaponLis発動");
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

        for (int i = 0; i < dropAreas.Length; i++)//②PlayerManagerに反映された引数WeaponSOをドラッグオブジェクトのGameObjectの中身としてセットする。textの表記も変える。
        {
            DragObj dragObj = dropAreas[i].GetComponentInChildren<DragObj>();

            if (dragObj != null)
            {

                if (i == 0)
                {
                    dragObj.Set(playerManager.weapon);
                    //                    if (playerManager.weapon == null)
                    //                  {
                    //                    dropAreas[i].GetComponentInChildren<DragObj>() = null;
                    //              }
                }
                if (i == 1)
                {
                    //                    if (playerManager.subWeapon1 == null)
                    //                  {
                    //                    dropAreas[i].gameObject.SetActive(false);
                    //              }
                    dragObj.Set(playerManager.subWeapon1);
                }
                if (i == 2)
                {
                    //                    if (playerManager.subWeapon2 == null)
                    //                  {
                    //                    dropAreas[i].gameObject.SetActive(false);
                    //              }
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
