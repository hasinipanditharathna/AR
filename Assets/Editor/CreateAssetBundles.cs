#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

// public class CreateAssetBundles : MonoBehaviour
// {
//    [MenuItem ("Assets/Build AssetBundles")]
//    static void BuildAllAssetBundles ()
//    {
//         // BuildPipeline.Build ("Assets/AssetBundles");
//         BuildPipeline.BuildAllAssetBundles ("Assets/AssetBundles");
//    }
// }


// using UnityEditor;

public class CreateAssetBundles : MonoBehaviour
{
    [MenuItem ("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles ()
    {
        BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
#endif
