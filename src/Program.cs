using System;

class Program
{
    private static List<Npc> GameNpcs = new List<Npc>();
    
    static void Main(string[] args)
    {
        CreateNpc();
        Console.Title = "FO (GOTY Edition)";
        Console.CursorVisible = false;
        WriteLogo();
        Console.WriteLine("Press ENTER to start.");
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
        } while (key != ConsoleKey.Enter);
        Stats stats = new Stats(100, 100, 12, 20, 100);
        Player p = new Player("Player", stats, null, null);
        Npc john = new Npc("John", NpcType.HUMAN, null, new Inventory(), true);


        // Quest quest1 = new Quest("kill the goblin", QuestTy*--pe.KILL, KillType.GOBLIN, 3);
        // p.StartQuest(quest1);
        // p.UpdateQuests();

        p.Inventory.Add(new Weapon(0, "Iron Sword", 23));
        p.Inventory.Add(new Weapon(0, "Golden Sword", 300));
        john.Inventory.Add(new Weapon(0, "Stone Sword", 10));

        Trade(p, john);

        // Console.WriteLine($"{p.Name} {p.Stats.Gold}");
        // Talk(GameNpcs[0]);
        // Stats s = new Stats(0, 0, 0, 0, 0);
        // Inventory inv = new Inventory(new List<object>(){
        //     new Armor(20, "", 10),
        //     new Weapon(20, "", 10)
        // });
        // Console.WriteLine(inv.Items.Count);
        // inv.Add(new Usable(0, "", 100));
        // Console.WriteLine(inv.Items.Count);
        // Console.WriteLine(inv.GetWeaponDamage());
        // Console.WriteLine(inv.GetArmorPoints());
        // inv.Sell(2, s);
        // Console.WriteLine(s.Gold);
    }

    static void CreateNpc()
    {
        Dialogue peterDialogue = new Dialogue();
        peterDialogue.AddNode("1", "Hey! I'm peter griffin.\nHihihi.", new List<Option>(){
            new Option("Hi peter...\nhow are u?", "1.1"),
            new Option("Not true!", "1.2"),
        });
        peterDialogue.AddNode("1.1", "Bye.", new List<Option>(){
            new Option("...", null, "1")
        });
        peterDialogue.AddNode("1.2", "It's true, I am really peter griffin.", new List<Option>(){
            new Option("...", null, "1")
        });
        GameNpcs.Add(
            new Npc("Peter Griffin", NpcType.HUMAN, new Stats(), new Inventory(), false, new List<Quest>(), peterDialogue)
        );
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

    static void WriteLogo()
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        WriteCenter($"\n\n   ▄████████  ▄██████▄ \n  ███    ███ ███    ███\n  ███    █▀  ███    ███\n ▄███▄▄▄     ███    ███\n▀▀███▀▀▀     ███    ███\n  ███        ███    ███\n  ███        ███    ███\n  ███         ▀██████▀   ");
        Console.ForegroundColor = ConsoleColor.White;
        WriteCenter("\n(GOTY Edition)\n\n");
    }
    
    static void WriteParchment(string text)
    {
        string[] lines = text.Split("\n");
        int maxLength = lines.OrderByDescending(l=>l.Length).First().Length;

        Console.Write($"/ \\----{new string('-', maxLength)}---, \n\\_,|   {new string(' ', maxLength)}   | \n");
        for (int i = 0; i < lines.Length; i++)
        {
            Console.Write($"   |   {lines[i]}{new String(' ', (maxLength-lines[i].Length)+3)}| \n");
        }
        Console.Write($"   |  ,{new string('-', maxLength)}----,\n   \\_/_{new string('_', maxLength)}___/ \n");
    }

    static void Talk(Npc npc)
    {
        if (!npc.CanTalk){ return; }
        Dialogue dialogueTree = npc.Dialogue;
        string npcName = npc.Name;
        int currentChoice = 0;
        // weird internal method since i cant do `break 2;`
        void printingLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                // calculate longest line to make box responsive
                int longestLine = npcName.Length;
                int maxTextLength = dialogueTree.CurrentNode.Text.Split('\n').OrderByDescending(l=>l.Length).First().Length;
                if (maxTextLength > longestLine) { longestLine = maxTextLength; }
                foreach (KeyValuePair<int, string> text in dialogueTree.GetChoices())
                {
                    string[] lines = text.Value.Split('\n');
                    int maxLength = lines.OrderByDescending(l=>l.Length).First().Length;
                    if (maxLength+4 > longestLine) { longestLine = maxLength+4; }
                }
                // print npc name and text
                Console.Write($"┌─{npcName}{new string('─', longestLine-npcName.Length)}─┐\n");
                string[] textLines = dialogueTree.CurrentNode.Text.Split('\n');
                for (int i = 0; i < textLines.Length; i++)
                {
                    Console.Write($"│ {textLines[i]}{new string(' ', Math.Max(0, longestLine-textLines[i].Length))} │\n");
                }
                Console.Write($"├─{new string('─', longestLine)}─┤\n");
                // print options
                foreach (KeyValuePair<int, string> text in dialogueTree.GetChoices())
                {
                    string[] lines = text.Value.Split('\n');
                    for (int i = 0; i < lines.Length; i++){
                        Console.Write("│ ");
                        if (text.Key == currentChoice) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                        if (i == 0)
                        {
                            Console.Write($">{text.Key+1}: {lines[i]}{new string(' ', Math.Max(0, longestLine-lines[i].Length-4))}");
                        }
                        else
                        {
                            Console.Write($"{lines[i]}{new string(' ', Math.Max(0, longestLine-lines[i].Length))}");
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" │\n");
                    }
                }
                Console.Write($"└─{new string('─', longestLine)}─┘\n");
                // choose option by pressing up/down/enter
                ConsoleKey key;
                do
                {
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        currentChoice--;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        currentChoice++;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        if (dialogueTree.Step(currentChoice))
                        {
                            return;
                        }
                        currentChoice = 0;
                        break;
                    }
                    currentChoice = Math.Clamp(currentChoice, 0, Math.Max(0, dialogueTree.GetChoices().Count-1));
                } while (key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.Enter && dialogueTree.GetChoices().Count > 0);
            }
        }
        printingLoop();
    }

    static void Fight(Player player, Npc npc)
    {
        if (!npc.CanFight) { return; }
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

    static void Trade(Player player, Npc npc)
    {
        if (!npc.CanTrade) { return; }
        int currentChoice = 0;
        bool sell = true;
        int longestPlayerString = (player.Name.Length > player.Inventory.GetLongestName()) ? player.Name.Length : player.Inventory.GetLongestName();
        int longestNpcString = (npc.Name.Length+8 > npc.Inventory.GetLongestName()) ? npc.Name.Length+8 : npc.Inventory.GetLongestName();

        // write loop
        while (true)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            string arrow = (sell) ? "->" : "<-";
            Console.Write($"┌─Your Items{new string('─', Math.Max(0, longestPlayerString-10))}─┬─Gold─┐ {arrow} ┌─{npc.Name}'s Items{new string('─', Math.Max(0, (longestNpcString-npc.Name.Length)-8))}─┬─Gold─┐\n");
            
            for (int i = 0; i < Math.Max(npc.Inventory.Items.Count, player.Inventory.Items.Count); i++)
            {
                Console.WriteLine(i);
            }

            string playerItemName = ((Weapon)player.Inventory.Items[0]).Name;
            int playerItemValue = ((Weapon)player.Inventory.Items[0]).Worth;
            string playerItem = $"│ {playerItemName}{new string(' ', longestPlayerString-playerItemName.Length)} │ {playerItemValue,4} │";

            string npcItemName = ((Weapon)npc.Inventory.Items[0]).Name;
            int npcItemValue = ((Weapon)npc.Inventory.Items[0]).Worth;
            string npcItem = $"│ {npcItemName}{new string(' ', longestNpcString-npcItemName.Length)} │ {npcItemValue,4} │";

            Console.Write($"{playerItem}    {npcItem}");
            ConsoleKey key;
            do{
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    sell = true;
                }
                if (key == ConsoleKey.RightArrow)
                {
                    sell = false;
                }
            } while (key != ConsoleKey.Enter && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow);
        }
    }
}

// ┌─Your Items────────┬─Gold─┐ <- ┌─John's Items─────┬─Gold─┐
// │ Sword             │    1 │    │ Buying this item │    1 │
// │ Helmet            │   13 │    │ Iron Chestplate  │   13 │
// │ Potion of healing │   13 │    └──────────────────┴──────┘
// └───────────────────┴──────┘

//    ▄████████  ▄██████▄ \n
//   ███    ███ ███    ███\n
//   ███    █▀  ███    ███\n
//  ▄███▄▄▄     ███    ███\n
// ▀▀███▀▀▀     ███    ███\n
//   ███        ███    ███\n
//   ███        ███    ███\n
//   ███         ▀██████▀ \n

//       .,-,.\n
//    _-'/   \'-_\n
// _-'  /     \  '-_\n
// |'--|-------|--'|\n
// | / \       / \ |\n
// |/   \ 666 /   \|\n
// /     \   /     \\n
// |------\ /------|\n
//  '-._   |   _.-'\n
//      '-.'.-'\n

// some ending: WriteParchment("Dearest Player.\n\nThank you for playing our game!\n\nFrom,\nTeam FO");

// / \------------------------, \n
// \_,|                       | \n
//    |  Travel to the town?  | \n
//    |  ,---------------------,\n
//    \_/_____________________/ \n

//        _   _   _           _                      /)_
//   |\  (_) (_) (_)         (_) /|-----------------/  o\__
//   |_\__|___|___|___________|_/_|    ____________/ \ ____)
//    \                          /    /    Horsey   \ /
//     \   ___            ___   /    / / _________   |
//      \_/___\__________/___\_/    /_/| | |     | | |
//        \___/          \___/         |_|_|     |_|_|

// ┌─npc name────┐\n
// │ npc text    │\n
// ├─────────────┤\n
// │ >1: option1 │\n
// │ >1: option2 │\n
// └─────────────┘\n
