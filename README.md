# Brass Knuckles

## Overview

Brass Knuckles is a reasonably complete 2D game engine written in C#, using the Microsoft XNA Framework 4.0. The project was originally started as the basis for a top-down 2D _Gauntlet_ or rogue-like game, but the game project was moved to Unity 3D instead.

Features of the engine include:

* Entity framework / component system. Everything in Brass Knuckles is a component, with various services to manage them. Services are then managed by a single Director instance. The entity framework is a heavily modified version of the C# port of Artemis. For more information, see http://ploobs.com.br/?p=1765 for the C# port or http://gamadu.com/artemis/ for the original Java version. To read more about entity systems in general, see this excellent series by Adam Martin: http://t-machine.org/index.php/2007/09/03/entity-systems-are-the-future-of-mmog-development-part-1/
* Uses the Nuclex framework (http://nuclexframework.codeplex.com/) for fast input management. Input is highly configurable using a generic button-to-input mapping system and supports keyboard, mouse and gamepad and multiple players
* Basic UI system, with a framework for building custom UI controls
* A 2D sprite-based animation and rendering system
* Behavior tree based AI framework, with the skeleton of a blackboard message passing system

## Requirements

Brass Knuckles requires the following:

* Visual Studio 2010 or later (or MonoDevelop, although this has not been tested)
* .NET Framework 4.0
* XNA Framework 4.0
* Nuclex (included, or can be downloaded at http://nuclexframework.codeplex.com/)

## Future

Although development has been on hiatus for a while, I do plan on converting this to use Mono and MonoGame. Additionally, an A* pathfinding system exists for Brass Knuckles, but was removed for the moment for polishing. The blackboard system will be built out and tested. A physics system does exist, but will be scrapped in favor of Box2D or Farseer. Finally, proper unit tests will be added back to the solution.