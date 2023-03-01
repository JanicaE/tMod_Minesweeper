using Microsoft.Xna.Framework;
using Minesweeper.Common.Utils;
using Minesweeper.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Minesweeper.Content.Tiles
{
    [Autoload(true)]
    internal class Blank_Unknown : MineTile
    {
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            // 插旗状态时不可操作
            if (Main.tile[i, j].TileFrameX == 18)
            {
                return;
            }

            // 计算雷数并给物块标上数字
            WorldGen.PlaceTile(i, j, ModContent.TileType<Blank_Known>());
            int count = MyUtils.MinesCount(i, j);

            // 如果周围8格没有雷则打开它们
            if (count == 0)
            {
                Point[] points = MyUtils.RoundPoints(i, j);
                foreach (Point p in points)
                {
                    if (Main.tile[p].TileType == ModContent.TileType<Blank_Unknown>())
                    {
                        WorldGen.KillTile(p.X, p.Y, fail: true);
                    }
                }
            }

            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().Remain--;
            // 如果地图上已不存在未打开的空白区域
            if (player.GetModPlayer<MinePlayer>().Remain == 0)
            {
                // 将未打开的地雷区域都插上旗
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        if (Main.tile[x, y].TileType == ModContent.TileType<Mine_Unknown>())
                        {
                            Tile tile = Main.tile[x, y];
                            tile.TileFrameX = 18;
                        }
                    }
                }
                // 释放胜利的烟花
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.FireworkFountain_Red, 0f, -10f);
                }
            }
        }
    }

    [Autoload(true)]
    internal class Blank_Known : MineTile
    {
    }
}
