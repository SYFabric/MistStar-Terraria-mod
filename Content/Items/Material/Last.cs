using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MistStar.Content.Items.Material
{
    public class Last : ModItem
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "Last");
        public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "Last End");

        public override void SetStaticDefaults()
        {
            // 可以在研究列表中解锁的数量
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(999, 0, 0, 0);
            Item.rare = ItemRarityID.Purple; // 13对应紫色稀有度
            Item.maxStack = Item.CommonMaxStack; // 使用标准最大堆叠数9999
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // 使用ItemID枚举替代魔法数字3467
            recipe.AddIngredient(ItemID.LunarBar, 999);
            recipe.Register();
        }
    }
}