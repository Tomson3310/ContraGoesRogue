namespace ContraGoesRogue.Core.Interfaces
{
    /// <summary>
    /// Interface for every object in the game that can receive damage (Player, Enemies, Shields, Boss etc)
    /// </summary>
    public interface IDamageable
    {
        // Amount: 1 - damage for normal attack, 2 - damage for strong attack
        void TakeDamage(int amount);

        int GetCurrentHealth();
    }
}