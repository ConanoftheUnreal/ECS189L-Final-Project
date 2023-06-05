Changelog
=========

[v1.8] Open-Source Formations
-----------------------------

### Changes 

+ Changed visibility of some editor classes for compatibility with the Formation behaviours

### Enhancements

+ Formations are available as source code on [GitHub](https://github.com/Polarith/AI-Formation)
+ For convenience, you can use the download script in /Polarith/AI/Extensions

### Fixes

+ Changed flat circle gizmo in AIMSteeringFilter to spherical gizmo if a spherical sensor is attached


[v1.7.1] Pandemic Special
-------------------------

### Changes

+ Minimum supported version Unity 2018.4
+ Compatible with the latest Unity 2020.2
+ Context instances in properties are now found automatically in addition to fields
+ RootMotionController now scales better with decided magnitude and has an option to limit the speed to a max value
+ Added our new logo to ressources

### Enhancements

+ **[Pro]** 3D controllers are now able to scale with the decided magnitude instead of having a constant speed factor
+ Improved the documentation for self perception

### Fixes

+ **[Pro]** Fixed broken lab scenes due to bad prefabs
+ **[Pro]** Fixed function to find nearest edge used in pathfinding that was buggy in rare constellations
+ Changed flat circle gizmo in AIMReduction to spherical gizmos if a spherical sensor is attached


[v1.7] 3D Patch - The World Is Not Flat!
----------------------------------------

### Changes

+ Removed the multiplayer network scene and its dependencies from the examples due to deprecated Unity scripts
+ Compatible with the latest Unity 2019
+ AIMPlanarSensorEditor now inherits from AIMSensorEditor
+ Updated AIMWander to be 3D-compatible and added new features for behaviour control

### Enhancements

+ **[Pro]** Added new spherical 3D sensors (UV and Ico spheres)
+ **[Pro]** Added new editors for 3D sensors
+ **[Pro]** Added AIMSeekBounds as new 3D behaviour
+ **[Pro]** Added AIMFleeBounds as new 3D behaviour
+ **[Pro]** Added AIMAvoidBounds as new 3D behaviour
+ **[Pro]** Added new gizmo for 3D sensors
+ **[Pro]** Added new roll, pitch and yaw (RPY) base controller
+ **[Pro]** Added PID controller to support RPY-based controllers
+ **[Pro]** Added aircraft controller based on RPY with aircraft physics
+ **[Pro]** Added copter controller based on RPY with copter physics
+ **[Pro]** Added spaceship controller based on RPY with gravity-free spaceship physics
+ **[Pro]** Added new 3D lab example scenes for AIMSeekBounds, AIMFleeBounds, AIMAvoidBounds, AIMWander
+ **[Pro]** Added new 3D game example scene with an aircraft flying through a town
+ **[Pro]** Added new 3D game example scene with a copter flying through an industrial hall
+ **[Pro]** Added new 3D game example scene with a spaceship flying around planets through the space
+ **[Pro]** Added a script to generate interest or danger on the ground under an object
+ **[Pro]** Added several minor example scripts which are necessary for the new scenes
+ **[Pro]** Added new 3D scene as an example for the AIMContextIndicator
+ _[Free]_ Added interactive questionnaire scene to show you that you definitely need the pro version :)
+ Added shader-based AIMContextIndicator for advanced sensor visualization, even during runtime
+ Added new 2D scene as an example for the new AIMContextIndicator
+ AIMSteeringTag now has an option to disable the perceiving of object-oriented bounds (thanks Matt 'juanitogan' Jernigan)
+ Documentation: Added new section for package examples within the component reference

### Fixes

+ **[Pro]** Fixed a bug in AIMLinearPath that prevents users from deselecting objects when using drag selection
+ **[Pro]** Fixed a bug in AIMLinearPath that changed only the previously selected point on a path when another one was selected
+ The velocity gizmo is now properly shown in AIMFollow, AIMFollowPath, AIMArrive, AIMFollowWaypoints and AIMOrbit
+ Fixed compatibility with Unity 2019.x that prevented compiling
+ Fixed component menu entry for the MousePointer script
+ Fixed typo regarding a private variable in the VehicleController script
+ AIMSteeringTag now prefers TrackVelocity to an attached kinematic Rigidbody(2D) (thanks Tony Zatelli)


[v1.6] Shiny Packages
---------------------

### Changes

+ Complete rework of the Polarith AI packages for both Pro and Free
+ New overall look of the examples using fresh public domain assets, models, sprites, textures, etc.
+ Packages now include proper Polarith image material to comply with the license

### Enhancements

+ **[Pro]** Added a 3D scene that illustrates different methods for improving the performance of scenes with many agents
+ Added laboratory scenes demonstrating the effects of components for both 2D and 3D scenarios
+ Added a 3D scene that shows a sophisticated example of vehicles moving in a roundabout using a state machine
+ Added a 3D scene where cars behave properly on a priority crossroad
+ Added a 3D scene where a character collects items in a forest
+ Added a 2D scene that demonstrates a boid using attraction, repulsion and alignment
+ Added a 2D scene with an example multiplayer space game
+ Added an example RootMotionController including source code
+ Added an example VehicleController including source code
+ Added several example scripts which are necessary for the new scenes
+ AIMContext: Added a public AddObjective method
+ AIMSeekNavMesh: Improved the whole concept of the behaviour, it now uses a more precise raycast method
+ Editor: Added default objects to the hierarchy within Unity's context menu

### Fixes

+ AIMContext: Fixed a bug with the indicator gizmo that occurred when changing the sensor
+ AIMSteeringPerceiver: Fixed an issue that occurred when using both layers and game object lists, whereby the layer object percepts were overwritten
+ AIMSeekNavMesh: Fixed a problem where the behaviour did not work when the SelfObject in AIMContext was null
+ Documentation: Added a missing UnityUtils namespace documentation
+ Documentation: Corrected a wrong description of the AIMContext.DecidedDirection


[v1.5.1] After Patch Fixing
---------------------------

### Changes

+ When using a prediction method for a steering behaviour, circle gizmos are now displayed at the predicted position

### Fixes

+ AIMWander: Works again now, was broken after the introduction of the better self-recognition


[v1.5] Performance and Usability
--------------------------------

### Changes

+ **[Pro]** With this patch, we laid the foundation for new formation components arriving in the next patches
+ All Polarith AI inspectors are now Unity-conform, especially all foldouts and tabs
+ All enhancements and fixes allow for great example and demo creation in future patches
+ The core steering algorithms are now deterministic when the whole application infrastructure is deterministic as well, have a look at the corresponding manual page

### Enhancements

+ **[Pro]** Added AIMPerformance, a new component that replaces AIMThreading and provides further performance optimizations
+ **[Pro]** AIMPerformance: Added single-threaded load balancing which equally distributes agent updates over the available frames
+ Added AIMContextEvaluation: By inheriting from this class, you have full control over the update cycle of all AIMContext instances within the scene
+ AIMSteeringPerceiver: Implemented spatial hashing using a regular grid to improve the performance of the overall perception pipeline
+ AIMSteeringPerceiver: Implemented lazy percept receiving such that a percept is processed only if it is really relevant to at least one agent, this improves the overall performance
+ AIMSteeringFilter: Added a Range parameter that filters percepts before they are processed by a behaviour, this can improve the performance a lot
+ AIMSteeringTag: Added a Radius parameter to improve the handling of differently sized objects, so the same parameters can now be used for different object sizes
+ Added LayerBlending and LayerNormalization: All AIMSteeringBehaviour components are now able to store their intermediate results, these can then be normalized or applied to the Context using a different operation, this allows for a much more powerful behaviour combination to easily achieve boids, herds etc.
+ All AIMSteeringBehaviour and AIMReduction components can now predict their own future position based on fixed parameters or their actual velocity
+ Implemented a better self-recognition so that a percept representing the agent itself is ignored by behaviours (no need to have an inner radius for this purpose any longer)
+ Optimized the velocity tracking: When the velocity reaches the zero vector, the last valid non-zero vector is used to provide at least a valid direction
+ Instead of using a tracked or physics-related velocity, it is now possible to let the behaviour use a direction based on its rotation (most behaviours only need the direction and not the magnitude of the velocity vector)
+ Added a button to the behaviour inspectors that displays or hides more advanced options to improve the overall user experience
+ Added the two special predefined sensors LeftRightXY and DiagonalXY which might be useful for certain 2D setups but impossible to create without having AIMPlanarShaper
+ Improved the documentation content, look and feel

### Fixes

+ **[Pro]** AIMFollowPath and AIMFollowWaypoints: Fixed a bug which prevented the Points property from setting data when there was no AIMPathConnector (thanks Joe)
+ **[Pro]** AIMPlanarShaper: Fixed a problem that occurred because of API changes in Unity 5.6
+ **[Pro]** PlanarSeekBounds: Fixed a bug that led to wrong results when using a significance value other than 1
+ AIMContext: Fixed an error that occurred sporadically within the inspector
+ AIMContext: Fixed a bug which led to an annoying flickering of the indicator gizmo
+ AIMAvoid: Fixed the velocity warning that was shown even when there was a valid velocity source
+ AIMAvoid and MoveBehaviour: Fixed the method MapBySensitivityPlane() which had problems with any other transform scale than (1, 1, 1)


[v1.4.3] Stabilization Fix
--------------------------

### Enhancements

+ AIMStabilization: Improved the default value of the parameter MaxIncrease

### Fixes

+ AIMStabilization: Fixed a bug that caused Stabilization to not work correctly (thanks Anthony for pointing this out)


[v1.4.2] Pathfinder Fixes
-------------------------

### Enhancements

+ Optimized component inspectors for Unity's dark skin
+ Documentation: Improved spacing of headlines and diagrams for a better readability

### Fixes

+ **[Pro]** AIMLinearPath: Fixed a bug which made it impossible to edit path points interactively in-scene
+ **[Pro]** AIMFollowWaypoints: This behaviour is now properly marked as thread-safe
+ **[Pro]** Fixed documentation links for all path-related components
+ **[Pro]** Fixed menu entries for all path-related components
+ AIMContext: Fixed a bug which caused the indicator gizmo to flicker sometimes while an agent is selected


[v1.4.1] Pathfinder Improvements
--------------------------------

### Enhancements

+ **[Pro]** AIMFollowWaypoints: Added a property called Patrol for setting up what to do when the end of a path has been reached
+ **[Pro]** AIMLinearPath: Improved the selection mechanism of path points so that other game objects are still selectable while a path object is selected
+ **[Pro]** AIMLinearPath: Added a new selection box shortcut (Ctrl + LMB + Drag) for selecting path points
+ Minor inspector improvements concerning foldouts

### Fixes

+ **[Pro]** Fixed AIMPlanarShaper so that it now works correctly for all supported Unity versions
+ Updated the offline documentation to v1.4


[v1.4] Pathfinder
-----------------

### Changes

+ **[Pro]** Added AIMFollowWaypoints behaviour to follow a path or an arbitrary collection of points
+ **[Pro]** Added AIMLinearPath for quickly creating your own paths or patrols
+ **[Pro]** Added AIMUnityPathfinding, an adapter for Unity's NavMesh system so that AIMFollowWaypoints can utilize this information
+ **[Pro]** Extended overall API such that the AIMFollowWaypoints behaviour can be easily used with your custom Pathfinding solution

### Enhancements

+ Improved custom editors, they now follow Unity's standard with respect to multi-selection and prefab emphasizing

### Fixes

+ Fixed a bug which broke the multi-selection capabilities of custom editors (if you implemented your own editor based on AIMBehaviourEditor, you need to override the BehaviourProperty)
+ Fixed a bug where Unity crashed on calling Polarith AI API methods in a specific order (AIMContext.ResizeObjectives, thanks Simon for pointing this out)
+ Fixed a bug in AIMContext which caused the indicator gizmo to flicker based on the set UpdateFrequency
+ Fixed the PlaneGizmo such that it is consistent with our other gizmos


[v1.3] Free versus Pro
----------------------

### Changes  

+ Polarith AI is now Polarith AI Pro, furthermore, we now provide a Free version in the store as well
+ Adapted the documentation for the new Free and Pro versions
+ Added source code of our controller components
+ **[Pro]** Every Pro component now have a Pro label icon

### Enhancements

+ **[Pro]** AIMSeekNavMesh, AIMFleeNavMesh: Improved editors, they now show a proper bitfield for AreaMask instead of just an integer

### Fixes  

+ Fixed a bug in our custom editor system which made proper multi-object editing impossible
+ Fixed a bug in character controller editors which caused the value of ObjectiveAsSpeed to be reset at runtime
+ AIMPhysicsController2D: Removed the unnecessary dependency to Rigidbody2D so that it can be decoupled properly from the actual physics object


[v1.2.5] Asynchronous Load Balancing
------------------------------------

### Changes

+ AIMContext: The Spacing in the indicator settings is now constraint to a minimum of 1

### Enhancements

+ AIMContext: Improved UI look and feel
+ AIMPlanarShaper: Improved UI look and feel
+ AIMThreading: Advanced asynchronous load balancing
+ AIMFollow, AIMArrive, AIMOrbit, AIMAlign: Added tags for assigning game objects
+ Documentation: Added syntax highlighting support for code snippets
+ Examples: Added scene for demonstrating the functionality of AIMLodGroup

### Fixes

+ AIMContext: Fixed a bug in the wizard where the default perceiver was not correctly detected
+ AIMContext: The Custom Scale in the indicator visualization of the result direction is now considered correctly
+ AIMSeekNavMesh: Minor UI improvements
+ Documentation: Minor corrections (links etc.)


[v1.2] Usability Boost
----------------------

### Changes

+ AIMStabilization: The last decision is used as reference direction instead of the last direction the agent faced

### Enhancements

+ Brand-new and extensible inspector interface which boosts up the general workflow
+ Components now support correct help URLs which reference to the online documentation
+ General usability improvements making the general workflow less error-prone (parameter checks, info boxes etc.)
+ Added level-of-detail component AIMLodGroup supporting automatic sensor switching for different distances
+ Added new avoidance component AIMAvoid which is an improvement and generalization of the planar version (it supports full 3D sensors in the future)

### Fixes

+ The objective normalization has no discontinuities anymore
+ Vector projection of OBBs was not working correctly for certain cases, but now it does
+ The bounds of a percept are now correctly extracted (before, they were in wrong space for certain cases)
+ AIMArrive: Gizmos in 3D were not visualized correctly, but now they are
+ AIMSimpleController: Fixed an error which could occur by setting an ObjectiveAsSpeed


[v1.1.2] The Fix of Shame
-------------------------

### Changes

+ Removed everything concerning the path structure until it is polished (we found some serious bugs to be present and decided to re-publish it later, a big SORRY)

### Fixes

+ Repaired gizmo functionality


[v1.1.1] Critical Hotfix
------------------------

### Fixes

+ Fixed critical bug preventing the build of full application executables


[v1.1] Path Structure
---------------------

### Changes

+ Polarith AI now requires at least Unity 5.3.0

### Enhancements

+ Path Structure: Added component AIMLinearPath for easily creating and modifying linear paths
+ Behaviour: Added AIMFollowPathDiscrete and AIMFollowPathContinuous for letting agents follow or patrolling on paths, e.g. an AIMLinearPath
+ Colors: Added static class for collecting all default colors used by Polarith

### Fixes

+ AIMSteeringFilter: Set the default value of ObjectTag to "Untagged" for preventing broken setups
+ AIMSteeringFilter: Fixed an issue where an exception was thrown if an AIMSteeringPerceiver was not attached
+ AIMPursue: Fixed Typo in AIMPursue (in Unity's component menu, it was named AIMPursu)


[v1.0.1] Release Bug Fixing
---------------------------

### Enhancements

+ AIMEnvironment can now use Unity's layer system to define perceived objects.
+ It is now possible to change the sensor's planar orientation in AIMPlanarShaper.
+ AIMContext indicator now considers the object scale.
+ Eliminated a remaining GetComponent call in the main update loop which caused 500 KByte of GC in editor mode.
+ Added a hierarchical distance check for the boundary behaviours to improve both performance and precision. Agents now use a fast approximation to check whether an object is relevant, and if it is, a more expensive but precise method is applied.
+ The general performance was improved to a noticeable degree.
+ Introduced new API methods for easily accessing epsilon constraints directly via AIMContext.

### Fixes

+ The AIMPlanarInterpolator did not correctly consider if a target objective is to be minimized or maximized.
+ Using static game objects led to major problems when using behaviours which need visual bounds information.
+ Spread parameter in AIMPlanarSeekBounds and AIMPlanarFleeBounds did not work properly.
+ Eliminated an exception caused by the AIMContext indicator in the editor when using ShowReceptors.
+ Removed obsolete parameter (DistanceMapping) in AIMOrbit.
+ Fixed a bug in AIMContext leading to an unhandled exception while the minus button was pushed and there were no objectives.
