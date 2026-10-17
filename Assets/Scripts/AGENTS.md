# Unity Project Instructions

## Project Overview

2D sci-fi combat game built with Unity.

The gameplay takes place inside vertical tunnels.
Characters move vertically using elastic ropes and fight
with opponents, weapons, and environmental hazards.

## Technical Requirements

- Use C# and Unity-supported APIs.
- Follow modern Unity development practices.
- Prefer composition over deep inheritance hierarchies.
- Keep gameplay logic separated from UI logic.
- Avoid unnecessary dependencies and global state.
- Preserve existing project architecture unless a refactor is requested.
- Do not introduce external packages without justification.

## Code Quality

- Use explicit types and meaningful names.
- Handle null references safely.
- Keep methods focused on a single responsibility.
- Avoid unnecessary allocations in frequently executed gameplay code.
- Consider Unity lifecycle methods and object destruction.
- Use appropriate Unity physics callbacks.
- Add comments when they clarify non-obvious behavior.

## Change Management

- Inspect related scripts and scene dependencies before making changes.
- Avoid modifying unrelated files.
- Explain which files were changed and why.
- Identify Unity Editor actions that cannot be completed through code alone.
- Do not claim that a change was tested in Unity unless it actually was.