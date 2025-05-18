# Bounce Dash

A hyper-casual 2D endless runner game built in Unity 6.

## Table of Contents
- [Game Overview](#game-overview)
- [Controls](#controls)
- [Setup Instructions](#setup-instructions)
- [Step-by-Step Implementation](#step-by-step-implementation)
- [Features](#features)
- [Approaches & Game Feel](#approaches--game-feel)
- [Project Structure](#project-structure)
- [Known Issues](#known-issues)

---

## Game Overview
Bounce Dash is a 2D endless runner where you control a bouncing ball, dodging obstacles and collecting coins to score points. The game features smooth controls, dynamic level generation, a minimalist UI, and a shop to unlock new ball skins. Designed for both desktop and mobile play.

---

## Controls
- **Keyboard:**
  - Move Left: `A` or `Left Arrow`
  - Move Right: `D` or `Right Arrow`
- **Mouse/Touch:**
  - Drag or swipe left/right to move the ball horizontally
- **Start Game:**
  - Click/tap the Play button or use movement input
- **Restart:**
  - Click/tap the Restart button on the Game Over screen
- **Shop:**
  - Open/close via UI button; select a ball skin by clicking/tapping

---

## Setup Instructions
1. Open the project in **Unity 6**.
2. Assign all player prefabs to `GameManager.characterPrefabs` in the Inspector.
3. Assign UI elements (buttons, panels) to `UIManager` in the Inspector.
4. Assign character prefabs and preview spot to `ShopManager` in the Inspector.
5. Press **Play**!
6. Use the shop to select a ball and see it spawn instantly in-game.

---

## Step-by-Step Implementation

### 1. **Project Setup**
- Created a new Unity 2D project.
- Organized folders: `Assets/Scripts`, `Assets/Prefabs`, `Assets/Scenes`, `Assets/Prefabs/PlayerPrefab`.

### 2. **Core Gameplay**
- **PlayerController.cs**: Handles bouncing physics (Rigidbody2D), smooth left/right movement, and input (keyboard, mouse drag, touch swipe). Detects collisions with platforms and obstacles.
- **GameManager.cs**: Manages game state, scoring (time survived + coins), player instantiation, and persistent data (high score, coins, selected character). Singleton pattern for global access.
- **CameraFollow.cs**: Smoothly follows the player, updating target for live character switching.
- **PathGenerator.cs**: Dynamically spawns platforms and obstacles as the player ascends. Obstacles are randomly placed on top of platforms.
- **CoinSpawner.cs**: Spawns coins ahead of the player, ensuring coins do not overlap with platforms.
- **Obstacle.cs & Coin.cs**: Handle collision logic for obstacles (game over) and coins (collect and update score).

### 3. **UI System**
- **UIManager.cs**: Manages all UI panels (start, game over, shop), score/coin display, and button logic (restart, open/close shop). Singleton pattern for global access.
- **ShopManager.cs**: Handles character selection, preview, and live switching of the player ball. Updates the selected character in PlayerPrefs and notifies GameManager to switch the player in-game.

### 4. **Shop & Character Selection**
- Player character prefabs are stored in `Assets/Prefabs/PlayerPrefab`.
- Shop UI allows selecting a ball; selection is saved and the new ball spawns instantly in-game.
- Character preview is shown in the shop using a dedicated preview spot.

### 5. **Persistence**
- High score, total coins, and selected character index are saved/loaded using `PlayerPrefs`.

### 6. **Game Flow**
- Game starts on tap/click/keyboard (unless over UI).
- Game over and restart logic resets all relevant state and respawns the selected ball.
- Shop can be opened/closed from the UI at any time.

---

## Features
### Core Gameplay
- Bouncing ball physics (Rigidbody2D)
- Smooth left/right movement (keyboard, mouse, touch)
- Dynamic spawning of platforms, obstacles, and coins
- Scoring system: time survived + coins collected
- Game Over on obstacle collision or falling

### UX & UI
- Start screen with Play button
- Game Over screen with score and restart option
- UI animations (score update, coin pop effect placeholder)
- Minimalist, clean design
- Responsive UI for desktop and mobile

### Technical
- Unity 6
- Mobile input support (touch swipe/drag)
- Modular, well-commented C# code (separate scripts for player, spawning, scoring, UI)
- Inspector-driven setup for easy extensibility

### Bonus
- Simple shop to unlock/select new ball skins (color/appearance)
- (Optional, not implemented) Daily challenge mode (e.g., "collect 50 coins")
- (Optional, not implemented) Addressables/ScriptableObjects for dynamic data

---

## Approaches & Game Feel
- **Game Feel:**
  - Fast, responsive controls for both keyboard and touch
  - Smooth camera follow and UI transitions
  - Minimalist visuals to keep focus on gameplay
  - Designed for quick restarts and addictive loop
- **Optimization:**
  - Object pooling could be added for further optimization (currently uses Instantiate/Destroy)
  - All gameplay logic is modular and separated for clarity and maintainability
- **Extensibility:**
  - New obstacles, coins, or player skins can be added by extending prefabs and scripts

---

## Project Structure
```
Assets/
  Prefabs/
    PlayerPrefab/         # All player character prefabs (Player, Player 1, Player 2, Player 3, etc.)
    Obstacle.prefab
    Coin.prefab
    Platform.prefab
  Scripts/
    GameManager.cs
    PlayerController.cs
    CameraFollow.cs
    PathGenerator.cs      # Spawns platforms and obstacles
    CoinSpawner.cs        # Spawns coins
    UIManager.cs
    ShopManager.cs
    Coin.cs
    Obstacle.cs
  Scenes/
    SampleScene.unity
  UI/                    # UI images and assets
  Animations/            # Animation assets (if any)
  Resources/             # Additional resources
  Settings/              # Game settings
  SimpleSky/             # Skybox or background assets
  TextMesh Pro/          # TextMesh Pro assets
```

---

## Known Issues
- Coin pop effect is a placeholder (can be improved with animation/VFX)
- No daily challenge mode implemented
- No Addressables or ScriptableObjects for gameplay data (except default Readme asset)
- Object pooling not implemented (uses Instantiate/Destroy)
- Only one main scene (SampleScene.unity)
- All assets are basic/minimal for demonstration purposes

--- 