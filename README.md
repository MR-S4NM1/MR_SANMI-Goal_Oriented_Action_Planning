README – Goal-Oriented Action Planning (GOAP) AI System

Author: Miguel Angel Garcia Elizalde  
Date: December 2025  
Language: C#  
Project Type: AI Behavior System / GOAP Implementation
Engine: Unity 6.0 6000.0.50f1+ 

---

Overview

This project implements a professional-grade Goal-Oriented Action Planning (GOAP) system for Unity, featuring reactive AI agents, global state management, and extensible architecture.

The goal was to design a production-ready AI framework that demonstrates complex decision-making, reactive behavior, and modular design patterns suitable for AAA game development.

The system was built entirely from scratch within Unity to showcase strong understanding of AI planning algorithms, state machines, event-driven architecture, and animation integration.

---

Core Features

GOAP Architecture with Reactive Planning

- Goals with priority-based selection for intelligent behavior prioritization
- Actions with preconditions and effects that modify world state
- Planner that uses forward-chaining search with heuristic action selection
- Reactive system that interrupts and re-plans when world conditions change

Global State Management System

- Shared WorldState singleton that all agents monitor and modify
- Event-driven state changes with observer pattern implementation
- Reactive keys system allowing agents to subscribe only to relevant state changes
- State suppression mode for silent initialization to prevent planning cascades

Multi-Agent Behavior System

- Three distinct NPC types with unique behavior profiles:
  - Guard: Patrol and law enforcement with emergency response
  - Sorceress: Resource gathering and magical intervention
  - Thief: Stealth, theft, and evasion with survival instincts
- Each agent has customized goals, actions, and reactive patterns

State Machine Execution Engine

- Four-state machine for agent behavior:
  - IdleState: Waiting when no goals are achievable
  - PlanningState: Goal evaluation and plan generation
  - ExecutingPlanState: Sequential action execution
  - ReplanningState: Forced replanning on world changes
- Graceful action cancellation and cleanup during state transitions

Animation Integration System

- AnimationsManager providing clean interface for animation state management
- Animation length synchronization for timed actions
- Parameter cleanup system to prevent animation state conflicts
- Support for both trigger (one-shot) and bool (state-based) animations

---

Design Patterns Used

- Observer Pattern – WorldState change notifications to subscribed agents
- State Pattern – Four-state machine for agent behavior management
- Template Method – GOAPAgent base class with overrideable initialization
- Command Pattern – Actions as encapsulated behavior units
- Singleton – GlobalWorldState for shared state management
- Factory Method – Implicit in goal and action creation systems

---

Technical Highlights

- Coroutine-based action execution with proper cancellation handling
- Frame-accurate animation timing with interruption support
- Heuristic planning optimization that prioritizes goal-direct actions
- Reactive system that prevents unnecessary replanning on duplicate state changes
- Modular architecture allowing easy addition of new agents and actions
- Extensive debug logging system for AI behavior analysis
- Idempotent initialization preventing race conditions in complex scenes

---

Agent Behavior Specifications

Guard Agent
- Primary: Area patrol with waypoint navigation and observation periods
- Secondary: Emergency pursuit and physical capture of threats
- Reactive To: TownInDanger state changes
- Animation States: Walk (patrol), Idle (observation), Melee Right Attack 01 (combat)

Sorceress Agent  
- Primary: Four-step crafting cycle (move → gather → move → brew)
- Secondary: Magical pursuit and spellcasting capture
- Reactive To: TownInDanger state changes
- Animation States: Walk (movement), Pick Up (gathering), Drink Potion (brewing), Cast Spell (combat)

Thief Agent  
- Primary: Theft sequence with danger escalation
- Secondary: Escape with capture contingency and death handling
- Reactive To: TownInDanger and ThiefCaught state changes
- Animation States: Walk (movement), Pick Up (stealing), Die (capture)

---

Example: Creating a Custom Agent

public class GOAPAgent_Custom : GOAPAgent
{
    protected override void PrepareWorldState()
    {
        worldState["CustomState1"] = false;
        worldState["CustomState2"] = 0;
    }
    
    protected override void PrepareGoals()
    {
        var goal = new GOAPGoal("CustomGoal", 5.0f);
        goal.desiredState["CustomState1"] = true;
        goals.Add(goal);
        
        reactiveKeys.Add("GlobalStateChange");
    }
}

Example: Creating a Custom Action

public class ACustomAction : GOAPAction
{
    private void Start()
    {
        AddPrecondition("RequiredState", true);
        AddEffect("CustomState1", true);
    }
    
    protected override IEnumerator PerformAction(WorldState state)
    {
        animationsManager.AnimationFunction("ActionName", true);
        yield return new WaitForSeconds(1.0f);
        Complete(state);
    }
}

---

How to Run

1. Import the GOAP system scripts into a Unity 6.0 6000.0.50f1+ project
2. Create a GameObject with GlobalWorldState component
3. Configure NPC GameObjects with appropriate agent scripts (GOAPAgent_Guard, GOAPAgent_Sorceress, GOAPAgent_Thief)
4. Attach required action components to each agent
5. Set up animation controllers with required parameters
6. Configure patrol waypoints, target positions, and other action-specific settings
7. Run the scene and observe AI behaviors

---

Key Scenarios Demonstrated

1. Normal Operations – Agents pursue their primary goals (patrol, craft, steal)
2. Emergency Response – TownInDanger triggers immediate behavior shifts
3. Reactive Planning – World state changes force real-time replanning
4. Animation Integration – Actions synchronize with precise animation timing
5. Multi-Agent Coordination – Different agents respond to same global events
6. Behavior Interruption – Ongoing actions cancel gracefully for emergencies

---

Performance Considerations

- Planner includes safety iteration limits to prevent infinite loops
- Reactive keys minimize unnecessary event processing
- Animation length caching available for frequently used animations
- State change suppression during initialization prevents cascading replans
- Frame-accurate cancellation checks in time-sensitive actions

---

Extension Points

1. Add new agent types by extending GOAPAgent base class
2. Create new actions by inheriting from GOAPAction or AGoToPosition
3. Implement more sophisticated planning heuristics in GOAPPlanner
4. Add animation events for precise action-animation synchronization
5. Integrate with Unity's NavMesh for pathfinding
6. Add utility-based goal selection for more dynamic behavior
7. Implement action costs based on dynamic factors
8. Add team coordination and multi-agent plans

---

Navigation Flexibility:
- Current: Direct movement with MoveTowards() for simplicity
- Upgrade Path: Replace AGoToPosition movement logic with Unity's NavMeshAgent
- Architecture: Movement system isolated for easy pathfinding integration

---

Final Notes

This project represents a complete AI behavior system built with production-quality architecture and extensible design. It demonstrates professional understanding of game AI patterns, reactive systems, state management, and animation integration.

The system is designed as both a functional AI framework and a portfolio piece showing engineering discipline in game development – merging complex AI decision-making with clean, maintainable C# architecture suitable for professional game development environments.
