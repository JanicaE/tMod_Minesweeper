using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Common.Players;
using Minesweeper.Common.Systems;
using Minesweeper.Common.UIs;
using Minesweeper.Content.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Minesweeper.Content.Items
{
    internal class ExecItem : ModItem
    {
        #region Settings
        private int MapWidth(Player player) => player.GetModPlayer<MinePlayer>().MapWidth;
        private int MapHeight(Player player) => player.GetModPlayer<MinePlayer>().MapHeight;
        private int MineNum(Player player) => player.GetModPlayer<MinePlayer>().MineNum;
        private int MineDensity(Player player) => player.GetModPlayer<MinePlayer>().MineDensity;
        //private bool breakable;
        #endregion

        private bool cancelled;
        private Point freeStart;
        private Point freeEnd;

        public override void SetDefaults()
        {
            Item.width = 31;
            Item.height = 31;
            Item.maxStack = 1;

            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.autoReuse = true;

            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode is not NetmodeID.Server && player.whoAmI == Main.myPlayer)
            {
                if (player.GetModPlayer<MinePlayer>().MineGenerateType == EnumMineGenerateType.Free.ToString())
                {
                    if (Main.mouseRight && cancelled == false)
                    {
                        cancelled = true;
                        //Main.NewText("Cancel");
                    }
                    freeEnd = Main.MouseWorld.ToTileCoordinates();

                    int x = Math.Min(freeStart.X, freeEnd.X);
                    int y = Math.Min(freeStart.Y, freeEnd.Y);
                    int width = Math.Abs(freeEnd.X - freeStart.X) + 1;
                    int height = Math.Abs(freeEnd.Y - freeStart.Y) + 1;

                    // 绘制预览
                    if (cancelled == false)
                    {                        
                        Rectangle rectangle = new(x, y, width, height);
                        DrawPreview(player, rectangle);
                    }
                    else
                    {
                        DrawPreviewSystem.Clear();
                    }

                    if (Main.mouseLeft)
                    {
                        player.itemAnimation = 8;
                    }
                    else
                    {
                        player.itemAnimation = 0;
                        if (cancelled == false)
                        {
                            //Main.NewText($"Generate: ({freeStart.X},{freeStart.Y}),({freeEnd.X},{freeEnd.Y})");
                            int mineNum = (width * height) * MineDensity(player) / 100;
                            GenerateMap(player, x, y, width, height, mineNum);
                            DrawPreviewSystem.Clear();
                        }
                    }
                }
                else if (player.GetModPlayer<MinePlayer>().MineGenerateType == EnumMineGenerateType.Fixed.ToString())
                {
                    GenerateFixedMap(player);
                }
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<MinePlayer>().MineGenerateType == EnumMineGenerateType.Free.ToString())
            {
                Item.autoReuse = true;
            }
            else if (player.GetModPlayer<MinePlayer>().MineGenerateType == EnumMineGenerateType.Fixed.ToString())
            {
                Item.autoReuse = false;
            }

            if (player.noBuilding)
            {
                return false;
            }

            if (player.altFunctionUse == 2)
            {
                //切换设置界面打开关闭状态
                if (SettingUI.Visible)
                {
                    SettingUI.Visible = false;
                }
                else
                {
                    SettingUI.Visible = true;
                }
                return false;
            }

            if (player.GetModPlayer<MinePlayer>().MineGenerateType == EnumMineGenerateType.Fixed.ToString())
            {
                return true;
            }

            cancelled = false;
            freeStart = Main.MouseWorld.ToTileCoordinates();
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {
            if (player.GetModPlayer<MinePlayer>().MineGenerateType == EnumMineGenerateType.Fixed.ToString())
            {
                int x = (int)Main.MouseWorld.X / 16;
                int y = (int)Main.MouseWorld.Y / 16;
                Rectangle rectangle = new(x, y, MapWidth(player), MapHeight(player));
                DrawPreview(player, rectangle);
            }
        }

        public override void UpdateInventory(Player player)
        {
            // 当前没有手持该物品时清空Box对象
            if (Main.LocalPlayer.HeldItem.type != Type)
            {
                DrawPreviewSystem.Clear();
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // 物品信息中显示未打开的空白区域数目
            Player player = Main.LocalPlayer;
            TooltipLine line = new(Mod, "Remain", $"{Language.GetText("Mods.Minesweeper.Items.ExecItem.Line")}:{player.GetModPlayer<MinePlayer>().Remain}");
            tooltips.Add(line);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Grenade, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        #region 私有方法

        private void GenerateFixedMap(Player player)
        {
            int x = (int)Main.MouseWorld.X / 16;
            int y = (int)Main.MouseWorld.Y / 16;
            int mapWidth = MapWidth(player);
            int mapHeight = MapHeight(player);
            int mineNum = MineNum(player);

            // 判定在某些条件下生成失败
            // 宽或高为0时
            if (mapWidth * mapHeight == 0)
            {
                Main.NewText(Language.GetText("Mods.Minesweeper.Items.ExecItem.MainText.Zero"), new Color(255, 0, 0));
                return;
            }
            // 雷数大于区域面积时
            if (mineNum > mapWidth * mapHeight)
            {
                Main.NewText(Language.GetText("Mods.Minesweeper.Items.ExecItem.MainText.Over"), new Color(255, 0, 0));
                return;
            }

            GenerateMap(player, x, y, mapWidth, mapHeight, mineNum);
            return;
        }

        private void GenerateMap(Player player, int x, int y, int mapWidth, int mapHeight, int mineNum)
        {
            // 设置为不可破坏地形且目标区域内存在除扫雷以外的物块时
            if (!Config.BlockBreakable)
            {
                bool hasTile = false;
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        if (Main.tile[x + i, y + j].HasTile && !MineTiles.Contains(Main.tile[x + i, y + j].TileType))
                        {
                            hasTile = true;
                            break;
                        }
                    }
                    if (hasTile)
                    {
                        break;
                    }
                }
                if (hasTile)
                {
                    Main.NewText(Language.GetText("Mods.Minesweeper.Items.ExecItem.MainText.False"), new Color(255, 0, 0));
                    return;
                }
            }

            // 先生成一片空白区域
            for (int i = x; i < x + mapWidth; i++)
            {
                for (int j = y; j < y + mapHeight; j++)
                {
                    if (Main.tile[i, j].TileType != ModContent.TileType<Blank_Unknown>())
                    {
                        player.GetModPlayer<MinePlayer>().Remain++;
                    }
                    WorldGen.PlaceTile(i, j, ModContent.TileType<Blank_Unknown>());

                    Tile tile = Main.tile[i, j];
                    tile.TileFrameX = 0;
                    tile.TileFrameY = 0;
                }
            }

            // 在区域内随机挑选空白物块替换为地雷物块
            Random random = new();
            bool MineSet = false;
            for (int num = 0; num < mineNum; num++)
            {
                while (!MineSet)
                {
                    int i = random.Next(x, x + mapWidth);
                    int j = random.Next(y, y + mapHeight);
                    if (Main.tile[i, j].TileType == ModContent.TileType<Blank_Unknown>())
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<Mine_Unknown>());
                        player.GetModPlayer<MinePlayer>().Remain--;
                        MineSet = true;
                    }
                }
                MineSet = false;
            }

            // 若新区域边界处存在已知空白区域，重新计算其周围雷数
            for (int i = x - 1; i < x + mapWidth + 1; i++)
            {
                if (Main.tile[i, y - 1].TileType == ModContent.TileType<Blank_Known>())
                {
                    MinesCount(i, y - 1);
                }
                if (Main.tile[i, y + mapHeight].TileType == ModContent.TileType<Blank_Known>())
                {
                    MinesCount(i, y + mapHeight);
                }
            }
            for (int i = y; i < y + mapWidth; i++)
            {
                if (Main.tile[x - 1, i].TileType == ModContent.TileType<Blank_Known>())
                {
                    MinesCount(x - 1, i);
                }
                if (Main.tile[x + mapWidth, i].TileType == ModContent.TileType<Blank_Known>())
                {
                    MinesCount(x + mapWidth, i);
                }
            }
        }

        /// <summary>
        /// 绘制预览
        /// </summary>
        /// <param name="player"></param>
        /// <param name="rectangle"></param>
        private void DrawPreview(Player player, Rectangle rectangle)
        {
            // 无物块区域的预览样式
            Texture2D textureT = GetTexture("Normal_prev").Value;
            // 有物块区域的预览样式
            Texture2D textureF;
            if (!Config.BlockBreakable)
            {
                textureF = GetTexture("Unbreakable_prev").Value;
            }
            else
            {
                textureF = GetTexture("Breakable_prev").Value;
            }
            // 如果开启预览，就生成一个Box对象，对应绘制一个方框
            if (player.GetModPlayer<MinePlayer>().Preview)
            {
                DrawPreviewSystem.NewBox(textureT, textureF, rectangle);
            }
            // 反之则清空Box对象
            else
            {
                DrawPreviewSystem.Clear();
            }
        }

        #endregion
    }
}
