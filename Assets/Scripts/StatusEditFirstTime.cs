using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEditFirstTime : MonoBehaviour
{
    int usablePoint = 200;
    [SerializeField] Text usablePointText = default;

    [SerializeField] PlayerStatusSO playerStatusSO = default;
    [SerializeField] SceneTransitionManager sceneTransitionManager=default;

    public void Start()
    {
    }

    public void DicideEdit(string sceneName)
    {
        if (usablePoint == 0)
        {
            sceneTransitionManager.LoadTo("TownScene");
        }
    }

    public void UpStatus(PlayerStatusSO.Status type)
    {
        // たす
        int runtimeStatus = GetStatus(type);
        if (usablePoint <= 0 || runtimeStatus >= 100)
        {
            return;
        }
        usablePoint -= 10;
        usablePointText.text = $"残りポイント:{usablePoint}";
        playerStatusSO.SetStatus(type, 10);
    }
    public void DownStatus(PlayerStatusSO.Status type)
    {
        // ひく
        int runtimeStatus = GetStatus(type);
        if (usablePoint >= 200 || runtimeStatus <= 0)
        {
            return;
        }
        usablePoint += 10;
        usablePointText.text = $"残りポイント:{usablePoint}";
        playerStatusSO.SetStatus(type, -10);
    }

    public int GetStatus(PlayerStatusSO.Status type)
    {
        return playerStatusSO.GetStatus(type);
    }

}