
# Tutorial

How to create an RTS game using the Rosemary Rts Toolkit.

This tutorial is INCOMPLETE!

## New Scene

- Create a new scene and name it "Rts Tutorial Scene". (Project window > right click > Create > Scene)

- Open the scene.

- Add the scene to the build settings. (File &gt; Build Settings &gt; Scenes In Build &gt; Add Open Scenes)

## Hierarchy

- Create empty GameObjects with the following structure.
	- Note that spawners start with an @ sign which should be included in the GameObject's name.

```
- (Rts Tutorial Scene)
	- Player Singletons
	- Factions
		- Blue Faction
		- Red Faction
		- Resource Faction
	- Spawners
		- Effects
			- @Default Destruction Effect Spawner
		- Player Effects
```

## Player Singletons GameObject

- Create an empty GameObject and name it "Player Singletons". (Hierarchy window > right click > Create Empty)

- Add the following components to the "Player Singletons" GameObject:

	- "Rosemary Selection" Component
	- "Rosemary Objective Manager" Component
	- "Rosemary Game Over Scene Loader" Component

- "Player Singletons" GameObject &gt; "Rosemary Game Over Scene Loader" Component &gt; "Scene Name" property &gt; Set it to the name of the scene ("Rts Tutorial Scene").

## Factions

- Create an empty GameObject and name it "Factions".

### Creating a Faction

- Create an empty GameObject and name it "Blue Faction".
- Parent it to the "Factions" GameObject.
- Add a "Rosemary Faction" Component.

### More Factions

- Duplicate the faction to make three factions in total. Name them "Blue Faction", "Red Faction", and "Resource Faction". (see Creating a Faction)

- Set the property ("Resource Faction" GameObject &gt; "Rosemary Faction" Component &gt; "Is Resource Faction") to true.

- Set the property ("Player Singletons" GameObject > "Rosemary Selection" Component > "Faction") to be the "Blue Faction" GameObject.

## Destruction Effect

- Create an empty GameObject and name it "Destruction Effect".
- Add a "Rosemary Spawn" Component.
- Add a "Rosemary Visual Effect" Component.

- Create a child Particle System. (Hierarchy window > right click "Destruction Effect" GameObject > Effects > Particle System)

- Add the Particle System to the "Pss" array property ("Destruction Effect" GameObject > "Rosemary Visual Effect" Component > "Pss").

### Spawner

- Create an empty GameObject and name it "@Default Destruction Effect Spawner".

- Add a "Rosemary Spawner" Component.

- Parent the "Destruction Effect" GameObject to the "@Default Destruction Effect Spawner" GameObject.

- Make the "Destruction Effect" GameObject a prefab. (Drag from Hierarchy window into Project window)

- Set the "Prefab" property to the saved prefab. ("@Default Destruction Effect Spawner" GameObject > "Rosemary Spawner" Component > "Prefab")

- Create an empty GameObject and name it "Spawners".

- Parent "@Default Destruction Effect Spawner" to the "Spawners" GameObject.

## Units
