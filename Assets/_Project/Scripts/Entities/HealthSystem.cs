using UnityEngine;
using UnityEngine.Events;
using ContraGoesRogue.Core.Interfaces;

namespace ContraGoesRogue.Entities
{
    /// <summary>
    /// Universal segment-based health component.
    /// Can be used for Player, Enemies, or destructible objects.
    /// </summary>
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [Tooltip("Maximum amount of health segments")]
        [SerializeField] private int maxHealth = 5;

        private int currentHealth;

        [Header("Events")]
        // Sends current and max health values
        public UnityEvent<int, int> OnHealthChanged;

        // Called when the object dies
        public UnityEvent OnDied;

        private void Start()
        {
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void TakeDamage(int amount)
        {
            if (currentHealth <= 0) return;

            currentHealth -= amount;

            // Prevent health from going below 0
            currentHealth = Mathf.Max(currentHealth, 0);

            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth == 0)
            {
                OnDied?.Invoke();
            }
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        // Heals the object
        public void Heal(int amount)
        {
            if (currentHealth <= 0) return;

            currentHealth += amount;

            // Prevent healing above max health
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}