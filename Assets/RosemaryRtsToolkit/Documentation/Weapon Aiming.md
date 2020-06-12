
# Weapon Aiming

- A unit can have one or more weapons.
- A unit can have a reference to an enemy unit which should be fired upon.
- A weapon must be aimed at the target before it can fire projectiles effectively.
- A weapon can be aimed at a point in world space.
	- Trajectories and gravity are not taken into account.
- Aiming takes time.

## GameObjects

### Transform Hierarchy
- Unit ![GameObject](./Images/docimgGameObject.png) (e.g. the tank's hull) 
	- Centre ![GameObject](./Images/docimgGameObject.png) (e.g. the centre point of the hull)
	- Body ![GameObject](./Images/docimgGameObject.png) (e.g. the tank's turret)
		- Head ![GameObject](./Images/docimgGameObject.png) (e.g. the tank's gun)
			- Weapon ![GameObject](./Images/docimgGameObject.png) (e.g. the gun muzzle)

### Components
- Unit ![GameObject](./Images/docimgGameObject.png)
	- RosemaryUnit ![Component](./Images/docimgComponent.png)
- Body ![GameObject](./Images/docimgGameObject.png)
	- RosemaryTurretBody ![Component](./Images/docimgComponent.png)
	- RosemaryTurretMotor ![Component](./Images/docimgComponent.png)
- Head ![GameObject](./Images/docimgGameObject.png)
	- RosemaryTurretHead ![Component](./Images/docimgComponent.png)
	- RosemaryTurretMotor ![Component](./Images/docimgComponent.png)
- Weapon ![GameObject](./Images/docimgGameObject.png)
	- RosemaryWeapon ![Component](./Images/docimgComponent.png)

### Requirements
- Body ![GameObject](./Images/docimgGameObject.png)
	- localEulerAngles Y axis controlled by the `RosemaryTurretBody` component.
	- Should have localEulerAngles X=0 and Z=0, so that the Head is aligned correctly.
	- Can have custom localPosition.
- Head ![GameObject](./Images/docimgGameObject.png)
	- localEulerAngles X axis controlled by the `RosemaryTurretHead` component.
	- Should have localPosition=(0,0,0), so that the Weapon is aimed correctly relative to the Body.
	- Should have localEulerAngles Y=0, so that the Weapon is aligned correctly.
- Weapon ![GameObject](./Images/docimgGameObject.png)
	- Should have localPosition X=0 and Y=0, so that the weapon is aligned correctly.
	- Should have localEulerAngles=(0,0,0), so that the weapon is aligned correctly.
	- Can have custom localPosition Z, to offset the spawn point of projectiles.

## Behaviours
- RosemaryUnit ![Component](./Images/docimgComponent.png)
	- Stores information about the enemy unit that is being attacked.
	- Has a Vector3 `centre` property for enemy weapons to aim at the centre of this unit.
		- The point is represented by the Centre GameObject.
- RosemaryTurretBody ![Component](./Images/docimgComponent.png)
	- Can be rotated around the local Y axis.
	- Angle stored as a value from -180 to +180 degrees.
	- Can rotate full 360 degrees.
	- Can calculate the angle required to aim at a target point.
- RosemaryTurretHead ![Component](./Images/docimgComponent.png)
	- Can be rotated around the local X axis.
	- Angle stored as a value from -90 to +90 degrees, clamped.
	- Can calculate the angle required to aim at a target point.
		- The angle calculated also takes into account where the body would have to aim.
- RosemaryTurretMotor ![Component](./Images/docimgComponent.png)
	- Controls one component, either a RosemaryTurretBody or a RosemaryTurretHead.
	- Rotates the turret to aim in a specific direction.
	- Takes time to aim.
	- Has maxAngularSpeed property.
- RosemaryWeapon ![Component](./Images/docimgComponent.png)
	- Fires projectiles at the enemy unit based on information from the `RosemaryUnit` component.
	- Only fires if the weapon is aimed at the target.
	- Has maxAimAngle property.
