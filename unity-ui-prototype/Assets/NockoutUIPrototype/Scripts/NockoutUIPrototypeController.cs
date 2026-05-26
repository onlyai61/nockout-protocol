using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Nockout.UIPrototype
{
    public sealed class NockoutUIPrototypeController : MonoBehaviour
    {
        private readonly List<GameObject> modalRoots = new List<GameObject>();
        private Font font;
        private bool darkMode = true;
        private Color backgroundTint;
        private Color panel;
        private Color panelStrong;
        private Color text;
        private Color muted;
        private Color cyan;
        private Color gold;
        private Color coral;
        private Color green;

        private struct SupportBot
        {
            public string Name;
            public string Type;
            public string Role;
            public string Bonus;
            public string Cost;
            public Color Accent;
        }

        private readonly SupportBot[] supportBots =
        {
            new SupportBot
            {
                Name = "PIT-01",
                Type = "바퀴형",
                Role = "기본 정비",
                Bonus = "수리비 -8% / 배터리 +5%",
                Cost = "RC 120",
                Accent = new Color(0.35f, 0.91f, 1f)
            },
            new SupportBot
            {
                Name = "QUAD-K9",
                Type = "4족 보행",
                Role = "파손 회수",
                Bonus = "부품 회수 +18% / 균형 회복",
                Cost = "WC 95",
                Accent = new Color(0.47f, 1f, 0.66f)
            },
            new SupportBot
            {
                Name = "BIPED-7",
                Type = "2족 보행",
                Role = "정밀 튜닝",
                Bonus = "업그레이드 비용 -10%",
                Cost = "VC 140",
                Accent = new Color(1f, 0.76f, 0.27f)
            }
        };

        private void Awake()
        {
            Build();
        }

        [ContextMenu("Rebuild UI")]
        public void Build()
        {
            ClearChildren();
            ApplyTheme();
            EnsureEventSystem();

            font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            var canvas = CreateCanvas();
            CreateArenaBackground(canvas.transform);

            var root = Panel("UI Root", canvas.transform, Stretch(), Color.clear);
            var safe = Panel("Landscape Safe Area", root.transform, Rect(0, 0, 0, 0, 24, 18, -24, -18), Color.clear);

            CreateHeader(safe.transform);
            CreateNav(safe.transform);
            CreateArenaStage(safe.transform);
            CreateSupportLoadout(safe.transform);
            CreateBottomStatus(safe.transform);
            CreateModalLayer(canvas.transform);
        }

        private void ApplyTheme()
        {
            if (darkMode)
            {
                backgroundTint = new Color(0.02f, 0.05f, 0.09f, 0.88f);
                panel = new Color(0.05f, 0.08f, 0.13f, 0.78f);
                panelStrong = new Color(0.08f, 0.12f, 0.19f, 0.92f);
                text = new Color(0.95f, 0.98f, 1f);
                muted = new Color(0.62f, 0.72f, 0.82f);
            }
            else
            {
                backgroundTint = new Color(0.92f, 0.96f, 1f, 0.68f);
                panel = new Color(1f, 1f, 1f, 0.78f);
                panelStrong = new Color(0.9f, 0.96f, 1f, 0.94f);
                text = new Color(0.04f, 0.09f, 0.15f);
                muted = new Color(0.26f, 0.34f, 0.45f);
            }

            cyan = new Color(0.31f, 0.88f, 1f);
            gold = new Color(1f, 0.77f, 0.25f);
            coral = new Color(1f, 0.36f, 0.31f);
            green = new Color(0.46f, 1f, 0.65f);
        }

        private Canvas CreateCanvas()
        {
            var go = new GameObject("NOCKOUT UI Prototype Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            go.transform.SetParent(transform, false);

            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(844, 390);
            scaler.matchWidthOrHeight = 0.5f;

            return canvas;
        }

        private void CreateArenaBackground(Transform parent)
        {
            var bg = Image("Arena Background", parent, Stretch(), Color.white);
            var texture = Resources.Load<Texture2D>("NockoutArenaBackdrop");
            if (texture != null)
            {
                bg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                bg.type = UnityEngine.UI.Image.Type.Simple;
                bg.preserveAspect = false;
            }

            Image("Arena Tint", parent, Stretch(), backgroundTint);
        }

        private void CreateHeader(Transform parent)
        {
            var header = Panel("Header", parent, Rect(0, 1, 1, 1, 0, -52, 0, 0), new Color(0f, 0f, 0f, 0.2f));
            TextBlock("NOCKOUT PROTOCOL", header.transform, Rect(0, 0, 0, 1, 0, 0, 260, 0), 22, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            TextBlock("격투 리그 편성 UI / 전투 시작 없음", header.transform, Rect(0, 0, 0, 1, 248, 0, 530, 0), 11, FontStyle.Normal, muted, TextAnchor.MiddleLeft);

            Button("랭킹", header.transform, Rect(1, 0.5f, 1, 0.5f, -212, -17, -142, 17), gold, text, () => ShowRankingModal());
            Button(darkMode ? "Light" : "Dark", header.transform, Rect(1, 0.5f, 1, 0.5f, -132, -17, -62, 17), cyan, new Color(0.02f, 0.05f, 0.08f), () =>
            {
                darkMode = !darkMode;
                Build();
            });
            Button("저장", header.transform, Rect(1, 0.5f, 1, 0.5f, -52, -17, 0, 17), green, new Color(0.02f, 0.05f, 0.08f), () => ShowToast("현재 편성을 저장했습니다."));
        }

        private void CreateNav(Transform parent)
        {
            var nav = Panel("Main Navigation", parent, Rect(0, 0, 0, 1, 0, 0, 88, -62), new Color(0.02f, 0.04f, 0.07f, 0.5f));
            string[] items = { "홈", "업그레이드", "정비실", "리그", "훈련" };
            for (int i = 0; i < items.Length; i++)
            {
                int captured = i;
                var yTop = -12 - i * 54;
                var color = i == 0 ? cyan : panelStrong;
                var labelColor = i == 0 ? new Color(0.02f, 0.05f, 0.08f) : text;
                Button(items[i], nav.transform, Rect(0.5f, 1, 0.5f, 1, -38, yTop - 38, 38, yTop), color, labelColor, () => ShowSectionModal(items[captured]));
            }
        }

        private void CreateArenaStage(Transform parent)
        {
            var stage = Panel("Combat Bot Stage", parent, Rect(0, 0, 1, 1, 104, 70, -326, -62), new Color(0.01f, 0.02f, 0.04f, 0.25f));
            TextBlock("전투봇", stage.transform, Rect(0, 1, 0, 1, 12, -34, 92, -8), 16, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            TextBlock("MECH BOXER / 360도 회전 검사 영역", stage.transform, Rect(0, 1, 1, 1, 92, -32, -12, -8), 11, FontStyle.Normal, muted, TextAnchor.MiddleLeft);

            var robot = Image("Combat Robot Cutout", stage.transform, Rect(0.5f, 0.5f, 0.5f, 0.5f, -116, -122, 116, 110), Color.white);
            var texture = Resources.Load<Texture2D>("NockoutCombatRobot");
            if (texture != null)
            {
                robot.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                robot.preserveAspect = true;
            }

            CreateSlot(stage.transform, "전원", "배터리 효율 82%", Rect(0, 0.5f, 0, 0.5f, 12, 42, 120, 92), cyan, () => ShowPartModal("전원 설계", "배터리, 컨버터, 과열 안정성을 조정합니다."));
            CreateSlot(stage.transform, "배선", "신호 안정 76%", Rect(0, 0.5f, 0, 0.5f, 12, -24, 120, 26), gold, () => ShowPartModal("배선", "전선 등급, 커넥터, 노이즈 차폐를 튜닝합니다."));
            CreateSlot(stage.transform, "카메라/센서", "인식률 91%", Rect(1, 0.5f, 1, 0.5f, -132, 42, -12, 92), green, () => ShowPartModal("카메라/센서", "시야각, 표적 추적, 충격 감지를 조정합니다."));
            CreateSlot(stage.transform, "측정실", "진단 완료", Rect(1, 0.5f, 1, 0.5f, -132, -24, -12, 26), coral, () => ShowPartModal("측정실", "멀티미터, 부하 테스트, 리포트를 확인합니다."));

            TextBlock("DRAG TO ROTATE", stage.transform, Rect(0.5f, 0, 0.5f, 0, -90, 12, 90, 36), 12, FontStyle.Bold, muted, TextAnchor.MiddleCenter);
        }

        private void CreateSupportLoadout(Transform parent)
        {
            var loadout = Panel("Support Bot Loadout", parent, Rect(1, 0, 1, 1, -310, 70, 0, -62), panel);
            TextBlock("서포트봇 편성", loadout.transform, Rect(0, 1, 1, 1, 14, -34, -14, -8), 17, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            TextBlock("최대 3개 장착 / 전투 후 정비와 랭킹 보너스", loadout.transform, Rect(0, 1, 1, 1, 14, -54, -14, -32), 10, FontStyle.Normal, muted, TextAnchor.MiddleLeft);

            for (int i = 0; i < supportBots.Length; i++)
            {
                CreateSupportCard(loadout.transform, supportBots[i], i, Rect(0, 1, 1, 1, 14, -122 - i * 70, -14, -62 - i * 70));
            }

            Button("편성 랭킹 보기", loadout.transform, Rect(0, 0, 0.5f, 0, 14, 12, -4, 44), gold, new Color(0.02f, 0.05f, 0.08f), () => ShowRankingModal());
            Button("정비봇 강화", loadout.transform, Rect(0.5f, 0, 1, 0, 4, 12, -14, 44), cyan, new Color(0.02f, 0.05f, 0.08f), () => ShowSectionModal("정비실"));
        }

        private void CreateSupportCard(Transform parent, SupportBot bot, int index, RectTransformPreset rect)
        {
            var card = Panel(bot.Name, parent, rect, panelStrong);
            Image("Accent", card.transform, Rect(0, 0, 0, 1, 0, 0, 5, 0), bot.Accent);
            TextBlock((index + 1) + "  " + bot.Name, card.transform, Rect(0, 1, 0.55f, 1, 14, -26, 0, -6), 14, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            TextBlock(bot.Type + " / " + bot.Role, card.transform, Rect(0, 1, 1, 1, 14, -46, -88, -26), 10, FontStyle.Normal, muted, TextAnchor.MiddleLeft);
            TextBlock(bot.Bonus, card.transform, Rect(0, 0, 1, 0, 14, 8, -88, 28), 10, FontStyle.Bold, bot.Accent, TextAnchor.MiddleLeft);
            Button("커스텀", card.transform, Rect(1, 0.5f, 1, 0.5f, -76, -18, -10, 18), bot.Accent, new Color(0.02f, 0.05f, 0.08f), () => ShowSupportModal(bot));
        }

        private void CreateBottomStatus(Transform parent)
        {
            var bottom = Panel("Bottom Status", parent, Rect(0, 0, 1, 0, 104, 0, 0, 58), new Color(0.02f, 0.04f, 0.07f, 0.56f));
            TextBlock("리그 편성 점수", bottom.transform, Rect(0, 0, 0, 1, 12, 0, 108, 0), 11, FontStyle.Normal, muted, TextAnchor.MiddleLeft);
            TextBlock("8,420", bottom.transform, Rect(0, 0, 0, 1, 104, 0, 178, 0), 22, FontStyle.Bold, gold, TextAnchor.MiddleLeft);
            TextBlock("수리 안정성  A-", bottom.transform, Rect(0, 0, 0, 1, 210, 0, 330, 0), 13, FontStyle.Bold, green, TextAnchor.MiddleLeft);
            TextBlock("예상 랭킹  Top 12%", bottom.transform, Rect(0, 0, 0, 1, 342, 0, 470, 0), 13, FontStyle.Bold, cyan, TextAnchor.MiddleLeft);
            Button("UI만 확인", bottom.transform, Rect(1, 0.5f, 1, 0.5f, -126, -18, -12, 18), coral, Color.white, () => ShowToast("전투 시작 없이 UI 흐름만 확인하는 씬입니다."));
        }

        private void CreateModalLayer(Transform parent)
        {
            var blocker = Panel("Modal Blocker", parent, Stretch(), new Color(0f, 0f, 0f, 0.62f));
            blocker.SetActive(false);
            modalRoots.Add(blocker);
        }

        private void ShowSupportModal(SupportBot bot)
        {
            var modal = Modal("서포트봇 커스텀", 540, 284);
            TextBlock(bot.Name + "  /  " + bot.Type, modal.transform, Rect(0, 1, 1, 1, 22, -66, -22, -36), 20, FontStyle.Bold, bot.Accent, TextAnchor.MiddleLeft);
            TextBlock(bot.Role + " - " + bot.Bonus, modal.transform, Rect(0, 1, 1, 1, 22, -92, -22, -66), 13, FontStyle.Bold, text, TextAnchor.MiddleLeft);

            CreateUpgradeRow(modal.transform, "섀시", "내구/무게", "Lv 4", bot.Cost, Rect(0, 1, 1, 1, 22, -140, -22, -104));
            CreateUpgradeRow(modal.transform, "작업 암", "수리 속도", "Lv 3", "RC 80", Rect(0, 1, 1, 1, 22, -182, -22, -146));
            CreateUpgradeRow(modal.transform, "AI 루틴", "전투 후 판단", "Lv 2", "VC 60", Rect(0, 1, 1, 1, 22, -224, -22, -188));

            Button("이 편성 적용", modal.transform, Rect(1, 0, 1, 0, -142, 18, -22, 52), bot.Accent, new Color(0.02f, 0.05f, 0.08f), () => ShowToast(bot.Name + " 커스텀을 적용했습니다."));
        }

        private void ShowRankingModal()
        {
            var modal = Modal("편성 랭킹", 560, 294);
            TextBlock("@vector_claw 랭킹 보드", modal.transform, Rect(0, 1, 1, 1, 22, -66, -22, -38), 20, FontStyle.Bold, gold, TextAnchor.MiddleLeft);
            string[] rows =
            {
                "01  Iron Choir     9,820  /  2족 스트라이커 + 드론 분석",
                "02  Vector Lab     8,420  /  복서 + 바퀴 + 4족 + 2족 정비",
                "03  Null Arena     7,960  /  중장갑 + 수리 특화",
                "04  Blue Socket    7,510  /  카메라 센서 특화",
                "05  Punch Garden   7,120  /  비용 절감 빌드"
            };

            for (int i = 0; i < rows.Length; i++)
            {
                TextBlock(rows[i], modal.transform, Rect(0, 1, 1, 1, 26, -104 - i * 34, -24, -78 - i * 34), 13, i == 1 ? FontStyle.Bold : FontStyle.Normal, i == 1 ? cyan : text, TextAnchor.MiddleLeft);
            }

            Button("내 편성 분석", modal.transform, Rect(1, 0, 1, 0, -142, 18, -22, 52), cyan, new Color(0.02f, 0.05f, 0.08f), () => ShowToast("정비봇 3개 조합으로 회복형 랭킹에 유리합니다."));
        }

        private void ShowPartModal(string title, string description)
        {
            var modal = Modal(title, 500, 248);
            TextBlock(description, modal.transform, Rect(0, 1, 1, 1, 22, -78, -22, -42), 15, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            TextBlock("측정 결과, 코인 요구량, 위험도는 여기에서 확인합니다. 실제 게임에서는 이 패널이 부품 교체/업그레이드 UI로 연결됩니다.", modal.transform, Rect(0, 1, 1, 1, 22, -128, -22, -82), 12, FontStyle.Normal, muted, TextAnchor.UpperLeft);
            Button("업그레이드 후보 보기", modal.transform, Rect(0, 0, 0, 0, 22, 20, 172, 54), gold, new Color(0.02f, 0.05f, 0.08f), () => ShowToast(title + " 후보를 열었습니다."));
        }

        private void ShowSectionModal(string section)
        {
            var modal = Modal(section, 500, 250);
            string detail = section == "정비실"
                ? "서포트봇 3개를 선택하고, 4족/바퀴/2족 타입별로 커스텀합니다."
                : section == "업그레이드"
                    ? "측정실, 전원설계, 배선, 카메라/센서 탭으로 전투봇 부품을 강화합니다."
                    : section == "리그"
                        ? "전투는 아직 시작하지 않고, 편성 점수와 랭킹만 확인합니다."
                        : "현재는 UI 프로토타입 흐름 확인용 화면입니다.";
            TextBlock(detail, modal.transform, Rect(0, 1, 1, 1, 24, -96, -24, -44), 16, FontStyle.Bold, text, TextAnchor.UpperLeft);
        }

        private void CreateUpgradeRow(Transform parent, string label, string desc, string level, string cost, RectTransformPreset rect)
        {
            var row = Panel(label + " Row", parent, rect, new Color(1f, 1f, 1f, darkMode ? 0.08f : 0.5f));
            TextBlock(label, row.transform, Rect(0, 0, 0, 1, 12, 0, 86, 0), 13, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            TextBlock(desc, row.transform, Rect(0, 0, 0, 1, 88, 0, 222, 0), 11, FontStyle.Normal, muted, TextAnchor.MiddleLeft);
            TextBlock(level, row.transform, Rect(1, 0, 1, 1, -142, 0, -84, 0), 12, FontStyle.Bold, cyan, TextAnchor.MiddleCenter);
            TextBlock(cost, row.transform, Rect(1, 0, 1, 1, -78, 0, -12, 0), 12, FontStyle.Bold, gold, TextAnchor.MiddleRight);
        }

        private void CreateSlot(Transform parent, string label, string value, RectTransformPreset rect, Color color, UnityEngine.Events.UnityAction onClick)
        {
            var slot = Panel(label + " Slot", parent, rect, panelStrong);
            Image("Dot", slot.transform, Rect(0, 0.5f, 0, 0.5f, 10, -6, 22, 6), color);
            TextBlock(label, slot.transform, Rect(0, 1, 1, 1, 30, -26, -8, -8), 12, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            TextBlock(value, slot.transform, Rect(0, 0, 1, 0, 30, 8, -8, 26), 10, FontStyle.Bold, color, TextAnchor.MiddleLeft);
            var button = slot.AddComponent<Button>();
            button.transition = Selectable.Transition.ColorTint;
            button.onClick.AddListener(onClick);
        }

        private GameObject Modal(string title, float width, float height)
        {
            HideModals();
            var blocker = modalRoots[0];
            blocker.SetActive(true);

            var modal = Panel(title + " Modal", blocker.transform, Rect(0.5f, 0.5f, 0.5f, 0.5f, -width / 2, -height / 2, width / 2, height / 2), panelStrong);
            TextBlock(title, modal.transform, Rect(0, 1, 1, 1, 22, -36, -58, -8), 18, FontStyle.Bold, text, TextAnchor.MiddleLeft);
            Button("X", modal.transform, Rect(1, 1, 1, 1, -48, -38, -14, -8), coral, Color.white, HideModals);
            return modal;
        }

        private void ShowToast(string message)
        {
            var modal = Modal("알림", 360, 150);
            TextBlock(message, modal.transform, Rect(0, 0, 1, 1, 24, 24, -24, -48), 15, FontStyle.Bold, text, TextAnchor.MiddleCenter);
        }

        private void HideModals()
        {
            if (modalRoots.Count == 0)
            {
                return;
            }

            var blocker = modalRoots[0];
            for (int i = blocker.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(blocker.transform.GetChild(i).gameObject);
            }
            blocker.SetActive(false);
        }

        private Image Image(string name, Transform parent, RectTransformPreset rect, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            ApplyRect(go.GetComponent<RectTransform>(), rect);
            var image = go.GetComponent<Image>();
            image.color = color;
            return image;
        }

        private GameObject Panel(string name, Transform parent, RectTransformPreset rect, Color color)
        {
            return Image(name, parent, rect, color).gameObject;
        }

        private Text TextBlock(string value, Transform parent, RectTransformPreset rect, int size, FontStyle style, Color color, TextAnchor anchor)
        {
            var go = new GameObject(value, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            ApplyRect(go.GetComponent<RectTransform>(), rect);
            var textBlock = go.GetComponent<Text>();
            textBlock.text = value;
            textBlock.font = font;
            textBlock.fontSize = size;
            textBlock.fontStyle = style;
            textBlock.color = color;
            textBlock.alignment = anchor;
            textBlock.resizeTextForBestFit = true;
            textBlock.resizeTextMinSize = Mathf.Max(8, size - 4);
            textBlock.resizeTextMaxSize = size;
            return textBlock;
        }

        private Button Button(string label, Transform parent, RectTransformPreset rect, Color color, Color labelColor, UnityEngine.Events.UnityAction onClick)
        {
            var go = Panel(label + " Button", parent, rect, color);
            var button = go.AddComponent<Button>();
            button.transition = Selectable.Transition.ColorTint;
            button.onClick.AddListener(onClick);
            TextBlock(label, go.transform, Stretch(6, 2, -6, -2), 12, FontStyle.Bold, labelColor, TextAnchor.MiddleCenter);
            return button;
        }

        private void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() != null)
            {
                return;
            }

            var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            go.transform.SetParent(transform, false);
        }

        private void ClearChildren()
        {
            modalRoots.Clear();
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private static RectTransformPreset Stretch(float left = 0, float bottom = 0, float right = 0, float top = 0)
        {
            return Rect(0, 0, 1, 1, left, bottom, right, top);
        }

        private static RectTransformPreset Rect(float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY, float left, float bottom, float right, float top)
        {
            return new RectTransformPreset
            {
                AnchorMin = new Vector2(anchorMinX, anchorMinY),
                AnchorMax = new Vector2(anchorMaxX, anchorMaxY),
                OffsetMin = new Vector2(left, bottom),
                OffsetMax = new Vector2(right, top)
            };
        }

        private static void ApplyRect(RectTransform rect, RectTransformPreset preset)
        {
            rect.anchorMin = preset.AnchorMin;
            rect.anchorMax = preset.AnchorMax;
            rect.offsetMin = preset.OffsetMin;
            rect.offsetMax = preset.OffsetMax;
        }

        private struct RectTransformPreset
        {
            public Vector2 AnchorMin;
            public Vector2 AnchorMax;
            public Vector2 OffsetMin;
            public Vector2 OffsetMax;
        }
    }
}
