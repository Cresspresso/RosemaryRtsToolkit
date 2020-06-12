
# Allegiance

## GameObjects

### Transform Hierarchy
- Units ![GameObject](./Images/docimgGameObject.png)
	- Unit ![GameObject](./Images/docimgGameObject.png)
	- Unit (1)
	- Unit (2)
	- ...
	- Unit (N)
- Factions ![GameObject](./Images/docimgGameObject.png)
	- Faction ![GameObject](./Images/docimgGameObject.png)
	- Faction (1)
	- Faction (2)
	- ...
	- Faction (N)

### Components
- Unit ![GameObject](./Images/docimgGameObject.png)
	- RosemaryUnit ![Component](./Images/docimgComponent.png)
- Faction ![GameObject](./Images/docimgGameObject.png)
	- RosemaryFaction ![Component](./Images/docimgComponent.png)

## Behaviours
- RosemaryUnit ![Component](./Images/docimgComponent.png)
	- Is a member of one `RosemaryFaction` ![Component](./Images/docimgComponent.png).
	- Can be ordered to attack another unit.
	- Can only attack a unit if that unit's faction is an enemy of this unit's faction.
	- Can not attack itself.
- RosemaryFaction ![Component](./Images/docimgComponent.png)
	- There is a way to check if two factions are enemies.
	- Can not be an enemy of itself.
	- Has a set of other factions that this faction is allied with.
