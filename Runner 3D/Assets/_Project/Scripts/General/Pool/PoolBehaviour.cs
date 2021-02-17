using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoaoSantos.General.Asset;

namespace JoaoSantos.General
{
    public class PoolBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PoolAsset poolAsset;

        private bool active;

        public bool Active
        {
            get { return this.active; }
        }

        public PoolAsset PoolAsset
        {
            get { return this.poolAsset; }
            set { this.poolAsset = value; }
        }
        public virtual void Disable()
        {
            gameObject.SetActive(false);
            PoolSelector.Instance.Disable(this);
            this.active = false;
        }

        public virtual void Enable()
        {
            this.active = true;
            gameObject.SetActive(true);
        }

        internal void Set(Vector3 position, Transform parent, Quaternion rotation)
        {
            transform.SetParent(parent);
            transform.position = position;
            transform.rotation = rotation;
        }

        internal void Set(Vector3 position, Quaternion rotation)
        {
            transform.SetParent(null);
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}