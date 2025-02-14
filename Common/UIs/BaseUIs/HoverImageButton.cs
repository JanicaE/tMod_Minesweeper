﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace Minesweeper.Common.UIs.BaseUIs
{
    /// <summary>
    /// 允许按钮在悬停时显示文本工具提示
    /// </summary>
    internal class HoverImageButton : UIImageButton
    {
        /// <summary>
        /// 悬停时将显示的工具提示文本
        /// </summary>
        public LocalizedText hoverText;

        public HoverImageButton(Asset<Texture2D> texture) : base(texture)
        {
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // 重写UIElement方法时，不要忘记调用基方法
            // 这有助于保持 UIElement 的基本行为
            base.DrawSelf(spriteBatch);

            // 当鼠标悬停在当前UIElement上时，IsMouse悬停变为真
            if (IsMouseHovering)
                Main.hoverItemName = hoverText.ToString();
        }
    }
}
