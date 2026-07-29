using Microsoft.Xna.Framework;
using MistStar.Utils;
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
    class FangTianRed : ModProjectile
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "FangTianRed");
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // // DisplayName.SetDefault("FTHJ1");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 33;
            Projectile.height = 33;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            //projectile.aiStyle = 27;
            Projectile.timeLeft = 800;
            //Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            //Projectile.damage = 485887767;
            Projectile.penetrate = 90;
            AIType = -1;
        }
        /*public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
            return base.Colliding(projHitbox,targetHitbox);
        }*/
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
            for (int i = 0; i < 2; i++)
            {

                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                // 生成dust
                //Dust d = Dust.NewDustDirect(Projectile.position, 20, 20,
                //    MyDustId.Smoke, 1, 1, 100, Color.White, 1f);
                //// 粒子效果无重力
                //d.noGravity = true;
                //// 粒子效果初速度乘以二
                //d.velocity *= 1.5f;
                // Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            }
            
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            // 火焰粒子特效
            /*Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,56, 0f, 0f, 100, default, 3f);
            // 粒子特效不受重力
            dust.noGravity = true;*/
        }

    }
}



    
    

