using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleManager : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO;

    [SerializeField] Text strText;
    [SerializeField] Text dexText;
    [SerializeField] Text agiText;
    [SerializeField] Text vitText;
    [SerializeField] Text menText;

    [SerializeField] Slider strSlider;
    [SerializeField] Slider dexSlider;
    [SerializeField] Slider agiSlider;
    [SerializeField] Slider vitSlider;
    [SerializeField] Slider menSlider;

    private int randomNumberMemo;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    private void Start()
    {
        strText.text = $"筋力：{ playerStatusSO.runtimeStr.ToString()}";
        dexText.text = $"器用さ：{ playerStatusSO.runtimeDex.ToString()}";
        agiText.text = $"敏捷性：{ playerStatusSO.runtimeAgi.ToString()}";
        vitText.text = $"耐久力：{ playerStatusSO.runtimeVit.ToString()}";
        menText.text = $"精神力：{ playerStatusSO.runtimeMen.ToString()}";
    }

    public void OnClickButton(string type)
    {
        StartCoroutine(OnClickButtonCoroutine(type));
    }

    public IEnumerator OnClickButtonCoroutine(string type)
    {
        CloseResultPanel();
        yield return new WaitForSeconds(0.1f);
        if (playerStatusSO.runtimeGold < 100 && playerStatusSO.runtimeExp < 100)
        {
            resultPanel.SetActive(true);
            resultText.text = $"経験点と金貨が足りません";
        }
        else if (playerStatusSO.runtimeGold < 100)
        {
            resultPanel.SetActive(true);
            resultText.text = $"金貨が足りません";
        }
        else if (playerStatusSO.runtimeExp < 100)
        {
            resultPanel.SetActive(true);
            resultText.text = $"経験点が足りません";
        }
        else
        {
            switch (type)
            {
                case "str":
                    if (playerStatusSO.runtimeStr >= 100)
                    {
                        resultText.text = $"筋力はこれ以上上がらない";
                        break;
                    }
                    if (playerStatusSO.runtimeStr < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (playerStatusSO.runtimeStr < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    playerStatusSO.runtimeGold -= 100;
                    playerStatusSO.runtimeExp -= 100;
                    playerStatusSO.runtimeStr += randomNumberMemo;
                    resultText.text = $"筋力が{randomNumberMemo}上がった";
                    break;

                case "dex":
                    if (playerStatusSO.runtimeDex >= 100)
                    {
                        resultText.text = $"器用さはこれ以上上がらない";
                        break;
                    }
                    if (playerStatusSO.runtimeDex < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (playerStatusSO.runtimeDex < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    playerStatusSO.runtimeGold -= 100;
                    playerStatusSO.runtimeExp -= 100;
                    playerStatusSO.runtimeDex += randomNumberMemo;
                    resultText.text = $"器用さが{randomNumberMemo}上がった";
                    break;

                case "agi":
                    if (playerStatusSO.runtimeAgi >= 100)
                    {
                        resultText.text = $"敏捷性はこれ以上上がらない";
                        break;
                    }
                    if (playerStatusSO.runtimeAgi < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (playerStatusSO.runtimeAgi < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    playerStatusSO.runtimeGold -= 100;
                    playerStatusSO.runtimeExp -= 100;
                    playerStatusSO.runtimeAgi += randomNumberMemo;
                    resultText.text = $"敏捷性が{randomNumberMemo}上がった";
                    break;

                case "vit":
                    if (playerStatusSO.runtimeVit >= 100)
                    {
                        resultText.text = $"耐久力はこれ以上上がらない";
                        break;
                    }
                    if (playerStatusSO.runtimeVit < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (playerStatusSO.runtimeVit < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    playerStatusSO.runtimeGold -= 100;
                    playerStatusSO.runtimeExp -= 100;
                    playerStatusSO.runtimeVit += randomNumberMemo;
                    resultText.text = $"耐久力が{randomNumberMemo}上がった";
                    break;

                case "men":
                    if (playerStatusSO.runtimeMen >= 100)
                    {
                        resultText.text = $"精神力はこれ以上上がらない";
                        break;
                    }
                    if (playerStatusSO.runtimeMen < 80)
                    {
                        randomNumberMemo = Random.Range(1, 10);
                    }
                    else if (playerStatusSO.runtimeMen < 96)
                    {
                        randomNumberMemo = Random.Range(1, 6);
                    }
                    else
                    {
                        randomNumberMemo = 1;
                    }
                    playerStatusSO.runtimeGold -= 100;
                    playerStatusSO.runtimeExp -= 100;
                    playerStatusSO.runtimeMen += randomNumberMemo;
                    resultText.text = $"精神力が{randomNumberMemo}上がった";
                    break;
            }
            Start();
            resultPanel.SetActive(true);
        }
    }

    public void CloseResultPanel()
    {
        resultPanel.SetActive(false);

    }
}
