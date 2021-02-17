using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoaoSantos.Runner3D.WorldElement
{
    public class SpawnCollectables : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> spawnCollectablePoints;

        public List<Transform> SpawnCollectablePoints
        {
            get { return this.spawnCollectablePoints; }
        }

        public void Spawn()
        {
        }
    }
}