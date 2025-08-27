using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class AddressableDataLoader : IDisposable
{
    private readonly List<AsyncOperationHandle> _handles = new();

    public async UniTask<T> LoadAsync<T>(string address)
    {
        var locationHandle = Addressables.LoadResourceLocationsAsync(address);
        await locationHandle.Task;

        if (locationHandle.Status != AsyncOperationStatus.Succeeded || locationHandle.Result.Count == 0)
        {
            Addressables.Release(locationHandle);
            Debug.LogWarning($"Failed to load Addressable at '{address}' of type {typeof(T).Name}");
            return default;
        }
        
        var loadHandle = Addressables.LoadAssetAsync<T>(address);
        _handles.Add(loadHandle);
        await loadHandle;

        Addressables.Release(locationHandle);

        if (loadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(loadHandle);
            return default;
        }

        return loadHandle.Result;
    }

    public async UniTask<List<T>> LoadManyAsync<T>(IEnumerable<string> addresses)
    {
        var tasks = new List<UniTask<T>>();

        foreach (var address in addresses)
            tasks.Add(LoadAsync<T>(address));
        
        var results = await UniTask.WhenAll(tasks);
        
        return new List<T>(results);
    }

    public void Dispose()
    {
        foreach (var handle in _handles)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }

        _handles.Clear();
    }
}

