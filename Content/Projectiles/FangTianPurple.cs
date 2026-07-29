using Microsoft.Xna.Framework;
using MistStar.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
//using MistStar.Content.Systems; // 添加这行
namespace MistStar.Content.Projectiles
{
    class FangTianPurple : ModProjectile
    {
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "FangTianPruple");
        private List<int> hitTargets = new List<int>(); // 记录已击中的目标，避免重复击退


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 113;
            Projectile.height = 113;
            Projectile.scale = 1f;
            Projectile.friendly = false;
            Projectile.hostile = false;
            //projectile.aiStyle = 27;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.damage = 0;
            Projectile.penetrate = -1;
            AIType = -1;
            Projectile.light = 1f;  // 弹幕自身发光


        }
        public override void AI()
        {
            float forwardOffset = 80f; // 根据你的弹幕尺寸调整
            Vector2 lightCenter = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * forwardOffset;
            Lighting.AddLight(lightCenter, 163 / 255f, 54 / 255f, 194 / 255f);
            Dust dust = Dust.NewDustDirect(Projectile.position, 68, 73, DustID.PurpleCrystalShard, 0f, 0f, 100, default, 1.2f);
            dust.noGravity = true;


            Projectile.frameCounter++;
            if (Projectile.frameCounter == 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame == 5)
                    Projectile.frame = 0;
            }

            CheckKnockbackCollisions();
        }
        private void CheckKnockbackCollisions()
        {
            // 检测与所有NPC的碰撞（包括友好和敌对）
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.life > 0 && !hitTargets.Contains(npc.whoAmI) && npc.type != 488)
                {
                    if (Projectile.Hitbox.Intersects(npc.Hitbox))
                    {
                        ApplyKnockbackToNPC(npc);
                        hitTargets.Add(npc.whoAmI);
                    }
                }
            }

            // 检测与所有玩家的碰撞
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && !hitTargets.Contains(player.whoAmI + 1000))
                {
                    if (Projectile.Hitbox.Intersects(player.Hitbox))
                    {
                        ApplyKnockbackToPlayer(player);
                        hitTargets.Add(player.whoAmI + 1000);
                    }
                }
            }
        }
        private void ApplyKnockbackToNPC(NPC npc)
        {
            // 计算击退方向（从弹幕指向NPC中心）
            Vector2 knockbackDirection = (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);

            // 如果方向相反，则反转（确保击退方向正确）
            if (Vector2.Dot(knockbackDirection, Projectile.velocity) < 0)
                knockbackDirection = -knockbackDirection;

            // 应用大量击退（10.0f是很强的击退）
            float knockbackStrength = 50.0f;
            npc.velocity += knockbackDirection * knockbackStrength;

            // 限制NPC速度避免飞出屏幕
            float maxSpeed = 50f;
            if (npc.velocity.Length() > maxSpeed)
                npc.velocity = Vector2.Normalize(npc.velocity) * maxSpeed;

            // 击退特效
            CreateKnockbackEffect(npc.Center);

            // 播放音效
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.7f }, npc.Center);
        }

        private void ApplyKnockbackToPlayer(Player player)
        {
            // 计算击退方向
            Vector2 knockbackDirection = (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);

            // 如果方向相反，则反转
            if (Vector2.Dot(knockbackDirection, Projectile.velocity) < 0)
                knockbackDirection = -knockbackDirection;

            // 应用击退（对玩家可以稍微弱一点）
            float knockbackStrength = 30f;
            player.velocity += knockbackDirection * knockbackStrength;

            // 限制玩家速度
            float maxSpeed = 30f;
            if (player.velocity.Length() > maxSpeed)
                player.velocity = Vector2.Normalize(player.velocity) * maxSpeed;

            // 击退特效
            CreateKnockbackEffect(player.Center);

            // 播放音效
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.7f }, player.Center);

            // 可选：添加屏幕震动效果（仅对本地玩家）
            if (player.whoAmI == Main.myPlayer)
            {
                // 轻微屏幕震动
                Main.SetCameraLerp(1f, 10);
            }
        }

        private void CreateKnockbackEffect(Vector2 position)
        {
            // 创建冲击波特效
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(position, 10, 10, DustID.Cloud, 0f, 0f, 100, Color.White, 2f);
                dust.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
                dust.noGravity = true;
            }

            // 创建圆形冲击波
            for (int i = 0; i < 36; i++)
            {
                float angle = MathHelper.TwoPi * i / 36f;
                Vector2 dustPos = position + Vector2.UnitX.RotatedBy(angle) * 30f;
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Smoke, Vector2.Zero, 100, Color.LightGray, 1.5f);
                dust.velocity = Vector2.UnitX.RotatedBy(angle) * 5f;
                dust.noGravity = true;
            }
        }

        // 禁用所有默认伤害相关方法
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 空实现，不造成伤害
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // 空实现，不造成伤害
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false; // 禁用默认NPC碰撞检测
        }

        public override bool CanHitPlayer(Player target)
        {
            return false; // 禁用默认玩家碰撞检测
        }


        /*
        public override void Kill(int timeLeft)
        {
            // 销毁时的爆炸特效
            for (int i = 0; i < 25; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                                             DustID.Cloud, 0f, 0f, 100, Color.White, 2f);
                dust.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 6f);
                dust.noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.8f }, Projectile.Center);
        }*/



        
        public override void OnKill(int timeLeft)
        {

            // 弹幕中心点
            Vector2 center = Projectile.Center;
            int particleCount = 240; // 粒子数量 - 总共生成36个粒子组成圆形
            float baseRadius = 50f; // 基础半径 - 圆形初始半径为20像素
            float maxRadius = 200f; // 最大半径 - 圆形最大可扩展到200像素
            float radius = baseRadius; // 当前半径 - 存储当前圆的半径值
            float radiusIncrement = 5f; // 半径每帧增加量 - 每帧半径增加5像素
            for (int i = 0; i < particleCount; i++)  // 循环36次，创建36个粒子
            {
                float angle = MathHelper.TwoPi * i / particleCount;
                Vector2 offset = angle.ToRotationVector2() * radius;
                Vector2 particlePosition = Projectile.Center + offset;
                particlePosition.X += Main.rand.NextFloat(-20f, 20f);
                particlePosition.Y += Main.rand.NextFloat(-20f, 20f);
                Dust ringParticle = Dust.NewDustPerfect(
                    particlePosition,
                    MyDustId.PurpleLingering,//MyDustId.PurpleBubble, //DustID.BlueFairy, // 使用蓝色魔法粒子
                    Vector2.Zero, // 初始速度为零
                    180, // 透明度
                    Color.White,
                    //Color.Lerp(Color.Blue, Color.White, 0.2f), // 蓝青色
                    4f // 初始大小很小
                );
                ringParticle.noGravity = true; // 无重力 - 粒子不会下落
                ringParticle.velocity = offset * 0.5f; // 设置速度：沿半径方向向外扩散，速度=偏移量*0.05
                ringParticle.fadeIn = 3f; // 淡入效果：粒子在1.5秒内从透明渐变到指定透明度
            }

            /*
            Projectile.NewProjectile(
                Projectile.GetSource_Death(), // 使用死亡作为源
                center,//Projectile.Center,//P,            // 位置
                Vector2.Zero,                     // 速度
                ModContent.ProjectileType<FangTianPurple>(), // 子弹幕类型
                0, // 伤害
                100f,    // 击退
                Projectile.owner
            );*/
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.8f }, Projectile.Center);

        }

        
    }
}
