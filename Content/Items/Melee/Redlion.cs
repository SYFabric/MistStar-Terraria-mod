using Microsoft.Xna.Framework;
using MistStar.Content.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MistStar.Content.Items.Melee
{
    public class Redlion : ModItem
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "Redlion");
        public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "Look, it's a red lion!");

        public override void SetStaticDefaults()
        {
            // 可以在这里添加其他静态配置
            Item.ResearchUnlockCount = 1; // 研究需要的数量
        }

        public override void SetDefaults()
        {
            Item.damage = 99999999; // 调整为合理的伤害值
            Item.DamageType = DamageClass.Melee;
            Item.width = 148;
            Item.height = 151;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 20f;
            Item.value = Item.sellPrice(0, 50, 0, 0); // 调整为合理的价格
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.MonkStaffT2Ghast;
            Item.shootSpeed = 50f;
            Item.noMelee = true;
            Item.noUseGraphic = false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30, -30);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // 使用ModContent.ItemType获取mod物品
            recipe.AddIngredient(ModContent.ItemType<Last>(), 1);
            recipe.Register();
        }
    }
}