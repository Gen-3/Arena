using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public abstract class MagicBaseSO : ScriptableObject
{
    //abstractとvertualの違い。
    //abstractは呼ばれない抽象的な関数。virtualは継承先でoverrideされた関数がなかったら継承元のvertualを使う、defaultみたいな感じ。 
    public virtual void Execute(Battler user, Battler target)
    {

    }
}
