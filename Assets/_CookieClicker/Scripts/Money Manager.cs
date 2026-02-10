using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance {  get; private set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public int moneyCollected = 0;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI storeMoneyText;
    public int money_second = 0;
    public TextMeshProUGUI MoneyPerSecondText;

    //Cantidad de mejoras pasivas
    private Dictionary<string, int> passiveDictionary;
    //Cantidad de mejoiras activas
    private Dictionary<string, bool> activeDictionary;

    public Dictionary<string, string> IDToNameTranslator = new Dictionary<string, string>() {
        ["item_01"] = "obrera", 
        ["item_02"] = "tanque", 
        ["item_03"] = "ladrona", 
        ["item_04"] = "falsificadora", 
        ["item_05"] = "stiky", 
        ["item_06"] = "gimnasio", 
        ["item_07"] = "botas", 
        ["item_08"] = "bolsa" 
    };
    public Dictionary<string, string> NameToIDTranslator = new Dictionary<string, string>() {
        ["obrera"] = "item_01",
        ["tanque"] = "item_02",
        ["ladrona"] = "item_03",
        ["falsificadora"] = "item_04", 
        ["stiky"] = "item_05",
        ["gimnasio"] = "item_06",
        ["botas"] = "item_07",
        ["bolsa"] = "item_08"
    };


    //Timer
    float timeSinceLast = 0;

    private void Start()
    {
        UpdateMoneyText();
        SetDictionarys();
    }

    private void Update()
    {
        if(timeSinceLast - 5 > 0) {
            timeSinceLast -= 5;
            UpdateMoney(GainPassiveMoney());
        }
        else timeSinceLast += Time.deltaTime;
    }

    private void SetDictionarys()
    {
        passiveDictionary = new Dictionary<string, int>();
        passiveDictionary["obrera"] = 0;
        passiveDictionary["tanque"] = 0;
        passiveDictionary["ladrona"] = 0;
        passiveDictionary["falsificadora"] = 0;

        activeDictionary = new Dictionary<string, bool>();
        activeDictionary["stiky"] = false;
        activeDictionary["gimnasio"] = false;
        activeDictionary["botas"] = false;
        activeDictionary["bolsa"] = false;
    }

    public void BuyAnUpgrade(string id)
    {
        string name = IDToNameTranslator[id];
        if (passiveDictionary.ContainsKey(name)) 
            passiveDictionary[name] += 1;
        else if (activeDictionary.ContainsKey(name))
            activeDictionary[name] = true;
        else
            Debug.LogError("no hay una mejora con el nombre: " + name);        
    }

    public void SetUpgradeNumber(string id, int quant)
    {
        string name = IDToNameTranslator[id];
        if (passiveDictionary.ContainsKey(name))
            passiveDictionary[name] = quant;
        else if (activeDictionary.ContainsKey(name))
            activeDictionary[name] = true;
        else
            Debug.LogError("no hay una mejora con el nombre: " + name);
    }

    private int GainPassiveMoney()
    {
        //obreras
        int obrera = passiveDictionary["obrera"];
        if (activeDictionary["gimnasio"]) obrera *= 2;
        //Tanques
        int tanque = passiveDictionary["tanque"] * 10;
        if (activeDictionary["botas"]) tanque *= 2;
        //Ladronas
        int ladrona = passiveDictionary["ladrona"] * 100;
        if (activeDictionary["bolsa"]) ladrona *= 2;
        //Falsificadora
        int falsificadora = passiveDictionary["falsificadora"] * 1000;

        money_second = obrera + tanque + ladrona + falsificadora;
        MoneyPerSecondText.text = "$" + money_second.ToString();

        return money_second;        
    }

    public void OnCickMoneyButton()
    {
        int mg = activeDictionary["stiky"] ? 2 : 1;
        UpdateMoney(mg);
    }

    public void UpdateMoney(int moneyGain)
    {
        moneyCollected += moneyGain;
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "$" + moneyCollected.ToString();
        storeMoneyText.text = "$" + moneyCollected.ToString();
    }
}
