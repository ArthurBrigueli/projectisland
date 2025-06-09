using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenShop : MonoBehaviour
{
    public GameObject Shop;

    public GameObject ItemShop;

    public static bool isShopOpen = false;

    public GameObject money;

    private TextMeshProUGUI moneyText;

    public void Open()
    {
        bool nextState = !Shop.activeSelf;
        Shop.SetActive(nextState);
        isShopOpen = nextState;
    }

    public void Start()
    {
        moneyText = money.GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {

        if (moneyText != null)
        {
            if (int.TryParse(moneyText.text, out int valorDinheiro))
            {

                if (valorDinheiro < 10)
                {
                    Image image = ItemShop.GetComponent<Image>();
                    Color color = image.color;
                    color.a = 0.5f;
                    image.color = color;
                }
                else
                {
                    Image image = ItemShop.GetComponent<Image>();
                    Color color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
            }
        }

    }
}
