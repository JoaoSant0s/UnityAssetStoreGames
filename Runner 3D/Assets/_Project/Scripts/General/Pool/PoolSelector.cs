using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using JoaoSantos.General.Asset;

namespace JoaoSantos.General
{
    public class PoolSelector : SingletonBehaviour<PoolSelector>
    {
        [Header("Areas")]
        [SerializeField]
        private Transform disabledPoolArea;

        [Header("Prefabs")]
        [SerializeField]
        private List<PoolBehaviour> basePrefabs;

        private Dictionary<string, List<PoolBehaviour>> poolDictionary;

        protected override void Awake()
        {
            base.Awake();
            this.poolDictionary = new Dictionary<string, List<PoolBehaviour>>();
            CreateBases();
        }

        public T CreateOrSpawn<T>(Vector3 position, Transform parent, Quaternion rotation) where T : PoolBehaviour
        {
            return InternalCreateOrSpawn<T>(position, parent, rotation);
        }

        public T CreateOrSpawn<T>(Vector3 position, Transform parent) where T : PoolBehaviour
        {
            return InternalCreateOrSpawn<T>(position, parent, Quaternion.identity);
        }

        public T CreateOrSpawn<T>(PoolAsset asset, Transform parent) where T : PoolBehaviour
        {
            return InternalCreateOrSpawn<T>(asset, Vector3.zero, parent, Quaternion.identity);
        }

        public T CreateOrSpawn<T>(Vector3 position) where T : PoolBehaviour
        {
            return InternalCreateOrSpawn<T>(position, Quaternion.identity);
        }

        public void Disable(PoolBehaviour poolBehaviour)
        {
            poolBehaviour.transform.SetParent(this.disabledPoolArea);
            poolBehaviour.transform.position = Vector3.zero;
        }

        private T InternalCreateOrSpawn<T>(Vector3 position, Transform parent, Quaternion rotation) where T : PoolBehaviour
        {
            var element = Extract<T>();

            element.Set(position, parent, rotation);
            element.Enable();

            return element;
        }

        private T InternalCreateOrSpawn<T>(PoolAsset asset, Vector3 position, Transform parent, Quaternion rotation) where T : PoolBehaviour
        {
            var element = Extract<T>(asset);

            element.Set(position, parent, rotation);
            element.Enable();

            return element;
        }

        private T InternalCreateOrSpawn<T>(Vector3 position, Quaternion rotation) where T : PoolBehaviour
        {
            var element = Extract<T>();

            element.Set(position, rotation);
            element.Enable();

            return element;
        }

        private T Extract<T>(PoolAsset asset = null) where T : PoolBehaviour
        {
            var type = typeof(T).ToString();

            Debug.Assert(this.poolDictionary.ContainsKey(type), "Don't have the register of this value");

            var list = this.poolDictionary[type];

            var element = (asset != null) ? list.Find(x => !x.Active && x.PoolAsset.Equals(asset)) : list.Find(x => !x.Active);

            if (!element)
            {
                element = Create<T>(asset);
            }

            return (T)element;
        }

        private T Create<T>(PoolAsset asset = null) where T : PoolBehaviour
        {
            var type = typeof(T).ToString();
            Debugs.Log(type);

            var elementPrefab = this.basePrefabs.Find(prefab => prefab is T && prefab.PoolAsset.Equals(asset));

            var element = Instantiate(elementPrefab, this.disabledPoolArea);
            this.poolDictionary[type].Add(element);

            return (T)element;
        }

        private void CreateBases()
        {
            for (int i = 0; i < this.basePrefabs.Count; i++)
            {
                var elementPrefab = this.basePrefabs[i];
                var element = Instantiate(elementPrefab, this.disabledPoolArea);

                var type = element.GetType().ToString();

                if (!this.poolDictionary.ContainsKey(type))
                {
                    this.poolDictionary[type] = new List<PoolBehaviour>();
                }

                this.poolDictionary[type].Add(element);
                element.Disable();
            }
        }

    }
}
