using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMeSomeDragons : MonoBehaviour
{
    public AssetQualitySetting QualitySetting;
        
    private AssetBundle m_DragonAssetBundle;
    private List<Object> m_Dragons = new List<Object>();

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    private const string Platform = "OSX";
#else
    private const string Platform = "iOS";
#endif
     
    void Start()
    {
        StartCoroutine(LoadDragonAssetBundle());
    }

    private void OnDestroy()
    {
        for (int i=0; i<m_Dragons.Count; ++i)
            Destroy(m_Dragons[i]);
        m_Dragons.Clear();
        if (m_DragonAssetBundle != null)
            m_DragonAssetBundle.Unload(true);
        m_DragonAssetBundle = null;
    }

    private IEnumerator LoadDragonAssetBundle()
    {
        var bundleRequest = AssetBundle.LoadFromFileAsync($"Assets/StreamingAssets/AssetBundles/{Platform}-{QualitySetting}/dragons");
        
        yield return bundleRequest;

        m_DragonAssetBundle = bundleRequest.assetBundle;
        if (m_DragonAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }

        yield return LoadDragonAssets();
    }

    private IEnumerator LoadDragonAssets()
    {
        var load = m_DragonAssetBundle.LoadAllAssetsAsync<GameObject>();
        yield return load;
        if (!load.isDone)
        {
            Debug.Log("Failed to load Assets from AssetBundle!");
        }

        for (int i = 0; i < load.allAssets.Length; ++i)
            m_Dragons.Add(Instantiate(load.allAssets[i]));
    }
}
