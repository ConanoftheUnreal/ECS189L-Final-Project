# Game Basic Information #

# Main Roles #

## Game Logic + Movement/Physics - Steven To

### Early Player Movement ###
During the early stages of development, I was involved with setting the groundwork for player movement. From the very start, we knew we wanted to utilize the WASD
keys to allow the player to move up, down, left, and right in our 2D top-down world. This was captured through the use of Input.GetAxisRaw("Horizontal") and
Input.GetAxisRaw("Vertical") to obtain both the horizontal and vertical inputs of the player. It was crucial for us to normalize the 2D vector formed by these
inputs, otherwise the speed of the player would effectively double if they were to move diagonally. Furthermore, we wanted to incorporate a dash into our game as a mechanism to evade enemies and quickly move from one location to another. I designated the spacebar key to allow the player to dash, which works similarly to how the player typically moves. To perform a dash using spacebar, the player must be initially moving in a certain direction. Upon clicking the spacebar, the speed of the player is greatly increased and then reverted back to its original state to simulate a dashing movement in a given direction. I also added a cooldown timer that would allow the user to dash in certain intervals. This design choice was to establish game balance in player movement, as we noticed that being able to constantly dash with no cooldown is rather overpowered. 

### Shop + Shop Logic ###
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

### Enemy Drop Logic ###
Another core gameplay mechanic we have for our game is the chance for enemies to drop items when they have been defeated. I was in charge of establishing a
system for enemy drops and coming up with appropriate drop rates to balance our game. In our current game, enemies can drop either currency or health potions
for healing. I designed it so that an enemy has a flat out 60% chance of dropping an item. If they do end up dropping an item, there are varying drop rates for
the available item drops. Because coins are the least valuable of the currencies, they have a drop rate of 50%. Gold comes after coins and is more valuable, so 
their drop rate was set to 30%. Finally, with gems being the most valuable of the currencies, their drop rate was set to 10%. Adding up the drop rates for the three types of currencies, we get 90%, and so the remaining 10% is the drop rate for health potions. With this setup, it is most often the case that players will 
encounter currency to pick up as opposed to health potions for healing. This is an intentional design choice as we wanted to add challenge to our game so that 
players have to be mindful of their positioning and play carefully to avoid losing health, especially when healing is scarce during a run. That way, if a player 
ends up dying mid run, they can at least keep half the gold they earned during the run to buy upgrades to better prepare themselves for their next run. 

# Sub Roles #

## Audio - Steven To ##
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
