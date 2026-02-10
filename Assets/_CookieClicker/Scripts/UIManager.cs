using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UILoadScreen;
    public GameObject UIAuthScreen;
    public GameObject UIStoreScreen;
    public GameObject UIGameplayScreen;
    
    public GameObject currentScreen;
    public UIShopManager shop;
    public DataSaver dataSaver;
    private void Start()
    {
        AppEventHub.OnGameStateChange.AddListener(OnGameStateChanged);
    }

    private void OnDisable()
    {
        AppEventHub.OnGameStateChange.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        currentScreen = newGameState switch
        {
            GameState.Loading => UILoadScreen,
            GameState.Auth => UIAuthScreen,
            GameState.Store => UIStoreScreen,
            GameState.Gameplay => UIGameplayScreen,
            _ => null
        };

        foreach(GameObject screen in new GameObject[] { UILoadScreen, UIAuthScreen, UIStoreScreen, UIGameplayScreen})
            screen.SetActive(screen == currentScreen);

    }
    public void LoadGameplayScreen()
    {
        AppEventHub.OnGameStateChange.Invoke(GameState.Gameplay);
    }
    public void LoadStoreScreen()
    {
        AppEventHub.OnGameStateChange.Invoke(GameState.Store);
        shop.LoadStore();
    }

    public void OnSignIn()
    {
        AuthManager.Instance.RegisterUser();
        dataSaver.SaveDataFn();
        LoadGameplayScreen();        
    }
    public void OnLogIn()
    {
        AuthManager.Instance.LoginUser();        
        dataSaver.LoadDataFn();
        LoadGameplayScreen();
    }
}
