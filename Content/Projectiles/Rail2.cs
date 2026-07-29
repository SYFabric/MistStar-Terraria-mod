using Microsoft.Xna.Framework;
using MistStar.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MistStar.Content.Projectiles
{
    class Rail2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Rail2");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 976;
            Projectile.height = 458;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            //projectile.aiStyle = 27;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.damage = 85;
            Projectile.penetrate = 1000;
            AIType = -1;
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
            for (int i = 0; i < 35; i++)
            {
                // 生成dust
                Dust d = Dust.NewDustDirect(Projectile.position, 1000, 500,
                    MyDustId.BlueMagic, 1, 1, 100, Color.White, 4f);
                Dust e = Dust.NewDustDirect(Projectile.position, 1000, 500,
                    MyDustId.BlueMagic, 1, 1, 100, Color.White, 4f);
                e.noGravity = true;
                // 粒子效果初速度乘以二
                e.velocity *= 1f;
                // 粒子效果无重力
                d.noGravity = true;
                // 粒子效果初速度乘以二
                d.velocity *= 1.5f;
                // Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            }
        }
        public override void AI()
        {
            // 火焰粒子特效
            /*Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height,56, 0f, 0f, 100, default, 3f);
            // 粒子特效不受重力
            dust.noGravity = true;*/
        }

    }
}



    
    

