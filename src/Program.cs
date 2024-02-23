using System;
using System.Reflection.Metadata;

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

        // player example
        // Player p = new Player();
        // p.Stats.Gold = 666;

        // npc example with callback (run code after choosing an option)
        // bool wannaTrade = false;
        // Dialogue johnDialogue = new Dialogue();
        // johnDialogue.AddNode("1", "Yo i got cookies want some?", new List<Option>(){
        //     new Option("Yes", null, "1", () => {
        //         wannaTrade = true;
        //     }),
        //     new Option("No", null, "1", () => {
        //         wannaTrade = false;
        //     }),
        // });
        // Npc john = new Npc("John", NpcType.HUMAN, null, new Inventory(), true, null, johnDialogue);
        // john.Stats.Gold = 2000;

        // adding stuff to inventories
        // p.Inventory.Add(new Weapon(0, "Iron Sword", 23));
        // p.Inventory.Add(new Weapon(0, "Golden Sword", 300));
        // john.Inventory.Add(new Usable(1, "Cookie", 667));
        // john.Inventory.Add(new Weapon(0, "Wooden Sword", 10));
        // john.Inventory.Add(new Weapon(0, "Wooden Sword", 10));
        // john.Inventory.Add(new Weapon(0, "Wooden Sword", 10));
        // john.Inventory.Add(new Weapon(0, "Wooden Sword", 10));
        // john.Inventory.Add(new Weapon(0, "Wooden Sword", 10));
        // john.Inventory.Add(new Weapon(0, "Wooden Sword", 10));

        
        // start talking
        // Talk(john);
        // if (wannaTrade)
        // {
        //     // start trading
        //     Trade(p, john);
        // }

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
        // List<Location> locations = InitializeLocations();
        // World myWorld = new World(locations);
        // Stats playerStats = new Stats(CurrentHealth: 100, MaxHealth: 100, Attack: 20, Defence: 10, Gold: 100);
        // Console.WriteLine("Available Locations:");
        // myWorld.PrintAllLocations();
        // Location destination = GetValidLocationInput(locations);

        // myWorld.Travel(destination, playerStats);
        // Console.WriteLine($"Current Location: {myWorld.CurrentLocation.Name}");
        // Console.WriteLine($"Remaining Gold: {playerStats.Gold}");
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
    
    static Location GetValidLocationInput(List<Location> locations) // maybe remove
    {
        Location destination = null;

            while (destination == null)
            {
                Console.Write("Enter the name of the location you want to travel to: ");
                string userInput = Console.ReadLine();

                destination = locations.Find(loc => loc.Name.Equals(userInput, StringComparison.OrdinalIgnoreCase));

                if (destination == null)
                {
                    Console.WriteLine("Invalid location name. Please enter a valid name.");
                }
            }

        return destination;
    }

    static List<Location> InitializeLocations()
    {   
        // Location: Town SubLocations: Bar, Fountain, Town_Sqaure, Shop, Hospital 
        SubLocation Bar = new SubLocation("Bar", new List<Npc>());
        SubLocation Fountain = new SubLocation("Fountain", new List<Npc>());
        SubLocation Town_Square = new SubLocation("Town Square", new List<Npc>());
        SubLocation Shop = new SubLocation("Shop", new List<Npc>());
        SubLocation Hospital = new SubLocation("Hospital", new List<Npc>()); 

        Location Town = new Location("Town", new List<SubLocation>{Bar, Fountain, Town_Square, Shop, Hospital}, 20);
        // Console.WriteLine(Town);

        // Location Castle SubLocations: Treasury, Throne Room, Dungeon
        SubLocation Treasury = new SubLocation("Treasury", new List<Npc>());
        SubLocation Throne_Room = new SubLocation("Throne Room", new List<Npc>());
        SubLocation Dungeon = new SubLocation("Dungeon", new List<Npc>());

        Location Castle = new Location("Castle", new List<SubLocation>{Treasury, Throne_Room, Dungeon}, 40);
        // Console.WriteLine(Castle);

        // Location: Mountain SubLocations: Cave, Vulcano
        SubLocation Cave = new SubLocation("Cave", new List<Npc>());
        SubLocation Vulcano = new SubLocation("Vulcano", new List<Npc>());

        Location Mountain = new Location("Mountain", new List<SubLocation>{Cave, Vulcano}, 60);
        // Console.WriteLine(Mountain);

        // Location: Farm SubLocations: River, Woods, Farmhouse
        SubLocation River = new SubLocation("River", new List<Npc>());
        SubLocation Woods = new SubLocation("Woods", new List<Npc>());
        SubLocation Farmhouse = new SubLocation("Farmhouse", new List<Npc>());

        Location Farm = new Location("Farm", new List<SubLocation>{River, Woods, Farmhouse}, 80);
        // Console.WriteLine(Farm);
        return new List<Location> { Town, Castle, Mountain, Farm };
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
            string arrow = (sell) ? "->" : "<-";

            // print menu with all player and npc items
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Write($"Your Gold: {player.Stats.Gold}{new string(' ', longestPlayerString-player.Stats.Gold.ToString().Length)}    {npc.Name}'s Gold: {npc.Stats.Gold}\n");
            Console.Write($"┌─Your Items{new string('─', Math.Max(0, longestPlayerString-10))}─┬─Gold─┐ {arrow} ┌─{npc.Name}'s Items{new string('─', Math.Max(0, (longestNpcString-npc.Name.Length)-8))}─┬─Gold─┐\n");
            for (int i = 0; i < Math.Max(npc.Inventory.Items.Count, player.Inventory.Items.Count)+1; i++)
            {
                // print player items
                Console.BackgroundColor = ConsoleColor.Black;
                if (i < player.Inventory.Items.Count) {
                    string playerItemName = player.Inventory.GetName(i);
                    int playerItemValue = player.Inventory.GetValue(i);
                    // write the name of the player item
                    Console.Write("│ ");
                    if (sell && currentChoice == i) { Console.BackgroundColor = ConsoleColor.DarkGray; } // if current selected item is this one we change the colors
                    Console.Write($"{playerItemName}{new string(' ', longestPlayerString-playerItemName.Length)} │ {playerItemValue,4}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" │    ");
                } else if (i == player.Inventory.Items.Count) {
                    Console.Write($"└─{new string('─', longestPlayerString)}─┴──────┘    ");
                } else {
                    Console.Write($"{new string(' ', longestPlayerString+11)}    ");
                }
                // print npc items
                Console.BackgroundColor = ConsoleColor.Black;
                if (i < npc.Inventory.Items.Count) {
                    string npcItemName = npc.Inventory.GetName(i);
                    int npcItemValue = npc.Inventory.GetValue(i);
                    Console.Write("│ ");
                    if (!sell && currentChoice == i) { Console.BackgroundColor = ConsoleColor.DarkGray; } // if current selected item is this one we change the colors
                    Console.Write($"{npcItemName}{new string(' ', longestNpcString-npcItemName.Length)} │ {npcItemValue,4}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" │\n");
                } else if (i == npc.Inventory.Items.Count) {
                    Console.Write($"└─{new string('─', longestNpcString)}─┴──────┘\n");
                } else {
                    Console.Write("\n");
                }
            }
            Console.Write("Up/Down/Left/Right = Move around\nEnter = Buy/Sell\nESC = Stop trading");
            // handle input like up/down/left/right/enter
            ConsoleKey key;
            do{
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    if (player.Inventory.Items.Count > 0)
                    {
                        sell = true;
                        currentChoice = 0;
                    }
                }
                if (key == ConsoleKey.RightArrow)
                {
                    if (npc.Inventory.Items.Count > 0)
                    {
                        sell = false;
                        currentChoice = 0;
                    }
                }
                if (key == ConsoleKey.UpArrow)
                {
                    currentChoice--;
                }
                if (key == ConsoleKey.DownArrow)
                {
                    currentChoice++;
                }
                if (key == ConsoleKey.Enter)
                {
                    if (!sell && player.Inventory.Items.Count < 10 && player.Stats.Pay(npc.Inventory.GetValue(currentChoice)))
                    {
                        // buy from npc
                        // could add logic here to increase the price its sold for
                        player.Inventory.Add(npc.Inventory.Items[currentChoice]);
                        npc.Inventory.Sell(currentChoice, npc.Stats);
                        currentChoice -= 1;
                        if (npc.Inventory.Items.Count == 0)
                        {
                            currentChoice = 0;
                            sell = true;
                        }
                    }
                    else if (sell && npc.Inventory.Items.Count < 10 && npc.Stats.Pay(player.Inventory.GetValue(currentChoice)))
                    {
                        // sell to npc
                        // could add logic to decrease the price its sold for to the npc like some npc's are scammers and some are generous or something
                        npc.Inventory.Add(player.Inventory.Items[currentChoice]);
                        player.Inventory.Sell(currentChoice, player.Stats);
                        if (player.Inventory.Items.Count == 0)
                        {
                            currentChoice = 0;
                    sell = false;
                    sell = false;
                            sell = false;
                        }
                    }
                }
                if (key == ConsoleKey.Escape)
                {
                    return;
                }
                currentChoice = (sell) ? Math.Clamp(currentChoice, 0, Math.Max(0, player.Inventory.Items.Count-1)) : Math.Clamp(currentChoice, 0, Math.Max(0, npc.Inventory.Items.Count-1)); 
            } while (key != ConsoleKey.Escape && key != ConsoleKey.Enter && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow);
        }
    }
}

// Your Gold: 940                  John's Gold: 69420
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
