using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

// 注意这里命名空间变了，多了个.Items
// 因为这个文件在Items文件夹，而读取图片的时候是根据命名空间读取的，如果写错了可能图片就读不到了
namespace MistStar.Content.Items.Magic
{

    // 保证类名跟文件名一致，这样也方便查找
    public class ButterflyLoveFlowers : ModItem
    {


        public override LocalizedText DisplayName => this.GetLocalization("DisplayName", () => "ButterflyLoveFlowers");
        public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");


        public override void SetDefaults()
        {
            // 伤害！想都不要想，后面这个值随便改吧，但是不要超过2147483647
            // 不然…… 你试试就知道了
            Item.damage = 350;
            Item.mana = 5;

            // 决定了这个武器的伤害属性，
            // melee 代表近战
            // ranged 代表远程
            // magic 代表膜法，不，魔法
            // summon 代表召唤
            // thrown 代表投掷
            Item.DamageType = DamageClass.Magic;

            // 物品的碰撞体积大小，可以与贴图无关，但是建议设为跟贴图一样的大小
            // 不然鬼知道会不会发生奇怪的事情
            Item.width = 70;
            Item.height = 26;

            // 攻击速度和攻击动画持续时间！
            // 这个数值越低越快，因为TR游戏速度每秒是60帧，这里的20就是
            // 20.0 / 60.0 = 0.333 秒挥动一次！也就是一秒三次
            // 一般来说我们要把这两个值设成一样，但也有例外的时候，我们以后会讲
            Item.useTime = 15;
            Item.useAnimation = 15;

            // 使用方式，这个值决定了武器使用时到底是按什么样的动画播放
            // 1 代表挥动，也就是剑类武器！
            // 2 代表像药水一样喝下去，emmmm这个放在剑上会不会很奇怪（吞
            // 3 代表像同志短剑一样刺x 出去
            // 4 唔，这个一般不是用在武器上的，想象一下生命水晶使用的时候的动作
            // 5 手持，枪、弓、法杖类武器的动作，用途最广
            Item.useStyle = ItemUseStyleID.Shoot;

            // 击退，你懂的，但是这个击退有个上限就是20，超过20击退效果跟20没什么区别
            // 后面的 'f' 表示这是个浮点数：8.25，但是这个'f'不可省略
            Item.knockBack = 5f;

            // 物品的价格，这里用sellPrice，也就是卖出物品的价格作为基准
            // 这件物品卖出时会获得 0白金 1金 60银 0铜 这么多的钱 （就这？
            Item.value = Item.sellPrice(0, 15, 0, 0);

            // 物品的稀有度，由-1到13越来越高，具体参考维基百科
            //https://terraria.gamepedia.com/Rarity 或者裙中世界的补充栏目
            Item.rare = ItemRarityID.Red;

            // 设置这个物品使用时发出的声音，以后会讲到怎么调出其他声音
            // 在这里我用的是普通的挥剑声音
            Item.UseSound = SoundID.Item21;

            // 决定了这个武器鼠标按住不放能不能一直攻击， true代表可以, false代表不行
            // （鼠标别按废了
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.Butterfiy>();
            //item.shoot = ProjectileID.MonkStaffT2Ghast;
            Item.shootSpeed = 7f;
            Item.noMelee = true;
            Item.crit = 80;
            //item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            // 计算鼠标位置
            Vector2 mousePos = Main.MouseWorld;

            // 计算法杖前端位置（向前50像素）
            Vector2 direction = Vector2.Normalize(mousePos - player.Center);
            Vector2 muzzlePosition = player.Center + direction * 80f;

            // 使用计算出的法杖前端位置
            Vector2 P = muzzlePosition;  // 使用 muzzlePosition 作为发射点
            Vector2 Z = mousePos - player.Center;    // 计算从法杖前端到鼠标的方向


            //int pro = new Projectile();
            // 创建新的Random实例用于生成整数随机数，用于控制散射角度
            Random ri = new Random();
            // 创建新的Random实例用于生成浮点随机数，用于控制散射角度
            Random rf = new Random();
            // 生成一个范围在[-2, 2)之间的随机整数，即-2, -1, 0, 1
            int RI = ri.Next(-2, 2);
            // 生成一个范围在[0.0, 1.0)之间的随机双精度浮点数
            double RF = rf.NextDouble();
            // 将整数部分和小数部分合并，得到最终的角度随机偏移量
            float R = RI + (float)RF;
            // 获取鼠标在世界中的坐标位置
            Vector2 M = Main.MouseWorld;
            // 注释掉的代码：原本打算从玩家中心发射
            // Vector2 P = player.Center;
            // Vector2 Z = M - P;
            // 计算弹幕速度向量：
            // 1. Z.ToRotation(): 将Z向量（从发射点到鼠标的方向向量）转换为角度（弧度制）
            // 2. R * (MathHelper.Pi / 36.0f): 将随机偏移R转换为弧度（π/36 ≈ 5度），乘以R得到实际偏移
            // 3. 将基础角度加上随机偏移，得到最终发射角度
            // 4. .ToRotationVector2(): 将角度转换回单位方向向量
            // 5. * 5: 将单位向量乘以速度标量5，得到最终速度向量
            Vector2 PV = (Z.ToRotation() + (R * (MathHelper.Pi / 36.0f))).ToRotationVector2() * 5;
            // 计算速度向量PV的角度（弧度制），使用Math.Atan2计算Y/X的反正切值
            // 这是计算向量与X轴正方向夹角的标准方法
            float PVA = (float)Math.Atan2(PV.Y, PV.X);
            // 创建新的弹幕实体：
            // 1. player.GetSource_ItemUse(Item): 获取物品使用的实体源（标识发射来源）
            // 2. P: 发射起始位置（需要定义）
            // 3. PV: 计算出的速度向量
            // 4. type: 弹幕类型ID
            // 5. damage: 弹幕伤害值
            // 6. knockback: 弹幕击退值
            // 7. player.whoAmI: 发射玩家的索引
            // 返回值PR是新创建弹幕在Main.projectile数组中的索引
            int PR = Projectile.NewProjectile(player.GetSource_ItemUse(Item), P, PV, type, damage, knockback, player.whoAmI);
            // 将计算出的角度PVA加到弹幕的当前旋转值上
            // 这会使弹幕贴图朝向与运动方向一致（如果弹幕没有自动设置旋转的话）
            Main.projectile[PR].rotation += PVA;
            // 返回false表示不消耗弹药（如果返回true则会消耗弹药）
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            // X坐标往里，Y坐标向上
            return new Vector2(-2, 0);
        }

        // 物品合成表的设置部分
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // 合成材料，需要10个泥土块
            recipe.AddIngredient(ItemID.FlowerofFire, 1);
            recipe.AddIngredient(ItemID.LunarBar, 1);
            recipe.AddIngredient(ItemID.ButterflyDust, 1);
            recipe.AddIngredient(ItemID.LunarBar, 1);
            recipe.AddIngredient(ItemID.FlowerofFrost, 1);
            recipe.AddTile(TileID.LunarCraftingStation);

            recipe.Register();
        }
    }
}



