using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;

namespace JoaoSantos.General
{
    [GenerateAuthoringComponent]
    public struct PrefabEntity : IComponentData
    {
        public Entity collectablePrefab;
    }
}
