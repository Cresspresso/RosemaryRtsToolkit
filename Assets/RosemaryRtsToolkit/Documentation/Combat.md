
# Combat

- Units have Weapons.
- Projectiles hit Units.
	- Travelling takes time.
	- Hit animation/effects have a duration. (e.g. sparks).
- Units have health (HP).
- When a projectile hits a unit, it can deal damage (decrease the unit's health).
- If a unit's health falls to zero, it is destroyed.
	- Destruction animation/effects have a duration. (e.g. explosion).

## GameObjects

### Transform Hierarchy
- Units ![GameObject](./Images/docimgGameObject.png)
	- Unit ![GameObject](./Images/docimgGameObject.png)
		- Body
			- Head
				- Weapon ![GameObject](./Images/docimgGameObject.png)
	- Unit (1)
	- Unit (2)
	- ...
	- Unit (N)
- Projectiles ![GameObject](./Images/docimgGameObject.png)
	- Projectile ![GameObject](./Images/docimgGameObject.png)
	- Projectile (1)
	- Projectile (2)
	- ...
	- Projectile (N)
- ProjectileHitEffects ![GameObject](./Images/docimgGameObject.png)
	- ProjectileHitEffect ![GameObject](./Images/docimgGameObject.png)
	- ProjectileHitEffect (1)
	- ProjectileHitEffect (2)
	- ...
	- ProjectileHitEffect (N)

### Spawner Objects
- Units ![GameObject](./Images/docimgGameObject.png)
	- RosemarySpawner ![Component](./Images/docimgComponent.png)
	- RosemaryUnitSpawner ![Component](./Images/docimgComponent.png)
- Projectiles ![GameObject](./Images/docimgGameObject.png)
	- RosemarySpawner ![Component](./Images/docimgComponent.png)
	- RosemaryProjectileSpawner ![Component](./Images/docimgComponent.png)
- ProjectileHitEffects ![GameObject](./Images/docimgGameObject.png)
	- RosemarySpawner ![Component](./Images/docimgComponent.png)

### Spawn Objects
- Unit ![GameObject](./Images/docimgGameObject.png)
	- RosemaryUnit ![Component](./Images/docimgComponent.png)
- Weapon ![GameObject](./Images/docimgGameObject.png)
	- RosemaryWeapon ![Component](./Images/docimgComponent.png)
- Projectile ![GameObject](./Images/docimgGameObject.png)
	- RosemaryProjectile ![Component](./Images/docimgComponent.png)
- ProjectileHitEffect ![GameObject](./Images/docimgGameObject.png)
	- RosemaryEffect ![Component](./Images/docimgComponent.png)

## Behaviours
- RosemaryUnit ![Component](./Images/docimgComponent.png)
	- Has current health amount.
	- Has maximum health amount.
	- Spawns with maximum health.
	- Can have weapons.
- RosemaryWeapon ![Component](./Images/docimgComponent.png)
	- Can spawn a projectile.
	- Firing animation/effects have a duration. (e.g. muzzle blast).
	- Reloading after firing takes time.
	- Has a maximum range.
	- Can have a minimum range.
	- Can only fire at the target unit if this weapon is in range.
- RosemaryProjectile ![Component](./Images/docimgComponent.png)
	- Travels through the air towards a target unit.
	- Travelling takes time.
	- If a projectile hits a unit:
		- It can deal damage (decrease the unit's health).
		- It can spawn hit animation/effects.
- RosemaryEffect ![Component](./Images/docimgComponent.png)
	- The effects can be started (played).
	- Can be auto-started on awake.
	- Has a duration.
	- Can be auto-destroyed once the effect has ended.
- RosemarySpawner ![Component](./Images/docimgComponent.png)
	- Holds a set of pre-instantiated GameObjects with the `RosemarySpawn` ![Component](./Images/docimgComponent.png) component.
	- A prefab is instantiated many times before the game starts.
	- The instances are disabled until spawned.
	- If there are no more pre-instantiated objects to spawn, it instantiates a few more and returns one.
	- The spawned GameObjects are activated and deactivated instead of destroyed and instantiated.
- RosemarySpawn ![Component](./Images/docimgComponent.png)
	- Have an event method for when it is spawned.
	- Have an event method for when it is despawned.
	- Can be despawned.
- RosemaryUnitSpawner ![Component](./Images/docimgComponent.png)
	- Spawns RosemaryUnit ![Component](./Images/docimgComponent.png) objects via a RosemarySpawner ![Component](./Images/docimgComponent.png).
- RosemaryProjectileSpawner ![Component](./Images/docimgComponent.png)
	- Spawns RosemaryProjectile ![Component](./Images/docimgComponent.png) objects via a RosemarySpawner ![Component](./Images/docimgComponent.png).
