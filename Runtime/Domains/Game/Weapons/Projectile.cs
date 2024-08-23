using UnityEngine;

namespace StoryTime.Domains.Game.Weapons
{
    using StoryTime.Domains.Game.Pooling;
    
    public abstract class Projectile : MonoBehaviour, IPooled<Projectile>
    {
        public int poolID { get; set; }
        public ObjectPooler<Projectile> pool { get; set; }

        public abstract void Shot(Vector3 target, RangeWeapon shooter);
    }
}
