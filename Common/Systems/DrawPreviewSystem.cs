using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Minesweeper.Common.Systems
{
    /// <summary>
    /// 预览方框绘制系统（一次只能绘制一个）
    /// </summary>
    internal class DrawPreviewSystem : ModSystem
    {
        private static Texture2D textureT;
        private static Texture2D textureF;
        private static Rectangle? rectangle = null;

        /// <summary>
        /// 方框绘制
        /// </summary>
        /// <param name="textureT"></param>
        /// <param name="textureF"></param>
        /// <param name="rectangle">绘制的位置，用物块坐标表示</param>
        public static void NewBox(Texture2D textureT, Texture2D textureF, Rectangle rectangle)
        {
            DrawPreviewSystem.textureT = textureT;
            DrawPreviewSystem.textureF = textureF;
            DrawPreviewSystem.rectangle = rectangle;
        }

        /// <summary>
        /// 清除方框绘制
        /// </summary>
        public static void Clear()
        {
            rectangle = null;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // 寻找绘制层，并且返回那一层的索引
            int Index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Ruler"));
            if (Index != -1)
            {
                // 往绘制层集合插入一个成员，第一个参数是插入的地方的索引，第二个参数是绘制层
                layers.Insert(Index, new LegacyGameInterfaceLayer(
                    // 绘制层的名字
                    "Test : Preview",
                    // 匿名方法
                    delegate
                    {
                        if (rectangle != null)
                        {
                            int x = rectangle.Value.X;
                            int y = rectangle.Value.Y;
                            int width = rectangle.Value.Width;
                            int height = rectangle.Value.Height;
                            Texture2D texture;
                            for (int i = x; i < x + width; i++)
                            {
                                for (int j = y; j < y + height; j++)
                                {
                                    // 根据物块情况选择该物块内绘制的内容
                                    if (Main.tile[i, j].HasTile && !MineTiles.Contains(Main.tile[i, j].TileType))
                                    {
                                        texture = textureF;
                                    }
                                    else
                                    {
                                        texture = textureT;
                                    }
                                    Main.spriteBatch.Draw(texture,
                                                        new Vector2(i, j) * 16f - Main.screenPosition,
                                                        null,
                                                        Color.White * 0.5f,
                                                        0f,
                                                        Vector2.Zero,
                                                        16f,
                                                        SpriteEffects.None,
                                                        0f);
                                }
                            }

                        }
                        return true;
                    },
                    // 绘制层的类型
                    InterfaceScaleType.Game)
                );
            }
        }
    }
}
