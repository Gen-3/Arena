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
            randomNumberMemo = Random.Range(1, 10);
            playerStatusSO.runtimeGold -= 100;
            playerStatusSO.runtimeExp -= 100;
            playerStatusSO.runtimeMagicLevel += randomNumberMemo;
            resultText.text = $"魔法レベルが{randomNumberMemo}上がった";
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
