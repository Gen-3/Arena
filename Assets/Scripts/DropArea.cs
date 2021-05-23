using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    public void Setup()
    {
        DragObj child = GetComponentInChildren<DragObj>();

    }

    public void OnDrop(PointerEventData data)
    {
        Debug.Log("OnDrop()発動");

        DragObj dragObj = data.pointerDrag.GetComponent<DragObj>();

        if (dragObj != null)
        {
            Debug.Log("OnDrop()のdragObjがった場合を発動");
            DragObj child = GetComponentInChildren<DragObj>();
            // すでにオブジェクトを持っていれば,dragObjの情報を与える
            if (child)
            {
                Debug.Log("チャイルドありました");
                child.parentTransform = dragObj.parentTransform;
                child.OnEndDrag(null);
            }
            else
            {
                Debug.Log("チャイルドありませんでした");
            }

            // dragObjを自分の子にする
            dragObj.parentTransform = this.transform;
        }
        else
        {
            Debug.Log("dragObjがありません（何もドラッグしてへんで）");
        }
    }

}