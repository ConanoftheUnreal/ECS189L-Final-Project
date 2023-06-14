# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**


**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

## Producer (Conar)

As the producer, I tried to keep a very active role throughout the project, giving feedback and making suggestions between work flow and finally bringing the game together towards the end of our development cycle. Below are the actions that I took as a producer to maintain our work flow and to produce the last version of our project.

### **Work Organization:**

To start with, I first created a github repo to keep track of all our progress. After some discussion with the team, we came to the solution of using a merging system that had individuals create branches with their own changes and to later merge those changes to Main. All new branches are always created from Main in order to ensure that what you are working on had everything that you could need from the finished work within main. Later down the road, this became less common as we began to only make changes around bugs, but in general we kept to this structure of merging and branching.

I also created a discord server ([link to discord to see messages](https://discord.gg/TJtYEVg4)) for our team to interact with. I tried to separate our dicussions to occur in different threads based on the material we were going over. For example, I created a game-assets message chat so that we could keep all of our discussions about game assets in one place. This was the same for the issues, meeting-notes, and jobs-done threads. General was often used for most of our communications though because it was convienent to use and often we were discussing with each other about design decions that didn't fit a specific category.

I also maintained a weekly meeting schedule with our group. These meetings were all decided by word of mouth and via discord annoucements. We met initially every Monday and Wednesday, but this later changed to Mondays and Thursdays. We often talked about design choices and updates on what people were doing. I found doing meetings like this gives motivation to keep people working and provides a space for teams to discuss issues that individuals find or brainstorm implementation methods to achieve specific goals. These meetings were also essential later into development when we started to notice that the goals we had set were much more than we could handle, giving us space to decide what to keep in our final design and what to drop.

We finally had a majority of our documents located within a [google drive](https://drive.google.com/drive/folders/1reMBPnhWwYCnDz5uq0yoSfqcnvCtCAPV?usp=sharing) as well, keeping information like written reports and other documents that were too large for github. Having this space to create documents was also helpful because it allowed people to develop word docs and other files with multiple people at the same time.

### **Picking up Missing Items**

Because of the limited amount of time, I decided to implement the Prerun scene within the game and the Runwin scene within our game. The Prerun scene was created using the [Exterior tileset within the Lucifer asset pack](https://itch.io/queue/c/1557879/lucifer?game_id=1020813) Within the Prerun scene, I implemented a way for the player to switch between classes using the [SwitchSorceress.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/SwitchSorceress.cs) and the [SwitchWarrior.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/SwitchWarrior.cs) scripts. These scripts were assigned to a visible warrior sprite and sorceresss sprite within the scene. This allowed the player to switch classes within the prerun area. I also added the shop keeper using the [Cultist sprites from the Lucifer sprite pack](https://itch.io/queue/c/1557879/lucifer?game_id=1021185). Later Steven created the shop and the interactions within it and Rustle later made fixes within the Prerun area in terms of tile maps and text.

For the Runwin scene, I decided to create a scene that would reward the player with more money once they passed through the Run scene without dying. This scene would then transition back to the Prerun scene for the player to explore the Run scene again. I used the gems, coins, and gold prefabs created by Seth and the tile maps of the dungeon that Russell pushed to create the dungeon.

### **Small changes to UI**

To help out Gabriel, I implemented the money counter within the PlayerCanvas prefab. I also created shop icons using the shoe, sword, and health potion within [Kyrise's 16x16 RPG Icon Pack](https://kyrise.itch.io/kyrises-free-16x16-rpg-icon-pack).

**Images of icons**

### **Putting the Game Scenes Together:**

Towards the end of development, I was the one that joined all the scenes together to make the final game. I added scene loading and scripts like [SuccessTransition.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/SuccessTransition.cs) and [PrerunTransition.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/PrerunTranistion.cs) to control the movement of scenes. The scene transition tree looks like the following:

**Graph of image transition**

I also tried to make an attempt to keep the player state between games. I first tried to make the player object a NoDestroy object, but found that this method interfered with Russell's and Brad's implmentation of the Run scene. I next tried to implement [scriptable objects](https://docs.unity3d.com/2022.3/Documentation/Manual/class-ScriptableObject.html) that would keep the game state between scenes. This seemed to work until Russell noticed a bug where transitioning from scenes without the player would reset the scriptable object back to its default values. Russell was the last to implement a C# object that was treated as a resource and didn't become destroyed until the game was closed.

## Player Design (Conar)

### Definition of Role:

This role focuses on developing the playable character that is present in our game. The person in charge of this role will be designing the actions that the player will make, along with the special features that the player might have while playing the game. The person with this role will need to interact with every member of the team to produce the player.

### Work Done in Role:

#### **Initial player design:**

When thinking about the design of our game during the initial document phase, our group decided that we would design a playable character that would change based on the class assigned to that character. Each class would have an unique set of base stats, as well as contain unique fighting styles that the players could use. Our initial design incorporated a range and melee class, each with two different attacks, one that was the default attack type that the players could used whenever and secondary attack that could only be activated if the player had enough special points (SP). Because of time constraints, our group decided to drop the inclusion of the secondary attack and the SP.

Because of time constraints early in project development, I didn't have much chance to work on the player design within coding implementation, but did help our team by providing instructions on the feel and look of our playable characters. Using the [Lucifer sprite pack](https://itch.io/c/1557879/lucifer), I decided that our team would use both the Warrior and the Sorceress as our playable classes. The Warrior I decided to design as a melee type that players could get up close to enemies. The Warrior was imagined to have a higher amount of health, slow movement, but high attack. This is shown through the base stats that the warrior contains below:

**image of warrior stats from shop**

We used the assets attack one sprites for the warrior's primary attack sprites.

Our second playable class I decided on was the Sorceress class. This class was designed to be both faster and ranged, but had less base health and attack over the warrior. Players could use this class to evade enemies and position themselves in ideal areas to do attacks that could give them enough room to remaneuver if need be. Here is the base stats that we decided the sorceress should have:

**image of sorceress stats from shop**

We used the assets attack one sprites for the sorceress' primary attack sprites.

#### **Creating Modularity within the Player Scripts:**

Because of the large amount of work that was needed by this assignment, I decided to separate the existing player scripts into more specific scripts that dealt with their own part of the player logic. Originally we had only a PlayerController.cs and PlayerAttackController.cs script. However, I decided to break those two scripts into [PlayerController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerController.cs) which dealt with player stats, [PlayerAnimationController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAnimationController.cs) which dealt with animations, [PlayerAttackController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerAttackController.cs) which dealt with projectile attacking, and [PlayerMovementController.cs](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Player/PlayerMovement.cs) which dealt with player movement. This occurred within [this merge](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/0e73ba025059aac65c37bae1d14e5579d730c2ae).

#### **Later Implementation of Player Logic/Design:**

After Seth and Steven had implemented the majority of the player scripts related to movement, animation, and physics, I moved to adding the final details to the player. I initially started by making changing the Health indicator for the player within the UI to the health slider that we have now. I followed [this tutorial](https://www.youtube.com/watch?v=v1UGTTeQzbo). I used this setup to do both the enemy and the player health bars, but had to modify the scripts slightly between both the enemy implementation and the players because the slight changes needed to do that. All the scripts and changes to the Unity project to do this occurred within [this commit](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/commit/dc78c5376f69820c196222307b41e289b2cf8ab4). This would later result in the [HealthbarController.cs script](https://github.com/ConanoftheUnreal/ECS189L-Final-Project/blob/main/Lucifer's%20Trials/Assets/Scripts/Enemy/HealthBarController.cs), the Health slider within the Player canvas prefab and the enemy healthbar prefab.

These were some of the major contributions that I made towards this role. I may have skipped over some other implementations that I did, but these were the main ones that impacted my role.

# Sub-Roles

## Debugging (Conar)

This role mainly consists of finding issues within our game and trying to fix those issues. Seth, Brad, Russell, and myself often found most of the existing bugs and fixed them, even if the bugs didn't occur within our own roles. In terms of the bugs that I fixed, I was able to do the following:

- Tried fixing the player canvas sizing within the Run and Prerun scenes by having the elements be static sized objects.Here is a link to the responses:
- Fixed some collision issues when it came to double hitting the player using [multilayering collision detection](https://docs.unity3d.com/Manual/LayerBasedCollision.html).
- Fixed issues surround the enemies colliding with money and health prefabs.

## Gameplay Testing (Conar)

For user testing I only had about a day to complete this task. Because of this, I really didn't have time to record how the people were playing, and only gave about 10 minutes of playing time for each tester. I found people outside of this course to complete this task, using 2 college seniors with little gaming experience, 2 college graduates with moderate gaming experience, 1 undergraduate Junior with moderate gaming experience, and 1 experience software engineer with considerable gaming experience.

I found that the game was overall interesting to the testors, but because of the limited time they had to test, sometimes the testors didn't explore the game well enough to experience all the mechanics and options available within the game. I also found that a common issue was with th difficulty of the early game. People often felt that early game was more difficult compared to late game when you get more upgrades. This projected some issues that our team discovered with level scaling and may be an issue for us to explore in the future.

Along with that, we had a lot of comments about lacking actions that would have been more helpful in the game, like arrow deflecting or health poitions. These components do exist within our game, but it may be that these features are too rare or hard to find because of their low probability of occuring. Other comments focuses on issues that we didn't have time to explore or address like having a map of our rooms within the Run scene or having volume control within our game.

Here you can find all the responses within our github.