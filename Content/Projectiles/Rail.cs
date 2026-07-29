using Microsoft.Xna.Framework;
using MistStar.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MistStar.Content.Projectiles
{
    public class Rail : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 976;
            Projectile.height = 458;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1000;
            Projectile.aiStyle = -1;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 35; i++)
            {
                // 生成蓝色魔法粒子
                Dust d = Dust.NewDustDirect(Projectile.position, 1000, 500,
                    MyDustId.BlueMagic, 1, 1, 100, Color.White, 4f);
                d.noGravity = true;
                d.velocity *= 1.5f;
                
                // 生成额外的无重力粒子
                Dust e = Dust.NewDustDirect(Projectile.position, 1000, 500,
                    MyDustId.BlueMagic, 1, 1, 100, Color.White, 4f);
                e.noGravity = true;
                e.velocity *= 1f;
            }
        }

        public override void AI()
        {
            // 如果需要AI逻辑，可以在这里添加
            // 目前为空，弹幕将保持静止
        }
    }
}