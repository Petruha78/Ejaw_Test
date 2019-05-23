namespace Interfaces
{
    public interface IDestructable
    {
        int HitPoints { get; }
        
        void Die();
    }
}
