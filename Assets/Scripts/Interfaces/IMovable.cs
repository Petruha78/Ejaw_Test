using UnityEngine;

namespace Interfaces
{
    public interface IMovable
    {
        float Speed { get;}
        bool IsMoving { get; }
        float Accuracy { get; }

        void Move(Vector3 endPos);      
    }
}
