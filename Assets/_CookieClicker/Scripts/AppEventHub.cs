using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Loading,
    Store,
    Auth
}

public static class AppEventHub
{
    public static UnityEvent OnFirebaseInitialized = new UnityEvent();

    public static UnityEvent<GameState> OnGameStateChange = new();
}
