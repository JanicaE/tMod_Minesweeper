using Microsoft.Xna.Framework;
using Minesweeper.Common.Players;
using Minesweeper.Common.Utils;
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

        #region property

        /// <summary>
        /// 主面板
        /// </summary>
        private readonly DragablePanel panel = new();
        /// <summary>
        /// 按钮：关闭
        /// </summary>
        private readonly HoverImageButton buttonClose = new(GetTexture("Close"));
        /// <summary>
        /// 按钮：清空
        /// </summary>
        private readonly HoverImageButton buttonClear = new(GetTexture("Clear"));
        /// <summary>
        /// 按钮：重置
        /// </summary>
        private readonly HoverImageButton buttonReset = new(GetTexture("Reset"));
        /// <summary>
        /// 按钮：设置预览（弃用）
        /// </summary>
        private readonly HoverImageButton buttonPreview = new(GetTexture("Preview"));
        /// <summary>
        /// 按钮：设置可破坏（弃用）
        /// </summary>
        private readonly HoverImageButton buttonBreakable = new(GetTexture("Unbreakable_UI"));
        /// <summary>
        /// 按钮：固定大小
        /// </summary>
        private readonly HoverImageButton buttonFixedMap = new(GetTexture("Close"));//todo
        /// <summary>
        /// 按钮：自由大小
        /// </summary>
        private readonly HoverImageButton buttonFreeMap = new(GetTexture("Close"));//todo
        /// <summary>
        /// 标签：标题
        /// </summary>
        private readonly UIText labelTitle = new(Language.GetText("Mods.Minesweeper.UITips.Title"));
        /// <summary>
        /// 标签：宽度
        /// </summary>
        private readonly UIText labelWidth = new(Language.GetText("Mods.Minesweeper.UITips.Width"));
        /// <summary>
        /// 标签：高度
        /// </summary>
        private readonly UIText labelHeight = new(Language.GetText("Mods.Minesweeper.UITips.Height"));
        /// <summary>
        /// 标签：雷数
        /// </summary>
        private readonly UIText labelMineNum = new(Language.GetText("Mods.Minesweeper.UITips.MineNum"));
        /// <summary>
        /// 标签：密度
        /// </summary>
        private readonly UIText labelMineDensity = new(Language.GetText("Mods.Minesweeper.UITips.MineDensity"));
        /// <summary>
        /// 输入框：设置宽度
        /// </summary>
        private readonly WriteTextBox textboxSetMapWidth = new("");
        /// <summary>
        /// 输入框：设置高度
        /// </summary>
        private readonly WriteTextBox textboxSetMapHeight = new("");
        /// <summary>
        /// 输入框：设置雷数
        /// </summary>
        private readonly WriteTextBox textboxSetMineNum = new("");
        /// <summary>
        /// 输入框：设置密度
        /// </summary>
        private readonly WriteTextBox textboxSetMineDensity = new("");

        #endregion

        public override void OnInitialize()
        {
            #region panel
            panel.Width.Set(300f, 0f);
            panel.Height.Set(300f, 0f);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);
            #endregion

            #region buttonClose
            buttonClose.Width.Set(20f, 0f);
            buttonClose.Height.Set(20f, 0f);
            buttonClose.HAlign = 1f;
            buttonClose.VAlign = 0f;
            buttonClose.OnLeftClick += ButtonClose_OnLeftClick;
            buttonClose.hoverText = Language.GetText("Mods.Minesweeper.UITips.Close");
            panel.Append(buttonClose);
            #endregion

            #region buttonClear
            buttonClear.Width.Set(30f, 0f);
            buttonClear.Height.Set(30f, 0f);
            buttonClear.HAlign = 0.2f;
            buttonClear.VAlign = 0.9f;
            buttonClear.OnLeftClick += ButtonClear_OnLeftClick;
            buttonClear.hoverText = Language.GetText("Mods.Minesweeper.UITips.Clear");
            panel.Append(buttonClear);
            #endregion

            #region buttonReset
            buttonReset.Width.Set(30f, 0f);
            buttonReset.Height.Set(30f, 0f);
            buttonReset.HAlign = 0.4f;
            buttonReset.VAlign = 0.9f;
            buttonReset.OnLeftClick += ButtonReset_OnLeftClick;
            buttonReset.hoverText = Language.GetText("Mods.Minesweeper.UITips.Reset");
            panel.Append(buttonReset);
            #endregion

            #region buttonPreview(弃用)
            /* Preview固定为true
            //buttonPreview.Width.Set(30f, 0f);
            //buttonPreview.Height.Set(30f, 0f);
            //buttonPreview.HAlign = 0.6f;
            //buttonPreview.VAlign = 0.9f;
            //buttonPreview.OnLeftClick += Preview_OnLeftClick;
            //buttonPreview.hoverText = Language.GetText("Mods.Minesweeper.UITips.Preview");
            //panel.Append(buttonPreview);
            */
            #endregion

            #region buttonBreakable(弃用)
            /* Breakable配置转到ModConfig中
            //buttonBreakable.Width.Set(30f, 0f);
            //buttonBreakable.Height.Set(30f, 0f);
            //buttonBreakable.HAlign = 0.8f;
            //buttonBreakable.VAlign = 0.9f;
            //buttonBreakable.OnLeftClick += Breakable_OnLeftClick;
            //buttonBreakable.hoverText = Language.GetText("Mods.Minesweeper.UITips.Unbreakable");
            //panel.Append(buttonBreakable);
            */
            #endregion

            #region buttonFixedMap
            buttonFixedMap.Width.Set(30f, 0f);
            buttonFixedMap.Height.Set(30f, 0f);
            buttonFixedMap.HAlign = 0.1f;
            buttonFixedMap.VAlign = 0.1f;
            buttonFixedMap.OnLeftClick += ButtonFixedMap_OnLeftClick;
            buttonFixedMap.hoverText = Language.GetText("Mods.Minesweeper.UITips.FixedMap");
            panel.Append(buttonFixedMap);
            #endregion

            #region buttonFreeMap
            buttonFreeMap.Width.Set(30f, 0f);
            buttonFreeMap.Height.Set(30f, 0f);
            buttonFreeMap.HAlign = 0.3f;
            buttonFreeMap.VAlign = 0.1f;
            buttonFreeMap.OnLeftClick += ButtonFreeMap_OnLeftClick; ;
            buttonFreeMap.hoverText = Language.GetText("Mods.Minesweeper.UITips.FreeMap");
            panel.Append(buttonFreeMap);
            #endregion

            #region labelTitle
            labelTitle.Width.Set(80f, 0f);
            labelTitle.Height.Set(20f, 0f);
            labelTitle.HAlign = 0f;
            labelTitle.VAlign = 0f;
            panel.Append(labelTitle);
            #endregion

            #region labelWidth
            labelWidth.Width.Set(80f, 0f);
            labelWidth.Height.Set(20f, 0f);
            labelWidth.HAlign = 0f;
            labelWidth.VAlign = 0.25f;
            labelWidth.TextOriginX = 0f;
            panel.Append(labelWidth);
            #endregion

            #region labelHeight
            labelHeight.Width.Set(80f, 0f);
            labelHeight.Height.Set(20f, 0f);
            labelHeight.HAlign = 0f;
            labelHeight.VAlign = 0.45f;
            labelHeight.TextOriginX = 0f;
            panel.Append(labelHeight);
            #endregion

            #region labelMineNum
            labelMineNum.Width.Set(80f, 0f);
            labelMineNum.Height.Set(20f, 0f);
            labelMineNum.HAlign = 0f;
            labelMineNum.VAlign = 0.65f;
            labelMineNum.TextOriginX = 0f;
            panel.Append(labelMineNum);
            #endregion

            #region labelMineDensity
            labelMineDensity.Width.Set(80f, 0f);
            labelMineDensity.Height.Set(20f, 0f);
            labelMineDensity.HAlign = 0f;
            labelMineDensity.VAlign = 0.25f;
            labelMineDensity.TextOriginX = 0f;
            panel.Append(labelMineDensity);
            #endregion

            #region textboxSetMapWidth
            textboxSetMapWidth.Width.Set(100f, 0f);
            textboxSetMapWidth.Height.Set(20f, 0f);
            textboxSetMapWidth.HAlign = 0.6f;
            textboxSetMapWidth.VAlign = 0.25f;
            textboxSetMapWidth.TextHAlign = 0f;
            panel.Append(textboxSetMapWidth);
            #endregion

            #region textboxSetMapHeight
            textboxSetMapHeight.Width.Set(100f, 0f);
            textboxSetMapHeight.Height.Set(20f, 0f);
            textboxSetMapHeight.HAlign = 0.6f;
            textboxSetMapHeight.VAlign = 0.45f;
            textboxSetMapHeight.TextHAlign = 0f;
            panel.Append(textboxSetMapHeight);
            #endregion

            #region textboxSetMineNum
            textboxSetMineNum.Width.Set(100f, 0f);
            textboxSetMineNum.Height.Set(20f, 0f);
            textboxSetMineNum.HAlign = 0.6f;
            textboxSetMineNum.VAlign = 0.65f;
            textboxSetMineNum.TextHAlign = 0f;
            panel.Append(textboxSetMineNum);
            #endregion

            #region textboxSetMineDensity
            textboxSetMineDensity.Width.Set(100f, 0f);
            textboxSetMineDensity.Height.Set(20f, 0f);
            textboxSetMineDensity.HAlign = 0.6f;
            textboxSetMineDensity.VAlign = 0.25f;
            textboxSetMineDensity.TextHAlign = 0f;
            panel.Append(textboxSetMineDensity);
            #endregion
        }

        #region 点击事件

        private void ButtonClose_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Visible = false;
        }

        private void ButtonClear_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
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

        private void ButtonReset_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
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

        #region 弃用

        private void Preview_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().Preview = !player.GetModPlayer<MinePlayer>().Preview;
            // 切换UI图标
            if (player.GetModPlayer<MinePlayer>().Preview)
            {
                buttonPreview.SetImage(GetTexture("Preview"));
                buttonPreview.Width.Set(30f, 0f);
                buttonPreview.Height.Set(30f, 0f);
            }
            else
            {
                buttonPreview.SetImage(GetTexture("NoPreview"));
                buttonPreview.Width.Set(30f, 0f);
                buttonPreview.Height.Set(30f, 0f);
            }
        }

        private void Breakable_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().Breakable = !player.GetModPlayer<MinePlayer>().Breakable;
            // 切换UI图标
            if (player.GetModPlayer<MinePlayer>().Breakable)
            {
                buttonBreakable.SetImage(GetTexture("Breakable_UI"));
                buttonBreakable.Width.Set(30f, 0f);
                buttonBreakable.Height.Set(30f, 0f);
                buttonBreakable.hoverText = Language.GetText("Mods.Minesweeper.UITips.Breakable");
            }
            else
            {
                buttonBreakable.SetImage(GetTexture("Unbreakable_UI"));
                buttonBreakable.Width.Set(30f, 0f);
                buttonBreakable.Height.Set(30f, 0f);
                buttonBreakable.hoverText = Language.GetText("Mods.Minesweeper.UITips.Unbreakable");
            }
        }

        #endregion

        private void ButtonFixedMap_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().FixedOrFree = MinePlayer.Fixed;
            SetFixedOrFreeUI(MinePlayer.Fixed);
        }

        private void ButtonFreeMap_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.LocalPlayer;
            player.GetModPlayer<MinePlayer>().FixedOrFree = MinePlayer.Free;
            SetFixedOrFreeUI(MinePlayer.Free);
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Player player = Main.LocalPlayer;
            // 如果是初次进入世界，UI要先获取在player中保存的数据
            if (!player.GetModPlayer<MinePlayer>().UILoadData)
            {
                textboxSetMapWidth.Clear();
                textboxSetMapHeight.Clear();
                textboxSetMineNum.Clear();
                textboxSetMineDensity.Clear();
                textboxSetMapWidth.Write(player.GetModPlayer<MinePlayer>().MapWidth.ToString());
                textboxSetMapHeight.Write(player.GetModPlayer<MinePlayer>().MapHeight.ToString());
                textboxSetMineNum.Write(player.GetModPlayer<MinePlayer>().MineNum.ToString());
                textboxSetMineDensity.Write(player.GetModPlayer<MinePlayer>().MineDensity.ToString());

                string mapMode = player.GetModPlayer<MinePlayer>().FixedOrFree.DefaultIfEmpty("Fixed");
                SetFixedOrFreeUI(mapMode);
                
                player.GetModPlayer<MinePlayer>().UILoadData = true;
            }
            // 此后将UI中的数据同步至player中
            else
            {
                player.GetModPlayer<MinePlayer>().MapWidth = textboxSetMapWidth.Text.Length > 0 ? int.Parse(textboxSetMapWidth.Text) : 0;
                player.GetModPlayer<MinePlayer>().MapHeight = textboxSetMapHeight.Text.Length > 0 ? int.Parse(textboxSetMapHeight.Text) : 0;
                player.GetModPlayer<MinePlayer>().MineNum = textboxSetMineNum.Text.Length > 0 ? int.Parse(textboxSetMineNum.Text) : 0;
                player.GetModPlayer<MinePlayer>().MineDensity = textboxSetMineDensity.Text.Length > 0 ? int.Parse(textboxSetMineDensity.Text) : 0;
            }
        }

        private void SetFixedOrFreeUI(string mode)
        {
            panel.RemoveChild(labelWidth);
            panel.RemoveChild(labelHeight);
            panel.RemoveChild(labelMineNum);
            panel.RemoveChild(labelMineDensity);
            panel.RemoveChild(textboxSetMapWidth);
            panel.RemoveChild(textboxSetMapHeight);
            panel.RemoveChild(textboxSetMineNum);
            panel.RemoveChild(textboxSetMineDensity);
            if (mode == "Fixed")
            {
                panel.Append(labelWidth);
                panel.Append(labelHeight);
                panel.Append(labelMineNum);
                panel.Append(textboxSetMapWidth);
                panel.Append(textboxSetMapHeight);
                panel.Append(textboxSetMineNum);
            }
            else if (mode == "Free")
            {
                panel.Append(labelMineDensity);
                panel.Append(textboxSetMineDensity);
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
