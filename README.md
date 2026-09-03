# TRIANGUN

A fast-paced, top-down 2D shooter built in Unity 6 using C#. 

I made this entire game in **under 5 hours** for my college's **Design Thinking Lab**. The task was to design something using only basic geometric shapes: a circle, a square, and a triangle. Instead of making something static, I decided to build a survival game focusing heavily on "game juice" and overall game feel to make these simple shapes incredibly fun to interact with. 

It starts off simple but progressively scales into a difficult bullet-hell survival game the longer you stay alive.

## Features

### Core Gameplay
* **Action Camera:** A smooth, custom camera system that keeps the player in a deadzone while slightly looking ahead towards the mouse pointer.
* **Progressive Difficulty:** An AI director scales the game over time. As the timer goes up, the spawn rate decreases, enemies move faster, and tougher enemy variants start appearing.
* **Enemy Variants:** * *Basic Enemy (Triangle):* Follows the player and shoots single projectiles.
  * *Shotgun Enemy (Square):* Slower fire rate but shoots a wide spread of 3 bullets at once.
* **Health Systems:** A smooth-lerping UI health bar for the player, and a hidden hit-point system for enemies (takes 2-3 hits to kill).

### "Game Juice" & Game Feel
A major focus of this project was making the combat feel punchy and satisfying:
* **Hitstop & Screen Shake:** The game time briefly freezes (hitstop) on impact, combined with a heavy, unscaled screen shake whenever an enemy dies or the player shoots.
* **Visual Feedback:** * The player's sprite flashes red when taking damage.
  * Enemies spray a directional blood particle effect backwards based on the angle of the bullet that killed them.
* **Dynamic Audio Manager:** * Uses arrays to play randomized variations of shoot, hit, and death sounds to prevent audio fatigue. 
  * *Escalating Combo Sounds:* When getting rapid kills, the audio pitch shifts higher and more intense until the streak cashes in.
* **Combo Score System:** Rapid kills build up a streak meter. TextMeshPro UI elements pop and scale dynamically when getting kills, before absorbing into the main score.

## Structure
* **Modular Codebase:** Script logic is split up cleanly (e.g., `GameManager`, `AudioManager`, `CameraController`, `EnemySpawner`).
* **Singleton Pattern:** Global managers are easily accessible without cluttering the Unity Inspector with dragged references.
* **Game Loop:** Fully functional flow including a Main Menu, active gameplay scene, and a Game Over screen with instant restart functionality.

## Tech Stack
* **Engine:** Unity 6
* **Language:** C#
* **UI:** TextMeshPro (TMP) and Unity UI
