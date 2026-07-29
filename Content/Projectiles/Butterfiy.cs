using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MistStar.Content.Projectiles
{
    class Butterfiy : ModProjectile
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "Butterfly");

        private bool directionSet = false;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 68;
            Projectile.height = 73;
            Projectile.scale = 1.2f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            //projectile.aiStyle = 27;
            Projectile.timeLeft = 800;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            //Projectile.damage = 485887767;
            Projectile.penetrate = 90;
            AIType = -1;
            Projectile.light = 0.7f;  // 弹幕自身发光


        }
        public override void AI()
        {


            if (!directionSet)
            {
                if (Projectile.velocity.X < 0)
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation += MathHelper.Pi;
                }
                else
                {
                    Projectile.spriteDirection = 1;
                    Projectile.rotation += 0f;
                }
                directionSet = true;
            }
            //}
            float forwardOffset = 80f; // 根据你的弹幕尺寸调整
            Vector2 lightCenter = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * forwardOffset;

            // 添加光照

            Lighting.AddLight(lightCenter, 1f, 0f, 0.7f);

            // 粒子特效
            Dust dust = Dust.NewDustDirect(Projectile.position, 68, 73, DustID.VilePowder, 0f, 0f, 100, default, 1.2f);
            dust.noGravity = true;

            //弹幕动画

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame == 4)
                    Projectile.frame = 0;
            }

        }
        public override void OnKill(int timeLeft)
        {
            // 弹幕中心点
            Vector2 center = Projectile.Center;

            // 爆炸效果
            for (int i = 0; i < 15; i++)
            {
                // 主粒子 - 向外爆炸
                Dust dust = Dust.NewDustDirect(center, 0, 0,
                    DustID.VilePowder, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 6f);
                dust.scale = Main.rand.NextFloat(1.5f, 2.5f);

                // 慢速粒子
                if (i % 3 == 0)
                {
                    Dust slowDust = Dust.NewDustDirect(center, 0, 0,
                        DustID.MagicMirror, 0f, 0f, 150, Color.White, 1.2f);
                    slowDust.noGravity = true;
                    slowDust.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.5f, 1.5f);
                }
            }

            // 向上飘散的粒子
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(center, 0, 0,
                    DustID.Cloud, 0f, 0f, 100, Color.LightBlue, 1.5f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-3f, -1f));
            }
        }

    }
}

