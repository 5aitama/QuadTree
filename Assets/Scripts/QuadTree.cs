using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Saitama.QuadTree
{

    public class QuadTree : System.IDisposable
    {
        private NativeList<Branch> branches;
        private NativeArray<float> LOD;

        private Bounds2D bounds;

        public QuadTree(Bounds2D bounds, int maxLOD = 4)
        {
            this.bounds = bounds;

            branches = new NativeList<Branch>(Allocator.Persistent);
            LOD = new NativeArray<float>(maxLOD, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            
            for(var i = 0; i < maxLOD; i++)
                LOD[i] = 10 * math.pow(2, maxLOD - i);
        }

        public void Build(float2[] positions)
        {
            branches.Clear();

            // Add root branch
            branches.Add(new Branch(bounds));

            BuildRecursive(positions);
        }

        private void BuildRecursive(float2[] positions, int branchIndex = 0, int depth = 0)
        {
            
            var currentBranch = branches[branchIndex];

            /*
                If the index is the root branch then
                check if one of these positions is in branch bounds 
                to know if we need to subdivide it or not.
            */
            if(branchIndex == 0 && AABB2D.Contains(currentBranch.bounds, positions))
            {
                Subdivide(branchIndex, positions, depth);
                return;
            }

            if(depth < GetLOD(currentBranch, depth, positions))
                Subdivide(branchIndex, positions, depth);
            else
                branches[branchIndex].IsLeaf();
        }

        private int GetLOD(Branch branch, int currentDepth, float2[] positions)
        {
            int lod = 0;

            for(var posIndex = 0; posIndex < positions.Length; posIndex++)
            {
                var dist = math.distance(branch.bounds.Center, positions[posIndex]);

                for(int i = 0; i < LOD.Length && dist < LOD[i]; i++)
                    lod = lod > i ? lod : i;
                
                if(currentDepth < lod)
                    return lod;
            }

            return lod;
        }

        private void Subdivide(int branchIndex, float2[] positions, int depth)
        {
            var currentBranch = branches[branchIndex];

            currentBranch.IsBranch();
            currentBranch.ChildStartIndex = branches.Length;

            var SWBranch = new Branch(currentBranch, Direction.SW);
            var NWBranch = new Branch(currentBranch, Direction.NW);
            var NEBranch = new Branch(currentBranch, Direction.NE);
            var SEBranch = new Branch(currentBranch, Direction.SE);

            branches.Add(SWBranch);
            branches.Add(NWBranch);
            branches.Add(NEBranch);
            branches.Add(SEBranch);

            branches[branchIndex] = currentBranch;

            BuildRecursive(positions, currentBranch.ChildStartIndex    , depth + 1);
            BuildRecursive(positions, currentBranch.ChildStartIndex + 1, depth + 1);
            BuildRecursive(positions, currentBranch.ChildStartIndex + 2, depth + 1);
            BuildRecursive(positions, currentBranch.ChildStartIndex + 3, depth + 1);
        }

        public void Dispose()
        {
            branches.Dispose();
            LOD.Dispose();
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.green;
            for(var i = 0; i < branches.Length; i++)
            {
                if(!branches[i].isLeaf)
                    continue;

                var center = branches[i].bounds.Center;
                var extents = branches[i].bounds.Extents;

                var sw = new float3(center + new float2(-extents.x, -extents.y), 0);
                var nw = new float3(center + new float2(-extents.x,  extents.y), 0);
                var ne = new float3(center + new float2( extents.x,  extents.y), 0);
                var se = new float3(center + new float2( extents.x, -extents.y), 0);

                Gizmos.DrawLine(sw, nw);
                Gizmos.DrawLine(nw, ne);
                Gizmos.DrawLine(ne, se);
                Gizmos.DrawLine(se, sw);
            }
        }
    }
}
