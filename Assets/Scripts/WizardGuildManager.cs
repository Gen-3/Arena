using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardGuildManager : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO;

    [SerializeField] Text magicLevelText;

    private Slider Slider;

    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    private void Start()
    {
        magicLevelText.text = $"魔法レベル：{ playerStatusSO.runtimeMagicLevel.ToString()}";
        Slider = GameObject.Find("Slider").GetComponent<Slider>();
        Slider.value = (float)playerStatusSO.runtimeMagicLevel / 100;
    }

    public void OnClickButton()
    {
        StartCoroutine(OnClickButtonCoroutine());
    }

    public IEnumerator OnClickButtonCoroutine()
    {
        int randomNumberMemo = default;

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
            int beforeMagiclevel = playerStatusSO.runtimeMagicLevel;
            if (playerStatusSO.runtimeMagicLevel < 90)
            {
                randomNumberMemo = Random.Range(1, 10);
                playerStatusSO.runtimeGold -= 100;
                playerStatusSO.runtimeExp -= 100;
                playerStatusSO.runtimeMagicLevel += randomNumberMemo;
                resultText.text = $"魔法レベルが{randomNumberMemo}上がった";

                if (beforeMagiclevel < 1 && playerStatusSO.runtimeMagicLevel >= 1)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「エナジーボルト」をおぼえた";
                }
                if (beforeMagiclevel < 5 && playerStatusSO.runtimeMagicLevel >= 5)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「フラッシュ」をおぼえた";
                }
                if (beforeMagiclevel < 10 && playerStatusSO.runtimeMagicLevel >= 10)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「プロテクト」をおぼえた";
                }
                if (beforeMagiclevel < 15 && playerStatusSO.runtimeMagicLevel >= 15)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「スロウ」をおぼえた";
                }
                if (beforeMagiclevel < 20 && playerStatusSO.runtimeMagicLevel >= 20)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「スリープ」をおぼえた";
                }
                if (beforeMagiclevel < 25 && playerStatusSO.runtimeMagicLevel >= 25)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「パワー」をおぼえた";
                }
                if (beforeMagiclevel < 30 && playerStatusSO.runtimeMagicLevel >= 30)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「ファイアストーム」をおぼえた";
                }
                if (beforeMagiclevel < 40 && playerStatusSO.runtimeMagicLevel >= 40)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「クイック」をおぼえた";
                }
                if (beforeMagiclevel < 50 && playerStatusSO.runtimeMagicLevel >= 50)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「サイレンス」をおぼえた";
                }
                if (beforeMagiclevel < 60 && playerStatusSO.runtimeMagicLevel >= 60)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「ディスペル」をおぼえた";
                }
                if (beforeMagiclevel < 80 && playerStatusSO.runtimeMagicLevel >= 80)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「ライトニング」をおぼえた";
                }
            }
            else if(playerStatusSO.runtimeMagicLevel < 99)
            {
                int max = 100 - playerStatusSO.runtimeMagicLevel;
                randomNumberMemo = Random.Range(1, max);
                playerStatusSO.runtimeGold -= 100;
                playerStatusSO.runtimeExp -= 100;
                playerStatusSO.runtimeMagicLevel += randomNumberMemo;
                resultText.text = $"魔法レベルが{randomNumberMemo}上がった";
                if (playerStatusSO.runtimeMagicLevel >= 99)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「デス」をおぼえた";
                }
            }
            else
            {
                resultText.text = $"これ以上は上がらない";
            }
        }
        Reload();
        resultPanel.SetActive(true);
    }

    private void Reload()
    {
        magicLevelText.text = $"魔法レベル：{ playerStatusSO.runtimeMagicLevel.ToString()}";
        Slider.value = (float)playerStatusSO.runtimeMagicLevel / 100;
    }

    public void CloseResultPanel()
    {
        resultPanel.SetActive(false);

    }
}
