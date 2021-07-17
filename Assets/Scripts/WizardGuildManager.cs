using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardGuildManager : MonoBehaviour
{
//    [SerializeField] PlayerStatusSO playerStatusSO;

    [SerializeField] Text magicLevelText;

    private Slider Slider;

    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    private void Start()
    {
        magicLevelText.text = $"魔法レベル：{ PlayerStatusSO.Entity.runtimeMagicLevel.ToString()}";
        Slider = GameObject.Find("Slider").GetComponent<Slider>();
        Slider.value = (float)PlayerStatusSO.Entity.runtimeMagicLevel / 100;
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
            int beforeMagiclevel = PlayerStatusSO.Entity.runtimeMagicLevel;
            if (PlayerStatusSO.Entity.runtimeMagicLevel < 90)
            {
                randomNumberMemo = Random.Range(1, 10);
                PlayerStatusSO.Entity.runtimeGold -= 100;
                PlayerStatusSO.Entity.runtimeExp -= 100;
                PlayerStatusSO.Entity.runtimeMagicLevel += randomNumberMemo;
                resultText.text = $"魔法レベルが{randomNumberMemo}上がった";

                if (beforeMagiclevel < 1 && PlayerStatusSO.Entity.runtimeMagicLevel >= 1)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「エナジーボルト」をおぼえた";
                }
                if (beforeMagiclevel < 5 && PlayerStatusSO.Entity.runtimeMagicLevel >= 5)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「フラッシュ」をおぼえた";
                }
                if (beforeMagiclevel < 10 && PlayerStatusSO.Entity.runtimeMagicLevel >= 10)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「プロテクト」をおぼえた";
                }
                if (beforeMagiclevel < 15 && PlayerStatusSO.Entity.runtimeMagicLevel >= 15)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「スロウ」をおぼえた";
                }
                if (beforeMagiclevel < 20 && PlayerStatusSO.Entity.runtimeMagicLevel >= 20)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「スリープ」をおぼえた";
                }
                if (beforeMagiclevel < 25 && PlayerStatusSO.Entity.runtimeMagicLevel >= 25)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「パワー」をおぼえた";
                }
                if (beforeMagiclevel < 30 && PlayerStatusSO.Entity.runtimeMagicLevel >= 30)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「ファイアストーム」をおぼえた";
                }
                if (beforeMagiclevel < 40 && PlayerStatusSO.Entity.runtimeMagicLevel >= 40)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「クイック」をおぼえた";
                }
                if (beforeMagiclevel < 50 && PlayerStatusSO.Entity.runtimeMagicLevel >= 50)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「サイレンス」をおぼえた";
                }
                if (beforeMagiclevel < 60 && PlayerStatusSO.Entity.runtimeMagicLevel >= 60)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「ディスペル」をおぼえた";
                }
                if (beforeMagiclevel < 80 && PlayerStatusSO.Entity.runtimeMagicLevel >= 80)
                {
                    resultText.text = $"魔法レベルが{randomNumberMemo}上がり　「ライトニング」をおぼえた";
                }
            }
            else if(PlayerStatusSO.Entity.runtimeMagicLevel < 99)
            {
                int max = 100 - PlayerStatusSO.Entity.runtimeMagicLevel;
                randomNumberMemo = Random.Range(1, max);
                PlayerStatusSO.Entity.runtimeGold -= 100;
                PlayerStatusSO.Entity.runtimeExp -= 100;
                PlayerStatusSO.Entity.runtimeMagicLevel += randomNumberMemo;
                resultText.text = $"魔法レベルが{randomNumberMemo}上がった";
                if (PlayerStatusSO.Entity.runtimeMagicLevel >= 99)
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
        magicLevelText.text = $"魔法レベル：{ PlayerStatusSO.Entity.runtimeMagicLevel.ToString()}";
        Slider.value = (float)PlayerStatusSO.Entity.runtimeMagicLevel / 100;
    }

    public void CloseResultPanel()
    {
        resultPanel.SetActive(false);

    }
}
