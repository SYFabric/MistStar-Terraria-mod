using Microsoft.Xna.Framework;
using MistStar.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MistStar.Content.Items.Melee
{
    public class DragonSpring : ModItem
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "龙泉");
        public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "“从天而降一条傻龙”");

        public override void SetStaticDefaults()
        {
            // 研究解锁数量
            Item.ResearchUnlockCount = 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void SetDefaults()
        {
            // 基础属性

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 46;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 26, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 23; // 暴击率

            // 默认属性（左键）
            Item.shoot = ModContent.ProjectileType<Projectiles.DS>();
            Item.shootSpeed = 1f;
            Item.useTime = 20;
            Item.useAnimation = 8;
            Item.damage = 70;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // 右键
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.DSMagicCircle>();
                Item.shootSpeed = 1f;
                Item.useTime = 120;
                Item.useAnimation = 30;
                Item.damage = 90;
            }
            else // 左键
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.DS>();
                Item.shootSpeed = 1f;
                Item.useTime = 10;
                Item.useAnimation = 8;
                Item.damage = 70;
            }

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) // 右键：扇形弹幕
            {
                //Vector2 direction = Main.MouseWorld - player.Center;
                /*Vector2 direction = Vector2.UnitX;
                int count = 12;

                for (int i = -count; i < count; i++)
                {
                    Vector2 spreadDirection = (direction.ToRotation() + i * MathHelper.Pi / count).ToRotationVector2() * 16;
                    Projectile.NewProjectile(source, position, spreadDirection, type, damage, knockback, player.whoAmI);
                }*/
                SoundEngine.PlaySound(SoundID.Item119 with { Volume = 10f }, player.Center);
                Projectile.NewProjectile(source, player.Center, Vector2.Zero,ModContent.ProjectileType<DSMagicCircle>(), 150, 0f, player.whoAmI);
            }
            else // 左键：从天而降
            {
                Vector2 startPosition = Main.MouseWorld;
                startPosition.Y -= 700f; // 从屏幕上方发射
                Vector2 direction = Main.MouseWorld - startPosition;
                Vector2 finalVelocity = direction.SafeNormalize(Vector2.Zero) * 30f;
                SoundEngine.PlaySound(SoundID.Item88 with { Volume = 5f }, player.Center);
                Projectile.NewProjectile(source, startPosition, finalVelocity, type, damage, knockback, player.whoAmI);
                

            }

            return false; // 阻止默认射击
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // 合成材料
            recipe.AddIngredient(ItemID.ChlorophyteBar, 30);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddIngredient(ItemID.Sapphire, 20);
            recipe.AddIngredient(ItemID.SoulofFlight, 10);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddIngredient(ItemID.Arkhalis, 1);
            recipe.AddIngredient(ItemID.EnchantedSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}