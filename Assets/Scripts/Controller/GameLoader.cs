﻿using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameLoader : SingletonBase<GameLoader>
{
    public AssetReference fallbackSprite;
    public List<AssetReference> artAssets = new List<AssetReference>();

    
    private bool loadingFinished;

    private Sprite _fallback;
    private Dictionary<string, List<Action<Sprite>>> _onLoadCallbacks = new Dictionary<string, List<Action<Sprite>>>();
    
    
    [ShowInInspector] private Dictionary<string, Sprite> loadedSprites = new Dictionary<string, Sprite>();


    
    

    public static void RegisterForAssetLoad(string asset, Action<Sprite> callback)
    {
        if (GameLoader.instance.loadedSprites.ContainsKey(asset))
        {
            callback?.Invoke(instance.loadedSprites[asset]);
        }
        else
        {
            instance._onLoadCallbacks.AddOrReplace(asset, new List<Action<Sprite>>());
            instance._onLoadCallbacks[asset].Add(callback);
        }
    }

    
    

    public static bool IsFinishedLoading => instance.loadingFinished;

    public static Sprite GetLoadedSprite(string spriteName)
    {
        if(!IsFinishedLoading)throw new AssetNotLoadedException();
        Sprite sprite;
        if (!instance.loadedSprites.TryGetValue(spriteName, out sprite))
        {
            sprite = instance._fallback;
        }

        return sprite;
    }
    
    protected override void Awake()
    {
        loadingFinished = false;
        var handle = fallbackSprite.LoadAssetAsync<Sprite>();
        handle.Completed += OnFallbackLoaded;
        base.Awake();
        StartCoroutine(LoadAssets());

    }

    private void OnFallbackLoaded(AsyncOperationHandle<Sprite> obj)
    {
        _fallback = obj.Result;
    }


     public IEnumerator LoadAssets()
     {
          int finalCount = artAssets.Count;
          int currentCount = 0;
          Debug.Log("Started Loading Art Assets!");
          foreach (var assetReference in artAssets)
          { 
              var str = assetReference.SubObjectName;
             assetReference.LoadAssetAsync<Sprite>().Completed += handle =>
             {
                 currentCount++;
                 this.loadedSprites.Add(str, handle.Result);
             };
          }
          
          while (currentCount < finalCount)
          {
              yield return null;
          }

          foreach (var kvp in _onLoadCallbacks)
          {
              Sprite s = instance.loadedSprites.ContainsKey(kvp.Key) ? instance.loadedSprites[kvp.Key] : instance._fallback;
              foreach (var action in kvp.Value) {
                  action?.Invoke(s);
              }
          }
          Debug.Log("Finished Loading Art Assets!");
          loadingFinished = true;
          GetComponent<GameManager>().StartGame();
      }
}

public class AssetNotLoadedException : Exception
{
    
}