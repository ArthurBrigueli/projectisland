using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public int coinsPerSecond = 1;
    public delegate void OnEarnCoins(int amount);
    public event OnEarnCoins EarnedCoins;

    public GameObject coinPopupPrefab; // Referência para o prefab da moeda
    public Transform coinSpawnPoint;   // Onde a moeda vai aparecer (ex: topo da árvore)

    private void Start()
    {
        StartCoroutine(GenerateCoins());
    }

    private IEnumerator GenerateCoins()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            ShowCoinPopup(); // Mostra a moeda
            EarnedCoins?.Invoke(coinsPerSecond);
        }
    }

    private void ShowCoinPopup()
    {
        if (coinPopupPrefab != null && coinSpawnPoint != null)
        {
            GameObject coin = Instantiate(coinPopupPrefab, coinSpawnPoint.position, Quaternion.identity);
            StartCoroutine(AnimateAndDestroy(coin));
        }
    }

    private IEnumerator AnimateAndDestroy(GameObject coin)
    {
        float duration = .5f;
        Vector3 startPos = coin.transform.position;
        Vector3 endPos = startPos + Vector3.up * 2f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            coin.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(coin);
    }
}
