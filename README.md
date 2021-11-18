# Context Steering

This is a Unity package to add context steering to a wide range of projects quickly and easily.  
The inspiration for this project was due to a [Devlog](https://www.youtube.com/watch?v=6BrZryMz-ac) by [Game Endeavor](https://www.youtube.com/channel/UCLweX1UtQjRjj7rs_0XQ2Eg) and a subsequent read of the companion article [**Context Steering** Behavior-Driven Steering at the Macro Scale](http://www.gameaipro.com/GameAIPro2/GameAIPro2_Chapter18_Context_Steering_Behavior-Driven_Steering_at_the_Macro_Scale.pdf) by Andrew Fray.

# Contents
- [What is context steering?](#what-is-context-steering-)
- [Getting Started](#getting-started)
  * [Check out the Demo scene](#check-out-the-demo-scene)
  * [Basic usage](#basic-usage)
    + [Controllers](#controllers)
    + [Behaviours](#behaviours)
    + [Masks](#masks)
  * [Installation](#installation)

<small><i><a href='http://ecotrust-canada.github.io/markdown-toc/'>Table of contents generated with markdown-toc</a></i></small>


# What is context steering?

**It is not a replacement for pathfinding.** However it is relatively easy to use it in conjunction with a pathfinding system (like the Unity NavMesh) - an example of this is included in the Demo scene.

Context steering involves defining multiple behaviours on an entity (aka. GameObject), combining them, and producing a single output vector that represents a direction that is statistically good for the entity to move in. This allows us to define many discrete simplistic behaviours that when combined can produce seemingly complex movement.

---

# Getting Started

## Check out the Demo scene

First things first, a demo scene has been included that displays some basic usage of this tool. When importing the package into your project you should see a tick box to include the sample scene. The scene is stored at: [**Samples/DemoScene/Scenes/DemoScene.unity**](https://github.com/friedforfun/ContextSteering/blob/master/Samples/DemoScene/Scenes/DemoScene.unity)

This scene provides some examples showing how you might use this package. Cyan cylinders represent *agents* which are using steering behaviours, Orange spheres are *projectiles* or obstacles to avoid, and Mauve objects represent *targets* the agents will try to reach.

Taking a look at an agent (depicted below) there are a few scripts to pay particular attention to: 

- **[SelfSchedulingPlanarController](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Controllers/SelfSchedulingPlanarController.cs):** This is a type of **[PlanarSteeringController](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/PlanarSteeringBehaviour.cs)**, it is required for each agent that has any behaviours. It is the component that we use to get our output vector for use outside the package (for example to decide in which direction to move),  we define the parameters that are shared to all behaviours here.

- **[DotToTransform](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Behaviours/DotToTransform.cs)/[DotToTag](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Behaviours/DotToTag.cs):** These are examples of simple **Behaviours**, they all have an effective **Range**, name, and cruicially a **Direction** of effect. Behaviours that **ATTRACT** will weight the output vector towards their targets, **REPULSE** will weight the output vector away from the targets. The Position/Tag arrays on these components are how we select the targets of the behaviours.

- **[DotToLayerMask](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Masks/DotToLayerMask.cs):** This is a **Mask**, these behave very similarly to behaviours, but rather than attract or repulse, they block (or mask out) a direction from being selected

![BasicAgent](Documentation~/images/DemoGuide/BasicAgent.png)

<small>*Also note the **[PlanarMovement](https://github.com/friedforfun/ContextSteering/blob/master/Samples/DemoScene/Scripts/Agent/PlanarMovement.cs)** demo script, when using this package you would likely create a similar script yourself to decide how to use the output vector in your game.*</small>

Each demo includes some basic metrics; total collisions, collisions in the last n seconds, average collisions per n seconds, and number of goals achieved (contact with correct Mauve objects). Play with the PlanarController, Behaviour, and Mask parameters to see how they each effect agents movement, collisions, and goals.

The Map Debugger when enabled can help provide some visual context for what each behaviour is doing. Note that the length of the lines in this visualisation are normalised to the Map Size so they are not a true representation of the internal data.

---

## Basic usage

### [Controllers](https://github.com/friedforfun/ContextSteering/tree/master/Runtime/PlanarMovement/Controllers)

This package is designed so that once you have configured the behaviours on your agent you can use the output vector (or direction) however you wish.   

The output vector is provided by a controller component, currently the only controller that has been implemented is the [**SelfSchedulingPlanarController**](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Controllers/SelfSchedulingPlanarController.cs), but if you wish you can extend the [**PlanarSteeringController**](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/PlanarSteeringController.cs) class and implement your own custom planar controller.  

[See the PlanarMovement demo script](https://github.com/friedforfun/ContextSteering/blob/master/Samples/DemoScene/Scripts/Agent/PlanarMovement.cs) for an example of interacting with a controller.  

**Controller Parameters**
- *Context Map Resolution* - The number of directions to evaluate, each direction is evenly spaced. In Euler angles 360/ContextMapResolution is the angle between each spoke of the map.
- *Context Map Rotation Axis* - The axis around which we define our plane of movement (The axis should be perpendicular to all directions being evaluated). Usually **Y-AXIS** for a 3D game, and **Z-AXIS** for a 2D game.

![Context Map Visualised](Documentation~/images/DemoGuide/ContextMapVis.png)

**SelfSchedulingPlanarController Parameters**
- *Ticks Per Second* - The number of times per second the agent updates its output vector, must be greater than 0.
- *Direction Selector* - This enum allows you to choose 2 algorithms that pick a direction from the final combined context map. **BASIC** just picks the direction with the highest weight, **WITH_INERTIA** caps the angle delta per tick based on the value of *MinDotPerTick*.
- *Min Dot Per Tick* - Applies only when **WITH_INERTIA** is selected as the direction algorithm. Specifies the lowest dot product between the last tick vector and the next output vector. For example, a value of 0 would allow at most a 90 degree change in direction per tick.

### [Behaviours](https://github.com/friedforfun/ContextSteering/tree/master/Runtime/PlanarMovement/Behaviours)

Inclued in this package are 4 Behaviour classes; **DotToTag, DotToTransform, DotToLayer, DotToNavMesh**. Each behaviour differs only in how an array of Vector3 positions is generated each time the controller "thinks", adding new planar behaviours is as simple as overriding the GetPositionVectors method.

**Commmon Behaviour Parameters**
- *Range* - The max range the behaviour considers target positions.
- *Name* - Just a name for the behaviour, might help with debugging. Can be left blank.
- *Direction* - Set to **ATTRACT** to make the behaviour try to move towards the targets, or **REPULSE** to make the behaviour avoid the targets. 
- *Weight* - Modifies the intensity of this behaviour relative to the others. Particularly useful in conjunction with *Scale on distance*.
- *Scale on distance* - Scale the weight of this behaviour based on the distance from the target.
- *Invert scale* - Only applies when *Scale on distance* is enabled. Set to **true** to increase the importance of targets as they approach, **false** makes distant targets more important (and reduces their importance as they approach).


### [Masks](https://github.com/friedforfun/ContextSteering/tree/master/Runtime/PlanarMovement/Masks)

There are 3 Mask classes; **DotToTagMask, DotToTransformMask, and DotToLayerMask**. Masks are very similar to behaviours but instead of attracting or repulsing they block (or mask out) a direction entirely. They can be useful in conjunction with a scale on distance Repulse behaviours to create a lower band on distance to an object (like a wall). Masks do not guarentee that an agent will not move into something masked out, but they do reduce the probability.

**Mask Parameters**

See [behaviour](#behaviours) Parameters

---

## Installation
Either download the latest [Release](https://github.com/friedforfun/ContextSteering/releases) .zip file, or change to the UPM branch and hit the green `Code` button and select `Download ZIP`.  
1. Open the Package Manager (under Window -> Package Manager).  

    ![OpenPackage](Documentation~/images/Installation/openpackagemanager.png)  

2. Click the "+" icon, and select "Add package from disk...".  

    ![PlusIcon](Documentation~/images/Installation/addpackagefromdisk.png)  

3. Browse to the directory this package is installed at and open the package.json file.  

    ![SelectPackageJSON](Documentation~/images/Installation/SelectPackageJson.png)

---
