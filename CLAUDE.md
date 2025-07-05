# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity 3D implementation of the Quarto board game, developed in C# using Unity 6000.0.32f1. The project implements clean architecture with complete separation between backend game logic and frontend Unity UI components.

## Build and Development Commands

### Unity Build Process
- Open project in Unity 6000.0.32f1 or later
- Use Unity's built-in build system: **File > Build Settings**
- Select target platform and build
- No command-line build scripts are currently configured

### Project Files
- Main solution: `inoue-quarto2.sln`
- Assembly definition: `Assembly-CSharp.csproj`
- Package dependencies: `Packages/manifest.json`

## Architecture Overview

### Clean Architecture Implementation
The project follows clean architecture principles with strict separation between:

**Backend (`Assets/Scripts/Backend/`)**:
- `domain_layer/` - Core game models (Board, Piece, Pieces)
- `application_layer/` - Use cases and command/result patterns
- `Adoper/` - Game phase management using State pattern
- `Player/` - AI algorithms and player implementations
- `View/` - View controller bridging backend and frontend

**Frontend (`Assets/Scripts/Frontend/`)**:
- `UIScripts/Presenter/` - UI state management and updates
- `UIScripts/Executer/` - Unity object manipulation
- `UIScripts/` - Camera controls and UI components

### Key Architectural Patterns

**State Pattern**: Game phases (SelectPlayerPhase, PutPieceByUserPhase, etc.) extend `GamePhase` base class

**Command Pattern**: User actions flow through Command → UseCase → Result → Executer/Presenter

**Presenter/Executer Separation**: 
- Presenters handle UI state updates
- Executers handle Unity GameObject manipulation

### Game Logic Specifics

**Piece Encoding**: PieceId values encode 4 binary attributes in lower 4 bits:
- **H/F**: Hole (穴) / Flat (平) - 表面
- **T/S**: Tall (高い) / Short (低い) - 高さ  
- **C/S**: Circle (丸) / Square (四角) - 形
- **B/W**: Black (黒) / White (白) - 色

Example: FSCB = Flat + Short + Circle + Black (平面・低い・丸・黒)

**Win Detection**: `isQuarto()` method uses efficient bitwise operations for victory conditions

**AI Systems**: Two algorithms available - "defo" (basic) and "Youkan" (advanced with defensive strategies)

## Development Notes

### Known Issues
- `PutPice` is a consistent typo throughout the codebase (should be `PutPiece`)
- Camera sensitivity adjustable via `CameraMover.cs`

### Controls Implementation
- WASD + Arrow keys for camera movement
- Q/E for vertical camera movement  
- Space key toggles camera control
- P resets camera rotation
- Mouse: left-click drag for rotation, right-click drag for movement

### AI Extension
To add new AI algorithms:
1. Inherit from `SelectPieceAlgorithm` or `PutPieceAlgorithm`
2. Register in `SelectPlayerPhase` constructor
3. Algorithm automatically appears in UI dropdown

### Cursor Rules
The project includes Cursor IDE integration with specific Quarto development guidelines in `.cursor/rules/quarto.mdc`.