using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour,IPointerClickHandler
{
    public static TilemapManager instance;//変数名はなんでもOK：どこからでもアクセルできる
    private void Awake()
    {
        instance = this;//どこからでもアクセスできるものに自分を入れる
    }

    public BattleManager battleManager = default;//消す？
    public PlayerManager playerManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        //battleManager.SetPlayerPosition(eventData.pointerPressRaycast.worldPosition);
        playerManager.ClickOnTile(eventData.pointerPressRaycast.worldPosition);
    }

}
