using UnityEngine;
using System.Collections;
using SmallHedge.SoundManager;

public class Mug : MonoBehaviour
{
    IEnumerator Add()
    {
            SoundManager.PlaySound(SoundType.four);

        while(true)
        {

            CurrencyManager.Instance.AddMoney(100);
            yield return new WaitForSeconds(1);
        }
    }
    void Start()
    {
        StartCoroutine(Add());
    }

}