using UnityEngine;
using UnityEngine.Events;



public static class AppEventHub
{
    public static UnityEvent OnFirebaseInitialized = new UnityEvent();

    public static UnityEvent<GameState> OnGameStateChange = new();
}
