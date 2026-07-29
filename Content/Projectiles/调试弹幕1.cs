using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MistStar.Content.Projectiles
{
    public class 调试弹幕1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // 可选：设置弹幕名称等
            //DisplayName.SetDefault("Held Staff");
        }

        public override void SetDefaults()
        {
            // 手持弹幕的设置
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;    // 不造成伤害
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;       // 短暂存在，由物品代码更新
            Projectile.tileCollide = false; // 不碰撞方块
            Projectile.ignoreWater = true;
            Projectile.hide = true;         // 隐藏，但特殊绘制
        }

        public override void AI()
        {
            // 基本不做什么，位置和旋转由物品控制
            // 只是保持存活
            Projectile.timeLeft = 2;

            // 可选：添加持握时的粒子效果
            if (Main.rand.NextBool(10))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + new Vector2(0, -20f).RotatedBy(Projectile.rotation),
                    DustID.PurpleTorch,
                    Vector2.Zero,
                    100, Color.White, 0.8f
                ).noGravity = true;
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false; // 位置由物品控制，不自动更新
        }

        public override bool? CanDamage()
        {
            return false; // 手持弹幕不造成伤害
        }

        // 🎨 可选：自定义绘制
        public override bool PreDraw(ref Color lightColor)
        {
            // 这里可以添加自定义绘制逻辑
            // 比如发光效果、颜色叠加等
            return true; // 使用默认绘制
        }
    }
}