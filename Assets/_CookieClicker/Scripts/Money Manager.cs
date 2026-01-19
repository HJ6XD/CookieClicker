using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int moneyCollected = 0;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        UpdateMoneyText();
    }

    public void UpdateMoney(int moneyGain)
    {
        moneyCollected += moneyGain;
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = moneyCollected.ToString();
    }
}
