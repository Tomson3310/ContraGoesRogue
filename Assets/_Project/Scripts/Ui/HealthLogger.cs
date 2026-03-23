using UnityEngine;

namespace ContraGoesRogue.UI
{
    /// <summary>
    /// Prosty skrypt nasłuchujący (Obserwator) do testowania obrażeń w konsoli.
    /// W przyszłości zastąpimy go prawdziwymi serduszkami na ekranie.
    /// </summary>
    public class HealthLogger : MonoBehaviour
    {
        public void LogHealthChange(int currentHP, int maxHP)
        {
            Debug.Log($"[UI] Zaktualizowano HP! Stan: {currentHP} / {maxHP}");
        }

        public void LogDeath()
        {
            Debug.LogWarning("[UI] GRACZ NIE ŻYJE! Koniec gry.");
        }
    }
}