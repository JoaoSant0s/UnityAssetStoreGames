using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoaoSantos.General;

using JoaoSantos.General.Asset;

namespace JoaoSantos.Runner3D.Asset
{
    [System.Serializable]
    public struct LevelOptions
    {
        public PoolAsset[] options;

        #region Property Methods

        public PoolAsset GetRandomAsset
        {
            get { return this.options.GetRandomItem(); }
        }

        #endregion
    }
    [CreateAssetMenu(fileName = "PoolAsset", menuName = "Runner3D/Level/LevelAsset", order = 0)]
    public class LevelAsset : ScriptableObject
    {
        [SerializeField]
        private int loops;

        [SerializeField]
        private List<LevelOptions> levelSequence;

        public List<LevelOptions> LevelSequence
        {
            get { return this.levelSequence; }
        }

        public int Loops
        {
            get { return this.loops; }
        }
    }
}