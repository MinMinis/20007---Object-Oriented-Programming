﻿namespace RPGAdventure
{
    public class Action
    {
        private static Random random = new();
        public static void Attack(Player player, Enemy enemy)
        {
            int damage = random.Next(player.Damage, player.DamageMax) - enemy.Defense;
            if (damage <= 0)
            {
                GUI.Slowprint($"You attack, but the {enemy.Name}'s defense is too strong");
            }
            else
            {
                GUI.Slowprint($"You have used {player.Weapon} to attack {enemy.Name} and deal {damage} damage!");
                enemy.TakeDamage(damage);
            }
            EnemyAttack(player, enemy);
            GUI.WaitEnter();
        }

        public static void Defend(Player player, Enemy enemy)
        {
            player.IsDefending = true;
            GUI.Slowprint($"You prepare to defend against the {enemy.Name}'s attack.");
            EnemyAttack(player, enemy);
            GUI.WaitEnter();
        }

        public static void Heal(Player player, Enemy enemy)
        {
            player.UseHeal();
            EnemyAttack(player, enemy);
            GUI.WaitEnter();
        }

        public static void EnemyAttack(Player player, Enemy enemy)
        {
            int skillatk = 0;
            if (enemy.GetType() == typeof(Boss))
            {
                skillatk += random.Next(30, 50) * player.Level / 2;
                if (random.Next(0,1) >= 0)
                {
                    GUI.Slowprint($"The {enemy.Name} has used its ultimate to cause critical damage...");
                    Boss.UseSpecialAbility();
                }
            }
            if (enemy.IsDeath() == false)
            {
                int damage = skillatk + random.Next(enemy.Attack - 10, enemy.Attack + 10) - player.Defend;
                if (damage <= 0)
                {
                    damage = 0;
                    GUI.Slowprint($"The {enemy.Name} attacks, but your defense is too strong!");
                }
                else
                {
                    GUI.Slowprint($"The {enemy.Name} attacks and deals {damage} damage!");
                    player.TakeDamage(damage);
                }
                if (player.IsDefending)
                {
                    int hurt = damage / 2;
                    GUI.Slowprint($"You successfully defend against the {enemy.Name}'s attack! You have received {hurt} damage");
                    player.TakeDamage(hurt);
                    player.IsDefending = false;
                }
            }
        }
        public static void CheckExp(Player player)
        {
            while (true)
            {
                if (player.Exp >= 10)
                {
                    LevelUp(player);
                    GUI.Congrat($"You have level up. Current level: {player.Level}");
                }
                else
                {
                    int remainexp = 10 - player.Exp;
                    GUI.Slowprint($"You need {remainexp} exp to level up");
                    break;
                }
            }
        }
        private static void LevelUp(Player player)
        {
            if (player.Exp >= 10)
            {
                player.Level++;
                int upstat = random.Next(1, 10);
                player.Health += upstat;
                player.MaxHealth += upstat;
                int updmg = random.Next(1, 10);
                player.Damage += updmg;
                player.DamageMax += updmg;
                player.Defend += random.Next(1, 10);
                player.Exp -= 10;
            }
        }
    }
}
