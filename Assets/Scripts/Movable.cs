using Interfaces;
using UnityEngine;

public class Movable : MonoBehaviour, IMovable
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float accuracy = 0.05f;
    public float Speed => speed;
    public bool IsMoving { get; private set; }
    public float Accuracy => accuracy;
    

    public void Move(Vector3 endPos)
    {   
        Vector3 direction = endPos - transform.position;
        Vector3 delta = Speed * Time.fixedDeltaTime * direction.normalized;

        transform.position += delta;
        IsMoving = !(Mathf.Abs(transform.position.x - endPos.x) < accuracy && Mathf.Abs(transform.position.z - endPos.z) < accuracy);
    }
}
