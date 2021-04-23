using UnityEngine;
using Unity.Entities;

using Unity.Mathematics;


namespace JoaoSantos.General
{
    [GenerateAuthoringComponent]
    public struct WobbleComponentData : IComponentData
    {
        public float moveSpeed;
        public bool invertDirection;
        public float2 moveYLimit;

        private float3 startPosition;

        public float3 StartPosition
        {
            get { return this.startPosition; }
            set { this.startPosition = value; }
        }
    }
}
