using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MistStar.Content.Projectiles
{
    public class DS1 : ModProjectile
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "龙泉1");

        /*public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }*/

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            //Projectile.rotation = -MathHelper.PiOver2/2;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 35; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, 50, 50, 56, 0, 0, 100, Color.Blue, 1.5f);
                d.noGravity = true;
                d.velocity *= 1.5f;
            }
        }

        public override void AI()
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                // 计算运动方向的角度
                Projectile.rotation = Projectile.velocity.ToRotation();

                // 如果你的弹幕贴图是朝上的，需要加上90度
                // 注释掉其中一行，选择适合你贴图的方向
                // Projectile.rotation = Projectile.velocity.ToRotation(); // 贴图朝右
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // 贴图朝上
            }
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 41, 0f, 0f, 200, default, 3f);
            dust.noGravity = true;

            /*Projectile.frameCounter++;
            if (Projectile.frameCounter == 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 4)
                    Projectile.frame = 0;
            }*/
        }
    }
}