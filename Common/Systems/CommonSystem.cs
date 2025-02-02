using Microsoft.Xna.Framework;
using Minesweeper.Content.Tiles;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Minesweeper.Common.Systems
{
    internal class CommonSystem : ModSystem
    {
        /// <summary>
        /// Minesweeper相关物块
        /// </summary>
        public static int[] MineTiles =>
        [
            ModContent.TileType<Blank_Known>(),
            ModContent.TileType<Blank_Unknown>(),
            ModContent.TileType<Mine_Known>(),
            ModContent.TileType<Mine_Unknown>(),
        ];

        public static List<Point> MinePoints { get; set; }
    }
}
