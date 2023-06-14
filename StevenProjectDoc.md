# Game Basic Information #

# Main Roles #

## Game Logic + Movement/Physics - Steven To##

### Player Movement ###
During the early stages of development, I was involved with setting the groundwork for player movement. From the very start, we knew we wanted to utilize the WASD
keys to allow the player to move up, down, left, and right in our 2D top-down world. This was captured through the use of Input.GetAxisRaw("Horizontal") and
Input.GetAxisRaw("Vertical") to capture both the horizontal and vertical inputs of the player. It was crucial for us to normalize the 2D vector formed by these
inputs, otherwise the speed of the player would effectively double if they were to move diagonally. Furthermore, we wanted to incorporate a dash into our game as a
mechanism to evade enemies and quickly move from one location to another. I designated the spacebar key to allow the player to dash, which works similarly to how the
player typically moves. To perform a dash using spacebar, the player must be initially moving in a certain direction. Upon clicking the spacebar, the speed of the
player is greatly increased and then reverted back to its original state to simulate a dashing movement in a given direction. I also added a cooldown timer that
would allow the user to dash in certain intervals to establish game balance in player movement, as we noticed that being able to constantly dash with no cooldown is
rather overpowered. 

### Shop + Shop Logic ###
A core gameplay mechanic of our game is the ability to purchase upgrades before dungeon runs to strengthen the player before their quest. As such, I implemented the
shop logic to handle transactions made by the player when purchasing items as well as the shop GUI the player interacts with. The main shop HUD consists of a
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
its drop rate was set to 30%. Finally, with gems being the most valuable of the currencies, its drop rate was set to 10%. Adding up the drop rates for the three 
types of currencies, we get 90% and so the remaining 10% is the drop rate for health potions. With this setup, it is most often the case that players will 
encounter currency to pick up as opposed to health potions for healing. This is an intentional design choice as we wanted to add challenge to our game so that 
players have to be mindful of their positioning and play carefully to avoid losing health, especially when healing is scarce during a run. That way, if a player 
ends up dying mid run, they can at least keep half the gold they earned during the run to buy upgrades to better prepare themselves for their next run. 
