using UnityEngine;
public enum GameState
{
    Loading,
    Store,
    Auth
}
public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    public DataSaver dataSaver;

    void Start()
    {
        AppEventHub.OnFirebaseInitialized.AddListener(InitApp);
    }

    void InitApp()
    {
        AppEventHub.OnGameStateChange.Invoke(GameState.Auth);
    }

    private void OnDisable()
    {
        AppEventHub.OnFirebaseInitialized.RemoveAllListeners();
    }

    public bool TryBuy(ShopItem item)
    {
        if (dataSaver == null || dataSaver.dts == null)
        {
            Debug.LogError("[GameManager] Falta DataSaver o dts.");
            return false;
        }

        if (item == null)
        {
            Debug.LogError("[GameManager] Item null.");
            return false;
        }

        if (!int.TryParse(item.price, out int price))
        {
            Debug.LogError("[GameManager] Precio inválido: " + item.price);
            return false;
        }

        if (MoneyManager.instance.moneyCollected < price)
        {
            Debug.Log("No alcanza monedas.");
            return false;
        }

        // 1) Cobrar
        MoneyManager.instance.UpdateMoney( -price);

        // 2) Registrar compra en inventario
        dataSaver.RegisterPurchase(item.id, 1);
        MoneyManager.instance.BuyAnUpgrade(item.id);

        // 3) Guardar en Firebase
        dataSaver.SaveDataFn();

        Debug.Log($"Compra OK: {item.id} (price={price}). Coins restantes={dataSaver.dts.totalCoins}");
        return true;
    }
        
}
