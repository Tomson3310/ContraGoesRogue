using UnityEngine;
using ContraGoesRogue.Core.Interfaces;

namespace ContraGoesRogue.Entities
{

    public class TargetDummy : MonoBehaviour, IDamageable
    {
        private HealthSystem healthSystem;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            if (healthSystem == null)
            {
                Debug.LogWarning($"[TargetDummy] Brak komponentu HealthSystem na {gameObject.name}.");
            }
        }

        
        private void Start()
        {
            healthSystem.OnDied.AddListener(Die);
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
        }

        public int GetCurrentHealth()
        {
            return healthSystem.GetCurrentHealth();
        }
        private void Die()
        {
            Debug.Log($"{gameObject.name} został zniszczony!");
            Destroy(gameObject);
        }
    }
}