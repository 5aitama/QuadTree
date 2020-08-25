using UnityEngine;
using Unity.Mathematics;
using Saitama.QuadTree;

public class QuadTreeTest : MonoBehaviour
{
    private QuadTree quadTree;

    public float2 center;
    public float2 size;
    public int maxLOD = 4;
    public float2[] positions;

    private void Start()
    {
        quadTree = new QuadTree(new Bounds2D(center, size), maxLOD);
        quadTree.Build(positions);
    }

    private void Update()
        => quadTree.Build(positions);

    private void OnDestroy()
        => quadTree.Dispose();

    private void OnDrawGizmos()
    {
        if(quadTree == null)
            return; 
        
        quadTree.DrawGizmos();
    }
}
