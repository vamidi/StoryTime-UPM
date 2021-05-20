using UnityEngine;

namespace DatabaseSync.Components
{
    public class Spit : GrenadierGrenade
    {
        protected override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);

            if(explosionTimer < 0)
                Explosion();
        }
    }
}
