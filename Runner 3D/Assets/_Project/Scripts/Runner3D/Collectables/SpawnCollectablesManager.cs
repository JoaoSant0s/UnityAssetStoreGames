using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    public class SpawnCollectablesManager : SingletonBehaviour<SpawnCollectablesManager>
    {
        private List<SpawnCollectables> spawnStacks;

        protected override void Awake()
        {
            base.Awake();
            this.spawnStacks = new List<SpawnCollectables>();
        }

        public void Spawn(SpawnCollectables reference)
        {
            this.spawnStacks.Add(reference);
        }

        public bool HasSpawnStack()
        {
            return this.spawnStacks.Count > 0;
        }

        public SpawnCollectables GetSpawn()
        {
            if (!HasSpawnStack())
            {
                return null;
            }

            var element = this.spawnStacks[0];

            this.spawnStacks.Remove(element);

            return element;
        }
    }
}