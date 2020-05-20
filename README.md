# The Heretic - VFX Character

[![The Heretic - VFX Character intro video](https://assetstorev1-prd-cdn.unity3d.com/key-image/09899011-f9ea-48c8-9bb9-14d311975137.webp)](http://www.youtube.com/watch?v=pZ-C68QnJxo)
[Project overview video](http://www.youtube.com/watch?v=pZ-C68QnJxo)

This project is centered around Morgan, the vfx-based character introduced in the 2nd part of The Heretic short film created by Unity’s Demo Team.
[Learn more about The Heretic](https://unity.com/the-heretic)

Working on this character required pushing the capabilities of the VFX Graph to its limits. We are releasing the character publicly with the intention of showcasing some of the approaches that can be taken when building complex vfx-driven characters. 
[Learn more about the Visual Effect Graph](https://unity.com/visual-effect-graph)

* * *

From the story perspective, Morgan had a few clear requirements like being able to morph between multiple states, calm and angry, female and male or a combination of these, grow in height multiple times over, crumble and a few others.

The appearance on the other hand was less defined so one technical requirement was to build the character in a way that would allow for as much real-time authoring as possible so we decided to make use of the VFX Graph.

## Main Features
**VFX Morphing**: The effects covering Morgan can be morphed at any time regardless if other effects like fire or crumbling are active.

**Shape Morphing**: The underlying skinned mesh can be morphed between female and male shapes independently from the effect which adapt automatically.

**Appearance Tweaks**: Many parts of Morgan’s visual appearance, including how close the particles stick to the mesh, can be easily tweaked from the custom custom inspector.

**Fire**: While it looks more like ambers than proper fire, this highly customizable effect can be used to add an extra layer of intensity when required. It was the last effect built, just a few weeks before the final deadline.

**Crumble**: As the name suggests, Morgan can crumble into pieces, with several options for how this can happen. After the crumbling effect is triggered you need to use either the Reset or the Recompile VFX Graph option to bring the character back to its previous state. One note, this is an experimental crumble effect that was created for The Heretic, however in the end we decided to use another custom method for that exact shot that didn’t make sense to be included here.

**Debug**: Being able to debug properties early on was crucial for being able to experiment with different versions of this character. Debug options were built alongside each main feature.

**Realtime Customization**: Each of the features mentioned above has several customization options exposed in Morgan’s inspector, totalling to over 300 parameters that can be animated over time if desired.

## Customizing Morgan
* You will find Morgan’s controls and customization options on the prefab inspector pane. 
* The main features are separated using fold-out menus that can be expanded in order to access the options.
* In the Morgan Demo Scene, the prefab is located under Morgan_Root. The cloth animated mesh (alembic) is placed under Morgan_Root as well.
* The project has a timeline (Timeline_MorganDescends) that plays an animation with the character getting down the stairs. 
* The first track of the timeline has a few of Morgan’s parameters animated - this can be muted (select the track and press M) or removed if you want to customize Morgan while their animation is playing.
* The recommended workflow for customizing Morgan while the animation is active is to lock both the timeline and Morgan’s inspector pane.
* Be aware that changing the particles with high polycount meshes can lead to performance decrease.

## How It Works
* Morgan is made of 17 visual effect graphs each covering a different part - this was done for easier management.
* The position, normals and tangents for the base meshes are rendered in UV space, which are then set as texture parameters in the vfx graphs - this allows us to position and orient the particles correctly on the character.  
* The vertex color and the albedo texture are also rendered in UV space - these textures are used to manipules certain properties like size, scale, angle and pivot.
* A custom editor centralizes all the graphs making up Morgan - this makes it easy to update shared properties fast. There are about 300 parameters exposed but there’s no real limit for how many can be added, however too many parameters in the interface can make it less practical to work with.

## Known Issues
* Currently there is a known bug that prevents the rearrangement of parameters in the vfx graph blackboard. As a consequence, at this point the parameters are not sorted and categorized properly.
* Displaying some of the graphs inspector pane can cause a performance drop.
* The scene view Selection Outline can cause a drop in performance when selecting Morgan.
* The graphs and scripts contain some legacy elements and hardcoded values carried over The Heretic.
* The Reset button on Morgan’s prefab inspector is your best friend if it misbehaves or if you want to bring the character back after crumbling.
* Work on Morgan was started on an early version of the VFX Graph, without features like Subgraphs for example. While some parts of the graphs have been converted to Subgraphs, they are still not as streamlined as they would have been if built from the ground up today.

## Requirements
* Software
  * Unity 2019.3.9f1 +
  * Git-LFS - this repository contains large binary files and is relying on the [Git-LFS extension](https://git-lfs.github.com/)
* Hardware
  * Intel i7 6th Generation or equivalent
  * NVIDIA GeForce 1080 or equivalent
  * 4GB RAM
