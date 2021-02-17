using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoaoSantos.General.Asset
{
    public class PoolAsset : ScriptableObject
    {
        public string id;

        public bool Equals(PoolAsset obj)
        {            
            return obj.id.Equals(id);
        }
    }
}