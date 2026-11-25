# Architecture

The WPF app under src/AetherIsland.App is the independent Aether Island implementation. It must not reference the historical source folders at repository root.

## Boundaries
- UI: WPF views and code-behind.
- Models: serializable settings and feature state.
- Services: settings persistence and Windows integration APIs.
- Assets: Aether-owned visual resources.

## Principles
1. Keep the capsule lightweight and responsive.
2. Keep optional features independently switchable.
3. Store settings per Windows user.
4. Do not collect telemetry or make network requests unless a future feature explicitly requires them and is user-controlled.
5. Keep Windows integration behind services.
6. Failure of an optional integration must not prevent the island from opening.

Planned service boundaries: settings, Windows media sessions, notifications, shortcuts, startup registration and signed updates. Only settings persistence and the initial UI actions exist in the first shell.
