# Context Steering

This is a Unity package to add context steering to a wide range of projects quickly and easily.  
The inspiration for this project was due to a [Devlog](https://www.youtube.com/watch?v=6BrZryMz-ac) by [Game Endeavor](https://www.youtube.com/channel/UCLweX1UtQjRjj7rs_0XQ2Eg) and a subsequent read of the companion article [**Context Steering** Behavior-Driven Steering at the Macro Scale](http://www.gameaipro.com/GameAIPro2/GameAIPro2_Chapter18_Context_Steering_Behavior-Driven_Steering_at_the_Macro_Scale.pdf) by Andrew Fray.

# What is context steering?

**It is not a replacement for pathfinding.** However it is relatively easy to use it in conjunction with a pathfinding system (like the Unity NavMesh).

Context steering involves defining multiple behaviours on an entity (aka. GameObject), combining them, and producing a single output vector that represents a direction that is statistically good for the entity to move in. This allows us to define many discrete simplistic behaviours that when combined can produce seemingly complex movement.

---

## Getting Started
### Installation
1. Open the Package Manager (under Window -> Package Manager).  

    ![OpenPackage](Documentation~/images/Installation/openpackagemanager.png)  

2. Click the "+" icon, and select "Add package from disk...".  

    ![PlusIcon](Documentation~/images/Installation/addpackagefromdisk.png)  

3. Browse to the directory this package is installed at and open the package.json file.  

    ![SelectPackageJSON](Documentation~/images/Installation/SelectPackageJson.png)

---

## Check out the Demo scene

A demo scene has been included that displays some basic usage of this tool. When importing the package into your project you should see a tick box to include the sample scene. The scene is stored at: **Samples/DemoScene/Scenes/DemoScene.unity**

This scene provides some examples showing how you might use this package. Cyan cylinders represent *agents* which are using steering behaviours, Orange spheres are *projectiles* or obstacles to avoid, and Mauve objects represent *targets* the agents will try to reach.

Taking a look at an agent (depicted below) there are a few scripts to pay particular attention to: 

- **SelfSchedulingPlanarController:** This is a type of **PlanarController**, it is required for each agent that has any behaviours. It is the compenent that we use to get our output vector for movement, and in which we define the parameters that are shared to all behaviours.

- **DotToTransform/DotToTag:** These are examples of simple **Behaviours**, they all have an effective **Range**, name, and cruicially a **Direction** of effect. Behaviours that **ATTRACT** will weight the output vector towards their targets, **REPULSE** will weight the output vector away from the targets. The Position/Tag arrays on these components are how we select the targets of the behaviours.

- **DotToLayerMask:** This is a **Mask**, these behave very similarly to behaviours, but rather than attract or repulse, they block (or mask out) a direction from being selected

![BasicAgent](Documentation~/images/DemoGuide/BasicAgent.png)

*Also note the **PlanarMovement** script, when using this package you would likely create a similar script yourself to decide how to use the output vector in your game.*

Each demo includes some basic metrics; total collisions, collisions in the last n seconds, average collisions per n seconds, and number of goals achieved (contact with correct Mauve objects). Play with the PlanarController, Behaviour, and Mask parameters to see how they each effect the movement, collisions, and goals for each demo.

---

## Basic usage

