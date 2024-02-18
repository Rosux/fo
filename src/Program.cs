using System;

class Program
{
    static void Main(string[] args)
    {
        Stats Stats = new Stats(100, 100 ,12, 20, 100);
        Stats StatsEnemy = new Stats(100, 100 ,12, 20, 100);
        Player Mike = new Player("Mike", null, Stats, null);
        Npc Warlock = new Npc("Warlock", null, StatsEnemy, null, null);
        Combat(Mike, Warlock);
        // Console.Clear();
        // Console.BackgroundColor = ConsoleColor.Black;
        // Console.ForegroundColor = ConsoleColor.DarkRed;
        // WriteCenter($"\n\n   ▄████████  ▄██████▄ \n  ███    ███ ███    ███\n  ███    █▀  ███    ███\n ▄███▄▄▄     ███    ███\n▀▀███▀▀▀     ███    ███\n  ███        ███    ███\n  ███        ███    ███\n  ███         ▀██████▀   ");
        // Console.ForegroundColor = ConsoleColor.White;
        // WriteCenter("\n(GOTY Edition)\n\n");
        // // Console.Beep();
        // Audio a = new Audio();
        // a.Play(@"Assets\Audio\hihi.wav");
        // Console.ReadKey();
    }

    static void WriteCenter(string text)
    {
        // split text at lines
        string[] lines = text.Split('\n');

        // find longest line
        int maxLength = lines.OrderByDescending(l=>l.Length).First().Length;

        // Calculate left padding for each line
        int screenWidth = Console.WindowWidth;
        foreach (string line in lines)
        {
            int padding = Math.Max((screenWidth - maxLength) / 2, 0);
            Console.Write(new string(' ', padding));
            Console.WriteLine(line);
        }
    }
    static void Combat(Player player, Npc npc)
    {
        while (player.Stats.CurrentHealth > 0  && npc.Stats.CurrentHealth > 0)
        {
            Console.WriteLine($"{player.Name} vs  {npc.Name}");
            Console.WriteLine($"enter A to attack or H to heal");
            int turn = 1;
            while (turn == 1 && player.Stats.CurrentHealth > 0){
            string Choice = Console.ReadLine();
                if (Choice == "a" || Choice == "A")
                {
                    int Roll = Dice.Roll();
                    npc.Stats.Damage(Roll);
                    Console.WriteLine($"{player.Name} did {Roll} dmg and {npc.Name} has {npc.Stats.CurrentHealth} HP left");
                    turn = 2;
                }
                else if (Choice == "h" || Choice == "H")
                {
                    int Roll = Dice.Roll();
                    player.Stats.Heal(Roll);
                    Console.WriteLine($"{player.Name} has healed {Roll} HP you have {player.Stats.CurrentHealth} HP left");
                    turn = 2;
                }
                else
                {
                    Console.WriteLine("wrong input try again");
                }
            }
            if (turn == 2 && npc.Stats.CurrentHealth > 0)
            {
                int Roll = Dice.Roll();
                player.Stats.Damage(Roll);
                Console.WriteLine($"{npc.Name} did {Roll} dmg and {player.Name} has {player.Stats.CurrentHealth} HP left");
                turn = 1;
            }
        }
        if (npc.Stats.CurrentHealth <= 0)
        {
            Console.WriteLine("you won good job!");
        }else{
            Console.WriteLine("game over!");
        }
    }
}

/*
      .,-,.\n
   _-'/   \'-_\n
_-'  /     \  '-_\n
|'--|-------|--'|\n
| / \       / \ |\n
|/   \ 20  /   \|\n
/     \   /     \\n
|------\ /------|\n
 '-._   |   _.-'\n
     '-.'.-'\n
*/

//         ▄████████  ▄██████▄ \n
//        ███    ███ ███    ███\n
//        ███    █▀  ███    ███\n
//       ▄███▄▄▄     ███    ███\n
//      ▀▀███▀▀▀     ███    ███\n
//        ███        ███    ███\n
//        ███        ███    ███\n
//        ███         ▀██████▀ \n