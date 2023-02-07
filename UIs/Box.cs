using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Minesweeper.UIs
{
    internal class Box
    {
        public Texture2D textureT;
        public Texture2D textureF;
        public Rectangle rectangle;

        public Box(Texture2D textureT, Texture2D textureF, Rectangle rectangle)
        {
            this.textureT = textureT;
            this.textureF = textureF;
            this.rectangle = rectangle;
        }

        public static void newBox(Texture2D textureT, Texture2D textureF, Rectangle rectangle)
        {
            Box box = new(textureT, textureF, rectangle);
            BoxSystem.box = box;
        }

        public static void clear()
        {
            BoxSystem.box = null;
        }
    }

    internal class BoxSystem : ModSystem 
    {
        public static Box box;
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
                        if(box != null)
                        {
                            int x = box.rectangle.X;
                            int y = box.rectangle.Y;
                            int width = box.rectangle.Width;
                            int height = box.rectangle.Height;
                            Texture2D texture;
                            for (int i = x; i < x + width; i++)
                            {
                                for (int j = y; j < y + height; j++)
                                {
                                    if (Main.tile[i, j].HasTile && !MyUtils.MineTiles.Contains(Main.tile[i, j].TileType))
                                    {
                                        texture = box.textureF;
                                    }
                                    else
                                    {
                                        texture = box.textureT;
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
