using Microsoft.Xna.Framework;
using Minesweeper.Common.UIs.BaseUIs;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Minesweeper.Common.UIs
{
    internal class DebugUI : UIState
    {
        public static bool Visible = false;

        /// <summary>
        /// 主面板
        /// </summary>
        private readonly DragablePanel panel = new();
        private readonly UIText labelLeftMouse = new("左键:");
        private readonly UIText valueRightMouse = new("");
        private readonly UIText labelRightMouse = new("右键:");
        private readonly UIText valueLeftMouse = new("");

        public override void OnInitialize()
        {
            panel.Width.Set(300f, 0f);
            panel.Height.Set(300f, 0f);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            labelLeftMouse.Width.Set(80f, 0f);
            labelLeftMouse.Height.Set(20f, 0f);
            labelLeftMouse.HAlign = 0f;
            labelLeftMouse.VAlign = 0f;
            panel.Append(labelLeftMouse);

            valueLeftMouse.Width.Set(80f, 0f);
            valueLeftMouse.Height.Set(20f, 0f);
            valueLeftMouse.HAlign = 0.5f;
            valueLeftMouse.VAlign = 0f;
            panel.Append(valueLeftMouse);

            labelRightMouse.Width.Set(80f, 0f);
            labelRightMouse.Height.Set(20f, 0f);
            labelRightMouse.HAlign = 0f;
            labelRightMouse.VAlign = 0.3f;
            panel.Append(labelRightMouse);

            valueRightMouse.Width.Set(80f, 0f);
            valueRightMouse.Height.Set(20f, 0f);
            valueRightMouse.HAlign = 0.5f;
            valueRightMouse.VAlign = 0.3f;
            panel.Append(valueRightMouse);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Visible)
            {
                valueRightMouse.SetText(Main.mouseRight.ToString());
                valueLeftMouse.SetText(Main.mouseLeft.ToString());
            }
        }
    }

    internal class DebugUISystem : ModSystem
    {
        public DebugUI debugUI;
        public UserInterface userInterface;

        public override void Load()
        {
            debugUI = new DebugUI();
            debugUI.Activate();
            userInterface = new UserInterface();
            userInterface.SetState(debugUI);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (DebugUI.Visible)
            {
                userInterface?.Update(gameTime);
                //setting.Activate();
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // 寻找绘制层，并且返回那一层的索引
            int Index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (Index != -1)
            {
                // 往绘制层集合插入一个成员，第一个参数是插入的地方的索引，第二个参数是绘制层
                layers.Insert(Index, new LegacyGameInterfaceLayer(
                    // 绘制层的名字
                    "Test : DebugUI",
                    // 是匿名方法
                    delegate
                    {
                        //当UI开启时
                        if (DebugUI.Visible)
                        {
                            //绘制UI
                            debugUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    // 绘制层的类型
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
