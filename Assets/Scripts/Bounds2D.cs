using Unity.Mathematics;

namespace Saitama.QuadTree
{
    public struct Bounds2D
    {
        public float2 Center { get; private set; }
        public float2 Extents { get; private set; }

        public Bounds2D(float2 center, float2 size)
        {
            Center = center;
            Extents = size / 2f;
        }
    }
}