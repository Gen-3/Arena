using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEditButton : MonoBehaviour
{
    [SerializeField] PlayerStatusSO.Status type = default;
    [SerializeField] Text statusText = default;
    PlayerStatusManager playerStatus = default;

//    [SerializeField] PlayerStatusSO playerStatusSO=default;
    [SerializeField] InputField inputField=default;

    private void Start()
    {
        playerStatus = GameObject.Find("PlayerStatusManager").GetComponent<PlayerStatusManager>();
//        inputField = inputField.GetComponent<InputField>();
    }

    public void OnUp()
    {
        playerStatus.UpStatus(type);
        statusText.text = playerStatus.GetStatus(type).ToString();
    }

    public void OnDown()
    {
        playerStatus.DownStatus(type);
        statusText.text = playerStatus.GetStatus(type).ToString();
    }

    public void InputName()
    {
        PlayerStatusSO.Entity.runtimePlayerName = inputField.text;
    }
}