using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace JoaoSantos.Runner3D.WorldElement
{     
    [UpdateAfter(typeof(MoveCollectableSystem))]
    public class SpinCollectableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;

            Entities.ForEach((ref Rotation rotation, in CollectableComponentData collectable) =>
            {
                rotation.Value = math.mul(
                    math.normalize(rotation.Value),
                    quaternion.AxisAngle(math.up(), collectable.rotationSpeed * dt));
                rotation.Value = math.mul(
                    math.normalize(rotation.Value),
                    quaternion.AxisAngle(new float3(-1,0,0), collectable.rotationSpeed * dt));
            }).ScheduleParallel();
        }
    }
}