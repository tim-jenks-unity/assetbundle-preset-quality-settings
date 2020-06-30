using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public class SampleAssetPipeline : MonoBehaviour
{
    [MenuItem("Builder/Build All")]
    public static void BuildAllAssetBundles()
    {
        BuildAssetBundlesiOSStandard();
        BuildAssetBundlesiOSLow();
        BuildAssetBundlesOSXStandard();
        BuildAssetBundlesOSXLow();
    }

    [MenuItem("Builder/AssetBundles/iOS/Standard Def")]
    public static void BuildAssetBundlesiOSStandard()
    {
        Build(AssetQualitySetting.StandardDefinition, BuildTarget.iOS);
    }
    
    [MenuItem("Builder/AssetBundles/iOS/Low Def")]
    public static void BuildAssetBundlesiOSLow()
    {
        Build(AssetQualitySetting.LowDefintion, BuildTarget.iOS);
    }
    
    [MenuItem("Builder/AssetBundles/OSX/Standard Def")]
    public static void BuildAssetBundlesOSXStandard()
    {
        Build(AssetQualitySetting.StandardDefinition, BuildTarget.StandaloneOSX);
    }
    
    [MenuItem("Builder/AssetBundles/OSX/Low Def")]
    public static void BuildAssetBundlesOSXLow()
    {
        Build(AssetQualitySetting.LowDefintion, BuildTarget.StandaloneOSX);
    }

    private static void Build(AssetQualitySetting quality, BuildTarget buildTarget)
    {
        // Could (optionally) clean here.
        
        switch (quality)
        {
            case AssetQualitySetting.LowDefintion:
                ApplyLowDefinition();
                break;
            
            case AssetQualitySetting.StandardDefinition:
                ApplyStandardDefinition();
                break;
        }

        CreateAssetBundles(quality, buildTarget);
    }
    
    private static void ApplyStandardDefinition()
    {
        ApplyAtlasPreset("Assets/Presets/AtlasStandardDefinition.preset", "Assets/Sprites/64x64-tilemap.png");
    }
    
    private static void ApplyLowDefinition()
    {
        ApplyAtlasPreset("Assets/Presets/AtlasLowDefinition.preset", "Assets/Sprites/64x64-tilemap.png");
    }

    private static void ApplyAtlasPreset(string presetFullPath, string targetAtlasFullPath)
    {
        var targetAsset = AssetImporter.GetAtPath(targetAtlasFullPath);
        var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetFullPath);
        if (preset.CanBeAppliedTo(targetAsset) == false)
        {
            Debug.LogError("Cannot Apply Preset");
            return;
        }
        
        var result = preset.ApplyTo(targetAsset);
        Debug.LogError($"Preset apply result: {result}");

        AssetDatabase.ImportAsset(targetAtlasFullPath);
    }
    
    private static void CreateAssetBundles(AssetQualitySetting quality, BuildTarget buildTarget)
    {
        var relative = AssetBundleRelativePath(quality);
        var fullPath = Path.Combine(Application.streamingAssetsPath, relative);

        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);

        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;

        BuildPipeline.BuildAssetBundles($"Assets/StreamingAssets/{relative}", options, buildTarget);
    }
    
    private static string AssetBundleRelativePath(AssetQualitySetting quality)
    {
        BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
        var platform = GetPlatformForAssetBundles(buildTarget);
        var relative = $"AssetBundles/{platform}-{quality}/";
        return relative;
    }
    
    private static string GetPlatformForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            case BuildTarget.PS4:
                return "PS4";
            case BuildTarget.XboxOne:
                return "XBoxOne";
            case BuildTarget.Switch:
                return "Switch";
            default:
                Debug.Log("Failed to find asset bundle path for platform: " + target.ToString());
                return null;
        }
    }
}
