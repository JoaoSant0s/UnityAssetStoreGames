using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

using JoaoSantos.Runner3D.Asset;
using JoaoSantos.General;

namespace JoaoSantos.Runner3D.Editor
{
    public class LevelAssetEditor
    {
        private static string LevelAssets = "LevelAssets";
        private static string levelAssetsPath = CollectionsPaths.AssetDatabaseResources + "/" + LevelAssets;

        [MenuItem("Runner3D/Level/LevelAssets", false, 0)]
        private static void CreateLevelAssets()
        {
            var asset = ScriptableObject.CreateInstance<LevelAsset>();

            if (!AssetDatabase.IsValidFolder(levelAssetsPath))
            {
                AssetDatabase.CreateFolder(CollectionsPaths.AssetDatabaseResources, LevelAssets);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            AssetDatabase.CreateAsset(asset, levelAssetsPath + "/LevelAssets.asset");

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}