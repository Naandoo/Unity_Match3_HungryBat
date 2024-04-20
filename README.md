## Features ##
 - Tilemap
   - [x] Represents the board and allows the product owner to create boards with different shapes. 
 - Board
   - [x] Holds the items properly related to the Tilemap. 
   - [x] Has a fill behavior. 
   - [x] Controls the player input through a state system. 
   - [x] Validates when there aren't available moves and shuffles the items. 
   - [x] Validates when the Player hasn't interacted for a while and suggests a tip. 
   - [x] Validates movements that would result in a match, swapping back the items if it isn't the case. 
  - Items
    - [x] Move to available positions. 
    - [x] Can match in two directions. 
    - [x] Have information about themselves such as their current position and the ID. 
  - Skills
    - [x] Bomb that explodes a square range around the selected fruit.
    - [x] Potion that transforms X fruits into the selected one.
    - [x] Lightning strikes every single fruit on the board that matches the selected one.
  - UI
    - [x] Responsive UI created for the WEBGL platform.
    - [x] UI Animations to improve satisfaction.
    - [X] Integration between the system and the UI.  
  - Level System
    - [x] System that allows quick creation of levels based on inputs that represent the goals and obstacles.
    - [x] Skill availability per level to make it easier to create good game design.
  - Feedbacks
    - [x] Game Sounds.
    - [x] Game VFX.
    - [x] Game Animations to ensure smooth gameplay.
  - Settings
    - [X] Pause.
    - [X] Mute Sounds/Effects.
    - [X] Retry/Quit option.
    - [X] Win/Lose actions.

## Functionalities ##
- Asynchronous System
  - [x] Utilized Coroutines to manage the system in an appropriate way to ensure the functionalities without losing performance.
- Events
  - [x] Utilized events to reduce coupling and create nested code.
- Particles
  - [x] Created and edited particles using Unity's Particle System.
- Animations
  - [x] Created animations using Unity's Animator component and DoTween system.

## Gameplay Showcase ## 
