using Terraria;
using Terraria.ModLoader;

namespace Minesweeper.Tiles
{
    [Autoload(true)]
    internal class Mine_Unknown : MineTile
    {
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            // 插旗状态时不可操作
            if (Main.tile[i, j].TileFrameX == 18)
            {
                return;
            }
            WorldGen.PlaceTile(i, j, ModContent.TileType<Mine_Known>());
        }
    }

    [Autoload(true)]
    internal class Mine_Known : MineTile
    {
    }
}
