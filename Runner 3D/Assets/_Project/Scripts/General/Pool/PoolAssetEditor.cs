using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using JoaoSantos.General;

namespace JoaoSantos.Editor
{
    public class PoolAssetEditor
    {
        private static string poolAssetPath = "Assets/Resources/PoolsAssets/";

        [MenuItem("JoaoSant0s/Pool/PoolAsset", false, 0)]
        private static void CreatePoolAsset()
        {
            var asset = ScriptableObject.CreateInstance<PoolAsset>();

            if (!AssetDatabase.IsValidFolder(poolAssetPath))
            {
                AssetDatabase.CreateFolder("Resources", "PoolsAssets");
            }

            AssetDatabase.CreateAsset(asset, poolAssetPath + "/poolAsset.asset");

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
