
# Top-Down Camera

## Game Objects

### Transform Hierarchy
- Camera Pivot ![GameObject](./Images/docimgGameObject.png)
	- Boom ![GameObject](./Images/docimgGameObject.png)
		- Main Camera ![GameObject](./Images/docimgGameObject.png)

### Components
- Camera Pivot ![GameObject](./Images/docimgGameObject.png)
	- RosemaryCameraPivot ![Component](./Images/docimgComponent.png)
	- RosemaryPlayerCameraPivot ![Component](./Images/docimgComponent.png)
- Boom ![GameObject](./Images/docimgGameObject.png)
- Main Camera ![GameObject](./Images/docimgGameObject.png)
	- Camera ![Component](./Images/docimgComponent.png)

### Requirements
- Camera Pivot ![GameObject](./Images/docimgGameObject.png)
	- localPosition X and Z are controlled by the `RosemaryCameraPivot` ![Component](./Images/docimgComponent.png) component.
	- Can have a custom rotation for localEulerAngles axes X and Y.
- Boom ![GameObject](./Images/docimgGameObject.png)
	- Must be assigned to property `Boom Transform` of the `RosemaryCameraPivot` ![Component](./Images/docimgComponent.png) component.
	- localPosition Z is controlled by the `RosemaryCameraPivot` ![Component](./Images/docimgComponent.png) component.
	- Should have localPosition X=0 and Y=0, so that the camera looks at the pivot.
	- Should have localEulerAngles=(0,0,0), so that the camera looks at the pivot.

## Behaviours
- RosemaryCameraPivot ![Component](./Images/docimgComponent.png)
	- Pivot moves along the horizontal plane (XZ) by changing `localPosition`.
		- Pivot moves relative to its current rotation.
			- Right/Left will strafe along the horizontal plane.
			- Up/Down will move it forward/backward along the horizontal plane.
		- Pivot cannot move outside of the map boundaries.
		- Boundaries are described by mimimum and maximum points in world space (like an AABB).
	- Distance from the `Pivot` ![GameObject](./Images/docimgGameObject.png) to the `Boom` ![GameObject](./Images/docimgGameObject.png) is controlled by a zoom value.
		- This distance is proportional to the zoom amount based on a curve.
		- Zoom amount is clamped to the interval 0 to 1.
		- Zoom amount is changed linearly by input.
		- The curve can be modified to change the minimum and maximum distances.
- RosemaryPlayerCameraPivot ![Component](./Images/docimgComponent.png)
	- W/S/A/D to move `Camera Pivot` ![GameObject](./Images/docimgGameObject.png).
	- Mouse Scrollwheel to zoom in/out.
	- Scrolling speed depends on how far it is zoomed out.
	- Hold shift to move faster (increased input sensitivity).
