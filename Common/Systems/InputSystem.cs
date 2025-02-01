using Microsoft.Xna.Framework.Input;
using Minesweeper.Common.UIs;
using Terraria;
using Terraria.ModLoader;

namespace Minesweeper.Common.Systems
{
    internal class InputSystem : ModSystem
    {
        string text;

        public override void PostUpdateEverything()
        {
            // 简单的获取聊天框字符串的功能
            if (Main.chatText != "")
            {
                text = Main.chatText;
            }
            if (Main.inputText.IsKeyDown(Keys.Enter) && text != null)
            {
                #region 处理输入命令
#if DEBUG
                // Debug，查看ModTileType
                if (text.StartsWith("/modtiletype"))
                {
                    string typeStr = string.Empty;
                    foreach (int type in MineTiles)
                    {
                        typeStr += type.ToString() + " ";
                    }
                    Main.NewText(typeStr);
                }
                // Debug，打开DebugUI
                if (text.StartsWith("/debug"))
                {
                    DebugUI.Visible = !DebugUI.Visible;
                }
#endif
                #endregion
                text = null;
            }
        }
    }
}
