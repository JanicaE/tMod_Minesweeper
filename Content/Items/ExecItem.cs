using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Common.Players;
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
        private int MapWidth;
        private int MapHeight;
        private int MineNum;
        //private bool Breakable;

        public override void SetDefaults()
        {
            Item.width = 31;
            Item.height = 31;
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
            // 左键
            if (player.altFunctionUse == 0)  
            {
                int x = (int)Main.MouseWorld.X / 16;
                int y = (int)Main.MouseWorld.Y / 16;

                // 判定在某些条件下生成失败
                // 设置为不可破坏地形且目标区域内存在除扫雷以外的物块时                
                if (HasTile())
                {
                    Main.NewText(Language.GetText("Mods.Minesweeper.Items.ExecItem.MainText.False"), new Color(255, 0, 0));
                    return true;
                }
                // 宽或高为0时
                if (MapWidth * MapHeight == 0)
                {
                    Main.NewText(Language.GetText("Mods.Minesweeper.Items.ExecItem.MainText.Zero"), new Color(255, 0, 0));
                    return true;
                }
                // 雷数大于区域面积时
                if (MineNum > MapWidth * MapHeight)
                {
                    Main.NewText(Language.GetText("Mods.Minesweeper.Items.ExecItem.MainText.Over"), new Color(255, 0, 0));
                    return true;
                }

                // 先生成一片空白区域
                for (int i = x; i < x + MapWidth; i++)
                {
                    for (int j = y; j < y + MapHeight; j++)
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
                for (int num = 0; num < MineNum; num++)
                {
                    while (!MineSet)
                    {
                        int i = random.Next(x, x + MapWidth);
                        int j = random.Next(y, y + MapHeight);
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
                for (int i = x - 1; i < x + MapWidth + 1; i++)
                {
                    if (Main.tile[i, y - 1].TileType == ModContent.TileType<Blank_Known>())
                    {
                        MinesCount(i, y - 1);
                    }
                    if (Main.tile[i, y + MapHeight].TileType == ModContent.TileType<Blank_Known>())
                    {
                        MinesCount(i, y + MapHeight);
                    }
                }
                for (int i = y; i < y + MapWidth; i++)
                {
                    if (Main.tile[x - 1, i].TileType == ModContent.TileType<Blank_Known>())
                    {
                        MinesCount(x - 1, i);
                    }
                    if (Main.tile[x + MapWidth, i].TileType == ModContent.TileType<Blank_Known>())
                    {
                        MinesCount(x + MapWidth, i);
                    }
                }
            }
            // 右键
            else if (player.altFunctionUse == 2)  
            {
                // 切换设置界面打开关闭状态
                if (Setting.Visible)
                {
                    Setting.Visible = false;
                }
                else
                {
                    Setting.Visible = true;
                }
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {
            int x = (int)Main.MouseWorld.X / 16;
            int y = (int)Main.MouseWorld.Y / 16;
            Rectangle rectangle = new(x, y, MapWidth, MapHeight);
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
                Box.NewBox(textureT, textureF, rectangle);
#if DEBUG
                //Debug，查看当前TileType
                int type = Main.tile[x, y].TileType;
                Main.NewText(type);
#endif
            }
            // 反之则清空Box对象
            else
            {
                Box.Clear();
            }
        }

        public override void UpdateInventory(Player player)
        {
            // 实时从玩家处更新参数
            MapWidth = player.GetModPlayer<MinePlayer>().MapWidth;
            MapHeight = player.GetModPlayer<MinePlayer>().MapHeight;
            MineNum = player.GetModPlayer<MinePlayer>().MineNum;
            //Breakable = player.GetModPlayer<MinePlayer>().Breakable;

            // 当前没有手持该物品时也清空Box对象
            if (Main.LocalPlayer.HeldItem.type != Type)
            {
                Box.Clear();
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

        /// <summary>
        /// 在设置为Unbreakable的情况下，判断区域内是否有mod相关物块以外的物块
        /// </summary>
        private bool HasTile()
        {
            int x = (int)Main.MouseWorld.X / 16;
            int y = (int)Main.MouseWorld.Y / 16;
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    if (Main.tile[x + i, y + j].HasTile && !MineTiles.Contains(Main.tile[x + i, y + j].TileType) && !Config.BlockBreakable)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
