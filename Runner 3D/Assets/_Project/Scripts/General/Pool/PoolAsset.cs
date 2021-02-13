using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoaoSantos.General
{
    [CreateAssetMenu(fileName = "PoolAsset", menuName = "JoaoSant0s/Pool/PoolAsset", order = 0)]
    public class PoolAsset : ScriptableObject
    {
        public string id;

        public bool Equals(PoolAsset obj)
        {
            Debugs.Log(obj, obj.id, id);
            return obj.id.Equals(id);
        }
    }
}