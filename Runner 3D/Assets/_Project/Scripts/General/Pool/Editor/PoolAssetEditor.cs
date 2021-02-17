using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using JoaoSantos.General.Asset;

namespace JoaoSantos.Editor
{
    public class PoolAssetEditor
    {
        private static string PoolsAssets = "PoolsAssets";
        private static string poolAssetPath = CollectionPaths.AssetDatabaseResources + "/" + PoolsAssets;

        [MenuItem("JoaoSant0s/Pool/PoolAsset", false, 0)]
        private static void CreatePoolAsset()
        {
            var asset = ScriptableObject.CreateInstance<PoolAsset>();

            if (!AssetDatabase.IsValidFolder(poolAssetPath))
            {
                AssetDatabase.CreateFolder(CollectionPaths.AssetDatabaseResources, PoolsAssets);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            AssetDatabase.CreateAsset(asset, poolAssetPath + "/poolAsset.asset");

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
