//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.Localization;
//using Terraria.ModLoader;

//namespace MistStar.Content.Projectiles
//{
//    class FangTianBlue : ModProjectile
//    {
//        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "FangTianHalBlue");
//        public override void SetStaticDefaults()
//        {
//            base.SetStaticDefaults();
//            // // DisplayName.SetDefault("FTHJ2");
//        }
//        public override void SetDefaults()
//        {
//            base.SetDefaults();
//            Projectile.width = 50;
//            Projectile.height = 50;
//            Projectile.scale = 1f;
//            Projectile.friendly = true;
//            Projectile.hostile = false;
//            AIType = -1;
//            Projectile.timeLeft = 1;
//            Projectile.DamageType = DamageClass.Melee;
//            Projectile.ignoreWater = false;
//            Projectile.tileCollide = true;
//            //Projectile.damage = 485887767;
//            Projectile.penetrate = 90;
//        }
//        public override void AI()
//        {

//            /*
//            // 火焰粒子特效
//            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,41, 0f, 0f, 100, default(Color), 3f);
//            // 粒子特效不受重力
//            dust.noGravity = true;*/
//        }

//    }
//}




// 引入命名空间
//using MistStar.Content.Dusts;  // 注释掉的命名空间
using Microsoft.Xna.Framework;    // XNA框架，提供基础数学和图形功能
using Microsoft.Xna.Framework.Graphics;
using MistStar.Content.Items.Ranged;
using MistStar.Utils;            // 自定义工具类
using Terraria;                  // 泰拉瑞亚主命名空间
using Terraria.ID;               // 泰拉瑞亚ID常量
using Terraria.ModLoader;        // tModLoader模组加载器

namespace MistStar.Content.Projectiles  // 命名空间：模组名.内容.弹幕
{
    public class FangTianBlue : ModProjectile  // 类名：方天画戟蓝色弹幕，继承自模组弹幕
    {
        // 🔧 定义长矛弹幕的伸缩范围（可重写的属性）
        // 这些是受保护的虚属性，方便子类重写
        //protected virtual float HoldoutRangeMin => 24f;  // 最小伸缩距离：24像素
        //protected virtual float HoldoutRangeMax => 96f;  // 最大伸缩距离：96像素

        // 🎯 设置弹幕默认属性
        public override void SetDefaults()
        {
            // 克隆原版长矛的默认值
            //Projectile.CloneDefaults(ProjectileID.Spear);
            //Projectile.width = 283;
            //Projectile.height = 33;
            //Projectile.scale = 0.8f;
            //Projectile.aiStyle = -1;
            Projectile.width = 33;
            Projectile.height = 33;
            Projectile.friendly = false;    // 不造成伤害
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;       // 短暂存在，由物品代码更新
            Projectile.tileCollide = false; // 不碰撞方块
            Projectile.ignoreWater = true;
            //Projectile.hide = true;         // 隐藏，但特殊绘制
            // 这会自动设置以下属性：
            // - 宽度、高度
            // - AI样式（长矛专用）
            // - 友好性、穿透、方块碰撞
            // - 缩放、隐藏、所有者碰撞检查
            // - 近战属性
        }

        ///*public override void AI()
        //{
        //    // 基本不做什么，位置和旋转由物品控制
        //    // 只是保持存活
        //    Projectile.timeLeft = 2;

        //    // 可选：添加持握时的粒子效果
        //    if (Main.rand.NextBool(10))
        //    {
        //        Dust.NewDustPerfect(
        //            Projectile.Center + new Vector2(0, -20f).RotatedBy(Projectile.rotation),
        //            DustID.PurpleTorch,
        //            Vector2.Zero,
        //            100, Color.White, 0.8f
        //        ).noGravity = true;
        //    }
        //}*/
        //public override void AI()
        //{
        //    Player player = Main.player[Projectile.owner];

        //    // 如果弹幕所有者存活且是近战武器模式，跟随玩家
        //    if (player.active && !player.dead && Projectile.DamageType == DamageClass.MeleeNoSpeed)
        //    {
        //        // 跟随玩家位置
        //        Projectile.Center = player.Center;
        //        Projectile.timeLeft = 2; // 保持存活

        //        // 粒子效果
        //        if (Main.rand.NextBool(10))
        //        {
        //            Dust.NewDustPerfect(
        //                Projectile.Center + new Vector2(0, -20f).RotatedBy(Projectile.rotation),
        //                DustID.PurpleTorch,
        //                Vector2.Zero,
        //                100, Color.White, 0.8f
        //            ).noGravity = true;
        //        }
        //    }
        //    else
        //    {
        //        // 普通弹幕行为（用于右键射击）
        //        Projectile.timeLeft = 2;

        //        // 添加移动逻辑（如果需要）
        //        if (Projectile.velocity != Vector2.Zero)
        //        {
        //            Projectile.rotation = Projectile.velocity.ToRotation();
        //        }
        //    }
        //}

        //// 修改为可以造成伤害


        //public override bool ShouldUpdatePosition()
        //{
        //    return false; // 位置由物品控制，不自动更新
        //}

        //public override bool? CanDamage()
        //{
        //    return false; // 手持弹幕不造成伤害
        //}
        private bool directionSet = false;

        private bool IsWeaponInUse(Player player)
        {
            // 检查1：玩家是否持有指定武器
            bool holdingWeapon = player.HeldItem.type == ModContent.ItemType<FangTianHalberd>();

            // 检查2：玩家是否正在使用物品（有使用动画）
            bool usingItem = player.itemAnimation > 0;

            // 检查3：玩家是否存活
            bool playerAlive = player.active && !player.dead;

            return holdingWeapon && usingItem && playerAlive;
        }
        public override void AI()
        {
            // 基本不做什么，位置和旋转由物品控制
            // 只是保持存活
            //
            //int existingProjectile = -1;
            Player player = Main.player[Projectile.owner];
            //Vector2 B = player.Center + Vector2.Normalize(Main.MouseWorld - player.Center) * 30f;// 使用计算出的法杖前端位置

            if (!IsWeaponInUse(player))
            {
                Projectile.Kill(); // 武器未被使用，销毁弹幕
                return;
            }
            else {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile otherProj = Main.projectile[i];
                    if (otherProj.active && otherProj.type == Projectile.type &&
                        otherProj.owner == Projectile.owner && otherProj.whoAmI != Projectile.whoAmI)
                    {
                        // 如果找到其他相同类型的弹幕，销毁自己
                        Projectile.Kill();
                        return;
                    }
                    else
                    {
                        player.heldProj = Projectile.whoAmI;
                        Projectile.Center = player.Center - Vector2.Normalize(Main.MouseWorld - player.Center) * 25f;
                        Projectile.timeLeft = 2;
                    }
                }
            }



            Vector2 toMouse0 = Main.MouseWorld - player.Center;
            if (toMouse0.LengthSquared() < 1f)
            {
                toMouse0 = Vector2.UnitX; // 默认向右
            }
            Vector2 toMouse = Vector2.Normalize(toMouse0);
            Projectile.rotation = toMouse.ToRotation() - MathHelper.PiOver2;

            /*if (!directionSet)
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
            }*/
            /*
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
            /*
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
            }*/
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            // 简单的偏移量
            float offsetX = 10f;
            float offsetY = -5f;

            // 计算绘制位置
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(offsetX, offsetY);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                texture.Size() * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }*/
        /*
        public override bool ShouldUpdatePosition()
        {
            return false; // 位置由物品控制，不自动更新
        }

        public override bool? CanDamage()
        {
            return false; // 手持弹幕不造成伤害
        }
        */
        // ⚡ 返回false表示不执行原版AI
        //return false;
    }
}


