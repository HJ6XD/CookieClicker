using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using System.Collections;

[Serializable]
public class ShopItem
{
    public string id;
    public string iconPath;
    public string type;
    public string Descripcion;
    public string name;
    public string price;
}

public class UIShopManager : MonoBehaviour
{
    public Transform container;
    public GameObject itemPrefab;

    [Header("Refs")]
    public GameManager gameManager; // <-- NUEVO: asigna en Inspector

    string path = "store/items";

    DatabaseReference databaseReference;
    bool firebaseIsReady = false;

    private async void Start()
    {
        var dependency = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependency == DependencyStatus.Available)
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            firebaseIsReady = true;
        }
        else Debug.LogError("Firebase is unavailable");
    }

    public void LoadStore()
    {
        if (!firebaseIsReady || databaseReference == null)
        {
            Debug.LogWarning("Firebase no esta listos");
            StartCoroutine(WaitAndLoadStore());
            return;
        }
        StartCoroutine(FillStore());
    }

    IEnumerator WaitAndLoadStore()
    {
        yield return new WaitUntil(() => firebaseIsReady && databaseReference != null);
        StartCoroutine(FillStore());
    }

    IEnumerator FillStore()
    {
        var task = databaseReference.Child(path)
            .OrderByChild("active").EqualTo(true).GetValueAsync();

        while (!task.IsCompleted) yield return null;

        if (task.IsFaulted)
        {
            Debug.LogError("Error loading store: " + task.Exception);
            yield break;
        }

        var snap = task.Result;
        if (!snap.Exists) yield break;

        foreach (var child in snap.Children)
        {
            var item = JsonUtility.FromJson<ShopItem>(child.GetRawJsonValue());
            item.id = child.Key;

            var itemInstance = Instantiate(itemPrefab, container);
            UIStoreItem storeItem = itemInstance.GetComponent<UIStoreItem>();

            // NUEVO: inyección de GameManager al prefab instanciado
            if (storeItem != null)
                storeItem.Init(gameManager);

            var icon = LoadLocalSprite(item.iconPath);

            storeItem.Bind(item, icon);
        }
    }

    Sprite LoadLocalSprite(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath)) return null;
        return Resources.Load<Sprite>(iconPath);
    }
}
