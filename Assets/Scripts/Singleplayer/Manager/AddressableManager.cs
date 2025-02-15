using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressableManager : PersistantSingleton<AddressableManager>
{
    private readonly Dictionary<string, AsyncOperationHandle> dicAsset = new();
    private readonly Dictionary<string, int> listTypeAssetCount = new();

    public void CreateAsset<T>(string key, Action<T> onComplete, Action onFailed = null)
    {
        if (dicAsset.ContainsKey(key))
        {
            onComplete?.Invoke((T)(dicAsset[key].Result));
        }
        else
        {
            StartCoroutine(LoadAsset(key, onComplete, onFailed));
        }
    }

    private IEnumerator LoadAsset<T>(string key, Action<T> onComplete, Action onFailed = null, string groupList = null, Action onFinish = null)
    {
        AddToGroupList(groupList);
        var opHandle = Addressables.LoadAssetAsync<T>(key);
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            onComplete?.Invoke(opHandle.Result);
            if (dicAsset.ContainsKey(key))
            {
                RemoveAsset(key);
            }
            dicAsset[key] = opHandle;
            CheckGroupList(groupList, onFinish);
        }
        else if (opHandle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError($"Load Asset Failed: {key}");
            onFailed?.Invoke();
            CheckGroupList(groupList, onFinish);
        }
    }

    //public void LoadBindingAsset<T>(string key, string groupList = null, Action onFinish = null, Action<T> onComplete = null, Action onFailed = null, bool isDump = true)
    //{
    //    Debug.Log($"Load Binding: {key}");
    //    StartCoroutine(LoadAsset<T>(key, result =>
    //    {
    //        GlobalDependencyScope.GlobalScope.DependencyContainer.Register<T>(new InstanceBinding(result));
    //        if (isDump)
    //        {
    //            Debug.Log(result);
    //        }
    //        onComplete?.Invoke(result);
    //    }, onFailed, groupList, onFinish));
    //}

    private void AddToGroupList(string groupList = null)
    {
        if (!string.IsNullOrEmpty(groupList))
        {
            if (!listTypeAssetCount.ContainsKey(groupList))
            {
                listTypeAssetCount.Add(groupList, 0);
            }
            listTypeAssetCount[groupList]++;
        }
    }

    private void CheckGroupList(string groupList = null, Action onFinish = null)
    {
        if (!string.IsNullOrEmpty(groupList) && listTypeAssetCount.ContainsKey(groupList))
        {
            listTypeAssetCount[groupList]--;
            if (listTypeAssetCount[groupList] == 0)
            {
                onFinish?.Invoke();
            }
        }
    }

    public void RemoveAsset(string key)
    {
        Addressables.Release(dicAsset[key]);
        dicAsset.Remove(key);
    }

    //public void LoadJSON<T>(string key, Action<T> onComplete = null, Action<string> onFail = null)
    //{
    //    Debug.Log($"Load JSON: {key}");
    //    Addressables.LoadAssetAsync<TextAsset>(key).Completed += handle =>
    //    {
    //        if (handle.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            T obj = JsonConvert.DeserializeObject<T>(handle.Result.text);
    //            Debug.DumpToConsole(obj);
    //            onComplete?.Invoke(obj);
    //            Addressables.Release(handle);
    //        }
    //        else if (handle.Status == AsyncOperationStatus.Failed)
    //        {
    //            Debug.LogError($"Load JSON Fail {key}");
    //            onFail?.Invoke("Load JSON Fail");
    //            Addressables.Release(handle);
    //        }
    //    };
    //}

    public void CreateScene(string sceneKey, Action<bool> onComplete = null, Action<float> onProgress = null)
    {
        StartCoroutine(LoadSceneAsync(sceneKey, onComplete, onProgress));
    }

    private IEnumerator LoadSceneAsync(string sceneKey, Action<bool> onComplete, Action<float> onProgress)
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single, false);

        while (!handle.IsDone)
        {
            float progress = handle.PercentComplete * 100;
            onProgress?.Invoke(progress);
            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Scene '{sceneKey}' loaded successfully!");
            handle.Result.ActivateAsync();
            onComplete?.Invoke(true);
        }
        else
        {
            Debug.LogError($"Failed to load scene '{sceneKey}': {handle.OperationException}");
            onComplete?.Invoke(false);
        }

        Addressables.Release(handle);
    }

    public void PreloadAsset(string key, Action<bool> onComplete, Action<float> onProgress = null)
    {
        StartCoroutine(StartPreload(key, onComplete, onProgress));
    }

    private IEnumerator StartPreload(string key, Action<bool> onComplete, Action<float> onProgress = null)
    {
        AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync(true);
        yield return handle;

        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(key, false);
        float progress = 0;

        while (downloadHandle.Status == AsyncOperationStatus.None)
        {
            float percentageComplete = downloadHandle.GetDownloadStatus().Percent;
            if (percentageComplete > progress * 1.1) // Report at most every 10% or so
            {
                progress = percentageComplete; // More accurate %
                Debug.Log($"Preload data progress {progress}");
                onProgress?.Invoke(progress);
            }
            yield return null;
        }

        Debug.Log($"Download key '{key}' {downloadHandle.Status}");
        onComplete?.Invoke(downloadHandle.Status == AsyncOperationStatus.Succeeded);
        Addressables.Release(downloadHandle); //Release the operation handle
    }

    public void StartUpdate(Action<bool> onComplete = null, Action<float> onProgress = null)
    {
        StartCoroutine(CheckForUpdates(onComplete, onProgress));
    }

    private IEnumerator CheckForUpdates(Action<bool> onComplete = null, Action<float> onProgress = null)
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;
        Debug.Log(checkHandle.Status);
        if (checkHandle.Status == AsyncOperationStatus.Succeeded && checkHandle.Result.Count > 0)
        {
            Debug.Log("Updates available. Starting download...");

            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, false);
            yield return updateHandle;

            if (updateHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Catalogs updated. Downloading assets...");

                if (updateHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Catalogs updated successfully!");
                }
                else
                {
                    Debug.LogError("Failed to update catalogs!");
                }

                Addressables.Release(updateHandle);
            }
            else
            {
                Debug.LogError("Failed to update catalogs!");
                onComplete?.Invoke(false);
            }

            Addressables.Release(updateHandle);
        }
        else
        {
            Debug.Log("No updates available.");
            onComplete?.Invoke(true);
        }

        Addressables.Release(checkHandle);
    }
}
