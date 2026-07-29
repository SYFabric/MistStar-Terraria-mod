//// 引入命名空间
//using MistStar.Content.Items.Material;  // 模组材料物品
//using MistStar.Content.Projectiles;    // 模组弹幕
//using Terraria;                        // 主命名空间
//using Terraria.Audio;                  // 声音系统
//using Terraria.ID;                     // 原版ID常量
//using Terraria.ModLoader;              // 模组加载器

//namespace MistStar.Content.Items.Ranged  // 命名空间：远程武器
//{
//    public class 调试武器 : ModItem  // 类名：调试武器（中文类名）
//    {
//        // 🎵 设置静态默认值（在加载时调用一次）
//        public override void SetStaticDefaults()
//        {
//            // 🔇 跳过使用动画绑定的声音播放
//            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
//            // 说明：允许我们在UseItem()钩子中自定义声音播放时机

//            // 🎯 让游戏识别这个物品为长矛
//            ItemID.Sets.Spears[Item.type] = true;
//            // 说明：启用长矛的特殊行为（伸缩动画等）
//        }

//        // ⚙️ 设置物品实例属性（每个物品实例）
//        public override void SetDefaults()
//        {
//            // ========== 通用属性 ==========

//            // 🌈 稀有度：粉色
//            Item.rare = ItemRarityID.Pink;
//            // 说明：物品在游戏中的稀有度等级

//            // 💰 价值：10银币
//            Item.value = Item.sellPrice(silver: 10);
//            // 说明：NPC收购价格（10银币）

//            // ========== 使用属性 ==========

//            // 🎮 使用样式：射击样式（5）
//            Item.useStyle = ItemUseStyleID.Shoot;
//            // 说明：物品的使用动作样式（射击=手持动作）

//            // ⏱️ 使用动画：12帧（0.2秒）
//            Item.useAnimation = 12;
//            // 说明：动画播放时间（60帧=1秒）

//            // ⏰ 使用时间：18帧（0.3秒）
//            Item.useTime = 18;
//            // 说明：实际使用间隔时间

//            // 🔊 使用音效：原版音效ID 71
//            Item.UseSound = SoundID.Item71;
//            // 说明：使用物品时播放的音效

//            // 🔄 自动连发：开启
//            Item.autoReuse = true;
//            // 说明：按住鼠标可以连续使用（需要配合CanUseItem限制）

//            // ========== 武器属性 ==========

//            // ⚔️ 伤害：25点
//            Item.damage = 25;
//            // 说明：基础伤害值

//            // 💥 击退：6.5
//            Item.knockBack = 6.5f;
//            // 说明：击退力度

//            // 👻 不使用图形：开启
//            Item.noUseGraphic = true;
//            // 说明：使用时隐藏物品贴图（由弹幕显示）

//            // 🎯 伤害类型：近战
//            Item.DamageType = DamageClass.Melee;
//            // 说明：武器造成的伤害类型

//            // ❌ 非近战：开启
//            Item.noMelee = true;
//            // 说明：物品本身不产生近战伤害（由弹幕造成伤害）

//            // ========== 弹幕属性 ==========

//            // 🚀 发射速度：3.7像素/帧
//            Item.shootSpeed = 3.7f;
//            // 说明：弹幕的初始速度（影响长矛伸缩范围）

//            // 🎯 发射弹幕：FangTianBlue弹幕
//            Item.shoot = ModContent.ProjectileType<FangTianBlue>();
//            // 说明：使用自定义的蓝色方天画戟弹幕
//        }

//        // ✅ 检查是否可以使用的条件
//        public override bool CanUseItem(Player player)
//        {
//            // 确保同时只能存在一个长矛弹幕
//            return player.ownedProjectileCounts[Item.shoot] < 1;
//            // 说明：当玩家已经拥有该弹幕时，不能再使用
//            // 这是自动连发长矛的标准限制方式
//        }

//        // 🔊 物品使用时的自定义处理
//        public override bool? UseItem(Player player)
//        {
//            // 因为我们跳过了初始声音播放，需要在这里手动播放
//            /*
//            if (!Main.dedServ && Item.UseSound.HasValue)  // 不在专用服务器且有声效
//            {
//                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
//            }
//            */
//            // 说明：注释掉的代码展示了如何手动播放声音
//            // 目前使用原版的自动声音播放

//            return null;  // 返回null使用默认行为
//        }

//        // 🛠️ 合成配方设置
//        public override void AddRecipes()
//        {
//            // 创建合成配方
//            CreateRecipe()
//                .AddIngredient<Last>()  // 添加材料：Last物品
//                                        //.AddTile<Tiles.Furniture.ExampleWorkbench>()  // 注释掉的工作台
//                .Register();  // 注册配方
//            // 说明：只需要Last材料，在任何工作台都可以合成
//        }
//    }
//}

using Microsoft.Xna.Framework;
using MistStar.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MistStar.Content.Items.Ranged
{
    public class 调试武器 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // 启用法杖属性
            Item.staff[Item.type] = true; // 标记为法杖，自动处理持握位置
            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
        }

        public override void SetDefaults()
        {
            // ========== 基础属性 ==========
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 5);

            // ========== 使用属性 ==========
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot; // 使用射击样式
            Item.autoReuse = true;

            // ========== 武器属性 ==========
            Item.damage = 85;
            Item.knockBack = 3.5f;
            Item.DamageType = DamageClass.Magic; // 魔法伤害
            Item.mana = 12; // 消耗法力

            // ========== 视觉属性 ==========
            Item.noMelee = true;
            Item.noUseGraphic = true; // 🔥 关键：不显示物品贴图

            // ========== 射击属性 ==========
            Item.shoot = ModContent.ProjectileType<调试弹幕2>(); // 发射的弹幕
            Item.shootSpeed = 12f; // 弹幕速度

            // ========== 音效 ==========
            Item.UseSound = SoundID.Item43;
        }

        // 🔥 关键：重写UseStyle来控制手持贴图
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            // 只在客户端处理视觉效果
            if (Main.dedServ) return;

            // 确保玩家持有正确的弹幕
            if (player.ownedProjectileCounts[ModContent.ProjectileType<调试弹幕1>()] <= 0)
            {
                // 创建手持弹幕
                int holdProj = Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<调试弹幕1>(),
                    0, 0f, player.whoAmI
                );

                if (holdProj < Main.maxProjectiles)
                {
                    Main.projectile[holdProj].timeLeft = 2;
                    player.heldProj = holdProj;
                }
            }

            // 更新玩家手持的弹幕引用
            UpdateHeldProjectile(player);
        }

        // 🔥 持续更新手持弹幕
        public override void HoldItem(Player player)
        {
            UpdateHeldProjectile(player);
        }

        // 🔧 更新手持弹幕位置和旋转
        private void UpdateHeldProjectile(Player player)
        {
            // 查找现有的手持弹幕
            int holdProjIndex = -1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI &&
                    proj.type == ModContent.ProjectileType<调试弹幕1>())
                {
                    holdProjIndex = i;
                    break;
                }
            }

            // 如果找不到手持弹幕，尝试创建一个
            if (holdProjIndex == -1 && player.itemAnimation > 0)
            {
                holdProjIndex = Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<调试弹幕2>(),
                    0, 0f, player.whoAmI
                );
            }

            // 更新手持弹幕
            if (holdProjIndex >= 0 && holdProjIndex < Main.maxProjectiles)
            {
                Projectile holdProj = Main.projectile[holdProjIndex];
                holdProj.timeLeft = 2; // 保持存活
                player.heldProj = holdProjIndex;

                // 更新位置和旋转
                UpdateStaffPosition(player, holdProj);
            }
        }

        // 🎯 更新法杖位置（类似长矛的持握方式）
        private void UpdateStaffPosition(Player player, Projectile staffProj)
        {
            // 计算鼠标方向
            Vector2 mousePos = Main.MouseWorld;
            Vector2 direction = Vector2.Normalize(mousePos - player.MountedCenter);
            if (direction == Vector2.Zero) direction = Vector2.UnitX;

            // 计算旋转（假设法杖贴图朝上）
            float rotation = direction.ToRotation() + MathHelper.PiOver2;

            // 处理玩家朝向
            if (direction.X < 0)
            {
                player.ChangeDir(-1);
                rotation += MathHelper.Pi; // 朝左时翻转
            }
            else
            {
                player.ChangeDir(1);
            }

            // 应用旋转
            staffProj.rotation = rotation;

            // 设置位置（玩家手部）
            Vector2 holdoutOffset = new Vector2(20f * player.direction, -8f);
            staffProj.Center = player.MountedCenter + holdoutOffset;
            staffProj.spriteDirection = player.direction;
        }

        // 🎯 重写Shoot方法实现法杖射击
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
                                  Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // 计算法杖尖端位置
            Vector2 mouseDirection = Vector2.Normalize(Main.MouseWorld - player.Center);
            Vector2 staffTip = player.Center + mouseDirection * 50f; // 法杖尖端偏移

            // 创建魔法弹幕
            Projectile.NewProjectile(
                source,
                staffTip,                    // 从法杖尖端发射
                velocity,                    // 使用计算的速度
                type,                        // 弹幕类型
                damage,                      // 伤害
                knockback,                   // 击退
                player.whoAmI                // 所有者
            );

            // 添加施法特效
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDustPerfect(
                    staffTip,
                    DustID.MagicMirror,
                    mouseDirection.RotatedByRandom(0.3f) * Main.rand.NextFloat(2f, 4f),
                    100, Color.Purple, 1.2f
                ).noGravity = true;
            }

            return false; // 不消耗额外弹药
        }

        // 🎵 自定义使用音效时机
        public override bool? UseItem(Player player)
        {
            // 在合适的时机播放音效
            if (!Main.dedServ)
            {
                // 可以在这里添加自定义音效逻辑
            }
            return base.UseItem(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5f, 0f); // 轻微偏移
        }
    }
}