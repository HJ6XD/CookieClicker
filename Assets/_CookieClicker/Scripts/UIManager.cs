using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UILoadScreen;
    public GameObject UIAuthScreen;
    public GameObject UIStoreScreen;
    
    public GameObject currentScreen;

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
            _ => null
        };

        foreach(GameObject screen in new GameObject[] { UILoadScreen, UIAuthScreen, UIStoreScreen})
            screen.SetActive(screen == currentScreen);

    }
    public void LoadGameplayScreen()
    {
        AppEventHub.OnGameStateChange.Invoke(GameState.Store);
    }
}
