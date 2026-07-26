using UnityEngine;
using System.Collections;
using SmallHedge.SoundManager;

public class AlarmClock : MonoBehaviour
{
    IEnumerator Add()
    {
            SoundManager.PlaySound(SoundType.two);

        while(true)
        {
            CurrencyManager.Instance.AddMoney(5);
            yield return new WaitForSeconds(1);
        }
    }
    void Start()
    {
        StartCoroutine(Add());
    }

}