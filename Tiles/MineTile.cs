using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Minesweeper.Tiles
{
    [Autoload(false)]
    internal class MineTile : ModTile
    {        
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;  // 是否为实体
            Main.tileSolidTop[Type] = true;  // 顶端能否站人
            Main.tileNoAttach[Type] = false;  // 能否能在物块附近放东西
            Main.tileFrameImportant[Type] = true;  // 帧对齐
            Main.tileOreFinderPriority[Type] = 0;  // 金属探测器优先级
            MineResist = 1f;  // 要抡几下镐子
            MinPick = 9999;  // 最小镐力
            AddMapEntry(new Color(192, 192, 192));  // 在地图上的颜色
        }
    }
}
