using ContraGoesRogue.Entities;
using NUnit.Framework;
using UnityEngine;

namespace ContraGoesRogue.Tests
{
    public class TargetDummyTests
    {
        private GameObject targetDummyObject;

        [SetUp]
        public void Setup()
        {
            targetDummyObject = new GameObject("TestTargetDummy");

            targetDummyObject.AddComponent<HealthSystem>();
            targetDummyObject.AddComponent<TargetDummy>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(targetDummyObject);
        }

        [Test]
        public void TakeDamage_ReducesHealthSystemHP()
        {
            TargetDummy dummy = targetDummyObject.GetComponent<TargetDummy>();
            int startingHealth = dummy.GetCurrentHealth();

            dummy.TakeDamage(1);

            Assert.AreEqual(startingHealth - 1, dummy.GetCurrentHealth(), "TargetDummy nie przekazał obrażeń do HealthSystem!");
        }
    }
}