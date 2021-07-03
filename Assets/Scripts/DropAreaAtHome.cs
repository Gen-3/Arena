using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaAtHome : MonoBehaviour//, IDropHandler
{
    /*
    public void Setup()
    {
        DragObj child = GetComponentInChildren<DragObj>();

    }

    public void OnDrop(PointerEventData data)
    {
        DragObj dragObj = data.pointerDrag.GetComponent<DragObj>();

        if (dragObj != null)
        {
            Debug.Log("OnDrop()のdragObjがった場合を発動");
            DragObj child = GetComponentInChildren<DragObj>();
            // すでにオブジェクトを持っていれば,dragObjの情報を与える
            if (child)
            {
                Debug.Log("チャイルドありました");
                child.parentTransform = dragObj.parentTransform;//もともとのチャイルドを、ドラッグしてきたオブジェクトのピアレント（ドロップフィールド）のもとへ移動
                child.OnEndDrag(null);
            }
            else
            {
                Debug.Log("チャイルドありませんでした");
            }

            dragObj.parentTransform = this.transform;            // dragObjを自分の子にする
        }
        else
        {
            Debug.Log("dragObjがありません（何もドラッグしてへんで）");
        }
    }
    */
}