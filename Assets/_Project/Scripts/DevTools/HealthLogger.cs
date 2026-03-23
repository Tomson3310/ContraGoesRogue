using UnityEngine;

namespace ContraGoesRogue.DevTools
{
    /// <summary>
    /// Simple listener script for testing damage in the console.
    /// It will be replaced with a proper heart-based UI in the future.
    /// </summary>
    public class HealthLogger : MonoBehaviour
    {
        public void LogHealthChange(int currentHP, int maxHP)
        {
#if UNITY_EDITOR
            
            Debug.Log($"[UI] Zaktualizowano HP! Stan: {currentHP} / {maxHP}");

#endif        
        }

        public void LogDeath()
        {
#if UNITY_EDITOR
            
            Debug.LogWarning("[UI] GRACZ NIE ŻYJE! Koniec gry.");
#endif        
        }
    }
}