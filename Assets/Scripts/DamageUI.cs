using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField]Text text = default;

    private void Start()
    {
        gameObject.SetActive(true);//あとでfalseにする
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

    public void ShowDamage(float amount,Battler attacker,Battler target)
    {
        int amountInt = (int)amount;
        text.text = amountInt.ToString();
        transform.position = target.transform.position;
        Debug.Log(text.transform.position);
        gameObject.SetActive(true);
    }

/*    IEnumerator MoveUpCor()
    {
        gameObject.SetActive(true);
        transform.position = initPosition;
        for (int i = 0; i < 20; i++)
        {
            transform.position += new Vector3(0, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
*/
}