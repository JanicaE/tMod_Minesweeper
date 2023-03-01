using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Content.Tiles;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Minesweeper.Common.Utils
{
    internal static class MyUtils
    {
        /// <summary>
        /// Minesweeper相关物块
        /// </summary>
        public static int[] MineTiles =
        {
            ModContent.TileType<Blank_Known>(),
            ModContent.TileType<Blank_Unknown>(),
            ModContent.TileType<Mine_Known>(),
            ModContent.TileType<Mine_Unknown>(),
        };

        /// <summary>
        /// 返回周围8个位置坐标的数组
        /// </summary>
        public static Point[] RoundPoints(int i, int j)
        {
            Point[] points = {
                new(i - 1, j - 1),
                new(i - 1, j),
                new(i - 1, j + 1),
                new(i, j - 1),
                new(i, j + 1),
                new(i + 1, j - 1),
                new(i + 1, j),
                new(i + 1, j + 1)
            };
            return points;
        }

        /// <summary>
        /// 返回周围8个位置中雷的个数
        /// </summary>
        public static int MinesCount(int i, int j)
        {
            Point[] points = RoundPoints(i, j);
            int count = (from Point p in points
                         where Main.tile[p].TileType == ModContent.TileType<Mine_Unknown>() ||
                             Main.tile[p].TileType == ModContent.TileType<Mine_Known>()
                         select p).Count();

            Tile tile = Main.tile[i, j];
            tile.TileFrameX = (short)(count * 18);

            return count;
        }

        /// <summary>
        /// 返回鼠标与玩家之间的距离
        /// </summary>
        public static float MouseDistance()
        {
            Vector2 player = Main.LocalPlayer.Center;
            Vector2 mouse = Main.MouseWorld;
            return (mouse - player).Length();
        }

        /// <summary>
        /// ModContent.Request<Texture2D>()的简略写法
        /// </summary>
        public static Asset<Texture2D> GetTexture(string fileName)
        {
            return ModContent.Request<Texture2D>($"Minesweeper/Assets/Images/{fileName}", AssetRequestMode.ImmediateLoad);
        }
    }
}
