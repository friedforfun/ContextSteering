# Context Steering

This is a Unity package to add context steering to a wide range of projects quickly and easily.  
The inspiration for this project was due to a [Devlog](https://www.youtube.com/watch?v=6BrZryMz-ac) by [Game Endeavor](https://www.youtube.com/channel/UCLweX1UtQjRjj7rs_0XQ2Eg) and a subsequent read of the companion article [**Context Steering** Behavior-Driven Steering at the Macro Scale](http://www.gameaipro.com/GameAIPro2/GameAIPro2_Chapter18_Context_Steering_Behavior-Driven_Steering_at_the_Macro_Scale.pdf) by Andrew Fray.

# What is context steering?

**It is not a replacement for pathfinding.** However it is relatively easy to use it in conjunction with a pathfinding system (like the Unity NavMesh).

Context steering involves defining multiple behaviours on an entity (aka. GameObject), combining them, and producing a single output vector that represents a direction that is statistically good for the entity to move in. This allows us to define many discrete simplistic behaviours that when combined can produce seemingly complex movement.

---

# Getting Started
## Installation
1. Open the Package Manager (under Window -> Package Manager).  

    ![OpenPackage](Documentation~/images/Installation/openpackagemanager.png)  

2. Click the "+" icon, and select "Add package from disk...".  

    ![PlusIcon](Documentation~/images/Installation/addpackagefromdisk.png)  

3. Browse to the directory this package is installed at and open the package.json file.  

    ![SelectPackageJSON](Documentation~/images/Installation/SelectPackageJson.png)

---

## Check out the Demo scene

A demo scene has been included that displays some basic usage of this tool. When importing the package into your project you should see a tick box to include the sample scene. The scene is stored at: [**Samples/DemoScene/Scenes/DemoScene.unity**](https://github.com/friedforfun/ContextSteering/blob/master/Samples/DemoScene/Scenes/DemoScene.unity)

This scene provides some examples showing how you might use this package. Cyan cylinders represent *agents* which are using steering behaviours, Orange spheres are *projectiles* or obstacles to avoid, and Mauve objects represent *targets* the agents will try to reach.

Taking a look at an agent (depicted below) there are a few scripts to pay particular attention to: 

- [**SelfSchedulingPlanarController**](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Controllers/SelfSchedulingPlanarController.cs): This is a type of [**PlanarSteeringController**](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/PlanarSteeringBehaviour.cs), it is required for each agent that has any behaviours. It is the component that we use to get our output vector for movement, and in which we define the parameters that are shared to all behaviours.

- **[DotToTransform](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Behaviours/DotToTransform.cs)/[DotToTag](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Behaviours/DotToTag.cs):** These are examples of simple **Behaviours**, they all have an effective **Range**, name, and cruicially a **Direction** of effect. Behaviours that **ATTRACT** will weight the output vector towards their targets, **REPULSE** will weight the output vector away from the targets. The Position/Tag arrays on these components are how we select the targets of the behaviours.

- **[DotToLayerMask](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Masks/DotToLayerMask.cs):** This is a **Mask**, these behave very similarly to behaviours, but rather than attract or repulse, they block (or mask out) a direction from being selected

![BasicAgent](Documentation~/images/DemoGuide/BasicAgent.png)

*Also note the **[PlanarMovement](https://github.com/friedforfun/ContextSteering/blob/master/Samples/DemoScene/Scripts/Agent/PlanarMovement.cs)** demo script, when using this package you would likely create a similar script yourself to decide how to use the output vector in your game.*

Each demo includes some basic metrics; total collisions, collisions in the last n seconds, average collisions per n seconds, and number of goals achieved (contact with correct Mauve objects). Play with the PlanarController, Behaviour, and Mask parameters to see how they each effect the movement, collisions, and goals for each demo.

The Map Debugger when enabled can help provide some visual context for what each behaviour is doing. Note that the length of the lines in this visualisation are normalised to the Map Size so they are not a true representation of the internal data.

---

## Basic usage

### [Built in Controllers](https://github.com/friedforfun/ContextSteering/tree/master/Runtime/PlanarMovement/Controllers)
This package is designed so that once you have configured the behaviours on your agent you can use the output vector (or direction) however you wish.   

The output vector is provided by a controller component, currently the only controller that has been implemented is the [**SelfSchedulingPlanarController**](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/Controllers/SelfSchedulingPlanarController.cs), but if you wish you can extend the [**PlanarSteeringController**](https://github.com/friedforfun/ContextSteering/blob/master/Runtime/PlanarMovement/PlanarSteeringController.cs) class and implement your own custom controller.  

[See the PlanarMovement demo script](https://github.com/friedforfun/ContextSteering/blob/master/Samples/DemoScene/Scripts/Agent/PlanarMovement.cs) for an example of interacting with a controller.  

### [Built in Behaviours](https://github.com/friedforfun/ContextSteering/tree/master/Runtime/PlanarMovement/Behaviours)


