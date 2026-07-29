using Microsoft.Xna.Framework;
using System;
using System.Security.Policy;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MistStar.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;


// 注意这里命名空间变了，多了个.Items
// 因为这个文件在Items文件夹，而读取图片的时候是根据命名空间读取的，如果写错了可能图片就读不到了
namespace MistStar.Content.Items.Ranged
{
    // 保证类名跟文件名一致，这样也方便查找
    public class FangTianHalberd : ModItem
    {
        /*public override void Load()
        {
            // 加载左键专用的贴图
            leftClickTexture = ModContent.Request<Texture2D>("MistStar/Content/Projectiles/FangTianBlue").Value;
        }

        private Texture2D leftClickTexture;*/
        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "FangTianHalberd");
        public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

        // 设置物品名字，描述的地方，这个函数需要记住
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {

            Item.damage = 99999999;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 62;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 10.25f;
            Item.value = Item.sellPrice(999, 0, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item157;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 1f;
            //Item.shoot = Mod.Find<ModProjectile>("FangTianBlue").Type;
            //Item.CloneDefaults(ItemID.Spear);


            //item.useAmmo = AmmoID.Bullet;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        } //这个是可以右键使用此武器。
        //public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        //{
        //    // 如果是左键使用状态，使用蓝色贴图
        //    if (Main.myPlayer == whoAmI && Main.player[whoAmI].itemAnimation > 0 &&
        //        Main.player[whoAmI].HeldItem.type == Item.type &&
        //        Main.player[whoAmI].altFunctionUse != 2)
        //    {
        //        Texture2D texture = leftClickTexture;
        //        Vector2 position = Item.Center - Main.screenPosition;
        //        Rectangle frame = texture.Frame();
        //        Vector2 origin = frame.Size() * 0.5f;

        //        spriteBatch.Draw(texture, position, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
        //    }
        //}
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // 右键蓄爆
                Item.DamageType = DamageClass.Ranged;  // 如果不需要攻击速度缩放
                                                             // 或者使用 DamageClass.Melee 如果需要攻击速度缩放
                Item.UseSound = SoundID.Item1;
                Item.shoot = ModContent.ProjectileType<Projectiles.FangTianPurple>();
                Item.shootSpeed = 1f;
                Item.useTime = 10;
                Item.useAnimation = 10;
                Item.noUseGraphic = true;
                Item.autoReuse = false;
            }
            else
            {
                // 左键激光枪
                Item.DamageType = DamageClass.Ranged;
                Item.UseSound = SoundID.Item157;
                Item.shoot = ModContent.ProjectileType<Projectiles.FangTianRed>();
                Item.shootSpeed = 1f;
                Item.useTime = 8;
                Item.useAnimation = 8;
                Item.noUseGraphic = true;
                Item.autoReuse = true;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            return base.CanUseItem(player);
        }

      

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Vector2 mousePos = ;// 计算法杖前端位置（向前50像素
            //Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center);

            Vector2 FXXL = Vector2.Normalize(Main.MouseWorld - player.Center);//方向单位向量
            float RedSpeed = 16f;  //红色弹幕速度
            Vector2 RedV = FXXL * RedSpeed;//红色弹幕速度向量
            Vector2 RedP = player.Center + FXXL * 240f;//红色弹幕位置向量
            Vector2 RedPRand = RedP + new Vector2(Main.rand.NextFloat(-20f, 20f), Main.rand.NextFloat(-20f, 20f));//红色弹幕位置向量加随机数偏移

            float PurpleSpeed = 16f;  //紫色弹幕速度
            Vector2 PurpleV = FXXL * PurpleSpeed;//紫色弹幕速度向量
            Vector2 PurpleP = player.Center + FXXL * 300f;//紫色弹幕位置向量


            Vector2 BlueP = player.Center + FXXL * 50f;//蓝色弹幕位置向量


            if (player.altFunctionUse == 2)//右键
            {
                //创建紫色弹幕
                int Red = Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),                             // 参数1：实体源，描述谁/什么创建了这个弹幕
                    PurpleP,                                                   // 参数2：弹幕生成的位置（世界坐标）
                    PurpleV,                                                       // 参数3：弹幕的初始速度向量
                    type,                                                       // 参数4：弹幕的类型ID
                    damage,                                                     // 参数5：弹幕造成的伤害
                    knockback,                                                  // 参数6：弹幕的击退值
                    player.whoAmI                                               // 参数7：弹幕所有者的索引（通常是玩家whoAmI）
                );
            }
            else//左键
            {
                //创建红色弹幕
                int Red = Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),                             // 参数1：实体源，描述谁/什么创建了这个弹幕
                    RedPRand,                                                   // 参数2：弹幕生成的位置（世界坐标）
                    RedV,                                                       // 参数3：弹幕的初始速度向量
                    type,                                                       // 参数4：弹幕的类型ID
                    damage,                                                     // 参数5：弹幕造成的伤害
                    knockback,                                                  // 参数6：弹幕的击退值
                    player.whoAmI                                               // 参数7：弹幕所有者的索引（通常是玩家whoAmI）
                );
            }

            //创建蓝色弹幕
            int Blue = Projectile.NewProjectile(
                player.GetSource_ItemUse(Item),
                player.Center - Vector2.Normalize(Main.MouseWorld - player.Center) * 25f,
                BlueP * 0f,//Vector2.Zero,
                ModContent.ProjectileType<FangTianBlue>(),
                damage,
                knockback,
                player.whoAmI
            );
            return false;

        }

        //public override void UseStyle(Player player, Rectangle heldItemFrame)
        //{
        //    // 只在客户端处理视觉效果
        //    if (Main.dedServ) return;

        //    // 确保玩家持有正确的弹幕
        //    if (player.ownedProjectileCounts[ModContent.ProjectileType<FangTianBlue>()] <= 0)
        //    {
        //        // 创建手持弹幕
        //        int holdProj = Projectile.NewProjectile(
        //            player.GetSource_ItemUse(Item),
        //            player.Center,
        //            Vector2.Zero,
        //            ModContent.ProjectileType<FangTianBlue>(),
        //            0, 0f, player.whoAmI
        //        );

        //        if (holdProj < Main.maxProjectiles)
        //        {
        //            Main.projectile[holdProj].timeLeft = 2;
        //            player.heldProj = holdProj;
        //        }
        //    }

        //    // 更新玩家手持的弹幕引用
        //    UpdateHeldProjectile(player);
        //}

        //// 🔥 持续更新手持弹幕
        //public override void HoldItem(Player player)
        //{
        //    UpdateHeldProjectile(player);
        //}

        //// 🔧 更新手持弹幕位置和旋转
        //private void UpdateHeldProjectile(Player player)
        //{
        //    // 查找现有的手持弹幕
        //    int holdProjIndex = -1;
        //    for (int i = 0; i < Main.maxProjectiles; i++)
        //    {
        //        Projectile proj = Main.projectile[i];
        //        if (proj.active && proj.owner == player.whoAmI &&
        //            proj.type == ModContent.ProjectileType<FangTianBlue>())
        //        {
        //            holdProjIndex = i;
        //            break;
        //        }
        //    }

        //    // 如果找不到手持弹幕，尝试创建一个
        //    if (holdProjIndex == -1 && player.itemAnimation > 0)
        //    {
        //        holdProjIndex = Projectile.NewProjectile(
        //            player.GetSource_ItemUse(Item),
        //            player.Center,
        //            Vector2.Zero,
        //            ModContent.ProjectileType<调试弹幕2>(),
        //            0, 0f, player.whoAmI
        //        );
        //    }

        //    // 更新手持弹幕
        //    if (holdProjIndex >= 0 && holdProjIndex < Main.maxProjectiles)
        //    {
        //        Projectile holdProj = Main.projectile[holdProjIndex];
        //        holdProj.timeLeft = 2; // 保持存活
        //        player.heldProj = holdProjIndex;

        //        // 更新位置和旋转
        //        UpdateStaffPosition(player, holdProj);
        //    }
        //}

        //// 🎯 更新法杖位置（类似长矛的持握方式）
        //private void UpdateStaffPosition(Player player, Projectile staffProj)
        //{/*
        //    // 计算鼠标方向
        //    Vector2 mousePos = Main.MouseWorld;
        //    Vector2 direction = Vector2.Normalize(mousePos - player.MountedCenter);
        //    if (direction == Vector2.Zero) direction = Vector2.UnitX;

        //    // 计算旋转（假设法杖贴图朝上）
        //    float rotation = direction.ToRotation() + MathHelper.PiOver2;

        //    // 处理玩家朝向
        //    if (direction.X < 0)
        //    {
        //        player.ChangeDir(-1);
        //        //Projectile.spriteDirection = -1;
        //        rotation += MathHelper.Pi; // 朝左时翻转
        //    }
        //    else
        //    {
        //        player.ChangeDir(1);
        //    }

        //    // 应用旋转
        //    staffProj.rotation = rotation;

        //    // 设置位置（玩家手部）
        //    Vector2 holdoutOffset = new Vector2(20f * player.direction, -8f);
        //    staffProj.Center = player.MountedCenter + holdoutOffset;
        //    staffProj.spriteDirection = player.direction;*/
        //}

        //// 🎯 重写Shoot方法实现法杖射击
        //public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
        //                          Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        //{
        //    // 计算法杖尖端位置
        //    Vector2 mouseDirection = Vector2.Normalize(Main.MouseWorld - player.Center);
        //    Vector2 staffTip = player.Center + mouseDirection * 50f; // 法杖尖端偏移

        //    // 创建魔法弹幕
        //    Projectile.NewProjectile(
        //        source,
        //        staffTip,                    // 从法杖尖端发射
        //        velocity,                    // 使用计算的速度
        //        type,                        // 弹幕类型
        //        damage,                      // 伤害
        //        knockback,                   // 击退
        //        player.whoAmI                // 所有者
        //    );

        //    // 添加施法特效
        //    for (int i = 0; i < 3; i++)
        //    {
        //        Dust.NewDustPerfect(
        //            staffTip,
        //            DustID.MagicMirror,
        //            mouseDirection.RotatedByRandom(0.3f) * Main.rand.NextFloat(2f, 4f),
        //            100, Color.Purple, 1.2f
        //        ).noGravity = true;
        //    }

        //    return false; // 不消耗额外弹药
        //}

        //// 🎵 自定义使用音效时机
        //public override bool? UseItem(Player player)
        //{
        //    // 在合适的时机播放音效
        //    if (!Main.dedServ)
        //    {
        //        // 可以在这里添加自定义音效逻辑
        //    }
        //    return base.UseItem(player);
        //}


        public override Vector2? HoldoutOffset()
        {
            // X坐标往里移动15像素，Y坐标向下移动5像素
            return new Vector2(-35, +5);
            //return new Vector2(0, +5);
        }

        // 物品合成表的设置部分
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod, "Last", 1);
            recipe.Register();
        }
    }
}




