using UnityEngine;
using Random = UnityEngine.Random;


public class WaypointManager : MonoBehaviour
{
    [SerializeField] private MeshFilter plane;

    [SerializeField] private float border = 0.5f;

    private Rect _planeRect;

    private void Awake()
    {
        DependencyContainer.Add<WaypointManager>(this);
        
        var width = plane.mesh.bounds.size.x;
        var strength = plane.mesh.bounds.size.z;
        _planeRect = new Rect(transform.position.x - width / 2, transform.position.z - strength / 2, width, strength);
    }

    public Vector3 GetNextWayPoint()
    {
        return new Vector3(Random.Range(_planeRect.xMin + border, _planeRect.xMax - border), 0,
            Random.Range(_planeRect.yMin + border, _planeRect.yMax - border));
    }

    public Vector3 GetEnemyStartPos()
    {
        var randomX = Random.Range(_planeRect.xMin, _planeRect.xMax);
        var randomY = Random.Range(_planeRect.yMin, _planeRect.yMax);
        Vector3[] positions =
        {
            new Vector3(_planeRect.xMin, 0, randomY), new Vector3(_planeRect.xMax, 0, randomY),
            new Vector3(randomX, 0, _planeRect.yMin), new Vector3(randomX, 0, _planeRect.yMax)
        };
        return positions[Random.Range(0, positions.Length - 1)];
    }
}