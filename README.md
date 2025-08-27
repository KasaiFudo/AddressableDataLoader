# AddressableDataLoader

A lightweight utility class for Unity that simplifies working with **Addressables** using [`Cysharp/UniTask`](https://github.com/Cysharp/UniTask).  
It provides a clean API for loading and releasing assets asynchronously, while automatically managing memory through `IDisposable`.

## ✨ Features

- 🚀 **Async/await-style API** with UniTask  
- 📦 Load a single asset by address  
- 📂 Load multiple assets in parallel  
- 🧹 Automatic handle tracking and cleanup  
- ⚠️ Built-in safety checks with warnings on failed loads  
- ♻️ Implements `IDisposable` for simple lifecycle management  
- 👐 Flexible lifecycle control: use `using` for auto-cleanup **or** manage disposal manually when you need it  

## 📖 Usage

### 1. Create an instance

You can use a `using` block for automatic cleanup:
```csharp
using (var loader = new AddressableDataLoader())
{
    // Load your assets here
}
```

Or, you can manually control the lifecycle:
```csharp
var loader = new AddressableDataLoader();

// Load assets
var prefab = await loader.LoadAsync<GameObject>("MyPrefabAddress");

// ... later when no longer needed
loader.Dispose();
```

### 2. Load a single asset
```csharp
var prefab = await loader.LoadAsync<GameObject>("MyPrefabAddress");
if (prefab != null)
{
    Instantiate(prefab);
}
```

### 3. Load multiple assets
```csharp
var addresses = new[] { "EnemyPrefab", "WeaponData", "UITexture" };
var assets = await loader.LoadManyAsync<GameObject>(addresses);

foreach (var asset in assets)
{
    if (asset != null)
        Debug.Log($"Loaded asset: {asset.name}");
}
```

### 4. Automatic or manual release
- All assets loaded through this loader are tracked internally.  
- You can rely on `using` for automatic disposal **or** call `Dispose()` manually whenever you want to release all loaded assets.  

No need to worry about memory leaks or dangling references.

## 🛠 Requirements

- Unity 2021+  
- Addressables package  
- [UniTask](https://github.com/Cysharp/UniTask)  

## 🔮 Why use this?

Unity’s Addressables API can be verbose and error-prone.  
This utility wraps the complexity into a **clean, safe, and concise API** that you can drop into any project.  
Perfect for prototypes, indie games, or portfolio projects.  
