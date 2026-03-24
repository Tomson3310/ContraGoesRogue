# Changelog

All notable changes to this project will be documented in this file.

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