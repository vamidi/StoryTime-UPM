using UnityEngine;

namespace StoryTime.Components
{
    public abstract class Projectile : MonoBehaviour, IPooled<Projectile>
    {
        public int poolID { get; set; }
        public ObjectPooler<Projectile> pool { get; set; }

        public abstract void Shot(Vector3 target, RangeWeapon shooter);
    }
}
