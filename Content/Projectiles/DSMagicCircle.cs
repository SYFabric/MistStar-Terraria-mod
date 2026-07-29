using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MistStar.Utils;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace MistStar.Content.Projectiles
{
    public class DSMagicCircle : ModProjectile
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "龙泉2");
        private int timer = 0;  // 计时器
        private float scale = 0f;  // 缩放
        private float alpha = 0f;  // 透明度
        private Color currentColor = Color.Blue;  // 当前颜色

        public override void SetDefaults()
        {
            Projectile.width = 126;
            Projectile.height = 154;
            Projectile.scale = 5f;
            Projectile.timeLeft = 155;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            //Projectile.damage = 150;
            Projectile.knockBack = 0f;
            Projectile.light = 0f;
            Projectile.alpha = 0;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            timer++;

            // 跟随玩家
            /*Player player = Main.player[Projectile.owner];
            if (player.active)
            {
                Projectile.Center = player.Center;
            }*/

            // 淡入淡出动画

            if (timer < 10) // 淡入
            {

                scale = MathHelper.Lerp(0f, 5f, timer / 10f);
                alpha = MathHelper.Lerp(0f, 0.9f, timer / 10f);
            }
            else if (timer < 130) // 保持
            {

                scale = 5f;
                alpha = 0.9f;
            }
            else if (timer < 140) // 保持
            {

                scale = MathHelper.Lerp(5f, 6f, (timer - 130) / 10f);

            }
            else // 淡出
            {

                scale = MathHelper.Lerp(5f, 0f, (timer - 140) / 20f);
                alpha = MathHelper.Lerp(0.9f, 0f, (timer - 140) / 20f);
            }

            /*
            if (timer < 200) // 淡入
            {
                //scale = MathHelper.Lerp(0f, 8f, timer / 300f);
                //alpha = MathHelper.Lerp(0.8f, -5f, timer / 300f);
                scale = MathHelper.Lerp(0f, 50f, timer / 300f);
                alpha = MathHelper.Lerp(0.8f, -5f, timer / 300f);
            }*/


            float colorPulse = (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 2f + timer * 0.05f) * 0.5f + 0.5f;

            // 在蓝紫色系之间渐变
            if (colorPulse < 0.33f)
            {
                // 蓝色到青色
                float t = colorPulse * 3f;
                currentColor = Color.Lerp(Color.Blue, Color.Cyan, t);
            }
            else if (colorPulse < 0.66f)
            {
                // 青色到紫色
                float t = (colorPulse - 0.33f) * 3f;
                currentColor = Color.Lerp(Color.Cyan, Color.Purple, t);
            }
            else
            {
                // 紫色到蓝色
                float t = (colorPulse - 0.66f) * 3f;
                currentColor = Color.Lerp(Color.Purple, Color.Blue, t);
            }

            // 发光效果
            Lighting.AddLight(Projectile.Center, currentColor.ToVector3() * alpha * 0.6f);
        }



        public override bool PreDraw(ref Color lightColor)
        {
            // 获取贴图
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // 计算绘制参数
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // 添加发光外晕
            for (int i = 0; i < 3; i++)
            {
                float glowScale = scale * (1.1f + i * 0.1f);
                float glowAlpha = alpha * (0.5f - i * 0.1f);
                Color glowColor = currentColor * glowAlpha;

                // 添加轻微偏移
                Vector2 offset = new Vector2(0, 1 + i * 0.5f).RotatedBy(Main.GlobalTimeWrappedHourly + i);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + offset,
                    null,
                    glowColor * 0.4f,
                    0,//Main.GlobalTimeWrappedHourly * 0.1f * (i % 2 == 0 ? 1 : -1),  // 内外层反向旋转
                    origin,
                    glowScale,
                    SpriteEffects.None,
                    0
                );
            }

            // 绘制魔法阵（淡蓝色，带透明度）
            Color drawColor = Color.LightBlue * alpha;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                drawColor,
                0f,  // 不旋转
                origin,
                scale,  // 使用动画缩放
                SpriteEffects.None,
                0
            );

            return false; // 阻止默认绘制
        }
        public override void OnKill(int timeLeft)
        {
            // 定义颜色渐变
            Color[] colorPalette = new Color[]
            {
            new Color(100, 200, 255, 200),     // 亮蓝色
            new Color(150, 220, 255, 220),     // 淡蓝色
            new Color(200, 230, 255, 240),     // 淡青色
            new Color(255, 255, 255, 200),     // 白色
            new Color(180, 220, 255, 180),     // 冰蓝色
            new Color(220, 240, 255, 200),     // 天蓝色
            Color.Lerp(Color.Blue, Color.Cyan, 0.3f)  // 蓝青色混合
                    };
            

            // 创建圆形粒子环效果
            int particleCount = 360; // 粒子数量 - 总共生成36个粒子组成圆形
            float baseRadius = 50f; // 基础半径 - 圆形初始半径为20像素
            float maxRadius = 200f; // 最大半径 - 圆形最大可扩展到200像素
            float radius = baseRadius; // 当前半径 - 存储当前圆的半径值
            float radiusIncrement = 5f; // 半径每帧增加量 - 每帧半径增加5像素

            // 创建粒子环
            for (int i = 0; i < particleCount; i++)  // 循环36次，创建36个粒子
            {
                // 计算粒子在圆上的位置
                // MathHelper.TwoPi是2π，即360度对应的弧度值
                // 计算当前粒子的角度：将圆等分为36份，每份角度 = 2π * 当前序号/总粒子数
                float angle = MathHelper.TwoPi * i / particleCount;
                // 计算偏移量：将角度转换为方向向量，乘以半径得到从圆心到粒子位置的偏移
                // ToRotationVector2()将弧度角转换为单位向量
                Vector2 offset = angle.ToRotationVector2() * radius;
                // 计算粒子的实际位置：弹幕中心位置 + 偏移量
                Vector2 particlePositiona = Projectile.Center + offset;
                Vector2 particlePosition = particlePositiona;
                particlePosition.X += Main.rand.NextFloat(-20f, 20f);
                particlePosition.Y += Main.rand.NextFloat(-20f, 20f);

                //中层粒子环-----------------------------------------
                // 创建粒子
                // NewDustPerfect是精确创建粒子的方法，参数依次为：
                // 1. particlePosition: 粒子生成位置
                // 2. DustID.BlueFairy: 粒子类型ID，蓝色仙灵尘效果
                // 3. Vector2.Zero: 初始速度向量，为零表示初始静止
                // 4. 150: Alpha透明度值，范围0-255，150表示约60%不透明
                // 5. Color.Lerp(Color.Blue, Color.Cyan, 0.5f): 颜色混合，蓝色和青色各50%
                // 6. 0.5f: 粒子初始大小缩放，0.5倍原始大小
                Dust ringParticle = Dust.NewDustPerfect(
                    particlePosition,
                    MyDustId.CyanBubble, //DustID.BlueFairy, // 使用蓝色魔法粒子
                    Vector2.Zero, // 初始速度为零
                    0, // 透明度
                    default,
                    //Color.Lerp(Color.Blue, Color.White, 0.2f), // 蓝青色
                    5f // 初始大小很小
                );
                // 设置粒子属性
                ringParticle.noGravity = true; // 无重力 - 粒子不会下落
                ringParticle.velocity = offset * 0.5f; // 设置速度：沿半径方向向外扩散，速度=偏移量*0.05
                ringParticle.fadeIn = 3f; // 淡入效果：粒子在1.5秒内从透明渐变到指定透明度


                //内层粒子环-----------------------------------------
                Dust secondRing = Dust.NewDustPerfect(
                    Projectile.Center + angle.ToRotationVector2() * (radius + 10f), // 半径大10像素
                    DustID.BlueFairy,
                    Vector2.Zero,
                    200, // 稍微不同的透明度
                    Color.Lerp(Color.Cyan, Color.White, 10f), // 稍微不同的颜色
                    0.5f // 稍微不同的大小
                );
                secondRing.noGravity = true;
                secondRing.velocity = angle.ToRotationVector2() * (radius + 10f) * 0.01f;
                secondRing.fadeIn =2.5f;



                //外层粒子环-----------------------------------------
                Dust a = Dust.NewDustPerfect(
                    particlePosition,
                    DustID.BlueFairy, // 使用蓝色魔法粒子
                    Vector2.Zero, // 初始速度为零
                    200, // 透明度
                    Color.Lerp(Color.Blue, Color.White, 2f), // 蓝青色
                    1f // 初始大小很小
                );
                a.noGravity = false;
                a.velocity = angle.ToRotationVector2() * (radius + 10f) * 0.8f;
                a.fadeIn = 2.5f;
            }
            
        

            // 创建中心爆发粒子
            for (int i = 0; i < 480; i++)  // 循环12次，创建12个中心爆发粒子
            {
                // 随机角度：在0到2π（360度）之间随机生成一个角度
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);

                // 随机距离：在10到50像素之间随机生成距离
                float distance = Main.rand.NextFloat(10f, 50f);

                // 计算偏移量：随机角度对应的方向向量乘以随机距离
                Vector2 offset = angle.ToRotationVector2() * distance;

                // 计算粒子位置：弹幕中心位置 + 随机偏移
                Vector2 particlePosition = Projectile.Center + offset;

                // 创建中心粒子
                // 参数说明：
                // 1. particlePosition: 粒子位置
                // 2. DustID.BlueTorch: 蓝色火炬粒子，比BlueFairy更亮
                // 3. offset * 0.1f: 速度，沿偏移方向向外移动，速度较慢
                // 4. 100: 透明度，100表示约40%不透明，比外圈粒子更不透明
                // 5. Color.Lerp(Color.White, Color.Cyan, 0.3f): 颜色，白色和青色混合，30%青色
                // 6. 0.3f: 初始大小，0.3倍，比外圈粒子更小
                Dust centerParticle = Dust.NewDustPerfect(
                    particlePosition,
                    //MyDustId.CyanBubble, //
                    DustID.BlueTorch, // 使用蓝色火炬粒子
                    offset * 0.4f, // 向外扩散
                    100, // 较不透明
                    //default,
                    Color.Lerp(Color.White, Color.Cyan, 3f), // 白青色
                    8f // 很小
                );

                // 设置粒子属性
                centerParticle.noGravity = false; // 无重力
                centerParticle.fadeIn = 3f; // 缓慢淡入：2秒内从透明渐变到指定透明度
            }


            Player player = Main.player[Projectile.owner];
            float FloatCount = 24f;
            int IntCount = (int)FloatCount;
            //Vector2 direction = Main.MouseWorld - player.Center;
            Vector2 direction = Vector2.UnitX;
            Vector2 P0 = Projectile.Center;
            for (int i = -IntCount; i < IntCount; i++)
            {
                Vector2 spreadDirection = (direction.ToRotation() + i * MathHelper.Pi / FloatCount).ToRotationVector2() * 100f;
                Vector2 P = P0 - spreadDirection * 2f;
                int proj = Projectile.NewProjectile(
                    Projectile.GetSource_Death(), // 使用死亡作为源
                    P,//Projectile.Center,//P,            // 位置
                    spreadDirection,                     // 速度
                    ModContent.ProjectileType<DS1>(), // 子弹幕类型
                    90, // 伤害
                    70f,    // 击退
                    Projectile.owner);
                Projectile projectile = Main.projectile[proj];
                projectile.alpha = 230;
                projectile.tileCollide = false;
            }
            SoundEngine.PlaySound(SoundID.Item88 with { Volume = 6f }, Projectile.Center);
        }
    }
}
