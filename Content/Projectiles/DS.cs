using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MistStar.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MistStar.Content.Projectiles
{
    public class DS : ModProjectile
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "龙泉0");
        private int timer = 0;
        private float pulseIntensity = 0f;


        /*public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }*/

        public override void SetDefaults()
        {
            Projectile.width = 158;
            Projectile.height = 156;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 90;
            Projectile.rotation = -MathHelper.PiOver2 / 2;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            timer++;

            // 计算脉动强度
            pulseIntensity = (float)System.Math.Sin(timer * 0.1f) * 0.5f + 0.5f;

            // 缓慢旋转

            // 添加发光效果
            Lighting.AddLight(Projectile.Center, 0.2f, 0.5f, 1f);

            // 核心粒子效果1
            for (int i = 0; i < 2; i++)
            {
                Vector2 dustPositiona = Projectile.Center;
                dustPositiona.X += Main.rand.NextFloat(-Projectile.height * 0.3f, Projectile.height * 0.3f);
                dustPositiona.Y += -120f;
                // 大型蓝色核心粒子
                Dust coreDust = Dust.NewDustDirect(
                    dustPositiona,
                    //Projectile.Center,
                    0, 0,
                    //DustID.BlueFairy,
                    MyDustId.CyanBubble,
                    //DustID.RedMoss,
                    0f, 0f,
                    100,
                    default,
                    //Color.White,
                    2f + pulseIntensity * 1f // 超大尺寸，加上脉动
                );
                if (Main.rand.NextBool(2)) // 50%概率
                {
                    // 向左移动
                    coreDust.velocity = new Vector2(-5f, -10f) * Main.rand.NextFloat(0.5f, 2f);
                }
                else
                {
                    // 向右移动
                    coreDust.velocity = new Vector2(5f, -10f) * Main.rand.NextFloat(0.5f, 2f);
                }
                coreDust.noGravity = false;
                //coreDust.fadeIn = -500f;
              
                //coreDust.alpha += 500;      // 大幅增加透明度增量（5→15）
                //coreDust.scale -= 3f;    // 添加快速缩小效果
                //coreDust.fadeIn = 993f;
                //coreDust.scale -= 50f;
                //coreDust.velocity = Vector2.Zero;
            }
            /*// 核心粒子效果2
            for (int i = 0; i < 3; i++)
            {
                Vector2 dustPositiona = Projectile.Center;
                dustPositiona.X += +120f;
                dustPositiona.Y += -120f;
                // 大型蓝色核心粒子
                Dust coreDust = Dust.NewDustDirect(
                    dustPositiona,
                    //Projectile.Center,
                    0, 0,
                    DustID.BlueFairy,
                    0f, 0f,
                    70,
                    default,
                    4f + pulseIntensity * 2f // 超大尺寸，加上脉动
                );
                coreDust.noGravity = true;
                coreDust.velocity = Vector2.Zero;
            }*/

            // 粗大的拖尾粒子
            for (int i = 0; i < 5; i++)
            {
                // 1. 计算粒子位置
                Vector2 dustPosition = Projectile.Center;
                dustPosition.X += Main.rand.NextFloat(-Projectile.width * 0.3f, Projectile.width * 0.3f);
                dustPosition.Y += Main.rand.NextFloat(-Projectile.height * 0.3f, Projectile.height * 0.3f) - 50f;

                // 2. 创建粒子
                Dust trailDust = Dust.NewDustDirect(
                    //Projectile.Center,
                    dustPosition,  // 位置
                    0, 0,         // 宽度和高度
                    DustID.BlueTorch,  // 粒子类型
                    0f, -200f,       // 初始速度
                    100,           // Alpha透明度
                    Color.White,   // 颜色
                    3f          // 缩放
                );

                // 3. 设置粒子属性
                trailDust.noGravity = true;
                trailDust.velocity = Projectile.velocity * 0.1f + Main.rand.NextVector2Circular(1f, 1f);
            }

            // 大型旋转粒子 - 围绕弹幕旋转
            if (timer % 2 == 0)
            {
                float angle = timer * 0.2f;
                float radius = Projectile.width * 0.4f;

                for (int i = 0; i < 4; i++) // 4个旋转点
                {
                    float currentAngle = angle + i * MathHelper.PiOver2;
                    Vector2 offset = new Vector2(
                        (float)System.Math.Cos(currentAngle) * radius,
                        (float)System.Math.Sin(currentAngle) * radius
                    );

                    Dust orbitDust = Dust.NewDustDirect(
                        Projectile.Center + offset,
                        0, 0,
                        DustID.GemSapphire,
                        0f, 0f,
                        150,
                        Color.Cyan,
                        3f
                    );
                    orbitDust.noGravity = true;
                    orbitDust.velocity = offset * 0.05f;
                }
            }

            // 随机的大型爆发粒子
            if (Main.rand.NextBool(8)) // 12.5%概率
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 burstPosition = Projectile.Center;
                    burstPosition.X += Main.rand.NextFloat(-Projectile.width * 0.4f, Projectile.width * 0.4f);
                    burstPosition.Y += Main.rand.NextFloat(-Projectile.height * 0.4f, Projectile.height * 0.4f);

                    Dust burstDust = Dust.NewDustDirect(
                        burstPosition,
                        0, 0,
                        DustID.WhiteTorch,
                        0f, 0f,
                        200,
                        Color.LightBlue,
                        5f // 非常大的粒子
                    );
                    burstDust.noGravity = true;
                    burstDust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    burstDust.fadeIn = 2f; // 淡入效果
                }
            }

            // 边缘大型闪光粒子
            if (timer % 5 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 edgePosition = Projectile.Center;
                    if (Main.rand.NextBool(2))
                    {
                        edgePosition.X += Main.rand.NextBool(2) ? -Projectile.width * 0.45f : Projectile.width * 0.45f;
                        edgePosition.Y += Main.rand.NextFloat(-Projectile.height * 0.3f, Projectile.height * 0.3f);
                    }
                    else
                    {
                        edgePosition.Y += Main.rand.NextBool(2) ? -Projectile.height * 0.45f : Projectile.height * 0.45f;
                        edgePosition.X += Main.rand.NextFloat(-Projectile.width * 0.3f, Projectile.width * 0.3f);
                    }

                    Dust edgeDust = Dust.NewDustDirect(
                        edgePosition,
                        0, 0,
                        DustID.MagicMirror,
                        0f, 0f,
                        100,
                        default,
                        4f
                    );
                    edgeDust.noGravity = true;
                    edgeDust.velocity = Vector2.Normalize(edgePosition - Projectile.Center) * 2f;
                }
            }



            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFairy, 0f, 0f, 100, default, 3f);
            dust.noGravity = true;

            Player player = Main.player[Projectile.owner];
            if (Main.MouseWorld.X > player.Center.X)
            {
                Projectile.spriteDirection = -1;
                Projectile.rotation = MathHelper.PiOver2 / 2;
            }
            else
            {
                Projectile.spriteDirection = 1;
                Projectile.rotation = -MathHelper.PiOver2 / 2;
            }
        }
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
