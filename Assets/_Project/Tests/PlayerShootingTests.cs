using NUnit.Framework;
using UnityEngine;
using ContraGoesRogue.Entities;
using ContraGoesRogue.Core.Input;

namespace ContraGoesRogue.Tests
{
    public class PlayerShootingTests
    {
        private GameObject player;
        private GameObject dummyFirePoint;
        private GameObject dummyBullet;
        private PlayerShooting shooting;

        [SetUp]
        public void Setup()
        {
            player = new GameObject("TestPlayer");

            player.AddComponent<InputReader>();
            shooting = player.AddComponent<PlayerShooting>();

            
            dummyFirePoint = new GameObject("FirePoint");
            dummyBullet = new GameObject("Bullet");

            
            shooting.FirePoint = dummyFirePoint.transform;
            shooting.BulletPrefab = dummyBullet;
        }

        [TearDown]
        public void Teardown()
        {
            
            Object.DestroyImmediate(player);
            Object.DestroyImmediate(dummyFirePoint);
            Object.DestroyImmediate(dummyBullet);

            
            GameObject clone = GameObject.Find("Bullet(Clone)");
            if (clone != null)
            {
                Object.DestroyImmediate(clone);
            }
        }

        [Test]
        public void Shoot_InstantiatesProjectile()
        {
            
            shooting.Shoot();

            GameObject createdProjectile = GameObject.Find("Bullet(Clone)");

            Assert.IsNotNull(createdProjectile, "Klon pocisku nie został stworzony na scenie!");
        }
    }
}