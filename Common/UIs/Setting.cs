using Microsoft.Xna.Framework;
using Minesweeper.Common.Players;
using Minesweeper.Content.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Minesweeper.Common.UIs
{
    internal class Setting : UIState
    {
        public static bool Visible = false;

        private readonly DragablePanel panel = new();
        private readonly HoverImageButton close = new(GetTexture("Close"));
        private readonly HoverImageButton clear = new(GetTexture("Clear"));
        private readonly HoverImageButton reset = new(GetTexture("Reset"));
        private readonly HoverImageButton preview = new(GetTexture("Preview"));
        private readonly HoverImageButton breakable = new(GetTexture("Unbreakable_UI"));
        private readonly UIText title = new(Language.GetText("Mods.Minesweeper.UITips.Title"));
        private readonly UIText tipWidth = new(Language.GetText("Mods.Minesweeper.UITips.Width"));
        private readonly UIText tipHeight = new(Language.GetText("Mods.Minesweeper.UITips.Height"));
        private readonly UIText tipMine = new(Language.GetText("Mods.Minesweeper.UITips.MineNum"));
        private readonly WriteTextBox mapWidthSet = new("");
        private readonly WriteTextBox mapHeightSet = new("");
        private readonly WriteTextBox mineNumSet = new("");

        public override void OnInitialize()
        {
            panel.Width.Set(300f, 0f);
            panel.Height.Set(300f, 0f);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            close.Width.Set(20f, 0f);
            close.Height.Set(20f, 0f);
            close.HAlign = 1f;
            close.VAlign = 0f;
            close.OnLeftClick += Close_OnLeftClick;
            close.hoverText = Language.GetText("Mods.Minesweeper.UITips.Close");
            panel.Append(close);

            clear.Width.Set(30f, 0f);
            clear.Height.Set(30f, 0f);
            clear.HAlign = 0.2f;
            clear.VAlign = 0.9f;
            clear.OnLeftClick += Clear_OnLeftClick;
            clear.hoverText = Language.GetText("Mods.Minesweeper.UITips.Clear");
            panel.Append(clear);

            reset.Width.Set(30f, 0f);
            reset.Height.Set(30f, 0f);
            reset.HAlign = 0.4f;
            reset.VAlign = 0.9f;
            reset.OnLeftClick += Reset_OnLeftClick;
            reset.hoverText = Language.GetText("Mods.Minesweeper.UITips.Reset");
            panel.Append(reset);

            // preview固定为true
            //preview.Width.Set(30f, 0f);
            //preview.Height.Set(30f, 0f);
            //preview.HAlign = 0.6f;
            //preview.VAlign = 0.9f;
            //preview.OnLeftClick += Preview_OnLeftClick;
            //preview.hoverText = Language.GetText("Mods.Minesweeper.UITips.Preview");
            //panel.Append(preview);

            // 这部分配置转到ModConfig中
            //breakable.Width.Set(30f, 0f);
            //breakable.Height.Set(30f, 0f);
            //breakable.HAlign = 0.8f;
            //breakable.VAlign = 0.9f;
            //breakable.OnLeftClick += Breakable_OnLeftClick;
            //breakable.hoverText = Language.GetText("Mods.Minesweeper.UITips.Unbreakable");
            //panel.Append(breakable);

            title.Width.Set(80f, 0f);
            title.Height.Set(20f, 0f);
            title.HAlign = 0f;
            title.VAlign = 0f;
            panel.Append(title);

            tipWidth.Width.Set(80f, 0f);
            tipWidth.Height.Set(20f, 0f);
            tipWidth.HAlign = 0f;
            tipWidth.VAlign = 0.2f;
            tipWidth.TextOriginX = 0f;
            panel.Append(tipWidth);

            tipHeight.Width.Set(80f, 0f);
            tipHeight.Height.Set(20f, 0f);
            tipHeight.HAlign = 0f;
            tipHeight.VAlign = 0.4f;
            tipHeight.TextOriginX = 0f;
            panel.Append(tipHeight);

            tipMine.Width.Set(80f, 0f);
            tipMine.Height.Set(20f, 0f);
            tipMine.HAlign = 0f;
            tipMine.VAlign = 0.6f;
            tipMine.TextOriginX = 0f;
            panel.Append(tipMine);

            mapWidthSet.Width.Set(100f, 0f);
            mapWidthSet.Height.Set(20f, 0f);
            mapWidthSet.HAlign = 0.6f;
            mapWidthSet.VAlign = 0.2f;
            mapWidthSet.TextHAlign = 0f;
            panel.Append(mapWidthSet);

            mapHeightSet.Width.Set(100f, 0f);
            mapHeightSet.Height.Set(20f, 0f);
            mapHeightSet.HAlign = 0.6f;
            mapHeightSet.VAlign = 0.4f;
            mapHeightSet.TextHAlign = 0f;
            panel.Append(mapHeightSet);

            mineNumSet.Width.Set(100f, 0f);
            mineNumSet.Height.Set(20f, 0f);
            mineNumSet.HAlign = 0.6f;
            mineNumSet.VAlign = 0.6f;
            mineNumSet.TextHAlign = 0f;
            panel.Append(mineNumSet);
        }

        #region 点击事件

        private void Close_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Visible = false;
        }

        private void Clear_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (MineTiles.Contains(Main.tile[i, j].TileType))
                    {
                        WorldGen.KillTile(i, j);
                    }
                }
            }
            player.GetModPlayer<MinePlayer>().Remain = 0;
        }

        private void Reset_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            int Mine = 0;
            List<Point> point = [];

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    // 计算区域内雷数
                    if (Main.tile[i, j].TileType == ModContent.TileType<Mine_Unknown>() ||
                        Main.tile[i, j].TileType == ModContent.TileType<Mine_Known>())
                    {
                        Mine++;
                    }
                    if (MineTiles.Contains(Main.tile[i, j].TileType))
                    {
                        // 替换物块
                        WorldGen.PlaceTile(i, j, ModContent.TileType<Blank_Unknown>());
                        Tile tile = Main.tile[i, j];
                        tile.TileFrameX = 0;
                        // 将坐标存入数组，保存当前雷区的位置
                        point.Add(new Point(i, j));
                    }
                }
            }

            // 根据先前计算的雷数重新在区域内随机埋雷
            Random random = new();
            bool MineSet = false;
            for (int num = 0; num < Mine; num++)
            {
                while (!MineSet)
                {
                    int i = random.Next(0, point.Count);
                    if (Main.tile[point[i]].TileType == ModContent.TileType<Blank_Unknown>())
                    {
                        WorldGen.PlaceTile(point[i].X, point[i].Y, ModContent.TileType<Mine_Unknown>());
                        MineSet = true;
                    }
                }
                MineSet = false;
            }

            player.GetModPlayer<MinePlayer>().Remain = point.Count - Mine;
        }

        private void Preview_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().Preview = !player.GetModPlayer<MinePlayer>().Preview;
            // 切换UI图标
            if (player.GetModPlayer<MinePlayer>().Preview)
            {
                preview.SetImage(GetTexture("Preview"));
                preview.Width.Set(30f, 0f);
                preview.Height.Set(30f, 0f);
            }
            else
            {
                preview.SetImage(GetTexture("NoPreview"));
                preview.Width.Set(30f, 0f);
                preview.Height.Set(30f, 0f);
            }
        }

        private void Breakable_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().Breakable = !player.GetModPlayer<MinePlayer>().Breakable;
            // 切换UI图标
            if (player.GetModPlayer<MinePlayer>().Breakable)
            {
                breakable.SetImage(GetTexture("Breakable_UI"));
                breakable.Width.Set(30f, 0f);
                breakable.Height.Set(30f, 0f);
                breakable.hoverText = Language.GetText("Mods.Minesweeper.UITips.Breakable");
            }
            else
            {
                breakable.SetImage(GetTexture("Unbreakable_UI"));
                breakable.Width.Set(30f, 0f);
                breakable.Height.Set(30f, 0f);
                breakable.hoverText = Language.GetText("Mods.Minesweeper.UITips.Unbreakable");
            }
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Player player = Main.LocalPlayer;
            // 如果是初次进入世界，UI要先获取在player中保存的数据
            if (!player.GetModPlayer<MinePlayer>().UILoadData)
            {
                mapWidthSet.Clear();
                mapHeightSet.Clear();
                mineNumSet.Clear();
                mapWidthSet.Write(player.GetModPlayer<MinePlayer>().MapWidth.ToString());
                mapHeightSet.Write(player.GetModPlayer<MinePlayer>().MapHeight.ToString());
                mineNumSet.Write(player.GetModPlayer<MinePlayer>().MineNum.ToString());
                player.GetModPlayer<MinePlayer>().UILoadData = true;
            }
            // 此后将UI中的数据同步至player中
            else
            {
                player.GetModPlayer<MinePlayer>().MapWidth = mapWidthSet.Text.Length > 0 ? int.Parse(mapWidthSet.Text) : 0;
                player.GetModPlayer<MinePlayer>().MapHeight = mapHeightSet.Text.Length > 0 ? int.Parse(mapHeightSet.Text) : 0;
                player.GetModPlayer<MinePlayer>().MineNum = mineNumSet.Text.Length > 0 ? int.Parse(mineNumSet.Text) : 0;
            }
        }
    }

    internal class SettingUISystem : ModSystem
    {
        public Setting setting;
        public UserInterface userInterface;

        public override void Load()
        {
            setting = new Setting();
            setting.Activate();
            userInterface = new UserInterface();
            userInterface.SetState(setting);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (Setting.Visible)
            {
                userInterface?.Update(gameTime);
                //setting.Activate();
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // 寻找绘制层，并且返回那一层的索引
            int Index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (Index != -1)
            {
                // 往绘制层集合插入一个成员，第一个参数是插入的地方的索引，第二个参数是绘制层
                layers.Insert(Index, new LegacyGameInterfaceLayer(
                    // 绘制层的名字
                    "Test : Setting",
                    // 是匿名方法
                    delegate
                    {
                        //当UI开启时
                        if (Setting.Visible)
                        {
                            //绘制UI
                            setting.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    // 绘制层的类型
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
