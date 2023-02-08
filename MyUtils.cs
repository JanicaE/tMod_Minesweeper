using Microsoft.Xna.Framework;
using Minesweeper.Tiles;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Minesweeper
{
    internal static class MyUtils
    {
        public static int[] MineTiles =
        {
            ModContent.TileType<Blank_Known>(),
            ModContent.TileType<Blank_Unknown>(),
            ModContent.TileType<Mine_Known>(),
            ModContent.TileType<Mine_Unknown>(),
        };

        public static int MinesCount(int i, int j)
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
            int count = (from Point p in points
                         where Main.tile[p].TileType == ModContent.TileType<Mine_Unknown>() ||
                             Main.tile[p].TileType == ModContent.TileType<Mine_Known>()
                         select p).Count();

            Tile tile = Main.tile[i, j];
            tile.TileFrameX = (short)(count * 18);

            return count;
        }

        public static float MouseDistance()
        {
            Vector2 player = Main.LocalPlayer.Center;
            Vector2 mouse = Main.MouseWorld;
            return (mouse - player).Length();
        }
    }
}
