using System;
using System.Reflection.Metadata;
using System.Threading;

class Program
{
    static World GameWorld = InitializeWorld();
    static Audio GameAudio = new Audio();
    static void Main(string[] args)
    {
        Player GamePlayer = new Player("Player", new Stats(100, 1, 1, 10000), new Inventory(new List<Object>(){new Weapon(45, "Standard sword", 35)}));
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
        
        // Travel(GamePlayer);
        Npc badNpc = new Npc("Bob", NpcType.HUMAN, new Stats(100, 1, 1, 500), new Inventory(), false);
        Fight(GamePlayer, badNpc);
    }

    static void Travel(Player player)
    {
        bool chooseLocation = true;
        int currentLocationChoice = 0;
        int currentSubLocationChoice = 0;
        while (true)
        {
            int longestLocationString = (GameWorld.GetLongestLocation() > 9) ? GameWorld.GetLongestLocation() : 9;
            int longestSubLocationString = (GameWorld.GetLongestSubLocation(GameWorld.Locations[currentLocationChoice]) > 9) ? GameWorld.GetLongestSubLocation(GameWorld.Locations[currentLocationChoice]) : 9;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"Current location: {GameWorld.CurrentSubLocation.Name} in {GameWorld.CurrentLocation.Name}\nYour gold: {player.Stats.Gold}\n\n");
            // write the travel choice menu thing
            Console.Write($"┌─Locations{new string('─', Math.Max(0, longestLocationString-9))}─┬─Price─┐ -> ┌─Locations{new string('─', Math.Max(0, longestSubLocationString-9))}─┐\n");
            for (int i = 0; i < Math.Max(GameWorld.Locations.Count+1, GameWorld.Locations[currentLocationChoice].SubLocations.Count+1); i++)
            {
                // print Locations
                Console.BackgroundColor = ConsoleColor.Black;
                if (i < GameWorld.Locations.Count) {
                    Console.Write("│ ");
                    if (currentLocationChoice == i) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write($"{GameWorld.Locations[i].Name}{new string(' ', longestLocationString-GameWorld.Locations[i].Name.Length)}");
                    Console.Write($" │ {GameWorld.Locations[i].TravelPrice,5}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" │    ");
                } else if (i == GameWorld.Locations.Count) {
                    Console.Write($"└─{new string('─', longestLocationString)}─┴───────┘    ");
                } else {
                    Console.Write($"{new string(' ', longestLocationString+12)}    ");
                }

                // print Sub locations
                Console.BackgroundColor = ConsoleColor.Black;
                if (i < GameWorld.Locations[currentLocationChoice].SubLocations.Count) {
                    Console.Write("│ ");
                    if (currentSubLocationChoice == i && !chooseLocation) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write($"{GameWorld.Locations[currentLocationChoice].SubLocations[i].Name}{new string(' ', Math.Max(0, longestSubLocationString-GameWorld.Locations[currentLocationChoice].SubLocations[i].Name.Length))}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" │\n");
                } else if (i == GameWorld.Locations[currentLocationChoice].SubLocations.Count) {
                    Console.Write($"└─{new string('─', longestSubLocationString)}─┘\n");
                } else {
                    Console.Write("\n");
                }
            }
            Console.Write("Up/Down/Left/Right = Choose location/sublocation\nEnter = Travel to location\nESC = Stop traveling");
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    chooseLocation = true;
                    currentSubLocationChoice = 0;
                }
                if (key == ConsoleKey.RightArrow)
                {
                    chooseLocation = false;
                }
                if (key == ConsoleKey.UpArrow)
                {
                    if (chooseLocation) { currentLocationChoice--; } else { currentSubLocationChoice--; }
                }
                if (key == ConsoleKey.DownArrow)
                {
                    if (chooseLocation) { currentLocationChoice++; } else { currentSubLocationChoice++; }
                }
                if (key == ConsoleKey.Enter)
                {
                    if (!chooseLocation && !GameWorld.Locations[currentLocationChoice].Locked && (GameWorld.CurrentLocation == GameWorld.Locations[currentLocationChoice] || player.Stats.Pay(GameWorld.Locations[currentLocationChoice].TravelPrice)))
                    {
                        GameWorld.TravelToLocation(GameWorld.Locations[currentLocationChoice]);
                        GameWorld.TravelToSubLocation(GameWorld.Locations[currentLocationChoice].SubLocations[currentSubLocationChoice]);
                        return;
                    }
                }
                if (key == ConsoleKey.Escape)
                {
                    return;
                }
                currentLocationChoice = Math.Clamp(currentLocationChoice, 0, Math.Max(0, GameWorld.Locations.Count-1));
                currentSubLocationChoice = Math.Clamp(currentSubLocationChoice, 0, Math.Max(0, GameWorld.Locations[currentLocationChoice].SubLocations.Count-1));
            } while (key != ConsoleKey.Escape && key != ConsoleKey.Enter && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow);
        }
    }

    private static List<Npc> CreateNpc()
    {
        // create peter
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
        return new List<Npc>(){new Npc("Peter Griffin", NpcType.HUMAN, new Stats(), new Inventory(), false, new List<Quest>(), peterDialogue)};
    }

    private static World InitializeWorld()
    {
        // Location: Town SubLocations: Bar, Fountain, Town_Sqaure, Shop, Hospital 
        SubLocation Bar = new SubLocation("Bar", new List<Npc>());
        SubLocation Fountain = new SubLocation("Fountain", new List<Npc>());
        SubLocation Town_Square = new SubLocation("Town Square", new List<Npc>());
        SubLocation Shop = new SubLocation("Shop", new List<Npc>());
        SubLocation Hospital = new SubLocation("Hospital", new List<Npc>()); 

        Location Town = new Location("Town", new List<SubLocation>{Bar, Fountain, Town_Square, Shop, Hospital}, 400);
        // Console.WriteLine(Town);

        // Location Castle SubLocations: Treasury, Throne Room, Dungeon
        SubLocation Treasury = new SubLocation("Treasury", new List<Npc>());
        SubLocation Throne_Room = new SubLocation("Throne Room", new List<Npc>());
        SubLocation Dungeon = new SubLocation("Dungeon", new List<Npc>());

        Location Castle = new Location("Castle", new List<SubLocation>{Treasury, Throne_Room, Dungeon}, 500);
        // Console.WriteLine(Castle);

        // Location: Mountain SubLocations: Cave, Vulcano
        SubLocation Cave = new SubLocation("Cave", new List<Npc>());
        SubLocation Vulcano = new SubLocation("Vulcano", new List<Npc>());

        Location Mountain = new Location("Mountain", new List<SubLocation>{Cave, Vulcano}, 300);
        // Console.WriteLine(Mountain);

        // Location: Farm SubLocations: River, Woods, Farmhouse
        SubLocation River = new SubLocation("River", new List<Npc>());
        SubLocation Woods = new SubLocation("Woods", new List<Npc>());
        SubLocation Farmhouse = new SubLocation("Farmhouse", new List<Npc>());

        Location Farm = new Location("Farm", new List<SubLocation>{River, Woods, Farmhouse}, 200);
        // Console.WriteLine(Farm);
        World w = new World(new List<Location>(){ Town, Castle, Mountain, Farm });
        w.CurrentLocation = Farm;
        w.CurrentSubLocation = Farmhouse;
        return w;
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
        int RollDice()
        {
            // print animating dice roll
            Console.Clear();
            Console.Write("Rolling...\n\n");
            (int x, int y) cursorPos = Console.GetCursorPosition();
            int roll = -1;
            for (int i = 0; i < 15; i++)
            {
                roll = Dice.Roll();
                Console.SetCursorPosition(cursorPos.x, cursorPos.y);
                Console.Write($"      .,-,.\n   _-'/   \\'-_\n_-'  /     \\  '-_\n|'--|-------|--'|\n| / \\       / \\ |\n|/   \\ {FormatNumber(roll, true)} /   \\|\n/     \\   /     \\\n|------\\ /------|\n '-._   |   _.-'\n     '-.'.-'\n\n");
                if (i < 14)
                {
                    Thread.Sleep(i*30);
                }
            }
            Thread.Sleep(500);
            return roll;
        }

        if (!npc.CanFight) { return; }
        if (player.Inventory.GetWeaponDamage() <= 0)
        {
            Console.Clear();
            Console.WriteLine("You do not have a weapon to fight...\n\nPress any key to continue.");
            Console.ReadKey(true);
            return;
        }
        int currentChoice = 0;
        Random r = new Random();
        do
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;

            // print combat menu and ask for option choice (defend/attack/use item)
            Console.Write($"{player.Name} VS {npc.Name}\n{player.Name}: {player.Stats.CurrentHealth}/{player.Stats.MaxHealth}\n{npc.Name}: {npc.Stats.CurrentHealth}/{npc.Stats.MaxHealth}\n\n");
            Console.Write($"┌─Options───────┐\n│ ");
            if (currentChoice == 0) { Console.BackgroundColor = ConsoleColor.DarkGray; }
            Console.Write($">1: Attack");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("    │\n│ ");
            if (currentChoice == 1) { Console.BackgroundColor = ConsoleColor.DarkGray; }
            Console.Write($">2: Defend");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("    │\n│ ");
            if (currentChoice == 2) { Console.BackgroundColor = ConsoleColor.DarkGray; }
            Console.Write($">3: Use Items");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" │\n└───────────────┘\n");
            // handle input selection
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
                    // calculate npc options (based on if they got armor weapons or healing)
                    int npcChoice = -1;
                    List<int> allowedOptions = new List<int>();
                    allowedOptions.Add(0);
                    if ((npc.Inventory.HasUsables && npc.Stats.CurrentHealth <= npc.Stats.MaxHealth/2)) { allowedOptions.Add(2); }
                    if (npc.Inventory.GetArmorPoints() > 0) { allowedOptions.Add(1); }
                    npcChoice = allowedOptions[r.Next(allowedOptions.Count)];
                    int npcBlock = npc.Inventory.GetArmorPoints();
                    int npcDamage = (int)((float)npc.Inventory.GetWeaponDamage() * ((float)Dice.Roll() / 10f));
                    
                    // handle player attacks/blocks/item uses
                    if (currentChoice == 0) // player attacks
                    {
                        int roll = RollDice();
                        int totalDamage = (int)((float)player.Inventory.GetWeaponDamage() * ((float)roll / 10f));
                        if (npcChoice == 0)
                        {
                            // npc attack
                            npc.Stats.Damage(totalDamage);
                            player.Stats.Damage(npcDamage);
                            Console.Write($"You did {totalDamage} damage to {npc.Name}!\n{npc.Name} did {npcDamage} damage to you!\n\n");
                        }
                        else if (npcChoice == 1)
                        {
                            // npc block (subtract armor points from attack)
                            npc.Stats.Damage(Math.Max(0, totalDamage - npcBlock));
                            Console.Write($"You did {totalDamage} damage to {npc.Name}!\n{npc.Name} blocked {npc.Inventory.GetArmorPoints()} of that damage!\n\n");
                        }
                        else if (npcChoice == 2)
                        {
                            // npc uses random items
                            (Usable npcItem, int healing) = npc.Inventory.UseHealItem(npc.Stats);
                            Console.Write($"You did {totalDamage} damage to {npc.Name}!\n{npc.Name} used {npcItem.Name} for {healing} healing!\n\n");
                        }
                        if (player.Stats.CurrentHealth > 0 && npc.Stats.CurrentHealth <= 0) { GameAudio.Play(@"Assets\Audio\Hit1.wav"); } else if (npc.Stats.CurrentHealth <= 0) { GameAudio.PlayRandomDeath(); } else { GameAudio.PlayRandomHit(); }
                    }
                    else if (currentChoice == 1) // player blocks
                    {
                        int roll = RollDice();
                        int totalBlock = (int)((float)player.Inventory.GetArmorPoints() * ((float)roll / 10f));
                        if (npcChoice == 0)
                        {
                            // npc attack
                            player.Stats.Damage(Math.Max(0, npcDamage-totalBlock));
                            Console.Write($"{npc.Name} did {npcDamage} damage to you!\nYou blocked {totalBlock} of that damage!\n\n");
                        }
                        else if (npcChoice == 1)
                        {
                            // npc block
                            Console.Write($"{npc.Name} blocked 0 damage!\nYou blocked 0 damage!\n\n");
                        }
                        else if (npcChoice == 2)
                        {
                            // npc uses random items
                            (Usable npcItem, int healing) = npc.Inventory.UseHealItem(npc.Stats);
                            Console.Write($"{npc.Name} used {npcItem.Name} for {healing} healing!\nYou blocked 0 damage!\n\n");
                        }
                        if (player.Stats.CurrentHealth > 0 && npc.Stats.CurrentHealth <= 0) { GameAudio.Play(@"Assets\Audio\Hit1.wav"); } else if (npc.Stats.CurrentHealth <= 0) { GameAudio.PlayRandomDeath(); } else { GameAudio.PlayRandomBlock(); }
                    }
                    else if (currentChoice == 2) // player uses item
                    {
                        List<Usable> items = UseItem(player);
                        Console.Clear();
                        if (items.Count == 0)
                        {
                            break;
                        }
                        Usable item = items[0];
                        if (npcChoice == 0)
                        {
                            // npc attack
                            player.Stats.Damage(npcDamage);
                            Console.Write($"{npc.Name} did {npcDamage} damage to you!\nYou used {item.Name} for {item.Amount} healing!\n\n");
                        }
                        else if (npcChoice == 1)
                        {
                            // npc block
                            Console.Write($"{npc.Name} blocked 0 damage!\nYou used {item.Name} for {item.Amount} healing!\n\n");
                        }
                        else if (npcChoice == 2)
                        {
                            // npc uses random items
                            (Usable npcItem, int healing) = npc.Inventory.UseHealItem(npc.Stats);
                            Console.Write($"{npc.Name} used {npcItem.Name} for {healing} healing!\nYou used {item.Name} for {item.Amount} healing!\n\n");
                        }
                        player.Stats.Heal(item.Amount);
                        player.Inventory.Remove(item);
                        if (player.Stats.CurrentHealth > 0 && npc.Stats.CurrentHealth <= 0) { GameAudio.Play(@"Assets\Audio\Hit1.wav"); } else if (npc.Stats.CurrentHealth <= 0) { GameAudio.PlayRandomDeath(); }
                    }
                    // print health and npc health
                    Console.Write($"You: {player.Stats.CurrentHealth}/{player.Stats.MaxHealth}\n");
                    Console.Write($"{npc.Name}: {npc.Stats.CurrentHealth}/{npc.Stats.MaxHealth}\n\n");
                    Console.Write("Press any key to continue...\n");
                    // lol, clearing input buffer from windows because we call Thread.Sleep it stacks the inputs given in between the sleep start and end
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                    Console.ReadKey(true);
                    currentChoice = 0;
                    break;
                }
                currentChoice = Math.Clamp(currentChoice, 0, 2);
            } while (key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.Enter);
        } while (player.Stats.CurrentHealth > 0 && npc.Stats.CurrentHealth > 0);
        if (npc.Stats.CurrentHealth <= 0 && player.Stats.CurrentHealth <= 0)
        {
            // both died
            Console.Clear();
            WriteParchment($"Death Certificate\n\nToday our dearest {npc.Name} left us behind.\n\nR.I.P. {npc.Name}\n\nOh and you also died. monster.\n");
            Console.Write("\nPress any key to continue...\n");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
        else if (npc.Stats.CurrentHealth <= 0)
        {
            // player won
            Console.Clear();
            if(npc.Type == NpcType.DOG){
                WriteParchment($"Death Certificate\n\nToday our dearest {npc.Name} left us behind.\n\nR.I.P. {npc.Name}");
            }
            for (int i = 0; i < player.OngoingQuests.Count; i++)
            {
                if (player.OngoingQuests[i].QuestType == QuestType.KILL && player.OngoingQuests[i].KillType == npc.Type)
                {
                    player.OngoingQuests[i].CurrentKills++;
                    if(player.OngoingQuests[i].CheckCompletion())
                    {
                        if (player.OngoingQuests[i].Callback != null)
                        {
                            player.OngoingQuests[i].Callback();
                        }
                        player.OngoingQuests[i].Complete(player);
                    }
                }
            }
            Console.Write($"You killed {npc.Name}!!!\nPress any key to continue looting...\n");
            Console.ReadKey(true);
            Loot(player, npc);
        }
        else if (player.Stats.CurrentHealth <= 0)
        {
            // npc won
            Console.Clear();
            WriteParchment($"You died!\n\n{npc.Name} lived happily ever after.\nThey even got a medal for their bravery.\n\nLet their name be known\n{npc.Name.ToUpper()}!!! {npc.Name.ToUpper()}!!! {npc.Name.ToUpper()}!!!\n");
            Console.Write("\nPress any key to continue...\n");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }

    static List<Usable> UseItem(Player player)
    {
        int currentChoice = 0;
        int longestPlayerString = (player.Inventory.GetLongestName() > 12) ? player.Inventory.GetLongestName() : 12;
        ConsoleKey key;
        while (true){
            Console.Clear();
            Console.Write($"Select your item:\n┌─Your Usables{new string('─', Math.Max(0, longestPlayerString-12))}─┬─Amount─┐\n");
            List<Usable> items = player.Inventory.GetHealingUsables();
            for (int i = 0; i < items.Count; i++)
            {
                // print player items
                Console.BackgroundColor = ConsoleColor.Black;
                // write the name of the player item
                Console.Write("│ ");
                if (currentChoice == i) { Console.BackgroundColor = ConsoleColor.DarkGray; } // if current selected item is this one we change the colors
                Console.Write($"{items[i].Name}{new string(' ', longestPlayerString-items[i].Name.Length)} │ {items[i].Amount,6}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" │\n");
            }
            Console.Write($"└─{new string('─', longestPlayerString)}─┴────────┘\n\n");
            Console.Write("Up/Down = Move around\nEnter = Use\nESC = Close inventory");
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
                    return new List<Usable>(){items[currentChoice]};
                }
                else if (key == ConsoleKey.Escape)
                {
                    return new List<Usable>();
                }
                currentChoice = Math.Clamp(currentChoice, 0, items.Count-1);
            } while (key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.Enter && key != ConsoleKey.Escape);
        }
        return new List<Usable>();
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
                        GameAudio.PlayRandomDrop();
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
                        GameAudio.PlayRandomDrop();
                        if (player.Inventory.Items.Count == 0)
                        {
                            currentChoice = 0;
                            sell = false;
                        }
                        for (int i = 0; i < player.OngoingQuests.Count; i++)
                        {
                            if (player.OngoingQuests[i].QuestType == QuestType.FETCH)
                            {
                                if(player.OngoingQuests[i].CheckCompletion())
                                {
                                    if (player.OngoingQuests[i].Callback != null)
                                    {
                                        player.OngoingQuests[i].Callback();
                                    }
                                    player.OngoingQuests[i].Complete(player);
                                }
                            }
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
    
    static void Loot(Player player, Npc npc)
    {
        int currentChoice = 0;
        bool drop = true;
        int longestPlayerString = (player.Name.Length > player.Inventory.GetLongestName()) ? player.Name.Length : player.Inventory.GetLongestName();
        int longestNpcString = (npc.Name.Length+8 > npc.Inventory.GetLongestName()) ? npc.Name.Length+8 : npc.Inventory.GetLongestName();

        // write loop
        while (true)
        {
            Console.Clear();
            string arrow = (drop) ? "Drop" : " <- ";

            // print menu with all player and npc items
            Console.BackgroundColor = ConsoleColor.Black;

            // Console.Write($"Your Gold: {player.Stats.Gold}{new string(' ', longestPlayerString-player.Stats.Gold.ToString().Length)}    {npc.Name}'s Gold: {npc.Stats.Gold}\n");
            Console.Write($"┌─Your Items{new string('─', Math.Max(0, longestPlayerString-10))}─┬─Gold─┐{arrow}┌─{npc.Name}'s Items{new string('─', Math.Max(0, (longestNpcString-npc.Name.Length)-8))}─┬─Gold─┐\n");
            for (int i = 0; i < Math.Max(npc.Inventory.Items.Count, player.Inventory.Items.Count)+1; i++)
            {
                // print player items
                Console.BackgroundColor = ConsoleColor.Black;
                if (i < player.Inventory.Items.Count) {
                    string playerItemName = player.Inventory.GetName(i);
                    int playerItemValue = player.Inventory.GetValue(i);
                    // write the name of the player item
                    Console.Write("│ ");
                    if (drop && currentChoice == i) { Console.BackgroundColor = ConsoleColor.DarkGray; } // if current selected item is this one we change the colors
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
                    if (!drop && currentChoice == i) { Console.BackgroundColor = ConsoleColor.DarkGray; } // if current selected item is this one we change the colors
                    Console.Write($"{npcItemName}{new string(' ', longestNpcString-npcItemName.Length)} │ {npcItemValue,4}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" │\n");
                } else if (i == npc.Inventory.Items.Count) {
                    Console.Write($"└─{new string('─', longestNpcString)}─┴──────┘\n");
                } else {
                    Console.Write("\n");
                }
            }
            Console.Write("Up/Down/Left/Right = Move around\nEnter = Take/Drop\nESC = Stop looting");
            // handle input like up/down/left/right/enter
            ConsoleKey key;
            do{
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    if (player.Inventory.Items.Count > 0)
                    {
                        drop = true;
                        currentChoice = 0;
                    }
                }
                if (key == ConsoleKey.RightArrow)
                {
                    if (npc.Inventory.Items.Count > 0)
                    {
                        drop = false;
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
                    if (!drop && player.Inventory.Items.Count < 10)
                    {
                        // take from npc
                        player.Inventory.Add(npc.Inventory.Items[currentChoice]);
                        npc.Inventory.Remove(currentChoice);
                        GameAudio.PlayRandomDrop();
                        currentChoice -= 1;
                        if (npc.Inventory.Items.Count == 0)
                        {
                            drop = true;
                            currentChoice = 0;
                        }
                    }
                    else if (drop)
                    {
                        // drop
                        player.Inventory.Remove(currentChoice);
                        GameAudio.PlayRandomDrop();
                        if (player.Inventory.Items.Count == 0)
                        {
                            currentChoice = 0;
                            drop = false;
                        }
                    }
                }
                if (key == ConsoleKey.Escape)
                {
                    return;
                }
                currentChoice = (drop) ? Math.Clamp(currentChoice, 0, Math.Max(0, player.Inventory.Items.Count-1)) : Math.Clamp(currentChoice, 0, Math.Max(0, npc.Inventory.Items.Count-1)); 
            } while (key != ConsoleKey.Escape && key != ConsoleKey.Enter && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow);
        }
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
    
    private static string FormatNumber(int number, bool center)
    {
        string numberString = number.ToString();
        return (numberString.Length == 2) ? $" {numberString}" : $" {numberString} ";
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
}

// Your current location: Town
// ┌─Locations─┬─Price─┐ -> ┌─Locations─┐
// │ Town      │   300 │    │ Inkeep    │
// │ Castle    │   150 │    │ Bar       │
// │ Mountain  │   500 │    └───────────┘
// └───────────┴───────┘

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
//    |                       | \n
//    |    >Yes        >No    | \n
//    |  ,---------------------,\n
//    \_/_____________________/ \n

//        _   _   _           _                      /)_
//   |\  (_) (_) (_)         (_) /|-----------------/  o\__
//   |_\__|___|___|___________|_/_|    ____________/ \ ____)
//    \                          /    /    Horsey   \ /
//     \   ___            ___   /    / / _________   |
//      \_/___\__________/___\_/    /_/| | |     | | |
//        \___/          \___/         |_|_|     |_|_|

// .__        _______        __.
//  \ ''--___/ _____ \___--'' /
//   '-__   |  O   O  |   __-'
//       '--|    O    |--'
//          |  A___A  |
//           \_______/

// ┌─npc name────┐\n
// │ npc text    │\n
// ├─────────────┤\n
// │ >1: option1 │\n
// │ >1: option2 │\n
// └─────────────┘\n

//   ___________
//  /           \
// |   /~~!~~\   |
// |  _       _  |
// | |_)  |  |_) |
// | | \. |. | . |
// |             |
// |   \~~!~~/   |
// |_____________|

//  ,-=-. 
// /  +  \
// | ~~~ |
// |R.I.P|
// |_____|

//[ goblins ]=-  6/97
//             ,      ,
//            /(.-""-.)\
//        |\  \/      \/  /|
//        | \ / =.  .= \ / |
//        \( \   o\/o   / )/
//         \_, '-/  \-' ,_/
//           /   \__/   \
//           \ \__/\__/ /
//         ___\ \|--|/ /___
//       /`    \      /    `\
//  jgs /       '----'       \
