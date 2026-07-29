using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MistStar.Content.Items.Magic
{
    public class ImmortalRail : ModItem
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "Immortal Rail");
        public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "\"Pitiful fish out of water, this will be your first and last time seeing this great rail.\"\nEnemies in this field can only take damage from this field.");

        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.mana = 20;
            Item.DamageType = DamageClass.Magic;
            Item.width = 100;
            Item.height = 100;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 32, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.Rail>();
            Item.crit = 46;
        }

        public override bool CanUseItem(Player player)
        {
            bool dukeFishronExists = NPC.AnyNPCs(NPCID.DukeFishron);
            Item.shoot = dukeFishronExists 
                //? ModContent.ProjectileType<Projectiles.Rail2>() 
                ? ModContent.ProjectileType<Projectiles.Rail>() 
                : ModContent.ProjectileType<Projectiles.Rail>();
            
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spawnPosition = Main.MouseWorld;
            Vector2 projectileVelocity = Vector2.Zero;
            
            Projectile.NewProjectile(source, spawnPosition, projectileVelocity, type, damage, knockback, player.whoAmI);
            
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MinecartTrack, 50);
            recipe.AddIngredient(ItemID.TruffleWorm, 10);
            recipe.AddIngredient(ItemID.MinecartMech, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}