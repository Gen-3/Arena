using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public WeaponSO weaponSO = default;
    [SerializeField] Text text = default;
    // 親要素?
    public Transform parentTransform;

    [SerializeField] WeaponChangePanel weaponChangePanel = default;

    public void Set(WeaponSO weaponSO)//引数WeaponSOをドラッグオブジェクトのGameObjectの中身としてセットする。textの表記も変える。
    {
        this.weaponSO = weaponSO;
        if (weaponSO == null)
        {
            this.text.text = "";
        }
        else
        {
            this.text.text = weaponSO.equipName;
        }
    }

    // ドラッグ開始時に実行される
    public void OnBeginDrag(PointerEventData data)
    {
        parentTransform = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(transform.parent.parent);
        //transform.SetParent(transform.parent);
    }

    // ドラッグ中に実行される
    public void OnDrag(PointerEventData data)
    {
        transform.position = data.position;
    }

    // ドラッグ終了時に実行される
    public void OnEndDrag(PointerEventData data)
    {
        transform.SetParent(parentTransform);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        weaponChangePanel.SetPlayerWeapon();
    }
}
// OnBeginDrag:親を変更
// OnDrag:位置の変更
// OnDrop:親を内部的に変更
// OnEndDrag:親を変更