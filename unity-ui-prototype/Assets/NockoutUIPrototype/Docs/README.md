# NOCKOUT UI Prototype

Unity UI-only prototype for a landscape mobile flow.

## What This Contains

- Home UI for combat bot + up to 3 support/maintenance bots
- Support bot types: wheeled, quadruped, biped
- Ranking modal concept using the current loadout score
- Upgrade/maintenance modal placeholders
- Light/Dark toggle
- No combat logic and no game start flow

## How To Use

1. Copy `Assets/NockoutUIPrototype` into a Unity project.
2. In Unity, open `NOCKOUT > Create UI Prototype Scene`.
3. Open `Assets/NockoutUIPrototype/NockoutUIPrototype.unity`.
4. Set Game view to a landscape phone ratio, for example `844 x 390`.

## Intended UX

- The player selects one combat bot and up to three support bots.
- Support bots affect post-fight repair, recovery, tuning cost, and ranking.
- Details are hidden behind modals so the landscape phone screen stays readable.
- The central combat bot area should later become a 3D turntable model.

## Next Unity Implementation Notes

- Replace the static combat robot PNG with a real 3D model render or Unity model.
- Wire the support bot cards to ScriptableObject data.
- Replace placeholder ranking data with backend or local ranking records.
- Keep combat entry out until the UI flow feels correct.
