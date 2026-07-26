using UnityEngine;
using System.Collections;
using SmallHedge.SoundManager;

public class LeftOverPizza : MonoBehaviour
{
    IEnumerator Add()
    {
            SoundManager.PlaySound(SoundType.three);

        while(true)
        {
            CurrencyManager.Instance.AddMoney(10);
            yield return new WaitForSeconds(1);
        }
    }
    void Start()
    {
        StartCoroutine(Add());
    }

}