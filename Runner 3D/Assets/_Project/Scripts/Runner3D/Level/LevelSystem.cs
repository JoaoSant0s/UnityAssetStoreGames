using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JoaoSantos.General;
using JoaoSantos.General.Asset;

using JoaoSantos.Runner3D.Asset;

namespace JoaoSantos.Runner3D.WorldElement
{
    public class LevelSystem : SingletonBehaviour<LevelSystem>
    {
        [SerializeField]
        private LevelAsset levelAsset;

        private int index;

        private int loopsCount;

        #region Property Methods

        private List<LevelOptions> LevelSequence
        {
            get { return this.levelAsset.LevelSequence; }
        }

        public PoolAsset CurrentAsset
        {
            get { return LevelSequence[this.index].GetRandomAsset; }
        }

        #endregion

        public void SetValues()
        {
            this.loopsCount = this.levelAsset.Loops;
            this.index = 0;
        }

        public bool HasAsset()
        {
            return this.index < LevelSequence.Count;
        }

        public void UpdateToNextLevel()
        {
            this.index++;
            Debug.Log(this.index);

            if (HasAsset()) return;


            if (this.levelAsset.Loops == -1 || this.loopsCount < this.levelAsset.Loops)
            {
                this.loopsCount++;
                this.index = 0;
            }
        }

    }
}