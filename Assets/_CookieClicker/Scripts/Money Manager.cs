using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    private int moneyCollected = 0;
    [SerializeField] private TextMeshProUGUI moneyText;

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
