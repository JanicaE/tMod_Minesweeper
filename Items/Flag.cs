using Microsoft.Xna.Framework;
using Minesweeper.Tiles;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Minesweeper.Items
{
    internal class Flag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minesweeping Tool");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "扫雷工具");
            Tooltip.SetDefault("Use it if you want flaging");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "想插旗的话就用它吧");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;

            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;

            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool CanUseItem(Player player)
        {
            int x = (int)Main.MouseWorld.X / 16;
            int y = (int)Main.MouseWorld.Y / 16;
            Tile tile = Main.tile[x, y];
            if (player.altFunctionUse == 0)  // 左键
            {
                if (tile.TileType == ModContent.TileType<Blank_Known>())
                {
                    int num = tile.TileFrameY / 18;
                    Point[] points = {
                        new(x - 1, y - 1),
                        new(x - 1, y),
                        new(x - 1, y + 1),
                        new(x, y - 1),
                        new(x, y + 1),
                        new(x + 1, y - 1),
                        new(x + 1, y),
                        new(x + 1, y + 1)
                    };
                    int count = (from Point p in points
                                where 
                                    ((Main.tile[p].TileType == ModContent.TileType<Mine_Unknown>() ||
                                    Main.tile[p].TileType == ModContent.TileType<Blank_Unknown>()) &&
                                    Main.tile[p].TileFrameY == 18) || 
                                    Main.tile[p].TileType == ModContent.TileType<Mine_Known>()                                
                                select p).Count();
                    if (count == num)
                    {
                        foreach (Point p in points)
                        {
                            if (Main.tile[p].TileType == ModContent.TileType<Mine_Unknown>() ||
                                Main.tile[p].TileType == ModContent.TileType<Blank_Unknown>())
                            {
                                WorldGen.KillTile(p.X, p.Y, fail: true);
                            }
                        }
                    }
                }
                else if (tile.TileType == ModContent.TileType<Blank_Unknown>() ||
                        tile.TileType == ModContent.TileType<Mine_Unknown>())
                {
                    WorldGen.KillTile(x, y, fail: true);
                }
            }
            else if (player.altFunctionUse == 2)  // 右键
            {
                if (tile.TileType == ModContent.TileType<Blank_Unknown>() ||
                    tile.TileType == ModContent.TileType<Mine_Unknown>())
                {
                    if (tile.TileFrameY == 18)
                    {
                        tile.TileFrameY = 0;
                    }
                    else if (tile.TileFrameY == 0)
                    {
                        tile.TileFrameY = 18;
                    }
                }
            }            
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 2)
                .AddIngredient(ItemID.Silk, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
