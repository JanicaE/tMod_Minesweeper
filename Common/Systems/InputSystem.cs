using Microsoft.Xna.Framework.Input;
using Minesweeper.Content.Tiles;
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
#endif
                text = null;
            }
        }
    }
}
