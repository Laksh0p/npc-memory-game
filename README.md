# Persistent NPC Intelligence using Unity

## Overview
This project is a Unity-based game system where NPCs learn from the player's behavior and adapt over time. The goal was to create NPCs that do not reset every time the game restarts, but instead remember how the player acted in previous runs.


## Features

### Multi-NPC System
The game includes multiple NPCs that operate together instead of independently.

### Shared Memory
All NPCs use a shared memory system to store:
- Last known player position  
- Frequently used hiding spots  

This allows them to coordinate their behavior.


### Persistent Learning
The NPCs store player behavior using PlayerPrefs.

For example:
- If the player repeatedly hides in the same location  
- NPCs will start checking that location first in future runs  


### AI-Based Decision Making(using featherless ai)
Featherless AI is used to decide how NPCs react when the player disappears.

The AI chooses between:
- Checking likely hiding spots  
- Searching the surrounding area  

This makes NPC behavior less predictable.


## Gameplay
- The player moves around the environment  
- NPCs detect and chase the player  
- The player can hide  
- NPCs adapt their strategy based on previous behavior  
- If caught, the game ends and can be restarted  
- NPCs retain learned behavior across runs  


## Controls
- WASD: Move  
- F: Hide  
- R: Restart  


## Technologies Used
- Unity (C#)  
- NavMesh for NPC movement  
- Featherless AI API  


## Key Idea
The main focus of this project is to make NPCs that learn from the player and improve their behavior over time, instead of following fixed patterns.

## Note(PLEASE READ)
This repository contains only the core scripts. Unity assets have been excluded to keep the repo lightweight.
