using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaAtHome : MonoBehaviour, IDropHandler
{
/*
 * public void Setup()
    {
        DragObjAtHome child = GetComponentInChildren<DragObjAtHome>();

    }
*/
    public void OnDrop(PointerEventData data)
    {
        DragObjAtHome dragObjAtHome = data.pointerDrag.GetComponent<DragObjAtHome>();

        if (dragObjAtHome != null)
        {
            Debug.Log("OnDrop()のdragObjがあった場合を発動");
            DragObjAtHome child = GetComponentInChildren<DragObjAtHome>();
            // すでにオブジェクトを持っていれば,dragObjの情報を与える
            if (child)
            {
                Debug.Log("チャイルドありました");
                child.parentTransform = dragObjAtHome.parentTransform;//もともとのチャイルドを、ドラッグしてきたオブジェクトのピアレント（ドロップフィールド）のもとへ移動
                child.OnEndDrag(null);
            }
            else
            {
                Debug.Log("チャイルドありませんでした");
            }

            dragObjAtHome.parentTransform = this.transform;            // dragObjを自分の子にする
        }
        else
        {
            Debug.Log("dragObjがありません（何もドラッグしてへんで）");
        }
    }
}