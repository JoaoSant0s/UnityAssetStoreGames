using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoaoSantos.General.Asset
{
    public class PoolAsset : ScriptableObject
    {
        public int id;        

        public override string ToString()
        {
            var value = "id: " + this.id;
            return value;

        }
    }
}