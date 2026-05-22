# KNOCKOUT PROTOCOL UX/UI Direction — Professional HUD System

## 목표
프로그래밍 팀이 바로 참고할 수 있는 **게임 HUD 컴포넌트 규칙**을 만든다. 레퍼런스의 장점은 가져오되, AI가 대충 만든 듯한 과장된 장식은 줄이고 실제 UI로 구현 가능한 구조를 우선한다.

## 핵심 디자인 방향
- 톤: **navy tactical HUD + clean sci-fi frame**
- 느낌: 정비창/전투 인터페이스/로봇 진단 패널
- 금지: 의미 없는 장식 과다, 랜덤 네온, 과한 글로우, 읽기 어려운 얇은 텍스트
- 우선순위: **가독성 → 조작 안정성 → 분위기**

## 레퍼런스에서 가져갈 요소
- 얇은 흰색/시안 라인 프레임
- 모서리가 잘린 팔각형 패널
- 진행바/세그먼트 게이지
- 작은 상태 칩과 우측 세로 인디케이터
- 어두운 남색 배경 + 미세한 도트 그리드

## 컬러 토큰
```css
:root {
  --bg-deep: #061525;
  --bg-panel: rgba(7, 22, 36, 0.82);
  --bg-panel-solid: #0A2236;
  --line-main: #EAF7FF;
  --line-soft: rgba(234, 247, 255, 0.48);
  --cyan: #4DEBFF;
  --cyan-soft: rgba(77, 235, 255, 0.18);
  --orange: #FF8A2A;
  --red: #FF4D4D;
  --green: #6DFF9B;
  --text-main: #F4FAFF;
  --text-sub: #9FB9CC;
}
```

## 레이아웃 원칙
1. 화면은 크게 3영역으로 나눈다.
   - 좌측: 이동/조이스틱/플레이어 상태
   - 중앙: 로봇/전투 시야/주요 피드백
   - 우측: 전투 버튼/무기/상태 로그
2. 터치 영역은 최소 64px 이상.
3. 장식 프레임은 정보 묶음을 감싸는 용도로만 쓴다.
4. 모든 버튼은 “눌리는 면”이 있어야 한다. 선만 있는 버튼 금지.

## 컴포넌트 규칙

### HUD Panel
- 배경: `--bg-panel`
- 외곽선: 1.5~2px `--line-soft`
- 모서리: 일반 rounded가 아니라 한두 군데 cut corner를 사용
- 내부 여백: 16~24px

### Primary Button
- 채워진 면 사용: orange/cyan 계열
- 상단 highlight 10~15% opacity
- 하단 shadow로 눌림감 표현
- 텍스트는 짧고 굵게: `START`, `FIGHT`, `HANGAR`

### Combat Button
- 원형/육각형 중 하나로 통일
- ATK/DASH/KO처럼 조작 의미가 즉시 읽혀야 함
- KO/위험 기능만 red 사용

### Joystick
- 단순 동그라미 금지
- base ring + axis cross + thumb cap + 방향 tick으로 구성
- 평상시에는 투명도를 낮추고, 입력 중에는 cyan glow 증가

### Gauge / Status Bar
- 긴 진행바보다 segment bar를 우선
- CORE, ARMOR, ENERGY, HEAT처럼 짧은 라벨 사용
- 위험 상태는 색만 바꾸지 말고 label도 `WARN`, `OVERHEAT` 등으로 표시

## 화면별 UX 방향

### Start / Main
- 큰 로고 + 주요 CTA 1개
- 나머지는 작은 서브 버튼
- 배경은 로봇/정비창이 보여도 UI 가독성을 해치지 않게 처리

### Hangar
- 중앙: 로봇 프리뷰
- 좌측: 파츠 카테고리
- 우측: 선택 파츠 스펙/비교
- 하단: 장착 슬롯

### Battle HUD
- 중앙 시야는 최대한 비워둔다.
- 좌하단: joystick
- 우하단: combat buttons
- 상단: timer / enemy core / player core
- 좌우 패널은 반투명, 필요할 때만 확장

## 개발자가 구현할 때 우선순위
1. `HUDPanel`, `HUDButton`, `SegmentGauge`, `JoystickPad`, `CombatButton` 컴포넌트부터 만든다.
2. 레이아웃은 모바일 landscape 기준으로 먼저 고정한다.
3. 장식은 CSS pseudo-element로 처리하고 이미지 의존도를 낮춘다.
4. 실제 게임 상태값과 연결될 수 있도록 props 기반으로 만든다.

## React 컴포넌트 예시
```tsx
<HUDPanel title="ROBOT STATUS">
  <SegmentGauge label="CORE" value={72} tone="cyan" />
  <SegmentGauge label="ARMOR" value={58} tone="orange" />
  <SegmentGauge label="HEAT" value={24} tone="green" />
</HUDPanel>

<JoystickPad active={isMoving} x={stick.x} y={stick.y} />

<CombatButton label="ATK" tone="orange" onPress={attack} />
<CombatButton label="DASH" tone="cyan" onPress={dash} />
<CombatButton label="KO" tone="red" danger onPress={knockout} />
```

## 현재 결정
이 UI 방향을 프로젝트 기본 UX/UI 레퍼런스로 사용한다. 이후 programming 채널/개발 작업에서는 이 문서를 기준으로 화면을 구현한다.
