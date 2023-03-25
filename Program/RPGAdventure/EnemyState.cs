﻿namespace RPGAdventure
{
    public class EnemyState : State
    {
        private Player currentplayer;
        private Enemy currentenemy;
        private Boss boss;
        private Random random = new();
        public EnemyState(Stack<State> states, Player player, Enemy enemy) : base(states)
        {
            this.currentplayer = player;
            this.currentenemy = enemy;
        }
        public void Process(string num)
        {
            while (end == false)
            {
                if (currentenemy.IsDeath())
                {
                    currentplayer.Win(currentenemy);
                    Action.CheckExp(currentplayer);
                    end = true;
                    break;
                }
                else if (currentplayer.Health > 0 && currentenemy.Health > 0)
                {
                    switch (num.ToLower())
                    {
                        case "e": //Exit
                        case "exit":
                            end = true;
                            Console.Clear();
                            break;
                        case "a":
                        case "attack":
                            Action.Attack(currentplayer, currentenemy);
                            break;
                        case "d":
                        case "defend":
                            Action.Defend(currentplayer, currentenemy);
                            break;
                        case "h":
                        case "heal":
                            Action.Heal(currentplayer, currentenemy);
                            break;
                        case "r":
                        case "run":
                            Run();
                            break;
                        default:
                            Console.Clear();
                            break;
                    }
                }
                else if (currentplayer.Health <= 0)
                {
                    DeathFlag();
                    end = true;
                    break;
                }
                Console.Clear();
            }
        }
        public void Run()
        {
            GUI.Slowprint($"You try to run away from the {currentenemy}...");
            if (random.Next(0,5) == 5)
            {
                GUI.Congrat($"You successfully run away from the {currentenemy}!");
                GUI.WaitEnter();
                end = true;
            }
            else
            {
                GUI.Slowprint($"The {currentenemy} catches up to you and blocks your escape");
                currentplayer.TakeDamage(currentenemy.Attack);
                GUI.Slowprint($"You have received {currentenemy.Attack} damage because you were worried to escape");
            }
        }
        public void DeathFlag()
        {
            GUI.Slowprint("Your HP has return to 0, please start a new game with a new character");
        }
        public override void Update()
        {
            Console.WriteLine(currentenemy.Enemyinfo());
            if (currentenemy == boss)
            {
                GUI.Title("Boss Fight");
            }
            else
            {
                GUI.Title("Creep Fight");
            }

            GUI.Menu("A", "Attack");
            GUI.Menu("D", "Defend");
            GUI.Menu("H", "Heal");
            GUI.Menu("R", "Run");
            Console.WriteLine(currentplayer.AllInfo());

            string input = GUI.GetCommandCount("Enter your option (Player)");
            Process(input);
        }
    }
}