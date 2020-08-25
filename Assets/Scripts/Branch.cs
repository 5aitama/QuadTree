using Unity.Mathematics;

namespace Saitama.QuadTree
{
    public struct Branch
    {
        public bool isLeaf { get; private set; }
        public Bounds2D bounds { get; private set; }
        public int ChildStartIndex { get; set; }

        public Branch(Bounds2D bounds)
        {
            this.bounds = bounds;
            isLeaf = false;
            ChildStartIndex = 0;
        }

        public void IsLeaf()
            => isLeaf = true;

        public void IsBranch()
            => isLeaf = false;

        public Branch(Branch parent, Direction direction)
        {
            ChildStartIndex = 0;
            isLeaf = true;

            Bounds2D bounds;

            var center = parent.bounds.Center;
            var ext = parent.bounds.Extents;

            switch(direction)
            {
                case Direction.SW: 
                    bounds = new Bounds2D(center - new float2(ext.x, ext.y) / 2f, ext);
                    break;

                case Direction.NE:
                    bounds = new Bounds2D(center + new float2(ext.x, ext.y) / 2f, ext);
                    break;

                case Direction.NW:
                    bounds = new Bounds2D(center + new float2(-ext.x, ext.y) / 2f, ext);
                    break;

                case Direction.SE:
                    bounds = new Bounds2D(center + new float2(ext.x, -ext.y) / 2f, ext);
                    break;

                default: 
                    throw new System.Exception();
            }

            this.bounds = bounds;
        }
    }
}