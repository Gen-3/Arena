using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleManager : MonoBehaviour
{
//    [SerializeField] PlayerStatusSO playerStatusSO;

    [SerializeField] Text strText;
    [SerializeField] Text dexText;
    [SerializeField] Text agiText;
    [SerializeField] Text vitText;
    [SerializeField] Text menText;

    /*
    [SerializeField] Slider strSlider;
    [SerializeField] Slider dexSlider;
    [SerializeField] Slider agiSlider;
    [SerializeField] Slider vitSlider;
    [SerializeField] Slider menSlider;
    */
    private Slider strSlider;
    private Slider dexSlider;
    private Slider agiSlider;
    private Slider vitSlider;
    private Slider menSlider;

    private int randomNumberMemo;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    private void Start()
    {
        strText.text = $"筋力：{ PlayerStatusSO.Entity.runtimeStr.ToString()}";
        dexText.text = $"器用さ：{ PlayerStatusSO.Entity.runtimeDex.ToString()}";
        agiText.text = $"敏捷性：{ PlayerStatusSO.Entity.runtimeAgi.ToString()}";
        vitText.text = $"耐久力：{ PlayerStatusSO.Entity.runtimeVit.ToString()}";
        menText.text = $"精神力：{ PlayerStatusSO.Entity.runtimeMen.ToString()}";
        strSlider = GameObject.Find("strSlider").GetComponent<Slider>();
        dexSlider = GameObject.Find("dexSlider").GetComponent<Slider>(); ;
        agiSlider = GameObject.Find("agiSlider").GetComponent<Slider>(); ;
        vitSlider = GameObject.Find("vitSlider").GetComponent<Slider>(); ;
        menSlider = GameObject.Find("menSlider").GetComponent<Slider>(); ;
        strSlider.value = (float)PlayerStatusSO.Entity.runtimeStr / 100;
        dexSlider.value = (float)PlayerStatusSO.Entity.runtimeDex / 100;
        agiSlider.value = (float)PlayerStatusSO.Entity.runtimeAgi / 100;
        vitSlider.value = (float)PlayerStatusSO.Entity.runtimeVit / 100;
        menSlider.value = (float)PlayerStatusSO.Entity.runtimeMen / 100;
    }

    public void OnClickButton(string type)
    {
        StartCoroutine(OnClickButtonCoroutine(type));
    }

    public IEnumerator OnClickButtonCoroutine(string type)
    {
        CloseResultPanel();
        yield return new WaitForSeconds(0.1f);
        if (PlayerStatusSO.Entity.runtimeGold < 100 && PlayerStatusSO.Entity.runtimeExp < 100)
        {
            resultPanel.SetActive(true);
            resultText.text = $"経験点と金貨が足りません";
        }
        else if (PlayerStatusSO.Entity.runtimeGold < 100)
        {
            resultPanel.SetActive(true);
            resultText.text = $"金貨が足りません";
        }
        else if (PlayerStatusSO.Entity.runtimeExp < 100)
        {
            resultPanel.SetActive(true);
            resultText.text = $"経験点が足りません";
        }
        else
        {
            switch (type)
            {
                case "str":
                    if (PlayerStatusSO.Entity.runtimeStr >= 100)
                    {
                        resultText.text = $"筋力はこれ以上上がらない";
                        break;
                    }
                    if (PlayerStatusSO.Entity.runtimeStr < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (PlayerStatusSO.Entity.runtimeStr < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    PlayerStatusSO.Entity.runtimeGold -= 100;
                    PlayerStatusSO.Entity.runtimeExp -= 100;
                    PlayerStatusSO.Entity.runtimeStr += randomNumberMemo;
                    resultText.text = $"筋力が{randomNumberMemo}上がった";
                    break;

                case "dex":
                    if (PlayerStatusSO.Entity.runtimeDex >= 100)
                    {
                        resultText.text = $"器用さはこれ以上上がらない";
                        break;
                    }
                    if (PlayerStatusSO.Entity.runtimeDex < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (PlayerStatusSO.Entity.runtimeDex < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    PlayerStatusSO.Entity.runtimeGold -= 100;
                    PlayerStatusSO.Entity.runtimeExp -= 100;
                    PlayerStatusSO.Entity.runtimeDex += randomNumberMemo;
                    resultText.text = $"器用さが{randomNumberMemo}上がった";
                    break;

                case "agi":
                    if (PlayerStatusSO.Entity.runtimeAgi >= 100)
                    {
                        resultText.text = $"敏捷性はこれ以上上がらない";
                        break;
                    }
                    if (PlayerStatusSO.Entity.runtimeAgi < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (PlayerStatusSO.Entity.runtimeAgi < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    PlayerStatusSO.Entity.runtimeGold -= 100;
                    PlayerStatusSO.Entity.runtimeExp -= 100;
                    PlayerStatusSO.Entity.runtimeAgi += randomNumberMemo;
                    resultText.text = $"敏捷性が{randomNumberMemo}上がった";
                    break;

                case "vit":
                    if (PlayerStatusSO.Entity.runtimeVit >= 100)
                    {
                        resultText.text = $"耐久力はこれ以上上がらない";
                        break;
                    }
                    if (PlayerStatusSO.Entity.runtimeVit < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (PlayerStatusSO.Entity.runtimeVit < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    PlayerStatusSO.Entity.runtimeGold -= 100;
                    PlayerStatusSO.Entity.runtimeExp -= 100;
                    PlayerStatusSO.Entity.runtimeVit += randomNumberMemo;
                    resultText.text = $"耐久力が{randomNumberMemo}上がった";
                    break;

                case "men":
                    if (PlayerStatusSO.Entity.runtimeMen >= 100)
                    {
                        resultText.text = $"精神力はこれ以上上がらない";
                        break;
                    }
                    if (PlayerStatusSO.Entity.runtimeMen < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (PlayerStatusSO.Entity.runtimeMen < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    PlayerStatusSO.Entity.runtimeGold -= 100;
                    PlayerStatusSO.Entity.runtimeExp -= 100;
                    PlayerStatusSO.Entity.runtimeMen += randomNumberMemo;
                    resultText.text = $"精神力が{randomNumberMemo}上がった";
                    break;
            }            
        }
        Reload();
        resultPanel.SetActive(true);
    }

    private void Reload()
    {
        strText.text = $"筋力：{ PlayerStatusSO.Entity.runtimeStr.ToString()}";
        dexText.text = $"器用さ：{ PlayerStatusSO.Entity.runtimeDex.ToString()}";
        agiText.text = $"敏捷性：{ PlayerStatusSO.Entity.runtimeAgi.ToString()}";
        vitText.text = $"耐久力：{ PlayerStatusSO.Entity.runtimeVit.ToString()}";
        menText.text = $"精神力：{ PlayerStatusSO.Entity.runtimeMen.ToString()}";
        strSlider.value = (float)PlayerStatusSO.Entity.runtimeStr / 100;
        dexSlider.value = (float)PlayerStatusSO.Entity.runtimeDex / 100;
        agiSlider.value = (float)PlayerStatusSO.Entity.runtimeAgi / 100;
        vitSlider.value = (float)PlayerStatusSO.Entity.runtimeVit / 100;
        menSlider.value = (float)PlayerStatusSO.Entity.runtimeMen / 100;
    }

    public void CloseResultPanel()
    {
        resultPanel.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))//SetStatus:デバッグでいきなりArenaシーンを呼び出したときに能力値をセットするためのもの
        {
            PlayerStatusSO.Entity.runtimePlayerName = "テストプレイなう";
            PlayerStatusSO.Entity.runtimeStr = Random.Range(0,101);
            PlayerStatusSO.Entity.runtimeDex = Random.Range(0, 101);
            PlayerStatusSO.Entity.runtimeAgi = Random.Range(0, 101);
            PlayerStatusSO.Entity.runtimeVit = Random.Range(0, 101);
            PlayerStatusSO.Entity.runtimeMen = Random.Range(0, 101);
            Debug.Log("デバッグ用ステータスをセットしました");
            Reload();
        }
    }
}
