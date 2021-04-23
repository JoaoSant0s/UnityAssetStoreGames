using Unity.Entities;
using Unity.Mathematics;

namespace JoaoSantos.General
{
    [GenerateAuthoringComponent]
    public struct SpinComponentData : IComponentData
    {
        public float rotationSpeed;

        public float3 baseOrientation;
    }
}
