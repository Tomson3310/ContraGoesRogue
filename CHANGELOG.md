# Changelog

All notable changes to this project will be documented in this file.

## [v0.3.0] - 2026-04-23

### Added
- Implemented `EnemyRunner` AI script based on the Finite State Machine pattern (states: Idle, Chase).
- Integrated the enemy with the `IDamageable` interface and `HealthSystem` script using the Facade pattern, ensuring clean architecture (SRP) and preventing code duplication.
- Added melee damage logic for the Player on direct physical contact.
- Built the first playable level prototype.
- Added new layers: `Player` and `PlayerProjectile`.

### Fixed
- Resolved an issue where projectiles were being destroyed immediately after spawning by disabling collisions between player projectiles and the player in the Layer Collision Matrix.

---

## [v0.2.2] - 2026-03-26

### Added
- Created `GroundBlock` prefab assigned to the `Ground` layer, serving as a basic modular element for building 2.5D levels (using 3D Grid Snapping).
- Created and assigned a `ZeroFriction` physics material to the player (Friction: 0, Combine: Minimum), resolving the wall-sticking issue while airborne.
- Implemented a **Jump Buffer** system, allowing input buffering when the jump button is pressed shortly before landing.
- Added `OnDrawGizmosSelected` method to the `PlayerMovement` script for visualizing ground detection radius in the Unity editor.

### Changed
- Refactored `PlayerMovement` script:
  - Strict separation between input/timer handling (`Update`) and physics application (`FixedUpdate`).
- Updated player rotation logic:
  - `Flip()` now uses explicit rotation assignment via `Rigidbody.rotation` and `Quaternion.Euler`, preventing conflicts with the physics engine.
- Adjusted global gravity (Y set to -30) and increased jump force to achieve a more dynamic, arcade-like feel.

### Fixed
- Eliminated trampoline effect (**Velocity Accumulation**) by resetting vertical velocity (Y axis) before applying jump force.
- Fixed failing `PlayerMovementTests` after `Flip()` refactor by immediately synchronizing `transform.rotation` with `Rigidbody.rotation`.

---

## [v0.2.1] - 2026-03-25

### Added
- Implemented `PlayerShooting` script integrated with the new Input System (Shoot action bound to LMB).
- Added projectile system (`Projectile.cs`) using `Rigidbody` physics (no gravity, `IsTrigger` mode).
- Created `Bullet` prefab for the shooting system.
- Implemented test target `TargetDummy` (prefab) using the `IDamageable` interface and connected to `HealthSystem`.
- Added automated unit tests:
  - `TargetDummyTests` – verification of damage passing,
  - `PlayerShootingTests` – verification of correct bullet instantiation.
- Attached the `HealthLogger` developer tool to the `TargetDummy` prefab to facilitate debugging.
- Integrated **Cinemachine** package and configured a 2D virtual camera that follows the player on X/Y axes.

### Changed
- Refactored `PlayerShooting` script:
  - Introduced `[field: SerializeField]` properties,
  - Extracted `Shoot()` method to enable dependency injection in tests.

---

## [v0.2.0] - 2026-03-23

### Added
- Implemented core architecture interfaces: `IDamageable` and `IWeapon`.
- Created a universal **HealthSystem** based on segmented HP and the Observer pattern (UnityEvents).
- Introduced infrastructure for automated testing (configured Assembly Definition files for runtime and test domains).
- Wrote automated **unit tests (PlayMode)** for HealthSystem:
  - HP boundary validation
  - Healing logic
  - Event invocation
- Integrated the **Unity Input System** package and generated its C# wrapper class.
- Created the **InputReader** component to decouple Unity input from game logic.
- Added **HealthLogger** (DevTools) for testing HP changes in the console.
- Implemented a base **PlayerMovement** script using the new Input System.
- Added a jump mechanic based on `Rigidbody.AddForce` with `ForceMode.Impulse`.
- Introduced ground detection using `Physics.CheckSphere` and layer masks.
- Implemented player model flipping logic based on movement direction.
- Created automated tests for **PlayerMovement** (verifying `Flip` method and object rotation changes).

### Changed
- Protected developer tools (HealthLogger and test-related logic in HealthSystem) from being included in builds using preprocessor directives (`#if UNITY_EDITOR`).
- Configured `ContraGoesRogue.Runtime.asmdef` to correctly reference Unity Input System and player input actions.

### Fixed
- Fixed a **NullReferenceException** in automated tests by explicitly initializing `UnityEvent` instances in the HealthSystem class.

---

## [v0.1.0] - 2026-03-22

### Added
- Initialized Git repository with a dedicated Unity `.gitignore`.
- Created a new Unity project using the **Universal Render Pipeline (URP 3D)** template (targeting a 2.5D perspective).
- Prepared a **Game Design Document (GDD)** defining:
  - Core Loop
  - Upgrade categories (A–E)
  - Enemy list
  - Segmented health system concept
- Set up base folder structure following **Clean Architecture** principles (`_Project` domain-based structure).

### Removed
- Removed unnecessary default Unity files and folders (e.g., `TutorialInfo`).
- Moved environment configuration into `_Project/Settings`.