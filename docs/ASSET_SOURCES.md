# Asset Sources

Commercial-use screening notes for NOCKOUT Protocol demo assets.

## Mobile Controls

- Current web demo controls are implemented in local HTML/CSS/JS using existing in-repo UI sprites.
- Unity project candidate: Terresquall `Virtual Joystick Pack`
  - Source: https://assetstore.unity.com/packages/tools/utilities/virtual-joystick-pack-261317
  - Store listing: `FREE`, `Standard Unity Asset Store EULA`, `Extension Asset`
  - Decision: acceptable for Unity project use when acquired through the team's Unity account.
  - Restriction: do not commit downloaded Asset Store source/package files into this public web demo repo.
- Paid fallback for a fuller Unity mobile control system: SURIYUN `Mobile Controller System`
  - Source: https://assetstore.unity.com/packages/templates/systems/mobile-controller-system-161533
  - Store listing: `Standard Unity Asset Store EULA`, `Extension Asset`
  - Decision: acceptable only after purchase on the correct team Unity account.

## Combat Animations

- Unity project candidate: `Human Melee Animations FREE`
  - Source: https://assetstore.unity.com/packages/3d/animations/human-melee-animations-free-165785
  - Store listing/review notes indicate the free version includes run, sprint, combat idle, attacks, damage, and death.
  - Decision: acceptable for Unity prototyping if the product page shows `Standard Unity Asset Store EULA` and no `Restricted Asset` or `Non-standard` label at import time.
  - Restriction: do not expose raw animation FBX files from Asset Store packages in a public static repo.

## Unity Asset Store Rule

Unity's own EULA FAQ says Standard EULA assets can be distributed only as embedded/incorporated parts of a larger licensed product, not as extractable standalone assets. For public GitHub Pages/web demo source, prefer self-made, generated, or permissively licensed assets.
