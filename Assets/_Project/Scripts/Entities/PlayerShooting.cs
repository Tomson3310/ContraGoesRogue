using ContraGoesRogue.Core.Input;
using UnityEngine;

namespace ContraGoesRogue.Entities
{

    public class PlayerShooting : MonoBehaviour
    {
        [field: SerializeField] public GameObject BulletPrefab { get; set; }
        [field: SerializeField] public Transform FirePoint { get; set; }

        private InputReader inputReader;


        private void Awake()
        {
            inputReader = GetComponent<InputReader>();
            if (inputReader == null)
            {
                Debug.LogWarning($"[PlayerShooting] Brak komponentu InputReader na {gameObject.name}.");
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (inputReader.ShootTriggered)
            {
                Shoot();
            }
        }

        public void Shoot()
        {
            
            Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
            inputReader.ConsumeShoot();
                     
        }
    }
}