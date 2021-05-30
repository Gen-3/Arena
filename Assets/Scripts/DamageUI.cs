using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField]Text text = default;

    private void Start()
    {
        //gameObject.SetActive(true);//あとでfalseにする
    }

    //// エフェクトの移動
    //private void Update()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, target.position, 0.1f);
    //    if (Vector3.Distance(transform.position, target.position) < float.Epsilon)
    //    {
    //        // 破壊
    //    }
    //}

    public void ShowDamage(float amount)
    {
        int amountInt = (int)amount;
        text.text = amountInt.ToString();
        StartCoroutine(MoveUpCor());
    }

    public void ShowMiss()
    {
        text.text = "Miss";
        StartCoroutine(MoveUpCor());
    }

    IEnumerator MoveUpCor()
    {
        for (int i = 0; i < 8; i++)
        {
            transform.position += new Vector3(0, 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 8; i++)
        {
            transform.position -= new Vector3(0, 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}