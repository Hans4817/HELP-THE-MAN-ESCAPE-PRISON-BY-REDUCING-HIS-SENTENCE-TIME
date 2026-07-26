using UnityEngine;
using System.Collections;

public class Clicker : MonoBehaviour
{
    IEnumerator Add()
    {
        while(true)
        {
            CurrencyManager.Instance.AddMoney(1);
            yield return new WaitForSeconds(1);
        }
    }
    void Start()
    {
        StartCoroutine(Add());
    }

}