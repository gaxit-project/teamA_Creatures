using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{   private void OnTriggerEnter(Collider other)
    {
        // 他のオブジェクトと接触した際にログを表示
        Debug.Log(other.gameObject.name + "はパンチまたはキックを当てた");
    }
}
