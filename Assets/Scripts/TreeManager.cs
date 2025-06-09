using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public int totalCoins = 0;

    private List<Tree> trees = new List<Tree>();

    public AudioClip audioClip;



    // Registra uma árvore para controle e escuta o evento de moedas geradas
    public void RegisterTree(GameObject treeGO)
    {
        Tree tree = treeGO.GetComponent<Tree>();
        if (tree != null && !trees.Contains(tree))
        {
            trees.Add(tree);
            tree.EarnedCoins += AddCoins;
        }
    }

    private void AddCoins(int amount)
    {
        totalCoins += amount;

        SoundManager.instance.PlaySound(audioClip);
        if (moneyText != null)
            moneyText.text = totalCoins.ToString();

    }
}
