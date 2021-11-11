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

This scene provides some examples showing how you might use this package, Cyan cylinders represent agents which are using steering behaviours, Orange spheres are projectiles or obstacles to avoid, and mauve objects represent targets the agents will try to reach.

![BasicAgent](Documentation~\images\DemoGuide\BasicAgent.png)

---

## Basic usage

