using System.Collections;

namespace src.interfaces
{
    public interface IDestroyable
    {
        IDestroyable Destroyable();
        IDestroyable Damage(int damage);
        IDestroyable Heal(int damage);
        IEnumerator DestroyRoutine();
    }
}