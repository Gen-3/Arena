using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO = default;
    [SerializeField] PlayerStatusManager playerStatusManager = default;
    [SerializeField] GameObject confirmSavePanel;
    [SerializeField] GameObject confirmLoadPanel;

    // Unityの標準的なセーブ機能：playerPrefsというのがある
    // これはstring, int, floatをセーブ or ロードできる
    //PlayerPrefs.SetString("aaaKey", "セーブしたい文字列");

    //// 取得
    //string savedata = PlayerPrefs.GetString("aaaKey");

    //PlayerPrefs.SetInt("aaaIntKey", 10);

    //// 取得
    //int savedataInt = PlayerPrefs.GetInt("aaaIntKey");

    // Json化：オブジェクト(クラス?)を文字列に変換すること


    // PlayerStatusSOをセーブしたい
    public void Save()
    {
        string json = JsonUtility.ToJson(playerStatusSO, true);
        PlayerPrefs.SetString(SaveKey, json);
        confirmSavePanel.SetActive(false);
        Debug.Log("Save()");
        Debug.Log(json);
    }

    const string SaveKey = "SaveKey";

    public void Load()
    {
        playerStatusSO.Initialization();

        string json = PlayerPrefs.GetString(SaveKey);
        JsonUtility.FromJsonOverwrite(json, playerStatusSO);
        confirmLoadPanel.SetActive(false);
        playerStatusManager.Start();

        Debug.Log("Load()");
        Debug.Log($"jsonの中身を表示：{json}");
    }

    public void ShowSavepanel()
    {
        confirmSavePanel.SetActive(true);
    }

    public void ShowLoadpanel()
    {
        confirmLoadPanel.SetActive(true);
    }

    public void Canselconfirm()
    {
        confirmSavePanel.SetActive(false);
        confirmLoadPanel.SetActive(false);
    }

}