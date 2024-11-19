using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Minesweeper.Common.Configs
{
    internal class CommonConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public override void OnLoaded() => Config = this;

        /// <summary>
        /// 创建游戏区域是否会破坏原版方块
        /// </summary>
        [DefaultValue(false)]
        public bool BlockBreakable;
    }
}
