using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Players;
using Minesweeper.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Minesweeper.UIs
{
    internal class Setting : UIState
    {
        public static bool Visible = false;

        private DragablePanel panel = new();
        private HoverImageButton close = new(ModContent.Request<Texture2D>("Minesweeper/UIs/Close"), Language.GetTextValue("Mods.Minesweeper.UITips.Close"));
        private HoverImageButton clear = new(ModContent.Request<Texture2D>("Minesweeper/UIs/Clear"), Language.GetTextValue("Mods.Minesweeper.UITips.Clear"));
        private HoverImageButton reset = new(ModContent.Request<Texture2D>("Minesweeper/UIs/Reset"), Language.GetTextValue("Mods.Minesweeper.UITips.Reset"));
        private HoverImageButton preview = new(ModContent.Request<Texture2D>("Minesweeper/UIs/Preview"), Language.GetTextValue("Mods.Minesweeper.UITips.Preview"));
        private HoverImageButton breakable = new(ModContent.Request<Texture2D>("Minesweeper/UIs/Unbreakable"), Language.GetTextValue("Mods.Minesweeper.UITips.Unbreakable"));
        private UIText title = new(Language.GetTextValue("Mods.Minesweeper.UITips.Title"));
        private UIText tipWidth = new(Language.GetTextValue("Mods.Minesweeper.UITips.Width") + ":");
        private UIText tipHeight = new(Language.GetTextValue("Mods.Minesweeper.UITips.Height") + ":");
        private UIText tipMine = new(Language.GetTextValue("Mods.Minesweeper.UITips.MineNum") + ":");
        private WriteTextBox mapWidthSet = new("");
        private WriteTextBox mapHeightSet = new("");
        private WriteTextBox MineNumSet = new("");

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
            close.OnClick += Close_OnClick;
            panel.Append(close);

            clear.Width.Set(30f, 0f);
            clear.Height.Set(30f, 0f);
            clear.HAlign = 0.2f;
            clear.VAlign = 0.9f;
            clear.OnClick += Clear_OnClick;
            panel.Append(clear);

            reset.Width.Set(30f, 0f);
            reset.Height.Set(30f, 0f);
            reset.HAlign = 0.4f;
            reset.VAlign = 0.9f;
            reset.OnClick += Reset_OnClick;
            panel.Append(reset);

            preview.Width.Set(30f, 0f);
            preview.Height.Set(30f, 0f);
            preview.HAlign = 0.6f;
            preview.VAlign = 0.9f;
            preview.OnClick += Preview_OnClick;
            panel.Append(preview);

            breakable.Width.Set(30f, 0f);
            breakable.Height.Set(30f, 0f);
            breakable.HAlign = 0.8f;
            breakable.VAlign = 0.9f;
            breakable.OnClick += Breakable_OnClick;
            panel.Append(breakable);            
            
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
            
            MineNumSet.Width.Set(100f, 0f);
            MineNumSet.Height.Set(20f, 0f);
            MineNumSet.HAlign = 0.6f;
            MineNumSet.VAlign = 0.6f;
            MineNumSet.TextHAlign = 0f;
            panel.Append(MineNumSet);
        }               

        private void Close_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Visible = false;
        }


        private void Clear_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (MyUtils.MineTiles.Contains(Main.tile[i, j].TileType))
                    {
                        WorldGen.KillTile(i, j);
                    }
                }
            }
            player.GetModPlayer<MinePlayer>().Remain = 0;
        }

        private void Reset_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            int Mine = 0;
            List<Point> point = new();
            // 将地图内相关物块全部替换为未知空白物块
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
                    // 将区域各坐标存入数组
                    if (MyUtils.MineTiles.Contains(Main.tile[i, j].TileType))
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<Blank_Unknown>());
                        Tile tile = Main.tile[i, j];
                        tile.TileFrameY = 0;
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

            player.GetModPlayer<MinePlayer>().Remain = point.Count -  Mine;
        }

        private void Preview_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().Preview = !player.GetModPlayer<MinePlayer>().Preview;
            // 切换UI图标
            if (player.GetModPlayer<MinePlayer>().Preview)
            {
                preview.SetImage(ModContent.Request<Texture2D>("Minesweeper/UIs/Preview"));
                preview.Width.Set(30f, 0f);
                preview.Height.Set(30f, 0f);
            }
            else
            {
                preview.SetImage(ModContent.Request<Texture2D>("Minesweeper/UIs/NoPreview"));
                preview.Width.Set(30f, 0f);
                preview.Height.Set(30f, 0f);
            }
        }

        private void Breakable_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().Breakable = !player.GetModPlayer<MinePlayer>().Breakable;
            // 切换UI图标
            if (player.GetModPlayer<MinePlayer>().Breakable)
            {
                breakable.SetImage(ModContent.Request<Texture2D>("Minesweeper/UIs/Breakable"));
                breakable.Width.Set(30f, 0f);
                breakable.Height.Set(30f, 0f);
                breakable.hoverText = Language.GetTextValue("Mods.Minesweeper.UITips.Breakable");
            }
            else
            {
                breakable.SetImage(ModContent.Request<Texture2D>("Minesweeper/UIs/Unbreakable"));
                breakable.Width.Set(30f, 0f);
                breakable.Height.Set(30f, 0f);
                breakable.hoverText = Language.GetTextValue("Mods.Minesweeper.UITips.Unbreakable");
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Player player = Main.LocalPlayer;
            // 如果是初次进入世界，UI要先获取在player中保存的数据
            if (!player.GetModPlayer<MinePlayer>().UILoadData)
            {
                mapWidthSet.Clear();
                mapHeightSet.Clear();
                MineNumSet.Clear();
                mapWidthSet.Write(player.GetModPlayer<MinePlayer>().MapWidth.ToString());
                mapHeightSet.Write(player.GetModPlayer<MinePlayer>().MapHeight.ToString());
                MineNumSet.Write(player.GetModPlayer<MinePlayer>().MineNum.ToString());
                player.GetModPlayer<MinePlayer>().UILoadData = true;
            }
            // 此后将UI中的数据同步至player中
            else
            {
                player.GetModPlayer<MinePlayer>().MapWidth = mapWidthSet.Text.Length > 0 ? int.Parse(mapWidthSet.Text) : 0;
                player.GetModPlayer<MinePlayer>().MapHeight = mapHeightSet.Text.Length > 0 ? int.Parse(mapHeightSet.Text) : 0;
                player.GetModPlayer<MinePlayer>().MineNum = MineNumSet.Text.Length > 0 ? int.Parse(MineNumSet.Text) : 0;
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
