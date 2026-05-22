# KNOCKOUT PROTOCOL UX/UI Direction v0.3 — Apple-like Premium Game Interface

## 기준
사용자가 원하는 방향은 “애플에서 만들었을 법한 게임”이다. 즉, 흔한 네온 SF HUD가 아니라 **정제된 소재감, 큰 여백, 명확한 위계, 얇고 고급스러운 모션/피드백**이 중심이다.

참고한 Apple 디자인 축:
- Human Interface Guidelines: clarity, deference, depth
- Buttons / Controls: 명확한 affordance, 과한 장식보다 상태와 역할 구분
- Materials / visionOS 계열: glass-like material, blur, depth, layered surface
- Color / Typography: 제한된 accent, 높은 가독성, San Francisco 계열 느낌의 정돈된 타입

> 주의: Apple UI를 복제하지 않는다. KNOCKOUT의 로봇 전투 정체성에 Apple식 정제감을 적용한다.

## 기존 방향에서 버릴 것
- 과한 네온 라인
- 랜덤 cut-corner 장식 남발
- 버튼마다 다른 스타일
- “AI가 만든 SF 콘셉트아트”처럼 보이는 불필요한 장식
- 어둡고 칙칙한 배경 위에 색만 얹은 버튼

## 새 방향: Premium Glass Combat UI

### 1. Surface
- 배경은 완전 검정이 아니라 **deep blue/graphite gradient**.
- 패널은 불투명 박스가 아니라 **frosted glass card**.
- 각 카드에는 얇은 white stroke와 아주 약한 inner highlight.
- 그림자는 검은 drop shadow보다 **blurred depth shadow** 중심.

### 2. Shape
- 기본 shape는 Apple스러운 large rounded rectangle.
- 전투/로봇 느낌은 작은 notch나 status rail로만 추가한다.
- cut corner는 핵심 CTA나 KO 상태에만 제한적으로 사용.

### 3. Color
- 기본 UI는 white / graphite / blue glass.
- Accent는 3개만 사용:
  - Blue: default focus / active
  - Orange: primary action / energy
  - Red: danger / KO
- 한 화면에 강한 컬러는 1~2곳만.

### 4. Typography
- SF Pro 느낌: 큰 제목은 얇고 넓게, 액션 버튼은 굵게.
- 과한 monospace 사용 금지. 숫자/상태값에만 mono 사용.
- 텍스트는 짧고 명확하게:
  - `Ready to deploy`
  - `Core exposed`
  - `Hold to KO`

### 5. Buttons
Apple-like 버튼은 “누르는 물체”보다 “정제된 control” 느낌이다.

- Primary: 큰 capsule, warm orange gradient, subtle highlight
- Secondary: glass capsule + blue text/stroke
- Danger: red glass, hold interaction 권장
- Combat: 둥근 glass pad 안에 action chip 배치

### 6. Joystick
- 과한 기계식 조이스틱보다 **glass touch pad**.
- base는 투명한 원형 material.
- thumb cap은 작은 floating puck.
- 입력 중에만 blue aura와 vector trail 표시.

### 7. Battle HUD
- 중앙 시야는 비워둔다.
- 조작 UI는 좌하단/우하단에 floating glass island로 둔다.
- KO 가능 상태는 화면 전체를 덮는 경고가 아니라, Apple Watch activity ring처럼 **정돈된 링/배지**로 표현.

## 컴포넌트 우선순위
1. `GlassPanel`
2. `AppleGameButton`
3. `FloatingActionPad`
4. `GlassJoystick`
5. `CoreStatusRing`
6. `ResultRewardCard`

## CSS 토큰 초안
```css
:root {
  --kp-bg-0: #07111F;
  --kp-bg-1: #0D2742;
  --kp-glass: rgba(255, 255, 255, 0.105);
  --kp-glass-strong: rgba(255, 255, 255, 0.18);
  --kp-stroke: rgba(255, 255, 255, 0.28);
  --kp-stroke-strong: rgba(255, 255, 255, 0.44);
  --kp-text: #F7FAFF;
  --kp-text-sub: rgba(247, 250, 255, 0.68);
  --kp-blue: #5AC8FA;
  --kp-orange: #FF9F0A;
  --kp-red: #FF453A;
  --kp-green: #30D158;
  --kp-radius-xl: 32px;
  --kp-radius-pill: 999px;
  --kp-blur: 28px;
}
```

## React 구현 예시
```tsx
<GlassPanel title="Hangar" subtitle="Bruiser frame ready">
  <CoreStatusRing value={72} label="Core" />
</GlassPanel>

<AppleGameButton variant="primary">Deploy</AppleGameButton>
<AppleGameButton variant="glass">Customize</AppleGameButton>
<AppleGameButton variant="danger" holdToConfirm>KO Protocol</AppleGameButton>

<GlassJoystick value={stick} active={isMoving} />
<FloatingActionPad actions={["Attack", "Dash", "Guard"]} />
```

## 화면 톤 결론
KNOCKOUT은 “싸구려 네온 로봇 게임”이 아니라, **Apple Arcade 프리미엄 로봇 배틀 앱**처럼 보여야 한다. UI는 전투를 방해하지 않고, 사용자가 다음 행동을 자연스럽게 알게 해야 한다.
