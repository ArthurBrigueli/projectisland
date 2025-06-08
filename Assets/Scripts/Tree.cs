using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public int coinsPerSecond = 1;

    public delegate void OnEarnCoins(int amount);
    public event OnEarnCoins EarnedCoins;

    private void Start()
    {
        StartCoroutine(GenerateCoins());
    }

    private IEnumerator GenerateCoins()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            EarnedCoins?.Invoke(coinsPerSecond);
        }
    }
}
