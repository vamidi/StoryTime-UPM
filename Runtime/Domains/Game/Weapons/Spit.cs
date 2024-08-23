using UnityEngine;

namespace StoryTime.Domains.Game.Weapons
{
    using StoryTime.Domains.Game.NPC.Enemies.Behaviours.Grenadier;

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
