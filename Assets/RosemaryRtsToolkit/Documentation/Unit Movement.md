
# Unit Movement

## GameObjects

### Transform Hierarchy
- Player ![GameObject](./Images/docimgGameObject.png)
- Units ![GameObject](./Images/docimgGameObject.png)
	- Unit ![GameObject](./Images/docimgGameObject.png)
		- Centre ![GameObject](./Images/docimgGameObject.png)
	- Unit (2)
	- Unit (3)
	- ...
	- Unit (N)
- Terrain
- Scene Object with Collider...

### Components
- Player ![GameObject](./Images/docimgGameObject.png)
	- RosemarySelectionController ![Component](./Images/docimgComponent.png)
	- RosemaryCommandController ![Component](./Images/docimgComponent.png)
- Units ![GameObject](./Images/docimgGameObject.png)
	- RosemaryUnitLocalityManager ![Component](./Images/docimgComponent.png)
- Unit ![GameObject](./Images/docimgGameObject.png)
	- RosemaryUnit ![Component](./Images/docimgComponent.png)
	- NavMeshAgent ![Component](./Images/docimgComponent.png)

### Requirements
- Unit ![GameObject](./Images/docimgGameObject.png)
	- localPosition is controlled by `RosemaryUnit` ![Component](./Images/docimgComponent.png) component.

## Behaviours
- RosemarySelectionController ![Component](./Images/docimgComponent.png)
	- Has a set of all currently selected units.
- RosemaryCommandController ![Component](./Images/docimgComponent.png)
	- Right Click on empty ground to order selected units to move to that position.
	- Right Click on an enemy unit to order selected units to target and attack the enemy unit.
	- Uses the set of selected units from `RosemarySelectionController` ![Component](./Images/docimgComponent.png).
- RosemaryUnitLocalityManager ![Component](./Images/docimgComponent.png)
	- Tracks the position of all `RosemaryUnit` ![Component](./Images/docimgComponent.png) GameObjects ![GameObject](./Images/docimgGameObject.png).
	- Quickly search for units within a volume.
	- This is updated whenever a unit moves between regions.
- RosemaryUnit ![Component](./Images/docimgComponent.png)
	- Moves around in the scene with the NavMeshAgent ![Component](./Images/docimgComponent.png).
	- Can store a destination position for where it has been ordered to move.
	- Can store a reference to a unit which it has been .
