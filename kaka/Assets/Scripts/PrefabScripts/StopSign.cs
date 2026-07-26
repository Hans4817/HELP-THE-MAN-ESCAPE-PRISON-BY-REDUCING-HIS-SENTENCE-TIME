using UnityEngine;
using System.Collections;
using SmallHedge.SoundManager;

public class StopSign : MonoBehaviour
{
    IEnumerator Add()
    {
            SoundManager.PlaySound(SoundType.five);

        while(true)
        {

            CurrencyManager.Instance.AddMoney(1000);
            yield return new WaitForSeconds(1);
        }
    }
    void Start()
    {
        StartCoroutine(Add());
    }

}