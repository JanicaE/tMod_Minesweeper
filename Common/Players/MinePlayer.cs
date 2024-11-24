using Minesweeper.Content.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Minesweeper.Common.Players
{
    internal class MinePlayer : ModPlayer
    {
        public static string Fixed = "Fixed";
        public static string Free = "Free";

        public int MapWidth;
        public int MapHeight;
        public int MineNum;
        public int MineDensity;
        public string FixedOrFree;

        public bool Preview;
        public bool Breakable;

        public int Remain;
        public bool UILoadData;

        public override void Initialize()
        {
            Preview = true;
            Breakable = false;
        }

        public override void OnEnterWorld()
        {
            Remain = 0;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].TileType == ModContent.TileType<Blank_Unknown>())
                    {
                        Remain++;
                    }
                }
            }
            UILoadData = false;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["MapWidth"] = MapWidth;
            tag["MapHeight"] = MapHeight;
            tag["MineNum"] = MineNum;
            tag["MineDensity"] = MineDensity;
            tag["FixedOrFree"] = FixedOrFree;
        }

        public override void LoadData(TagCompound tag)
        {
            try
            {
                MapWidth = (int)tag["MapWidth"];
                MapHeight = (int)tag["MapHeight"];
                MineNum = (int)tag["MineNum"];
                MineDensity = (int)tag["MineDensity"];
                FixedOrFree = (string)tag["FixedOrFree"];
            }
            catch 
            {
                MapWidth = 10;
                MapHeight = 10;
                MineNum = 10;
                MineDensity = 10;
                FixedOrFree = Fixed;
            }            
        }
    }
}
