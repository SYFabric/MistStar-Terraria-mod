using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MistStar.Content.Projectiles
{
    public class 调试弹幕2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            // 魔法弹幕的AI
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // 发光效果
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 0.8f);

            // 轨迹粒子
            if (Main.rand.NextBool(3))
            {
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.PurpleTorch,
                    -Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1f, 1f),
                    100, Color.Purple, 1f
                ).noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            // 死亡效果
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.PurpleTorch,
                    Main.rand.NextVector2Circular(4f, 4f),
                    100, Color.Purple, 1.5f
                ).noGravity = true;
            }
        }
    }
}