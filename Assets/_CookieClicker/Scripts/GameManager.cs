using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AppEventHub.OnFirebaseInitialized.AddListener(InitApp);  
    }

    private void InitApp()
    {
        AppEventHub.OnGameStateChange.Invoke(GameState.Auth);
    }

    private void OnDisable()
    {
        AppEventHub.OnFirebaseInitialized.RemoveListener(InitApp);
    }

    public bool TryBuy(string itemID)
    {
        return false;
    }
}
