using Unity.Mathematics;
using Unity.Collections;

namespace Saitama.QuadTree
{
    public static class AABB2D
    {
        public static bool Contains(Bounds2D bounds, float2 position)
            => position.x > bounds.Center.x - bounds.Extents.x && position.y > bounds.Center.y - bounds.Extents.y &&
               position.x < bounds.Center.x + bounds.Extents.x && position.y < bounds.Center.y + bounds.Extents.y;

        public static bool Contains(Bounds2D bounds, NativeArray<float2> positions)
        {
            for(var i = 0; i < positions.Length; i++)
                if(Contains(bounds, positions[i]))
                    return true;

            return false;
        }

        public static bool Contains(Bounds2D bounds, float2[] positions)
            => Contains(bounds, new NativeArray<float2>(positions, Allocator.Temp));
    }
}