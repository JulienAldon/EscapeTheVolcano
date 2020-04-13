# Escape the volcano
This game has been designed during GameDesign lessons during my first year of master in St-Petersburg.

I decided to code the game because I thought the idea was nice and simple to realize as a first game. It wasn't...

I firstly made it in Python but the engine I coded wasn't optimized enough so a lot of functionnality were not possible. I decided to use an easy tool and more powerfull than python to develop the game, so i turned myself toward Unity.

I learned a lot of things about game design and game developpement in c# and using Unity.


# How to play

## Get The Game !
Game is available on Windows and Linux !

Go to release and download the latest version of the game.

## Goal
The goal is to go deep in the dungeon to a treasure room, where you'll need to gather as many crystals as possible in a delimited time. After this looting you'll need to get back to the enter of the dungeon, escaping the lava flow.

The game is in 4 phases:
- Choose your team
- Mark the level and try to locate your path in the random generated level (you have as many time as you want)
- Loot the tresure (timed)
- Escape before the lava kills you

For this you have to create a team of robbots at the first phase of the game.

It's a tinder like selection (inspired by Reigns) right for select, left for pass.

You'll need to choose between 6 classes:
- Hacker : can deactivate turret using is Action key
- Grenadier : can send grenades using is Action key
- Climber : can use a grappling using is Action key
- Runner : can dash using is Action key
- Tracker : can lend an land mark using is Action key
- Tank (not implemented)

You can switch between your team to use the best power in a given situation.

For the moment you can't gather crystals.

# TODO

## Importants stuff
- [X] GameOver scene
- [ ] Death of a member gestion
- [X] Input gestion
- [X] Win conditions

## Ennemies & traps
- [x] Ennemy AI
- [ ] Spikes
- [ ] Hidden Spikes
- [ ] Trap door
  - [ ] Opening on spikes
  - [ ] Opening on lava
- [ ] Lava as traps
  
## Map generation
- [ ] Add trap to generation
- [ ] Add ennemies to generation
- [ ] Add randomness in the different layouts

## Gameplay
- [ ] Scoreboard
- [ ] Point Counting
- [ ] Tutorial

## Story
- [ ] An Intro cinematic
- [X] A themed Menu
- [X] A themed GameOver
- [ ] A story ?

## Fix and Improvements
- [X] Fix the climber behavior.
- [X] Fix a bug when the bomber was activated by default.
- [X] Fix the dust effect with the player jump.
- [X] Enhence the size of the camera view to allow the player easier movements.
- [X] Lowered a bit the max speed of the player.
- [X] Remade the menu to something clearer and easier to use.
- [X] Fix a bug in the menu where you couldn't press the button randomly.
- [ ] Changed the lava behavior to a more fair one.
- [X] Add Dialogue window for the selection scene.
- [ ] Fix a bug where the turrets can bump you inside a block.
- [ ] Fix a graphic bug where turrets life can pass behind background.
- [ ] Change Bomber launching abilities for a more controlable sending.
- [X] Add a Pause Menu where you can reset or leave the game.
