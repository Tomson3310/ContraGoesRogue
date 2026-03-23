using UnityEngine;

namespace ContraGoesRogue.Core.Interfaces
{
    /// <summary>
    /// Interface defining the behavior of every weapon in the game.
    /// </summary>
    public interface IWeapon
    {
        // Method called when the player presses the fire button.
        // firePoint is the point (muzzle) from which the projectile will be fired.
        void Shoot(Transform firePoint);
    }
}