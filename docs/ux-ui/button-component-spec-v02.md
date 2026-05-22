# KNOCKOUT PROTOCOL Button Component Spec v0.2

## 방향
기존 버튼은 단순히 면을 채운 수준이라 조잡해 보였다. v0.2 버튼은 **실제 게임 UI에서 반복 사용 가능한 컴포넌트**로 정리한다.

핵심은 “네온 장식”이 아니라:
- 명확한 눌림감
- 일관된 shape language
- 버튼 등급별 차이
- 개발자가 CSS/React로 그대로 구현 가능한 구조

## Shape Language
버튼은 일반 rounded rectangle 대신 **slanted cut button**을 기본으로 한다.

- 좌상단/우하단에 10~16px 컷
- 외곽선은 얇게, 내부는 확실히 채움
- 상단 하이라이트와 하단 그림자로 눌림감 표현
- 텍스트는 짧고 굵게

## Variants

### 1. Primary Action
사용처: START, FIGHT, DEPLOY, READY

- Fill: orange gradient
- Text: dark navy or near-black
- Border: warm white 35~45% opacity
- Glow: 약하게만 사용
- 가장 눈에 띄어야 함

### 2. Secondary Action
사용처: HANGAR, CUSTOMIZE, INVENTORY

- Fill: dark cyan/navy
- Border: cyan
- Text: cyan-white
- Primary보다 덜 강하지만 버튼으로 보여야 함

### 3. Tactical Ghost
사용처: OPTION, BACK, FILTER

- Fill: transparent navy 35~50%
- Border: muted white/cyan
- Text: subdued
- 주요 CTA와 경쟁하지 않게 함

### 4. Danger / KO
사용처: KO, OVERDRIVE, EXECUTE

- Fill: red core + dark rim
- Border: red-white
- Label 옆에 작은 warning notch 사용 가능
- 남발 금지. 실제 위험/결정 액션에만 사용

### 5. Combat Compact
사용처: ATK, DASH, GUARD, SKILL

- 원형보다 **hex/angled capsule** 권장
- 모바일 엄지 버튼은 원형 가능하지만, 내부에 bevel/rim이 있어야 함
- label은 3~5자 이내

## CSS 구현 예시
```css
.kp-button {
  --tone: #4DEBFF;
  --tone-dark: #0A2236;
  position: relative;
  min-height: 56px;
  padding: 0 22px;
  border: 1.5px solid rgba(234, 247, 255, 0.45);
  clip-path: polygon(14px 0, 100% 0, 100% calc(100% - 14px), calc(100% - 14px) 100%, 0 100%, 0 14px);
  background: linear-gradient(180deg, color-mix(in srgb, var(--tone) 90%, white 10%), var(--tone));
  box-shadow:
    0 10px 0 rgba(0, 0, 0, 0.26),
    0 0 18px color-mix(in srgb, var(--tone) 28%, transparent);
  color: #061525;
  font-weight: 900;
  letter-spacing: 0.04em;
}

.kp-button::before {
  content: "";
  position: absolute;
  inset: 6px 8px auto 8px;
  height: 35%;
  background: linear-gradient(180deg, rgba(255,255,255,.22), rgba(255,255,255,0));
  pointer-events: none;
}

.kp-button.secondary {
  background: linear-gradient(180deg, rgba(24, 55, 72, .96), rgba(8, 31, 48, .96));
  color: #EAF7FF;
  border-color: rgba(77, 235, 255, .82);
}

.kp-button.ghost {
  background: rgba(7, 22, 36, .55);
  color: #9FB9CC;
  border-color: rgba(234, 247, 255, .35);
  box-shadow: 0 8px 0 rgba(0,0,0,.2);
}

.kp-button.danger {
  --tone: #FF4D4D;
  color: #FFF7F7;
  background: linear-gradient(180deg, #FF5E5E, #B82020);
}
```

## 사용 기준
- 한 화면에 Primary는 1개만 둔다.
- Danger는 KO/결정 액션에만 쓴다.
- Secondary와 Ghost를 섞어 정보 위계를 만든다.
- 버튼 배경이 어두울수록 텍스트는 white, 밝을수록 navy를 쓴다.
