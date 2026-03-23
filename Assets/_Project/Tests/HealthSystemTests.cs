using NUnit.Framework;
using UnityEngine;
using ContraGoesRogue.Entities;

namespace ContraGoesRogue.Tests
{
    public class HealthSystemTests
    {
        private GameObject playerObject;
        private HealthSystem healthSystem;

        // Runs before each test to ensure a clean test environment
        [SetUp]
        public void Setup()
        {
            playerObject = new GameObject("TestPlayer");
            healthSystem = playerObject.AddComponent<HealthSystem>();
        }

        // Runs after each test to clean up memory
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(playerObject);
        }

        [Test]
        public void TakeDamage_ReducesHealthCorrectly()
        {
            
            healthSystem.TakeDamage(2);

            Assert.AreEqual(3, healthSystem.GetCurrentHealth());
            
        }

        [Test]
        public void TakeDamage_HealthDoesNotDropBelowZero()
        {
            
            healthSystem.TakeDamage(10);

            Assert.AreEqual(0, healthSystem.GetCurrentHealth());

        }

        [Test]
        public void Heal_HealthDoesNotExceedMax()
        {

            healthSystem.TakeDamage(1);

            healthSystem.Heal(5);

            Assert.AreEqual(5, healthSystem.GetCurrentHealth());

        }

        [Test]
        public void TakeDamage_InvokesOnHealthChangedEvent()
        {
            
            bool eventInvoked = false;

            healthSystem.OnHealthChanged.AddListener((currentHP, maxHP) =>
            {
                eventInvoked = true;
                Assert.AreEqual(4, currentHP);
            });

            healthSystem.TakeDamage(1);
            Assert.IsTrue(eventInvoked, "Event OnHealthChanged nie został wywołany!");
        }

        [Test]
        public void TakeDamage_InvokesOnDiedEventWhenHealthReachesZero()
        {
            
            bool diedEventInvoked = false;
            healthSystem.OnDied.AddListener(() => diedEventInvoked = true);

            healthSystem.TakeDamage(5);

            Assert.IsTrue(diedEventInvoked, "Event OnDied nie został wywołany przy zerowym HP!");
        }
    }
}