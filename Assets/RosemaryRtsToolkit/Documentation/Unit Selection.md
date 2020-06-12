
# Unit Selection

## GameObjects

### Transform Hierarchy
- Player ![GameObject](./Images/docimgGameObject.png)
- Units ![GameObject](./Images/docimgGameObject.png)
	- Unit ![GameObject](./Images/docimgGameObject.png)
		- Centre ![GameObject](./Images/docimgGameObject.png)
		- Selected Effect ![GameObject](./Images/docimgGameObject.png)
	- Unit (2)
	- Unit (3)
	- ...
	- Unit (N)
- Overlay Canvas ![GameObject](./Images/docimgGameObject.png)
	- Box Select Rect Image ![GameObject](./Images/docimgGameObject.png)

### Components
- Player ![GameObject](./Images/docimgGameObject.png)
	- RosemarySelection ![Component](./Images/docimgComponent.png)
	- RosemaryPlayerSelection ![Component](./Images/docimgComponent.png)
- Units ![GameObject](./Images/docimgGameObject.png)
	- RosemaryUnitLocalityManager ![Component](./Images/docimgComponent.png)
- Unit ![GameObject](./Images/docimgGameObject.png)
	- RosemaryUnit ![Component](./Images/docimgComponent.png)
	- Collider ![Component](./Images/docimgComponent.png) (e.g. Box, Sphere)
- Box Select Rect Image ![GameObject](./Images/docimgGameObject.png)
	- Image ![Component](./Images/docimgComponent.png)

### Requirements
- Unit ![GameObject](./Images/docimgGameObject.png)
	- Layer set to "`Rosemary Unit Selection Colliders`" for some colliders.
- Selected Effect ![GameObject](./Images/docimgGameObject.png)
	- Must be a child or grandchild of Unit ![GameObject](./Images/docimgGameObject.png). (e.g. can be a child of Centre ![GameObject](./Images/docimgGameObject.png)).
- Box Select Rect Image ![GameObject](./Images/docimgGameObject.png)
	- Image ![Component](./Images/docimgComponent.png)
		- color alpha should be slightly transparent.
		- `Raycast Target` property should be false.
	- RectTransform ![Component](./Images/docimgComponent.png)
		- anchor set to bottom left (min=(0,0), max=(0,0)).
		- pivot set to bottom left (0, 0).

## Behaviours
- RosemarySelection ![Component](./Images/docimgComponent.png)
	- Has a set of all units currently selected by the player (client).
- RosemaryPlayerSelection ![Component](./Images/docimgComponent.png)
	- Left click to select single unit.
	- Left click and drag to box select multiple units.
	- Any previously selected units are deselected.
	- Layer mask must be set to hit objects on layer "`Rosemary Unit Selection Colliders`" or any terrain/ground layers.
- RosemaryUnitLocalityManager ![Component](./Images/docimgComponent.png)
	- Tracks the position of all `RosemaryUnit` ![Component](./Images/docimgComponent.png) GameObjects ![GameObject](./Images/docimgGameObject.png).
	- Quickly search for units within a volume (frustum of box select).
- RosemaryUnit ![Component](./Images/docimgComponent.png)
	- Can be selected and deselected.
	- Has a Selected Effect ![GameObject](./Images/docimgGameObject.png) which appears (is activated) when this unit is selected.
