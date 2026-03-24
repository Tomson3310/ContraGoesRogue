using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ContraGoesRogue.Entities;
using ContraGoesRogue.Core.Input;

namespace ContraGoesRogue.Tests
{
    public class PlayerMovementTests
    {
        private GameObject player;
        private PlayerMovement playerMovement;

        [SetUp]
        public void Setup()
        {
            player = new GameObject("TestPlayer");

            player.AddComponent<Rigidbody>();
            player.AddComponent<InputReader>();

            playerMovement = player.AddComponent<PlayerMovement>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(player);
        }

        [Test]
        public void Flip_ChangesFacingDirectionAndRotatesObject()
        {
            // Player should face right by default
            Assert.IsTrue(playerMovement.IsFacingRight);

            float initialRotationY = playerMovement.transform.rotation.eulerAngles.y;

            playerMovement.Flip();

            // Direction flag should change
            Assert.IsFalse(playerMovement.IsFacingRight);

            // Object should rotate by 180 degrees (with tolerance)
            Assert.AreEqual(
                initialRotationY + 180f,
                playerMovement.transform.rotation.eulerAngles.y,
                0.01f
            );
        }
    }
}