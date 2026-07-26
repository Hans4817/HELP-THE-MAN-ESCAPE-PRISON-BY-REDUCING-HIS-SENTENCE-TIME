using System.Collections;
using SmallHedge.SoundManager;
using UnityEngine;

public class Iphone : MonoBehaviour
{
    IEnumerator Add()
    {
        SoundManager.PlaySound(SoundType.one);

        while(true)
        {
            CurrencyManager.Instance.AddMoney(100000000);
            yield return new WaitForSeconds(1);
        }
    }
    void Start()
    {
        StartCoroutine(Add());
    }

}