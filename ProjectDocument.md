# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**


**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

## Producer

### **Conar**:
As the producer, I tried to keep a very active role throughout the project, giving feedback and making suggestions between work flow and finally bringing the game together towards the end of our development cycle. Below are the actions that I took as a producer to maintain our work flow and to produce the last version of our project.

#### **Work Organization:**

To start with, I first created a github repo to keep track of all our progress. After some discussion with the team, we came to the solution of using a merging system that had individuals create branches with their own changes and to later merge those changes to Main. All new branches are always created from Main in order to ensure that what you are working on had everything that you could need from the finished work within main. Later down the road, this became less common as we began to only make changes around bugs, but in general we kept to this structure of merging and branching.

I also created a discord server ([link to discord to see messages](https://discord.gg/TJtYEVg4)) for our team to interact with. I tried to separate our dicussions to occur in different threads based on the material we were going over. For example, I created a game-assets message chat so that we could keep all of our discussions about game assets in one place. This was the same for the issues, meeting-notes, and jobs-done threads. General was often used for most of our communications though because it was convienent to use and often we were discussing with each other about design decions that didn't fit a specific category.

I also maintained a weekly meeting schedule with our group. These meetings were all decided by word of mouth and via discord annoucements. We met initially every Monday and Wednesday, but this later changed to Mondays and Thursdays. We often talked about design choices and updates on what people were doing. I found doing meetings like this gives motivation to keep people working and provides a space for teams to discuss issues that individuals find or brainstorm implementation methods to achieve specific goals. These meetings were also essential later into development when we started to notice that the goals we had set were much more than we could handle, giving us space to decide what to keep in our final design and what to drop.

We finally had a majority of our documents located within a [google drive](https://drive.google.com/drive/folders/1reMBPnhWwYCnDz5uq0yoSfqcnvCtCAPV?usp=sharing) as well, keeping information like written reports and other documents that were too large for github. Having this space to create documents was also helpful because it allowed people to develop word docs and other files with multiple people at the same time.

#### **Picking up Missing Items**

Because of the limited amount of time, I decided to implement the Prerun scene within the game and the Runwin scene within our game. The Prerun scene was created using the [Exterior tileset within the Lucifer asset pack](https://itch.io/queue/c/1557879/lucifer?game_id=1020813) Within the Prerun scene, I implemented a way for the player to switch between classes using the [SwitchSorceress.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/SwitchSorceress.cs) and the [SwitchWarrior.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/SwitchWarrior.cs) scripts. These scripts were assigned to a visible warrior sprite and sorceresss sprite within the scene. This allowed the player to switch classes within the prerun area. I also added the shop keeper using the [Cultist sprites from the Lucifer sprite pack](https://itch.io/queue/c/1557879/lucifer?game_id=1021185). Later Steven created the shop and the interactions within it and Rustle later made fixes within the Prerun area in terms of tile maps and text.

For the Runwin scene, I decided to create a scene that would reward the player with more money once they passed through the Run scene without dying. This scene would then transition back to the Prerun scene for the player to explore the Run scene again. I used the gems, coins, and gold prefabs created by Seth and the tile maps of the dungeon that Russell pushed to create the dungeon.

#### **Small changes to UI**

To help out Gabriel, I implemented the money counter within the PlayerCanvas prefab. I also created shop icons using the shoe, sword, and health potion within [Kyrise's 16x16 RPG Icon Pack](https://kyrise.itch.io/kyrises-free-16x16-rpg-icon-pack).

![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Resources/Sprites/IconSprites/SpeedUp.png) ![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Resources/Sprites/IconSprites/attackUp.png) ![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Resources/Sprites/IconSprites/HealthUp.png)

#### **Putting the Game Scenes Together:**

Towards the end of development, I was the one that joined all the scenes together to make the final game. I added scene loading and scripts like [SuccessTransition.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/SuccessTransition.cs) and [PrerunTransition.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/PrerunTranistion.cs) to control the movement of scenes. The scene transition tree looks like the following:

![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/ConarImages/SceneTransitionDiagram.svg)

I also tried to make an attempt to keep the player state between games. I first tried to make the player object a NoDestroy object, but found that this method interfered with Russell's and Brad's implmentation of the Run scene. I next tried to implement [scriptable objects](https://docs.unity3d.com/2022.3/Documentation/Manual/class-ScriptableObject.html) that would keep the game state between scenes. This seemed to work until Russell noticed a bug where transitioning from scenes without the player would reset the scriptable object back to its default values. Russell was the last to implement a C# object that was treated as a resource and didn't become destroyed until the game was closed.

### **Brad**:
I want to start by saying that my work in this role and has been very challenging but some of the most rewarding work that I have done in my time here at Davis. Working with this group of people has been an incredible experience. This is the first group project that I have worked on here at Davis that I feel has been truly collaborative. We worked well together, gave feedback where needed and listned to others what had to say. Everyone worked hard and made major contributions to the game. So thank you for the positive experience before I graduate.

#### Role
I was very active in the early stages of this role. I spent a lot of time designing the game. I drew paper prototypes for the core game loop, which was quite distracting as I did much of this during lectures. Early concept for the game was a simple roguelite. You start in a dungeon, you move from room to room battling enemies. When you enter a room, you cannot leave it until all enemies are dead. Once all enemies are defeated, an item or meta currency drops and you move onto the next room. If you die, you go to what we call the pre-run area, lose all items and tempory stat buffs you received during the run, but keep the meta currency. Meta currency can be used to permanately buff the player character. This was the core design that our team began working with. Some elements are very different due to time or asset limitations but overall we stuck very closely to this core design loop and I am very happy with how it turned out.

After initial prototyping and planning the core game loop, we started discussing assets. We began early searching for free assets that we could use for our game. Most of our early meetings were spent discussing types of assets we were looking for, and searching for them. We put a lot of focus on finding assets first because we none in the group are designers. We needed good assets to design the mechanics of our game around. I think this strategy really payed off. We found, or more clearly Russel, an asset pack on itch.io by FoozleCC called [Lucifer](https://itch.io/c/1557879/lucifer "Lucifer Asset Collection") where we got most of our visual assets from. With what assets we planned on using, we divided work amongst ourselves and got to work creating our game.

From here the amount of time I spent in this role dwindled. Connor spend a good amount time and effort organizing our efforts and leading biweekly meatings where everyone could check in and update on their status. During these meetings I very much played an active role in discussing design decisions, but once the creative juices started flowing, I took a step back and focussed on my other role.

Below are some early prototypes I drew and wrote up for my initial design of the game. I think we were pretty faithful to this initial design plan, though the initial plan was a bit ambitious. We had to forgo bosses and environmental hazards, as well many other enemy types that we wanted to implement.

![Early Prototype of Lucifer's Trilas](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/EarlyPrototype.JPG)

![Early Prototype of Lucifer's Trials](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/EarlyPrototypeItems-1.JPG)

![Early Prototype of Enemies](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/EarlyPrototypeEnemies.JPG)

![Early Prototype of Player](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/EarlyPrototypeCharacter.JPG)

## Player Design

### **Conar:**

#### Definition of Role:

This role focuses on developing the playable character that is present in our game. The person in charge of this role will be designing the actions that the player will make, along with the special features that the player might have while playing the game. The person with this role will need to interact with every member of the team to produce the player.

#### Work Done in Role:

##### **Initial player design:**

When thinking about the design of our game during the initial document phase, our group decided that we would design a playable character that would change based on the class assigned to that character. Each class would have an unique set of base stats, as well as contain unique fighting styles that the players could use. Our initial design incorporated a range and melee class, each with two different attacks, one that was the default attack type that the players could used whenever and secondary attack that could only be activated if the player had enough special points (SP). Because of time constraints, our group decided to drop the inclusion of the secondary attack and the SP.

Because of time constraints early in project development, I didn't have much chance to work on the player design within coding implementation, but did help our team by providing instructions on the feel and look of our playable characters. Using the [Lucifer sprite pack](https://itch.io/c/1557879/lucifer), I decided that our team would use both the Warrior and the Sorceress as our playable classes. The Warrior I decided to design as a melee type that players could get up close to enemies. The Warrior was imagined to have a higher amount of health, slow movement, but high attack. This is shown through the base stats that the warrior contains below:

![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/ConarImages/WarriorStats.png)

![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/ConarImages/Warrior%20Showreel.gif)

We used the assets attack one sprites for the warrior's primary attack sprites.

Our second playable class I decided on was the Sorceress class. This class was designed to be both faster and ranged, but had less base health and attack over the warrior. Players could use this class to evade enemies and position themselves in ideal areas to do attacks that could give them enough room to remaneuver if need be. Here is the base stats that we decided the sorceress should have:

![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/ConarImages/SorceressStats.png)
![](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/ConarImages/Sorceress%20Animation%20Showreel.gif)

We used the assets attack one sprites for the sorceress' primary attack sprites.

##### **Creating Modularity within the Player Scripts:**

Because of the large amount of work that was needed by this assignment, I decided to separate the existing player scripts into more specific scripts that dealt with their own part of the player logic. Originally we had only a PlayerController.cs and PlayerAttackController.cs script. However, I decided to break those two scripts into [PlayerController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerController.cs) which dealt with player stats, [PlayerAnimationController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAnimationController.cs) which dealt with animations, [PlayerAttackController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAttackController.cs) which dealt with projectile attacking, and [PlayerMovementController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerMovement.cs) which dealt with player movement. This occurred within [this merge](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/0e73ba025059aac65c37bae1d14e5579d730c2ae).

##### **Later Implementation of Player Logic/Design:**

After Seth and Steven had implemented the majority of the player scripts related to movement, animation, and physics, I moved to adding the final details to the player. I initially started by making changing the Health indicator for the player within the UI to the health slider that we have now. I followed [this tutorial](https://www.youtube.com/watch?v=v1UGTTeQzbo). I used this setup to do both the enemy and the player health bars, but had to modify the scripts slightly between both the enemy implementation and the players because the slight changes needed to do that. All the scripts and changes to the Unity project to do this occurred within [this commit](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/dc78c5376f69820c196222307b41e289b2cf8ab4). This would later result in the [HealthbarController.cs script](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Enemy/HealthBarController.cs), the Health slider within the Player canvas prefab and the enemy healthbar prefab.

These were some of the major contributions that I made towards this role. I may have skipped over some other implementations that I did, but these were the main ones that impacted my role.

## Procedural Generation

### **Russell**:
The main scene of the game, in which you fights monsters as you navigate around a castle, is entirely procedurally generated. This includes both the individual rooms themselves, as well as the layout of the entire level (e.g. how the rooms are connected). The implementation of the procedural generation involves extensive use of the Factory Design pattern, and is an entirely custom algorithm that is not based on any widely used procedural level generation algorithm, such as Perlin Noise or Wave Function Collapse. This was chosen because those algorithms tend to be better at creating environments that looks *natural*, but I wanted levels that looked like they were designed and built by intelligent beings.

But before we get to the actual procedural level generation algorithm, we need to introduce the [Room](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/Room.cs) class. The `Room` class is the class which contains all of the components of a room in a level, such as references to the actual Unity GameObjects for the room and its tilemaps, lists of enemy GameObjects which are spawned in the room, and public functions for revealing pathways in the room once the player defeats all of the enemies within it. The `Room` is the basic building block of a level.

Now, the first, and arguably most important, part of the procedural level generation is the [DungeonGenerator](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/DungeonGenerator.cs), which is itself an implementation of the [IRoomGenerator](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/IRoomGenerator.cs) interface. This is the Factory that has a `Generate()` method which returns an instance of the `Room` class. The basic idea of this algorithm is that rooms start out as hollow rectangles, and [additional rectangles are added to the interior of the room](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/DungeonGenerator.cs#L426C4-L516) in random positions and with random sizes. The number of, and maximum size of, these interior rectangles are parameters of any `IRoomGenerator`, along with the dimensions of the room, and are set at instantiation of the Factory. However, whenever a rectangle is added, [checks are performed](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/DungeonGenerator.cs#L756-L898) to ensure that the rectangle would not create a pathway in the room that is less than two tiles wide, and that it would not create an inaccesible section in the room. If the rectangle fails either of those conditions, it is discarded and a new rectangle is generated at a different location and with a different size. To ensure that the algorithm does not get stuck in a potentially infinite loop of generating rectangles which fail those conditions, a [contant value](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/DungeonGenerator.cs#L18) defines the maximum number of retries the algorithm gives to any particular rectangle. Once all of the rectangles are placed, the tile map [is iterated over several times](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/DungeonGenerator.cs#L900-L1177) in order to place wall tiles and border trim in the appropriate spots depending on a given tiles surroundings. Additionally, [possible locations for exit pathways from the room](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/DungeonGenerator.cs#L519-L753) are located, which themselves must adhere to several restrictions (most of which are to ensure that the revealing of those pathways will not cause any visual anomalies, such as creating walls that are too short and look awkward). Then, finally, [decorations, such as banners and torches,](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/DungeonGenerator.cs#L164-L330) are randomly thrown up on the walls of the room to give them a little more life.

Next, we have the [LevelLayoutGenerator](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/LevelLayoutGenerator.cs) which is a Factory which contains a `Generate()` method which returns a tree-structure of [LevelLayoutNodes](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/LevelLayoutGenerator.cs#L14-L89) that adheres to the rules set by the `maxDepth`, `maxRooms`, and `maxChildren` specified in the instantiation of the Factory. This class basically generates a tree that is no deeper than `maxDepth`, has no more nodes than `maxRooms`, and where each node has no more children than `maxChildren`. The "exit room" node is always placed at the maximum depth in the tree. However, within these constraints, the structure of the tree (more specifically an acyclic graph) is completely random.

And the final factory in the algorithm is the, very simple, [LevelGenerator](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/LevelGenerator.cs). The `LevelGenerator` is simply instantiated with an `IRoomGenerator` and a `LevelLayoutGenerator`, and uses the `IRoomGenerator` to generate a room at every node in a tree that is generated by the `LevelLayoutGenerator`. The rooms will be placed in the Unity hierarchy as children GameObjects of their respective parents, and a reference to the `Room` instance at the root of the tree will be returned by the LevelGenerators `Generate()` function.

To allow for reference to the `Room` that the player is currently in, a [LevelManager](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/LevelManager.cs) component is attached to the root room GameObject. The `LevelManager` keeps track of the [current node](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/LevelManager.cs#L7) in the tree that the player resides in, and is updated by [ExitManagers](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/ExitManager.cs) that are attached as components to GameObjects that reside at every exit from a room and utilize colliders to know when the player is attemping to leave a room. As the player leaves and enters rooms, [GameObjects which contitute rooms are enabled and disabled as you'd expect](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/Room.cs#L232-L270).

[Enemies are spawned in rooms quite simply](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/Room.cs#L127-L164), by just iterating over the room to identify all of the tiles which are not occupied by a wall, and by removing any such tiles which [are too close to the opening](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Procedural%20Generation/Room.cs#L33) in order to avoid the player being instantly ambushed when they enter a room. Then, tiles are just randomly sampled, without replacement, from the list of valid tiles and enemies are spawned at the locations of those tiles.


## Enemy AI
### **Brad:**
#### Role Brief
This role focuses on developing the enemies that are going to be present in our game. The person in charge of this role will be designing the AI of the Enemies, the actions that the enemies can make, and the states that the enemy goes through during its life cycle. This also includes any bosses that are present within our game.

#### Research
Immediatly after taking on this role I started researching Enemy AI, or more accurately AI movement. We hadn't had the lecture on this topic in class yet, so I was on my own. Additionally, I have not taken ECS 170. Looking up different enemey movement algorithms, the first thing I stumbled upon was A*. After spending a little time learning about A*, I decided that I did not want to use A*. In my head, the combat area that enemies would be moving through would be open, with walls only around the edge of the battle area and A* seemed unecessary for an open area. 
In the those early days, we planned for environmental obsticals in the combat area. Ideally, I wanted an enemy that didn't just efficiently move to the enemy, but responded to the threats of the envrinoment. I happened to stumble upon this video:

 [![The Trick I Used to Make Combat Fun](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/HowToMakeCombatFunVideoScreenCap.JPG)](https://www.youtube.com/watch?v=6BrZryMz-ac "How to Make Combat Fun")
* * *
#### Context Steering
The video above, by Game Endeavor, introduced me to the idea of context steering as well as linked an article titled [Context Steering Behavior-Driven Steering at the Macro Scale](http://www.gameaipro.com/GameAIPro2/GameAIPro2_Chapter18_Context_Steering_Behavior-Driven_Steering_at_the_Macro_Scale.pdf) written by Andrew Fray. This article introduces the problem with some popular steering behaviors and introduces Context Steering. "The context steering framework deals in the currency of context maps...Internally, the context map is an array of scalar values, with each slot of the array representing a possible heading, and the contents of the slot representing how strongly the behavior feels about this heading."

![Context Steering](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/ContextSteering.JPG)

Multiple behaviors can be attritubed to a different context map which effect the values of each possible heading differently. Then different headings from the multiple behaviors can be averaged together. The heading with the highest value is the heading that the algorithm wants to go. The paper sets up different behaviors that could be implemented and argues that using this system, the AI can make an informed decision about where it wants to go with context from the environment without using and overbloated decision tree.

Needless to say, I was sold on this system. Early into this journey, I did attempt to implement my system for this, but I did not have the time to fully commit myself to this task. This is when I stumbled on [Polarith AI](https://assetstore.unity.com/packages/tools/ai/polarith-ai-free-movement-with-2d-sensors-92029), both my personal savior and the bane of my existance.

* * *
#### Polarith AI
On the unity page, Polarith AI advartises itself as, "Polarith AI offers all you need for achieving state-of-the-art movement AI with just a few clicks. Design complex movement behaviours and create immersive games or astonishing simulations by taking advantage of the sophisticated workflow." This is a context steering AI package that was a bit of a pain to work with. The core concept of their design is that they have many different pre-defined behaviors as components. You can attach these behaviors to gameObject, set different parameters of that behavior and system will give a direction that it wants to move based on the different behaviors attached. On these behaviours, you can assign individual gameObjects as objects of interest or a LayerMask, where everything on that layer is of interest. Each component has useful visual debugging gizmos to see how it responds to various inputs in the world.

![Debugging Gizmos showing Polarith sensors around enemies.](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/image.png)

![Alt text](<https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/Lucifer's%20Trials%20-%20Run%20-%20Windows%2C%20Mac%2C%20Linux%20-%20Unity%202022.2.13%20_DX11_%202023-06-13%2023-43-16(1).gif>)

The gif above shows the enemies moving around with their sensors visible. The yellow line shooting out from the enemies is the direction that the system has chosen to move. Green lines are directions that the enemies wish to move towards while red lines are directions the enemies want to avoid.  Ignore, the purple lines for now. Each behavior has a radius you can set for the behavior as well as a weight to that behavior and different options for what is called Radius Mapping. Radius Mapping uses different functions that change the weight based on distant to the target. Constant for example does not change the weight regardless of distance, whereas Inverse Quadratic drasticly increases the weight the closer you are to the player. You can use these to make some cool and behaviors. For example, I used Inverse Quadratic and a high weight for walls, because I did not want the enemy to every leave the combat arena or get trapped in a corner. Whereas, I made seeking the player constant because as long as the enemy had a clear path to the player, it should try to move to them.

Early tests with this system yielded positive results and after committing a frustrating amount of time, I realized a huge limitation to this package. Up until this point, all the implementations of Context Steering that I had learned about use Rays cast out from the enemy, which sense various things in the environmnet. Polarith does not use rays, but tracks the position of all objects of interest in the scene. This became frustratingly problematic. The enemies needed to avoid the walls surrounding the combat and arena, but the combat arena used a tilemap with collision. The position of the tilemap was (0,0). So instead of avoiding the edges of the play area, the AI would avoid the center of the map. Our solution to this was to position an empty gameObject to each tile on the map, and make those gameObjects items to avoid. This worked ok. With this approach, the enemy had trouble navigating around and getting stuck on corners. This, I believe, is do to them sensing the gameObject centered on the tile rather than the wall itself. So when going around corners, you can see they hug the corners cause they believe that there is no wall there.

In total, I used 11 different behavior components on each enemy. I turn different behaviors on and off dependent on the state of the enemy. Setting up these different behaviors in tandom with getting class script going was probably 2 weeks worth of effort. Below I have linked the various prefabs and scripts I wrote for enemey behavior:

This Prefab holds all the steering behaviors attached to the enemies.
https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Resources/Prefabs/ContextSteering.prefab

This Prefab is placed in the scene, and is used to assist the package in working with Unity Layer system, as well as tracking objects of interest added at runtime.
https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Resources/Prefabs/Steering%20Perceiver.prefab

This Script is the bulk of the code I wrote. It inherits from an abstract enemy class which inherits from an interface. The interface I used to implement the factory pattern for creating enemies.
https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Enemy/Goblin%20Beserker/GoblinBeserker.cs

This script is the factory for producing Goblin Beserkers, the melee enemy. Their is an identical factory for producing Goblin Slingers. Have two different factories with the same script cause it was easy to identify which factory produced which enemy based off the name of the factory. Both enemy types use the same script to control their behavior.
https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Factory/FactoryGoblinBeserker.cs

#### Decision Tree
Once I got all the behavior components worked out I moved onto implementing the descision script. This was relatively simple, I drew up a decision tree and implemented the tree using nesting if statements. The nest goes pretty deep but it the logic is simple and easy to follow.

![Enemy Decision Tree](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/20230614_002312.jpg)

https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/efdbc6babd30c1977519a158b6ddb377e5998599/Lucifer's%20Trials/Assets/Scripts/Enemy/Goblin%20Beserker/GoblinBeserker.cs#L341-L412

Below are a list of the enemy states and their descriptions:

**Patrol** - When the player is out of the enemies range of vision or not within line of sight, with some exception, the enemy will wander around aimlessly.
**Search** - If the player was within line of sight and range of vision but line of sight was lost, the enemy will move to last known location. Upon arriving if the player is not within line of sight, enter patrol.
**Move** - Moves directly to player, unless environmental obsticals are in the way.
**Orbit** - Orbit occurs when the enemies attack is on cooldown. When in orbit range, the enemy will move to cirlce the player out of range of its attacks.
**Attack** - Triggers attack animation and such. 
**Flee** - Runs away from player when attack is on cooldown.

Each state causes various behaviors to be enabled or disabled.

#### Corners and Walls
So I said earlier that I did not envision there being walls existing in the middle of the room. What I envisioned was wrong. There were two major problems with my implementation:

**Walls:**
When a player would be directly behind a wall, the AI would run straight at the wall, see the wall, turn, but then notice they weren't moving towards the player, turn back to the wall, and so on. This was problematic because it made the game feel badly designed in a way. To fix this, I implemented the SEARCH state. Here is how this state functions: When the player leaves line of sight behind a wall, the enemy would leave an empty gameObject at the last known location of the player. It would then replace the player as an object of interest with the empty gameObject and seek that gameObject. Once getting close to the object, if the player is in line of sight, it continues seeking the player, else it enters patrol. Doing this introduced the concept of line of sight, so enemies wouldn't run straight at a wall anymore. This did however bring to light a new problem to light...

**Corners:**
As I mentioned earlier, enemies have a tendency to get stuck on corners because of the way we used empty gameObjects to represent walls. The search state amplified this problem. Every time the player would disapear behind a wall, search would send that enemy almost directly at the corner seeking its last known location. A problem in a game whith lots of corners. Though my solution wasn't quite the homerun I imagined it would be, I am quite proud of its implementation.

My solution uses the relationship between 2 vectors. One between the enemy and player (EP), and the other, between the enemy and empty game object (EO). Knowing these two vectors, I could project (EP) onto the line (EO). This projection vector allows me to find a vector that is orthoginal to line (EO) to the player. By negating this vector, I could get a direction to push the gameObject away from the corner. This usually worked, unless the player moved below the enemy, then I think the gameObject would move in the wrong direction.  To fix this I added a timeout for search. If the player was not in line of sight after so much time, the enemy would start to patrol. 

In the image below, the two purple lines are vectors to the player and empty game object. The white line extending beyond the purple line is the projected vector and the white line orthogonal to the white vector is the vector I use to move the empty game object.
![Alt text](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/f20537e0a4e5d5c501c1c84ba9aee0a4fb90bff5/Brad%20Contribution/image-1.png)

This system wasn't perfect, and I am not 100% sure it works as inteaded, but it was a fun and challenging problem to work through. The challenge being that there were 4 different corners with 8 different ways the player could be around a corner in relation to the enemy. So the challenging part was finding a solution that did not rely on me determining what position corner we needed to round.

#### Problems Still
As I have mentioned already is that corners are a problem. Another problem though is that animation is tied to movement. So if there a lot of danger objects nearby, and the AI starts to bounce between them, the direction the enemey sprite faces jitters back and forth. There are simple remidies, like making the enemy face the player, but the enemy isn't always running at the player. Sometimes they are patrolling.

## User Interface

### **Gabriel**:
The first thing I worked on was our [*basic Health UI and Player Health logic*](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/pull/1/commits/f6cc07bd75ee0240c0b03ea9b98741923844212d) and our [SP gauge and pickups](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/pull/9/commits/2124ede148abe5cac5cf76317390345ae58ce1c7). The health later got put into a combined script with the players' stats and we had to scrap our SP and special moves idea due to time, so they are no longer in the main code. I also made the [*Main Menu*](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/pull/20/commits/0a28bf8d3c38e591699c73437c429a926081b3fc), [*Death Screen*](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/pull/22/commits/77b9cbf9a165519e2fff8632cb5730984df5ff60), and [*Pause Menu*](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/pull/28/commits/f6425f45a00762792696b23a8cb5371d5b6a18e1). These additions were all complete with working buttons that either change the scene or use a script to add or remove components in the Canvas. 

I used the following tutorials for my User Interface contributions:
- [Basic UI](https://www.youtube.com/watch?v=_RIsfVOqTaE)
- [Basic start menu](https://www.youtube.com/watch?v=_RIsfVOqTaE)
- [Basic pause menu](https://www.youtube.com/watch?v=JivuXdrIHK0)

## Movement/Physics

### **Steven:**
#### Early Player Movement ####
During the early stages of development, I was involved with setting the groundwork for player movement. From the very start, we knew we wanted to utilize the WASD
keys to allow the player to move up, down, left, and right in our 2D top-down world. This was captured through the use of Input.GetAxisRaw("Horizontal") and
Input.GetAxisRaw("Vertical") to obtain both the horizontal and vertical inputs of the player. It was crucial for us to normalize the 2D vector formed by these
inputs, otherwise the speed of the player would effectively double if they were to move diagonally. Furthermore, we wanted to incorporate a dash into our game as a mechanism to evade enemies and quickly move from one location to another. I designated the spacebar key to allow the player to dash, which works similarly to how the player typically moves. To perform a dash using spacebar, the player must be initially moving in a certain direction. Upon clicking the spacebar, the speed of the player is greatly increased and then reverted back to its original state to simulate a dashing movement in a given direction. I also added a cooldown timer that would allow the user to dash in certain intervals. This design choice was to establish game balance in player movement, as we noticed that being able to constantly dash with no cooldown is rather overpowered. 

## Game Logic

### **Steven:**
#### Shop + Shop Logic ####
A core gameplay mechanic of our game is the ability to purchase upgrades before dungeon runs to strengthen the player before their quest. As such, I implemented the shop logic to handle transactions made by the player when purchasing items as well as the shop GUI the player interacts with. The main shop HUD consists of a
bulletin board with three upgrades to choose from. This includes an attack upgrade which raises the player's attack, a health upgrade which increases the player's 
max health, and a speed upgrade which increases the player's speed. By purchasing any of these upgrades, the corresponding player stat increases by 1 and the
player's wallet is updated accordingly. When the player performs a transaction, text is displayed to indicate a valid purchase or an invalid purchase when the
player has insufficient funds. Along with the main shop HUD is a stats HUD at the bottom left of the screen to indicate the player's current stats as well as a 
wallet HUD at the bottom right of the screen to display how much money the player has. Finally, the shop has an exit button at the top right of the screen which
allows the player to leave as they so choose. To open the shop itself, the player must walk within the vicinity of the merchant in the prerun area of the game and
press the 'E' key. Upon doing so, the shop will open and the game is effectively paused as the player browses the shop, preventing any inputs from registering.

The assets associated with the shop GUI come from https://mounirtohami.itch.io/pixel-art-gui-elements, with all credits going to Mounir Tohami for creating them. 
As for the icons involved in the shop, the sword, health potion, boot, and coin icons come from https://kyrise.itch.io/kyrises-free-16x16-rpg-icon-pack and the
heart icon comes from https://gamedevshlok.itch.io/heartpack.

#### Enemy Drop Logic ####
Another core gameplay mechanic we have for our game is the chance for enemies to drop items when they have been defeated. I was in charge of establishing a
system for enemy drops and coming up with appropriate drop rates to balance our game. In our current game, enemies can drop either currency or health potions
for healing. I designed it so that an enemy has a flat out 60% chance of dropping an item. If they do end up dropping an item, there are varying drop rates for
the available item drops. Because coins are the least valuable of the currencies, they have a drop rate of 50%. Gold comes after coins and is more valuable, so 
their drop rate was set to 30%. Finally, with gems being the most valuable of the currencies, their drop rate was set to 10%. Adding up the drop rates for the three types of currencies, we get 90%, and so the remaining 10% is the drop rate for health potions. With this setup, it is most often the case that players will 
encounter currency to pick up as opposed to health potions for healing. This is an intentional design choice as we wanted to add challenge to our game so that 
players have to be mindful of their positioning and play carefully to avoid losing health, especially when healing is scarce during a run. That way, if a player 
ends up dying mid run, they can at least keep half the gold they earned during the run to buy upgrades to better prepare themselves for their next run. 

## Animation and Visuals

### **Seth:**

I greatly enjoyed learning the basics of animation in Unity. This was my first time programming animations at a large scale, so I am very thankful that there is such a large community of Unity developers on the internet. Particularly, YouTube was a great help here. Having delved a bit deeper on our class' Exercise 1, I noticed the basics of how Unity handles animation with its Animator component and the Finite State Machine one creates to handle where and when animations play. But when researching for this project, I happened upon a YouTube video that explained how to avoid the confusing web that can be a consequence of making an FSM, [by using a blend tree](https://www.youtube.com/watch?v=S3ys0jCUE9s) for one's animations. This turned out to be a blessing and a curse (more on this later).

As for the images to animate, we used many different free and publically available assets from the site itch.io:
- All Player, Enemy, Items, & Tilesets from the [Lucifer Collection](https://itch.io/c/1557879/lucifer)
- [Fireballs](https://xyezawr.itch.io/gif-free-pixel-effects-pack-6-forks-of-flame)
- [Pop Effect](https://nyknck.itch.io/fx071)
- [Combustion Effect](https://brullov.itch.io/fire-animation)
- [Dash Effect](https://nyknck.itch.io/pixel-art-effect-fx033)
- [Blood Effect](https://nyknck.itch.io/bloodfx)

Once the collection of sprites were found that matched the design style of our game, the sprites were all spliced within the Unity editor, and used to create animation clips. These clips are called upon within scripts such as [PlayerAnimationController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAnimationController.cs), [EnemyAnimation.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Enemy/EnemyAnimation.cs), and [AnimatingObject.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/AnimatingObject.cs), most of which I structured with the ideas borrowed from the blend tree idea along with other desired functionality. This additional functionality was very fun to implement; these were ideas I came up with, implemented, and got only positive reaction when showcasing them to both my project teammates and the class during our convention. Their purposes are all game feel oriented to create a dynamic, interactive, impactful world that augments one's interest and continually grabs the player's attention:
- [Blood Effect upon last hit](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAnimationController.cs#L139)
- Slowed Player death for dramatic effect (sprite placement stretched within animation clip itself)
- [Cloud of smoke upon destruction of Enemies](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/AttackableController.cs#L140)
- [Animating collectibles at slightly different speeds for a dynamic world](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/AnimatingObject.cs#L9)
- [Wind Effect upon Player dash](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAnimationController.cs#L282)
- [I-Frame blinking after hit](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAnimationController.cs#L98)
- [Dissipation of fireball for Sorceress](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAttackController.cs#L130)
- Combustion of fireball upon impact with something **
- Pop of enemy arrows upon impact with something **

** [These use the same logic](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/ProjectileScript.cs#L46)

Other important features such as tying the Player speed stat to the animator's speed were very helpful in creating visually appealing movement, as well as increasing functionality of gameplay; the Player attacks slightly faster per speed stat upgrade, as well as runs more quickly, both of which can be seen in the speed of the Player sprite during animation.

Any and all difficulties faced in the production of these animations was related to the learning curve in how Unity handled certain pieces of data. One such difficulty (the curse of the blend tree mentioned previously) is the issue of having multiple animations play at one point in time. This is the very idea of what makes a blend tree, but having not researched this enough until after beginning my implementation, I believed the motion field structure visible in the blend trees for each humanoid's animator was picking which animation to play based off of the Euclidean distance of the MoveX and MoveY parameters provided by my scripts from the coordinates of each animation queue in the field; I was under the assumption that it would only play the closest animation, and this is only partly true. For 2D animation, it appears Unity does choose the closest queue for which animation clip to make visible on the screen, but it actually loads both animations in memory, meaning any and all animation events (functions called at certain points in the animation) get played by all animation clips that Unity deemed had input close enough and would then "blend" said animation clips. Blending is clearly much more useful in 3D applications, though I was unaware of this and created an used an easy method of implementing movement at the cost of creating large difficulties in extra function calls that were annoying to debug and of which those fixes are valid but undesirable to begin with. If I could start developing this game again from scratch, I would have opted for the chaotic web of an animator FSM for the usefulness of animation events over the ease of blend trees with duct tape fixes for double function calls (unfortunately, it was too close to the deliverable date to scrap what we had by the time the bugs were discovered).

It was an extremely educational experience working on the animations for this game, not to mention very fun getting to immediately interact with one's work upon loading the game. This role was actually dropped on my lap due to the business of my quarter that I couldn't meet with the team during one of our early group discussions. But I suppose I have only good things to say for the chance to do Animation and Visuals, as I had a grand time developing part of what lets the world come to life.

# Sub-Roles

## Player & Enemy Movement

### **Seth:**
I adopted part of the development for Player & Enemy Movement at the time I realized I wanted a way to test if the animations I had produced were working properly. This included the updating and maintenence of [PlayerMovement.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerMovement.cs) as well as the implementation of [EnemyMovement.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Enemy/EnemyMovement.cs). Fortunately, much of the work on these two scripts were either a group effort or were made significantly easier by construction of other mechanics of the game. An example of this can be seen in EnemyMovement.cs where the FixedUpdate function simply accesses two pieces of data to send the enemy where they need to go, the `movementDirection` from Brad's use of Polarith AI and a `statelock` variable from my own PlayerAnimationController.cs.

The only particularly interesting concepts of the Player and Enemy movement is the dash mechanic for the player and the knock back upon being damaged. The implementation for these two ideas is simple, starting with [knock back](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Enemy/EnemyMovement.cs#L24): a reasonable vector is determined from the damaging object or a related entity to the damaged entity, towards which the damaged entity will be pushed. If the enemy is the damaged entity, the attack level of the Player is taken into account as well as whether a projectile was used (projectiles push the body less than a sword, but it's also for balance of close and long range battling). And then there is the dash mechanic: dashing is the idea that the player is dodging. It is a moment in time in which the player is invulerable to damage due to the fact that when the player is dashing, the damage function handled in PlayerAnimationController.cs (a bit of a weird spot, but this script is more like "PlayerAnimationAndInteraction.cs", but that is rather long) [simply returns](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAnimationController.cs#L122) as opposed to registering the hit, decreasing health, and playing the proper animation. The trail effect is actually just a gameobject used with an AnimatingObject component for ease. The only side effect of not using Unity's particle system or something similar is the rigid angle of the sprite you can see sometimes when dashing and turning quickly. I considered this a rather small artifact, and the other guys didn't seem to care, so we've kept it as is.

Helping with Player & Enemy Movement was enjoyable, and I appreciate that no one got worked up over the need for our subroles to expand and alter rather fluidly sometimes. This attitude is what allowed us to accomplish the amount we did in the time we had.

## Combative Interaction

### **Seth:**
Combat is a natural consequence of the style of game we planned to make. Being a roguelite, it is the core gameplay experience to try and push your way through the level, hoping to survive until the end. Brad was handling the Enemy AI, though it was around when he had finished developing the main structure for that component when I realized that we had no interaction actually implemented for the player and enemy. This is why I took up the subrole of what I call Comabtive Interaction, as we wanted to make sure there was something to do when the player and enemy finally found themselves in front of each other!

The logic for this interaction is handled within the four scripts [PlayerAttackController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAttackController.cs), [AttackableController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/AttackableController.cs), [EnemyAttackController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Enemy/EnemyAttackController.cs), and [EnemyAttackSprite.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Enemy/EnemyAttackSprite.cs). The main ideas behind how these scripts interact is through enabling and disabling "attack sprites" and checking for collision of an entity with said sprites. Attack sprites are child gameobjects of the Player and of each type of Enemy. These objects define the region in front of them in which an attack would collide with an entity standing in their path. There is an attack sprite for the four cardinal directions, and the Player and Enemy use different attack sprites that better fit the area of attack their weapons cover (Note that technically the Enemy "Slingers" never actives its attack sprites as it fires a projectile instead; I was planning to perhaps change this so that slingers would CQC if the player were close enough, but time constraints said no, and they're also already a difficult enemy to fight). Collision was handled with Collider components acting as triggers. The attack sprites are invisible game objects (sprite renderer is turned off), allowing the slash depicted in the animations themselves to feel more accurate to the damaging region, and the sprite shapes themselves were also modeled so that these slashes decently represent where one can hit or be hit by a given attack.

Handling the interaction of Player and Enemy in the combat scenario was rather fun, as it asked for A LOT of play testing (and who doesn't like playing games?). It was always fun to realize something could be bettered, implement it, and then test it to see that the gameplay was evolving into a smoother and more enjoyable experience. I am happy to have taken up this role, as I think it turned out nicely.

## Game Feel

### **Seth:**
Many things I implemented for my main role of Animation & Visuals was from the point of view of a player. Would fireballs combusting on impact look good? Are the feet hitting the ground at the proper time? Should I add another effect on top of all those other ones just because I think it would look nice? The answer either was or was made to be "yes" for all of those and more. I came to realize that I was naturally falling into the role of Game Feel by the simple fact that I wanted the game to feel great! I hadn't even spoken with anyone else about Game Feel by the time many of my additions had been implemented. So when I finally saw that this was important to me, I began working with Brad and Steven to conversate about what would go well where and when. It became a very common occurrence, and I appreciate the receptiveness of the two in allowing my voice to be heard in this domain. An example were the several times that I requested a sound from Steven (explosion for fire combustion, foot steps for humanoids, growl for enemies during attack, etc.), and he always got back to me ready to make or find something that would work. It was a joy working with the two to bring liveliness to our little world.

## Narrative

### **Seth:**
Unfortunately, we did not have the time to flesh out the game enough that a narrative felt neccesary. Hence, one does not exist! As seen above, my time was self allocated to more pressing matters at the time of development, such as more and better visuals (main role), Player & Enemy Movement, and Combat; each of these roles were inadequate at the time I had fully developed all basic animations, so I took over these roles so that the game would be reasonably finished by the expected due date. It was fun working on these subroles, as they are both logic that is immediately visible upon interacting with the world, an aspect I love about developing games.

## Audio

### **Steven:**
Because our 2D top-down game uses pixeled sprite assets, it felt most appropriate to use 8-bit sound effects and music to add immersion and liveliness to our game. The main mechanism I used for playing audio is the SoundManager that was used for the previous exercises of ECS189L. There are two music tracks used for the game and both come from https://xdeviruchi.itch.io/8-bit-fantasy-adventure-music-pack?download with credit going to xDeviruchi for creating them. The first music 
track we used was "Title Theme" which plays during our game's main menu. The second music track we used was "Mysterious Dungeon" which plays during the main
gameplay loop. As for the 8-bit sound effects, most of them were created using the handy "jsfxr" tool at https://sfxr.me/. This is a tool that allows people to
create their own 8-bit sounds, with a few buttons that randomly generate common 8-bit sounds like coin pickup and jumping. Certain sound effects were pretty easy to obtain like the item pickup, as I could continuously generate random 8-bit pickup noises until I found one that best fit our game's theme. Other sounds like
the enemy growls were less easy to come up with, as I had to experiment with the various waveforms and configurations. There were a few instances where I had to
borrow royalty-free sounds from Pixabay as I could not figure out a way to create more sophisticated sound effects for situations like a game over or clearing a
room full of enemies. These sounds from Pixabay can be found here: https://pixabay.com/sound-effects/search/victory/?pagi=4 (Titled "Victory") and https://pixabay.com/sound-effects/search/fail/ (Titled "Wrong Buzzer"). I was also in charge of queueing a majority of the sound effects and music in our game by
using the SoundManager's PlayMusicTrack() and PlaySoundEffect() to play sounds during certain conditions (eg: Playing a "Player Hurt" sound after the player gets
hit and takes damage). I also slightly adjusted the SoundManager by adding a StopCurrentTrack() function which stops a music track that is currently playing. This
was especially handy for when the player dies mid run since we wanted to stop the main gameplay music before the game transitions to the game over scene and plays
the game over sound effect. 

## Bug Fixer

### **Russell:**
Unfortunately, creating a procedural level generation algorithm ended up being a much larger endeavor than I had originally expected, and I ended up spending the vast majority of my development time on it. By the time I was happy with the state of the level generation, all of the other components of the game had already taken shape and were nearing a complete implementation. Desptite this, I was still very valuable in those final days at tracking down fixes for bugs that other members of the team did not have time to address (as they were too busy trying to get their features fully implemented).

Some of the more prominent bug fixes I pushed, outside of the realm of procedural generation, were:
- [Fixing visible gaps between tiles in our manually built treasure room](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/995743a280e142ea63ed9a9a8f74c4dd542d8824)
- [Fixing player stats being reset between scenes](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/05323609b2a4b30c851fd057fc3bf8ae6a083a81)
- [Fixing items not disappearing from the screen when the player leaves the room](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/a3c7e6a1befef68bbf380e7b13016bee57ec4a76)
- [Fixing visual errors in the "Pre-Run" area](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/0dfcdbce1c72c9d26998900f7ff64493ee3bf1a9)

### **Conar:**
This role mainly consists of finding issues within our game and trying to fix those issues. Seth, Brad, Russell, and myself often found most of the existing bugs and fixed them, even if the bugs didn't occur within our own roles. In terms of the bugs that I fixed, I was able to do the following:

- Tried fixing the player canvas sizing within the Run and Prerun scenes by having the elements be static sized objects.Here is a link to the responses:
- Fixed some collision issues when it came to double hitting the player using [multilayering collision detection](https://docs.unity3d.com/Manual/LayerBasedCollision.html).
- Fixed issues surround the enemies colliding with money and health prefabs.

## Playtesting

### **Russell:**
I also assisted in finding playtesters for our game. In total, I got three people, outside of this class, to play the game and provide feedback. Two of them filled out written questionnaires for feedback, and one even recorded a video of himself playing the game so that we could see his first impressions. Although we were only able to get feedback the night before the game project was due, their criticisms would be extremely helpful for us if we decided to continue development of this game in the future.

### **Conar:**
For user testing I only had about a day to complete this task. Because of this, I really didn't have time to record how the people were playing, and only gave about 10 minutes of playing time for each tester. I found people outside of this course to complete this task, using 2 college seniors with little gaming experience, 2 college graduates with moderate gaming experience, 1 undergraduate Junior with moderate gaming experience, and 1 experience software engineer with considerable gaming experience.

I found that the game was overall interesting to the testors, but because of the limited time they had to test, sometimes the testors didn't explore the game well enough to experience all the mechanics and options available within the game. I also found that a common issue was with th difficulty of the early game. People often felt that early game was more difficult compared to late game when you get more upgrades. This projected some issues that our team discovered with level scaling and may be an issue for us to explore in the future.

Along with that, we had a lot of comments about lacking actions that would have been more helpful in the game, like arrow deflecting or health poitions. These components do exist within our game, but it may be that these features are too rare or hard to find because of their low probability of occuring. Other comments focuses on issues that we didn't have time to explore or address like having a map of our rooms within the Run scene or having volume control within our game.

Here you can find all the responses within our github.

## Press Kit and Trailer

### **Gabriel:**
One thing I kept in mind while making the presskit was that I wanted it to be on a **website** where users can look at the game's screenshots, the trailer, and descriptions of the game all in one area. I decided to use itch.io since it met all of these requirements, while also being easy to set up. For the trailer, I used [OBS](https://obsproject.com/) to record all of the clips, and Microsoft ClipChamp to edit it all together. In the trailer, I wanted to show all of the important features of our game including combat, gold pickups, purchasing upgrades, the two classes, and I finally wanted to show that the dungeon is big and different every time. I explicitly recorded fast-paced clips that captured the difficulty and fast mechanics one must have to beat a room (at least as a knight). Similarly, I chose the screenshots for the same reason. I wanted to show off combat, the prerun area, and the shop GUI.

[Lucifer's Trials Trailer](https://www.youtube.com/watch?v=RlTm4MxdVE0) <br />
[Lucifer's Trials itch.io Link](https://luciferstrials.itch.io/lucifers-trials) <br />

## Brad:
I can't say I really had a sub-role. Of course I helped a lot with game design as mentioned above, bug fixing as well as game feel and balance, but most of my time was spent working on the enemy AI.