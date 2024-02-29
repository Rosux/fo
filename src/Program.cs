using System;
using System.Reflection.Metadata;
using System.Threading;

class Program
{
    static Player GamePlayer = new Player();
    static World GameWorld = InitializeWorld(GamePlayer);
    static Audio GameAudio = new Audio();
    static void Main(string[] args)
    {
        Console.Title = "FO (GOTY Edition)";
        Console.CursorVisible = false;
        ConsoleKey key;
        do
        {
            WriteLogo();
            Console.WriteLine("Press ENTER to start a new game.\nPress H for help.\nPress ESC to exit the game.");
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape)
            {
                return;
            }
            if (key == ConsoleKey.H)
            {
                // print help menu text stuff
                Console.Clear();
                WriteParchment("Fo is a game about stories\n\nYou can travel around and interact with people.\n");
                Console.Write("\n\nPress any key to go back.");
                Console.ReadKey();
            }
            if (key == ConsoleKey.Enter)
            {
                GamePlayer = new Player("PIETER POST", new Stats(300, 1, 1, 10000), new Inventory(new List<Object>(){new Weapon(90, "reddit moderator katana", 50), new Armor(50, "Stupid helmet", 15), new Usable(UseType.HEAL, 200, "Potion of health", 250) }));
                GameWorld = InitializeWorld(GamePlayer);
                MainGameplayLoop();
            }
        } while (true);
        
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
            Console.Write("     _   _   _           _                      /)_\n|\\  (_) (_) (_)         (_) /|-----------------/  o\\__\n|_\\__|___|___|___________|_/_|    ____________/ \\ ____)\n \\                          /    /             \\ /\n  \\   ___            ___   /    / / _________   |\n   \\_/___\\__________/___\\_/    /_/| | |     | | |\n     \\___/          \\___/         |_|_|     |_|_|\n");
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
                        GameAudio.PlayRandomCoin();
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

    private static World InitializeWorld(Player player)
    {

        // drunkard example
        Npc drunkard = new Npc("Terry", NpcType.HUMAN, new Stats(50, 0, 0, 60), new Inventory(), true, null, null);
        Dialogue drunkardDialogue = new Dialogue();
        drunkardDialogue.AddNode("1", "awwha whhah hahwhw whahh\nwwahahhahhhhwhah (What do you want?)", new List<Option>(){
            new Option("I can help you", "1.1"),
            new Option("Never mind", null, "1"),
        });
        drunkardDialogue.AddNode("1.1", "whahh ahwhhwh whhahhah\nhwhha ha (Can you get me some beer?\nFrom the cave dwarfs.)", new List<Option>(){
            new Option("Sure", "1.1.1", null, ()=>{
                player.AddQuest(
                    new Quest("Fetch a beer and deliver it to the drunkard.", QuestType.FETCH, ItemType.USABLE, "Beer", drunkard, ()=>{
                        drunkardDialogue.AddOption("1", new Option("I gave you that beer.", "1.2"));
                    })
                );
                drunkardDialogue.RemoveOption("1", 0);
            }),
            new Option("No", null, "1"),
        });

        drunkardDialogue.AddNode("1.2", "hwahhahw hawhh\nwahh awhhhha whh\nawh (Thanks I needed that)", new List<Option>(){
            new Option("...", null, "1", ()=>{
                drunkardDialogue.RemoveOption("1", 1);
            }),
        });
        
        drunkardDialogue.AddNode("1.1.1", "ahwha whahawhwu awuhwah uuhwa\nahhhhwhah (Thanks)", new List<Option>(){
            new Option("...", null, "1"),
        });
        drunkard.Dialogue = drunkardDialogue;
        drunkard.CanTalk = true;

        // ronnie mcnutt
        Npc Ronnie = new Npc("Ronnie mcnutt", NpcType.HUMAN, new Stats(1, 0, 0, 50), new Inventory(new List<Object>(){new Weapon(40, "Shotgun", 200)}), true, null, null);
        Dialogue ronnieDialogue = new Dialogue();
        ronnieDialogue.AddNode("1", "What brings you 'round here, stranger? You lookin' for trouble or just lost?", new List<Option>(){
            new Option("I heard you got something for me", "1.1"),
            new Option("Yeah I'm looking for trouble", "1.2"),
        });
        ronnieDialogue.AddNode("1.1", "Oh, its you! Here, I was going to use this but I changed my mind.", new List<Option>(){
            new Option("Take", null, "1", ()=>{
                player.Inventory.Add(new Weapon(40, "Shotgun", 200));
                ronnieDialogue.RemoveOption("1", 0);
            }),
        });
        ronnieDialogue.AddNode("1.2", "Well, you found it, partner. But let me tell ya, \ntrouble has a way of findin' folks like you even when they ain't lookin' for it.\n Best be careful 'round these parts", new List<Option>(){
            new Option("Allright, I'll be on my way", null, "1"),
        });
        Ronnie.Dialogue = ronnieDialogue;
        Ronnie.CanTalk = true;


        // guillotine guy
        Npc beheaded = new Npc("Guy in guillotine", NpcType.HUMAN, new Stats(200, 0, 0, 0), new Inventory(), true, null, null);
        Dialogue beheadedDialogue = new Dialogue();
        beheadedDialogue.AddNode("1", "Please, stranger, hear my plea. I'm innocent! \nThey've framed me! You must believe me", new List<Option>(){
            new Option("I don't believe you", "1.1"),
            new Option("Tell me more!", "1.2"),
        });
        beheadedDialogue.AddNode("1.1", "*Guy gets beheaded*", new List<Option>(){
            new Option("steal coins", null, "1", () => {
                GamePlayer.Stats.AddGold(40);
                beheadedDialogue.RemoveOption("1", 0);
            })
        });
        beheadedDialogue.AddNode("1.2", "I was framed by the king's son! He's the real culprit, I swear i- *SLPASH*", new List<Option>(){
            new Option("....", null),
        });
        beheaded.Dialogue = beheadedDialogue;
        beheaded.CanTalk = true;

        // nurse joy
        Dialogue nursejoyDialogue = new Dialogue();
        Npc Nurse = new Npc("Joy", NpcType.HUMAN, new Stats(200, 0, 0, 500), new Inventory(new List<Object>(){new Usable(UseType.HEAL, 100, "Health Potion", 50)}), true, null, nursejoyDialogue);
        nursejoyDialogue.AddNode("1", $"Hello, do you need some healing. (Current health: {GamePlayer.Stats.CurrentHealth}/{GamePlayer.Stats.MaxHealth})", new List<Option>(){
            new Option("Yes", null, "1", ()=>{
                Trade(player, Nurse);
            }),
            new Option("No, I don't need help.", "1.1")
        });
        nursejoyDialogue.AddNode("1.1", "Alright, have a nice day.", new List<Option>(){
            new Option("Bye.", null, "1")
        });
        
    
        // shopkeeper
        Dialogue shopDialogue = new Dialogue();
        Npc Shopkeeper = new Npc("Mort", NpcType.HUMAN, new Stats(300, 0, 0, 5000), new Inventory(new List<Object>(){new Weapon(100, "Steel Sword", 150), new Weapon(150, "Steel Axe", 200),new Weapon(200, "magnificent Sword", 300), new Weapon(250, "magnificent Axe", 350)}), true, null, shopDialogue);
        shopDialogue.AddNode("1", "Welcome to my shop. Want anything?", new List<Option>(){
            new Option("Yes", null, "1", ()=>{
                Trade(player, Shopkeeper);
            }),
            new Option("Got any quests?", "1.1")
        });
        shopDialogue.AddNode("1.1", "Yes, I heard the Dwarfs make great beer that I would like to sell, but they won't sell it to me.\nCould you fetch a beer for me so i can make it myself.", new List<Option>(){
            new Option("Sure", null, "1", ()=>{
                player.AddQuest(
                    new Quest("Fetch a beer and deliver it to the shopkeeper.", QuestType.FETCH, ItemType.USABLE, "Beer", Shopkeeper, null)
                );
                shopDialogue.RemoveOption("1", 1);
            }),
            new Option("No", null, "1")
         });

        // thief
        Dialogue ThiefDialogue = new Dialogue();
        Npc Thieff = new Npc("adiaq la", NpcType.HUMAN, new Stats(200, 0, 0, 500), new Inventory(), false, null, ThiefDialogue);
        ThiefDialogue.AddNode("1", "Hello, I'm adiaq la, not stealing or doing anything shady.\nBut would you mind distracting the shopkeeper for me?.", new List<Option>(){
            new Option("Ok, you look trustworthy.","1.1"),
            new Option("No, I don't trust you, I'm gonna warn the shopkeeper about you.", "1.2")
        });
        ThiefDialogue.AddNode("1.1", "Thanks for distracting him I stole all his money form the cash register", new List<Option>(){
            new Option("Nice, but i want half of the money.","1.2"),
            new Option("you are a thief, I'm gonna tell the shopkeeper.","1.2")
        });
        ThiefDialogue.AddNode("1.2","Nah man I can't have that", new List<Option>(){
            new Option("I'm gonna need to or else", null, "1", ()=>{
                ThiefDialogue.RemoveOption("1", 1);
                ThiefDialogue.RemoveOption("1", 0);
                ThiefDialogue.AddOption("1", new Option("No, you're a thief.", null, "1"));
            }
        )});

        Dialogue HobboDialogue = new Dialogue();
        Npc Hobbo = new Npc("Mallard", NpcType.HUMAN, new Stats(10, 0, 0, 0), new Inventory(), false, null, HobboDialogue);
        HobboDialogue.AddNode("1", "Hello, I'm Mallard, I recently lost all my money because of a nasty divorce.\nCould you give me 10 gold?.", new List<Option>(){
            new Option("Ok. (Give 10 gold)", null, "1", ()=>{
                if (GamePlayer.Stats.Pay(10))
                {
                    Hobbo.Stats.AddGold(10);
                }
            }),
            new Option("No, go make your own money.","1.2")
        });
        HobboDialogue.AddNode("1.1", "Thanks, have a nice day.", new List<Option>(){
            new Option("No problem, have a nice day.", null, "1")
        });
        HobboDialogue.AddNode("1.2", "ok, dont't waste my time.", new List<Option>(){
            new Option("Bye.", null, "1")
        });

        Dialogue peterDialogue = new Dialogue();
        Npc Bird = new Npc("Peter Griffin", NpcType.BIRD, new Stats(500, 0, 0, 0), new Inventory(), false, null, peterDialogue);
        peterDialogue.AddNode("1", "Hey! I'm peter griffin.\nHihihi.", new List<Option>(){
            new Option("Hi peter...\nhow are u?", "1.1"),
            new Option("Not true you are a bird!", "1.2")
        });
        peterDialogue.AddNode("1.1", "I am fine.", new List<Option>(){
            new Option("Ok, Have a nice day", null, "1")
        });
        peterDialogue.AddNode("1.2", "It's true, I am really peter griffin.", new List<Option>(){
            new Option("Prove it.", "1.3"),
            new Option("Sure and I am Obama.", "1.4")
        });
        peterDialogue.AddNode("1.3", "Ok, 'SHUT UP MEG'.", new List<Option>(){
            new Option("That doesn't prove anyting", null, "1")
        });
        peterDialogue.AddNode("1.4", "Well, hello obama nice to meet you.", new List<Option>(){
            new Option(".... Man forget this stupidity, I'm talking to a damn bird", null, "1")
        });

        Dialogue FishDialogue = new Dialogue();
        Npc Fish = new Npc("Fish", NpcType.FISH, new Stats(50, 0, 0, 0), new Inventory(new List<object>(){new Usable(UseType.HEAL, 15, "Fish", 40)}), false, null, FishDialogue);
        Npc Fish1 = new Npc("Fish", NpcType.FISH, new Stats(50, 0, 0, 0), new Inventory(new List<object>(){new Usable(UseType.HEAL, 15, "Fish", 40)}), false, null, FishDialogue);
        Npc Fish2 = new Npc("Fish", NpcType.FISH, new Stats(50, 0, 0, 0), new Inventory(new List<object>(){new Usable(UseType.HEAL, 15, "Fish", 40)}), false, null, FishDialogue);
        FishDialogue.AddNode("1", "Blub, blub, splash", new List<Option>(){
            new Option("Leave the fish alone.", null, "1")
        });

        Dialogue SnakeDialogue = new Dialogue();
        Npc Snakes = new Npc("Snake", NpcType.SNAKE, new Stats(75, 0, 0, 0), new Inventory(new List<Object>(){new Weapon(10, "Bite", 0)}), false, null, SnakeDialogue);
        SnakeDialogue.AddNode("1", "Hiss, Hiss", new List<Option>(){
            new Option("Leave the Snake alone.", null, "1")
        });

        Dialogue GoblinDialogue = new Dialogue();
        Npc Goblins = new Npc("Goblin", NpcType.GOBLIN, new Stats(200, 0, 0, 50), new Inventory(new List<Object>(){new Weapon(30, "Wooden Club", 75)}), false, null, GoblinDialogue);
        GoblinDialogue.AddNode("1", "Grr, Meat, Kill", new List<Option>(){
            new Option("Leave the Goblin alone.", null, "1")
        });

        Dialogue DwarfDialogue = new Dialogue();
        Npc Dwarfs = new Npc("Dwarf", NpcType.DWARF, new Stats(200, 0, 0, 50), new Inventory(new List<Object>(){new Weapon(30, "Axe", 75), new Usable(UseType.HEAL, 15, "Beer", 40)}), false, null, DwarfDialogue);
        DwarfDialogue.AddNode("1", "Hey!!, Human get out of my cave", new List<Option>(){
            new Option("Leave the Dwarf alone.", null, "1")
        });

        Dialogue MotherDialogue = new Dialogue();
        Npc Mother = new Npc("teressa", NpcType.HUMAN, new Stats(500, 0, 0, 100), new Inventory(), true, null, MotherDialogue);
        MotherDialogue.AddNode("1", "Hello my child, could you help me with something.", new List<Option>(){
            new Option("Yes, mother.", "1.1"),
            new Option("No, I don't have the time.", null, "1")
        });
        MotherDialogue.AddNode("1.1", "Thanks, could you go fetch a fish from the river for me it's for dinner.", new List<Option>(){
            new Option("Yes, mother.", null, "1", ()=>{
                player.AddQuest(
                    new Quest("Fetch a fish from the river and deliver it your mother.", QuestType.FETCH, ItemType.USABLE, "Fish", Mother, null)
                );
                MotherDialogue.RemoveOption("1", 0);
            }),
            new Option("No, I don't like fish.", null, "1")
        });


        //Patient prapor
        Npc Patient = new Npc("Prapor", NpcType.HUMAN, new Stats(50, 0, 0, 200), new Inventory(), false, null, null);
        Dialogue patientDialogue = new Dialogue();
        patientDialogue.AddNode("1", "Hey... you there... Could ya help me? I got this letter... for Ronnie McNutt in the\x1b[1m town center\x1b[0m... Could ya take it for me?", new List<Option>(){
            new Option("Of course, I'll make sure Ronnie gets it.", "1.1"),
            new Option("Sorry, I'm busy right now", null, "1"),
        });
        patientDialogue.AddNode("1.1", "Thank you... you're a real lifesaver... literally...", new List<Option>(){
            new Option("No problem.", null, "1", ()=>{
                player.AddQuest(
                    new Quest("Deliver letter to Ronnie McNutt in Town Centre", QuestType.FETCH, ItemType.USABLE, "Letter", Ronnie, null)
                );
                    GamePlayer.Inventory.Add(new Usable(UseType.HEAL, 1, "Letter", 0));
                patientDialogue.RemoveOption("1", 0);
            }),
        });
        Patient.Dialogue = patientDialogue;
        Patient.CanTalk = true;

        //King Nikita
        Npc King = new Npc("Crazy king Nikita", NpcType.HUMAN, new Stats(150, 0, 0, 750), new Inventory(), false, null, null);
        Dialogue kingDialogue = new Dialogue();
        kingDialogue.AddNode("1", "Well well well... \nLook who arrived at my Castle!", new List<Option>(){
            new Option("Continue", "1.1"),
        });
        kingDialogue.AddNode("1.1", "I have heard of you before. \nYou are one of the greatest fighters known.", new List<Option>(){
            new Option("Continue", "1.2"),
        });
        kingDialogue.AddNode("1.2", "So, I will give you a quest \nfor you to prove your skills\nAre you up for this?", new List<Option>(){
            new Option("Yes, what is it?", "1.3"),
            new Option("Sorry, I'm busy right now", null, "1"),
        });
        kingDialogue.AddNode("1.3", "You must defeat \x1b[1mRobert the Dragon\x1b[0m in\nthe \x1b[1mVolcano\x1b[0m in the \x1b[1mMountain!\x1b[0m", new List<Option>(){
            new Option("Let's do it!", "1.4"),
            new Option("Nevermind, this sounds too scary", null, "1"),
        });
        kingDialogue.AddNode("1.4", "Okay! Good luck soldier!", new List<Option>(){
            new Option("Continue", null, "1", ()=>{
                player.AddQuest(
                    new Quest("Kill the Dragon in the Volcano", QuestType.KILL, NpcType.DRAGON, 0)
                );
                kingDialogue.RemoveOption("1.2", 0);
            }),
        });
        King.Dialogue = kingDialogue;
        King.CanTalk = true;

        Npc Guards = new Npc("Guard", NpcType.HUMAN, new Stats(300, 0, 0, 250), new Inventory(new List<Object>(){new Weapon(20, "Royal Sword X 5", 200)}), false, null, null);
        Dialogue guardsDialogue = new Dialogue();
        guardsDialogue.AddNode("1", "STOP! Who are you? \nYou can't just come to the treasury", new List<Option>(){
            new Option("\"I am a traveller from the farms. I'm here for the King! \nWhere can i find him?\"", "1.1"),
        });
        guardsDialogue.AddNode("1.1", "Allright, the king is in his throne room.\nI'd watch out though if I were you\nHe's a bit crazy", new List<Option>(){
            new Option("Thanks", null, "1"),
        });
        Guards.Dialogue = guardsDialogue;
        Guards.CanTalk = true;

        //barkeeper
        Npc Barkeeper = new Npc("Barry", NpcType.HUMAN, new Stats(200, 0, 0, 1421), new Inventory(new List<object>(){new Usable(UseType.HEAL, 15, "Beer", 40)}), true, null, null);
        Dialogue barkeeperdialogue = new Dialogue();
        barkeeperdialogue.AddNode("1", "Welcome to my bar! \nWould you like anything to drink?", new List<Option>(){
            new Option("Buy Beer ($20)", "1", null, ()=>{
                if (GamePlayer.Stats.Pay(20))
                {
                    Barkeeper.Stats.AddGold(20);
                    GamePlayer.Inventory.Add(new Usable(UseType.HEAL, 15, "Beer", 20));
                }
            }),
            new Option("Buy Steak ($40)", "1", null, ()=>{
                if (player.Stats.Pay(40))
                {
                    Barkeeper.Stats.AddGold(40);
                    GamePlayer.Inventory.Add(new Usable(UseType.HEAL, 35, "Steak", 40));
                }
            }),
            new Option("Leave", null, "1"),
        });
        Barkeeper.Dialogue = barkeeperdialogue;
        Barkeeper.CanTalk = true;


        Npc badNpc = new Npc("Bob", NpcType.HUMAN, new Stats(100, 0, 0, 500), new Inventory(new List<Object>(){new Weapon(20, "Standard sword", 35)}), false);
        // Npc Mother = new Npc("teressa", NpcType.HUMAN, new Stats(500, 0, 0, 100), new Inventory(), false, null, null);
        // Npc Bird = new Npc("Peter Griffin", NpcType.BIRD, new Stats(500, 0, 0, 0), new Inventory(), false, null, null);
        // Npc Hobbo = new Npc("Kevin", NpcType.HUMAN, new Stats(10, 0, 0, 0), new Inventory(), false, null, null);
        // Npc Shopkeeper = new Npc("Mort", NpcType.HUMAN, new Stats(300, 0, 0, 5000), new Inventory(), true, null, null);
        // Npc Thieff = new Npc("adiaq la", NpcType.HUMAN, new Stats(200, 0, 0, 500), new Inventory(), false, null, null);
        // Npc Nurse = new Npc("Joy", NpcType.HUMAN, new Stats(200, 0, 0, 500), new Inventory(), true, null, null);
        // Npc Dwarfs = new Npc("Dwarf", NpcType.DWARF, new Stats(200, 0, 0, 50), new Inventory(new List<Object>(){new Weapon(30, "Axe", 75)}), false, null, null);
        // Npc Goblins = new Npc("Goblin", NpcType.GOBLIN, new Stats(200, 0, 0, 50), new Inventory(new List<Object>(){new Weapon(30, "Wooden Club", 75)}), false, null, null);
        // Npc Fish = new Npc("Fish", NpcType.FISH, new Stats(50, 0, 0, 0), new Inventory(), false, null, null);
        // Npc Snakes = new Npc("Snake", NpcType.SNAKE, new Stats(75, 0, 0, 0), new Inventory(new List<Object>(){new Weapon(10, "Bite", 75)}), false, null, null);
        Npc Dragon = new Npc("Dragon", NpcType.DRAGON, new Stats(500, 0, 0, 0), new Inventory(new List<Object>(){new Weapon(70, "Dragon Claws", 75)}), false, null, null);
        //Npc Mother = new Npc("teressa", NpcType.HUMAN, new Stats(500, 0, 0, 100), new Inventory(), false, null, null);
        //Npc Bird = new Npc("Peter Griffin", NpcType.BIRD, new Stats(500, 0, 0, 0), new Inventory(), false, null, null);
        //Npc Hobbo = new Npc("Kevin", NpcType.HUMAN, new Stats(10, 0, 0, 0), new Inventory(), false, null, null);
        // Npc Ronnie = new Npc("Ronnie mcnutt", NpcType.HUMAN, new Stats(1, 0, 0, 50), new Inventory(new List<Object>(){new Weapon(40, "Shotgun", 200)}), false, null, null);
        //Npc Thieff = new Npc("adiaq la", NpcType.HUMAN, new Stats(200, 0, 0, 500), new Inventory(), false, null, null);
        //Npc Nurse = new Npc("Joy", NpcType.HUMAN, new Stats(200, 0, 0, 500), new Inventory(), true, null, null);
        // Npc Patient = new Npc("Prapor", NpcType.HUMAN, new Stats(50, 0, 0, 200), new Inventory(), false, null, null);
        // Npc Guards = new Npc("Guard", NpcType.HUMAN, new Stats(300, 0, 0, 250), new Inventory(), false, null, null);
        // Npc King = new Npc("Crazy king Nikita", NpcType.HUMAN, new Stats(150, 0, 0, 750), new Inventory(), false, null, null);
        //Npc Dwarfs = new Npc("Dwarf", NpcType.DWARF, new Stats(200, 0, 0, 50), new Inventory(new List<Object>(){new Weapon(30, "Axe", 75)}), false, null, null);
        //Npc Fish = new Npc("Fish", NpcType.FISH, new Stats(50, 0, 0, 0), new Inventory(), false, null, null);
        //Npc Snakes = new Npc("Snake", NpcType.SNAKE, new Stats(75, 0, 0, 0), new Inventory(new List<Object>(){new Weapon(10, "Bite", 75)}), false, null, null);
        // Npc Dragon = new Npc("Dragon", NpcType.DEMON, new Stats(500, 0, 0, 0), new Inventory(new List<Object>(){new Weapon(30, "Axe", 75)}), false, null, null);
        // King Terry the Terrible

        // LOCATIONS:

        // Location: Town SubLocations: Bar, Fountain, Town_Sqaure, Shop, Hospital 
        SubLocation Bar = new SubLocation("Bar", new List<Npc>(){drunkard, Barkeeper}, " _____________________\n|         |_________\n|        [___________\n|          |   |   |\n|    @@   /_\\ /_\\ /_\\\n|   @()@\n|   _/\\_\n| <&,)(V)-,_ ________\n|  ~_) ( [_________ _\n|  (_( _) |          |\n|   \\ \\~  |          |\n|    \\,\\, |          |\n|    /'/'o===========|\n|_,__-'-_,+-----------\n");
        SubLocation Fountain = new SubLocation("Fountain", new List<Npc>(){Bird, Hobbo}, "       ('          \n      `),       \n   __cD|`.__    \n  !_________!   \n    \\_____/     \n     !___!      \n      | |       \n      | |       \n     _!_!_      \n   /_______\\    \n");
        SubLocation Town_Square = new SubLocation("Town Square", new List<Npc>(){Ronnie}, "~         ~~          __\n       _T      .,,.    ~--~ ^^\n ^^   // \\                    ~\n      ][O]    ^^      ,-~ ~\n   /''-I_I         _II____\n__/_  /   \\ ______/ ''   /'\\_,__\n  | II--'''' \\,--:--..,_/,.-[ },\n; '/__\\,.--';|   |[] .-.| O[ _ }\n:' |  | []  -|   ''--:.;[,.'\\,/\n'  |[]|,.--'' '',   ''-,.    |\n  ..    ..-''    ;       ''. '\n");
        SubLocation Shop = new SubLocation("Shop", new List<Npc>(){Shopkeeper, Thieff}, " __________________________________________________________________________\n|: : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : |\n| : : : : : : : : : : : : : : : :_________________________: : : : : : : : :|\n|: : : : : : : : : : : : : : : _)                         (_ : : : : : : : |\n| : : : : : : : : : : : : : : )_ :  Club 40 Gift Shoppe :  _( : : : : : : :|\n|: : Elevator  : : : :__________)_________________________(__________  : : |\n| _____________ : _ :/ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ\\: _ :|\n||  _________  | /_\\ '.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.' /_\\ |\n|| |    |    | |:=|=: |Flowers * Perfumes_________Lingerie * Candles| :=|=:|\n|| |    |    | | : : :|   ______    _  .'         '.  _    ______   |: : : |\n|| |    |    | |======| .' ,|,  '. /_\\ |           | /_\\ .'  ,|, '. |======|\n|| |    |    |:|Lounge| | ;;;;;  | =|= |           | =|= |  ;;;;; | |Casino|\n|| |    |    | |<---  | |_';;;'_n|     |  n______  |     |$_';;;'_| |  --->|\n|| |    |    | |      | |_|-;-|__|     |-|_______|-|     |__|-;-|_| |      |\n|| |    |    | |      | |________|     |           |     |________| |      |\n|| |    |    | |      |                |           |                |      |\nlc_|____|____|_|______|________________|           |________________|______|\n");
        SubLocation Hospital = new SubLocation("Hospital", new List<Npc>(){Nurse, Patient}, "      .---------.\n _    |:: [-=-] |\n| |   |_________|\n|~|\n|_|                    ,;;;;,\n I\\  ,__ ,;;;, __,    ///\\\\\\\\\\\n I |[   / . . \\   }   / '  \\\\||\n I | ) (   _   ) (    \\_= _///\n I |[___'-. .-'___}\\___ )_\\\n I ||~/,'~~~~~,\\~~|'---((  \\\n I \\ //        \\\\ |     \\ \\ \\\n I  \\/         // |     | /-/\n I (/         (/  |     |/||\\\n I  |             |     |    |\n I  |             |     |____/\n I  :-----_o_-----:      || |\n I  | /~~|===|~~\\ |      (( |\n I  ||   |===|   ||      ||_/\n/^\\ '~   '^^^'   ''     ((__|\n");

        Location Town = new Location("Town", new List<SubLocation>{Bar, Fountain, Town_Square, Shop, Hospital}, 400);
        // Console.WriteLine(Town);

        // Location Castle SubLocations: Treasury, Throne Room, Dungeon
        SubLocation Treasury = new SubLocation("Treasury", new List<Npc>(){Guards}, "|#######====================#######|\n|#(∞)*UNITED STATES OF AMERICA*(∞)#|\n|#**          /===\\   ********  **#|\n|*# [G}      | (') |             #*|\n|#*  ******  | /v\\ |    O N E    *#|\n|#(∞)         \\===/            (∞)#|\n|##=========	∞     ===========##|\n------------------------------------\n");
        SubLocation Throne_Room = new SubLocation("Throne Room", new List<Npc>(){King}, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢠⣤⣶⣾⣿⣿⣿⣿⣷⣶⣤⡄⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⠉⠛⠁⠈⠛⠉⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣇⣀⣀⣀⣀⣸⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⢠⣤⣤⣤⣤⡄⠸⣿⣿⣿⣿⣿⣿⣿⣿⡇⢠⣤⣤⣤⣤⡄⠀⠀⠀⠀\n⠈⠉⠉⠉⠉⠁⢠⣤⣤⣤⣤⣤⣤⣤⣤⡄⠈⠉⠉⠉⠉⠁⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⠘⠛⠛⠛⠛⠛⠛⠛⠛⠛⠛⠃⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⠸⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠇⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⠈⠛⠛⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠛⠛⠁⠀⠀⠀⠀⠀\n");
        SubLocation Dungeon = new SubLocation("Dungeon", new List<Npc>(){Goblins}, "   _________________________________________________________\n /|     -_-                                             _-  |\\\n/ |_-_- _                                         -_- _-   -| \\   \n  |                            _-  _--                      | \n  |                            ,                            |\n  |      .-'````````'.        '(`        .-'```````'-.      |\n  |    .` |           `.      `)'      .` |           `.    |          \n  |   /   |   ()        \\      U      /   |    ()       \\   |\n  |  |    |    ;         | o   T   o |    |    ;         |  |\n  |  |    |     ;        |  .  |  .  |    |    ;         |  |\n  |  |    |     ;        |   . | .   |    |    ;         |  |\n  |  |    |     ;        |    .|.    |    |    ;         |  |\n  |  |    |____;_________|     |     |    |____;_________|  |  \n  |  |   /  __ ;   -     |     !     |   /     `'() _ -  |  |\n  |  |  / __  ()        -|        -  |  /  __--      -   |  |\n  |  | /        __-- _   |   _- _ -  | /        __--_    |  |\n  |__|/__________________|___________|/__________________|__|\n /                                             _ -        lc \\\n/   -_- _ -             _- _---                       -_-  -_ \\\n");

        Location Castle = new Location("Castle", new List<SubLocation>{Treasury, Throne_Room, Dungeon}, 500);
        // Console.WriteLine(Castle);

        // Location: Mountain SubLocations: Cave, Vulcano
        SubLocation Cave = new SubLocation("Cave", new List<Npc>(){Dwarfs}, " ********************************************************************************\n*                    /   \\              /'\\       _                              *\n*\\_..           /'.,/     \\_         .,'   \\     / \\_                            *\n*    \\         /            \\      _/       \\_  /    \\     _                     *\n*     \\__,.   /              \\    /           \\/.,   _|  _/ \\                    *\n*          \\_/                \\  /',.,''\\      \\_ \\_/  \\/    \\                   *\n*                           _  \\/   /    ',../',.\\    _/      \\                  *\n*             /           _/m\\  \\  /    |         \\  /.,/'\\   _\\                 *\n*           _/           /MMmm\\  \\_     |          \\/      \\_/  \\                *\n*          /      \\     |MMMMmm|   \\__   \\          \\_       \\   \\_              *\n*                  \\   /MMMMMMm|      \\   \\           \\       \\    \\             *\n*                   \\  |MMMMMMmm\\      \\___            \\_      \\_   \\            *\n*                    \\|MMMMMMMMmm|____.'  /\\_            \\       \\   \\_          *\n*                    /'.,___________...,,'   \\            \\   \\        \\         *\n*                   /       \\          |      \\    |__     \\   \\_       \\        *\n*                 _/        |           \\      \\_     \\     \\    \\       \\_      *\n*                /                               \\     \\     \\_   \\        \\     *\n*                                                 \\     \\      \\   \\__      \\    *\n*                                                  \\     \\_     \\     \\      \\   *\n*                                                   |      \\     \\     \\      \\  *\n*                                                    \\ms          |            \\ *\n ********************************************************************************\n");
        SubLocation Vulcano = new SubLocation("Vulcano", new List<Npc>(){Dragon}, "              (   (( . : (    .)   ) :  )\n                (   ( :  .  :    :  )  ))\n                 ( ( ( (  .  :  . . ) )\n                  ( ( : :  :  )   )  )\n                   ( :(   .   .  ) .'\n                    '. :(   :    )\n                      (   :  . )  )\n                       ')   :   #@##\n                      #',### ' #@  #@\n                     #/ @'#~@#~/\\   #\n                   ##  @@# @##@  `..@,\n                 @#/  #@#   _##     `\\\n               @##;  `#~._.' ##@      \\_\n             .-#/           @#@#@--,_,--\\\n            / `@#@..,     .~###'         `~.\n          _/         `-.-' #@####@          \\\n       __/     &^^       ^#^##~##&&&   %     \\_\n      /       && ^^      @#^##@#%%#@&&&&  ^    \\\n    ~/         &&&    ^^^   ^^   &&&  %%% ^^^   `~._\n .-'   ^^    %%%. &&   ___^     &&   && &&   ^^     \\\n/akg ^^^ ___&&& X & && |n|   ^ ___ %____&& . ^^^^^   `~.\n         |M|       ---- .  ___.|n| /\\___\\  X\n                   |mm| X  |n|X    ||___|             \n");

        Location Mountain = new Location("Mountain", new List<SubLocation>{Cave, Vulcano}, 300);
        // Console.WriteLine(Mountain);

        // Location: Farm SubLocations: River, Woods, Farmhouse
        SubLocation River = new SubLocation("River", new List<Npc>(){Fish, Fish1, Fish2}, "                  _.._\n   _________....-~    ~-.______\n~~~                            ~~~~-----...___________..--------\n                                           |   |     |\n                                           | |   |  ||\n                                           |  |  |   |\n                                           |'. .' .`.|\n___________________________________________|0oOO0oO0o|____________\n -          -         -       -      -    / '  '. ` ` \\    -    -\n      --                  --       --   /    '  . `   ` \\    --\n---            ---          ---       /  '                \\ ---\n     ----               ----        /       ' ' .    ` `    \\  ----\n-----         -----         ----- /   '   '        `      `   \\\n     .-~~-.          ------     /          '    . `     `    `  \\\n      ( .._)-------           /  '    '      '      `\n		    --------/     '     '   '\n");
        SubLocation Woods = new SubLocation("Woods", new List<Npc>(){Snakes}, "              v .   ._, |_  .,\n           `-._\\/  .  \\ /    |/_\n               \\\\  _\\, y | \\//\n         _\\_.___\\\\, \\\\/ -.\\||\n           `7-,--.`._||  / / ,\n           /'     `-. `./ / |/_.'\n                     |    |//\n                     |_    /\n                     |-   |\n                     |   =|\n                     |    |\n--------------------/ ,  . \\--------._\n");
        SubLocation Farmhouse = new SubLocation("Farmhouse", new List<Npc>(){Mother}, "                            +&-\n                          _.-^-._    .--.\n                       .-'   _   '-. |__|\n                      /     |_|     \\|  |\n                     /               \\  |\n                    /|     _____     |\\ |\n                     |    |==|==|    |  |\n |---|---|---|---|---|    |--|--|    |  |\n |---|---|---|---|---|    |==|==|    |  |\n^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");

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

    static bool Fight(Player player, Npc npc)
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

        if (!npc.CanFight) { return true; }
        if (player.Inventory.GetWeaponDamage() <= 0)
        {
            Console.Clear();
            Console.WriteLine("You do not have a weapon to fight...\n\nPress any key to continue.");
            Console.ReadKey(true);
            return true;
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
            return false;
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
                        player.OngoingQuests[i].Complete(player);
                    }
                }
            }
            GameWorld.CurrentSubLocation.Npcs.Remove(npc);
            Console.Write($"You killed {npc.Name}!!!\nPress any key to continue looting...\n");
            Console.ReadKey(true);
            Loot(player, npc);
            return true;
        }
        else if (player.Stats.CurrentHealth <= 0)
        {
            // npc won
            Console.Clear();
            WriteParchment($"You died!\n\n{npc.Name} lived happily ever after.\nThey even got a medal for their bravery.\n\nLet their name be known\n{npc.Name.ToUpper()}!!! {npc.Name.ToUpper()}!!! {npc.Name.ToUpper()}!!!\n");
            Console.Write("\nPress any key to continue to main menu...\n");
            Console.ReadKey(true);
            return false;
        }
        return true;
    }

    static List<Usable> UseItem(Player player, bool heal=false)
    {
        int currentChoice = 0;
        int longestPlayerString = (player.Inventory.GetLongestName() > 12) ? player.Inventory.GetLongestName() : 12;
        ConsoleKey key;
        while (true){
            Console.Clear();
            Console.Write($"Current health: {player.Stats.CurrentHealth}/{player.Stats.MaxHealth}\n\nSelect your item:\n┌─Your Usables{new string('─', Math.Max(0, longestPlayerString-12))}─┬─Amount─┐\n");
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
                    if (items.Count > 0){
                        if (heal)
                        {
                            player.Stats.Heal(items[currentChoice].Amount);
                            player.Inventory.Remove(items[currentChoice]);
                        }else{
                            return new List<Usable>(){items[currentChoice]};
                        }
                    }else{
                        return new List<Usable>();
                    }
                }
                else if (key == ConsoleKey.Escape)
                {
                    return new List<Usable>();
                }
                currentChoice = Math.Clamp(currentChoice, 0, Math.Max(0, items.Count-1));
            } while (key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.Enter && key != ConsoleKey.Escape);
        }
        return new List<Usable>();
    }

    static void Trade(Player player, Npc npc)
    {
        if (!npc.CanTrade) { return; }
        int currentChoice = 0;
        bool sell = true;

        // write loop
        while (true)
        {
            int longestPlayerString = (10 > player.Inventory.GetLongestName()) ? 10 : player.Inventory.GetLongestName();
            int longestNpcString = (npc.Name.Length+8 > npc.Inventory.GetLongestName()) ? npc.Name.Length+8 : npc.Inventory.GetLongestName();
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
                    Console.Write($"{playerItemName}{new string(' ', Math.Max(0, longestPlayerString-playerItemName.Length))} │ {playerItemValue,4}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" │    ");
                } else if (i == player.Inventory.Items.Count) {
                    Console.Write($"└─{new string('─', Math.Max(0, longestPlayerString))}─┴──────┘    ");
                } else {
                    Console.Write($"{new string(' ', Math.Max(0, longestPlayerString+11))}    ");
                }
                // print npc items
                Console.BackgroundColor = ConsoleColor.Black;
                if (i < npc.Inventory.Items.Count) {
                    string npcItemName = npc.Inventory.GetName(i);
                    int npcItemValue = npc.Inventory.GetValue(i);
                    Console.Write("│ ");
                    if (!sell && currentChoice == i) { Console.BackgroundColor = ConsoleColor.DarkGray; } // if current selected item is this one we change the colors
                    Console.Write($"{npcItemName}{new string(' ', Math.Max(0, longestNpcString-npcItemName.Length))} │ {npcItemValue,4}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" │\n");
                } else if (i == npc.Inventory.Items.Count) {
                    Console.Write($"└─{new string('─', Math.Max(0, longestNpcString))}─┴──────┘\n");
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
                    if (!sell && player.Inventory.Items.Count < 10 && player.Stats.Pay(npc.Inventory.GetValue(currentChoice)) && npc.Inventory.Items.Count > 0)
                    {
                        // buy from npc
                        // could add logic here to increase the price its sold for
                        player.Inventory.Add(npc.Inventory.Items[currentChoice]);
                        npc.Inventory.Sell(currentChoice, npc.Stats);
                        GameAudio.PlayRandomDrop();
                        currentChoice = 0;
                        if (npc.Inventory.Items.Count == 0)
                        {
                            currentChoice = 0;
                            sell = true;
                        }
                    }
                    else if (sell && npc.Inventory.Items.Count < 10 && npc.Stats.Pay(player.Inventory.GetValue(currentChoice)) && player.Inventory.Items.Count > 0)
                    {
                        // sell to npc
                        // could add logic to decrease the price its sold for to the npc like some npc's are scammers and some are generous or something
                        npc.Inventory.Add(player.Inventory.Items[currentChoice]);
                        player.Inventory.Sell(currentChoice, player.Stats);
                        GameAudio.PlayRandomDrop();
                        currentChoice = 0;
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
                                    // TODO: fix sometime :)
                                    // Quest q = new Quest(player.OngoingQuests[i].Name, player.OngoingQuests[i].QuestType, player.OngoingQuests[i].ItemType, player.OngoingQuests[i].ItemName, player.OngoingQuests[i].Npc, player.OngoingQuests[i].Callback);
                                    // player.CompletedQuest.Add(q);
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
        int longestPlayerString = (10 > player.Inventory.GetLongestName()) ? 10 : player.Inventory.GetLongestName();
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
                    if (!drop && player.Inventory.Items.Count < 10 && npc.Inventory.Items.Count > 0)
                    {
                        // take from npc
                        player.Inventory.Add(npc.Inventory.Items[currentChoice]);
                        npc.Inventory.Remove(currentChoice);
                        GameAudio.PlayRandomDrop();
                        if (npc.Inventory.Items.Count == 0)
                        {
                            drop = true;
                            currentChoice = 0;
                        }
                        currentChoice -= 1;
                    }
                    else if (drop && player.Inventory.Items.Count > 0)
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

    private static void MainGameplayLoop()
    {
        int currentChoice = 0;
        int currentNpcChoice = 0;

        void ShowQuests()
        {
            // int longestQuestName = 9; // only if we fix CompletedQuests
            int longestQuestName = 6;
            for (int i = 0; i < GamePlayer.OngoingQuests.Count; i++)
            {
                if (GamePlayer.OngoingQuests[i].Name.Length > longestQuestName){ longestQuestName = GamePlayer.OngoingQuests[i].Name.Length; }
            }
            // for (int i = 0; i < GamePlayer.CompletedQuest.Count; i++)
            // {
            //     if (GamePlayer.CompletedQuest[i].Name.Length > longestQuestName){ longestQuestName = GamePlayer.CompletedQuest[i].Name.Length; }
            // }
            
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"┌─Quests{new string('─', Math.Max(0, longestQuestName-6))}─┬─Progress─┐\n");
            for (int i = 0; i < GamePlayer.OngoingQuests.Count; i++)
            {
                Quest q = GamePlayer.OngoingQuests[i];
                string progress = "";
                if (q.QuestType == QuestType.KILL){
                    progress = $"{new string(' ', Math.Max(0, 8-(q.CurrentKills.ToString()+'/'+q.RequiredKillAmount.ToString()).Length))}{q.CurrentKills}/{q.RequiredKillAmount}";
                }else if (q.QuestType == QuestType.FETCH){
                    progress = "     0/1";
                }
                Console.Write("│ ");
                Console.BackgroundColor = (currentChoice == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                Console.Write($"{q.Name}{new string(' ', Math.Max(0, longestQuestName-q.Name.Length))} │ {progress}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" │\n");
            }
            // for (int i = 0; i < GamePlayer.CompletedQuest.Count; i++)
            // {
            //     Quest q = GamePlayer.CompletedQuest[i];
            //     string progress = "";
            //     if (q.QuestType == QuestType.KILL){
            //         progress = $"{new string(' ', Math.Max(0, 8-(q.CurrentKills.ToString()+'/'+q.RequiredKillAmount.ToString()).Length))}{q.CurrentKills}/{q.RequiredKillAmount}";
            //     }else if (q.QuestType == QuestType.FETCH){
            //         progress = "     0/1";
            //     }
            //     Console.Write("│ ");
            //     Console.BackgroundColor = (currentChoice == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
            //     Console.Write($"{q.Name}{new string(' ', Math.Max(0, longestQuestName-q.Name.Length))} │ {progress}");
            //     Console.BackgroundColor = ConsoleColor.Black;
            //     Console.Write(" │\n");
            // }
            // Console.Write($"├─Completed{new string('─', Math.Max(0, longestQuestName-9))}─┼──────────┤\n");
            Console.Write($"└─{new string('─', longestQuestName)}─┴──────────┘\n");
            Console.Write("\nEnter/ESC = Go back");
            
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape || key == ConsoleKey.Enter)
                {
                    return;
                }
            } while (key != ConsoleKey.Escape && key != ConsoleKey.Enter);
        }

        bool NpcChoice()
        {
            Npc npc = GameWorld.CurrentSubLocation.Npcs[currentChoice];
            if (npc.Stats.CurrentHealth <= 0) { return true; }
            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"Selected {npc.Type.ToString().First()+npc.Type.ToString().ToLower().Substring(1)} {npc.Name}\n\n┌─Actions─┐\n");
                if (npc.CanTalk)
                {
                    Console.Write("│ ");
                    Console.BackgroundColor = (currentNpcChoice == 0) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($">Talk");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("   │\n");
                }
                if (npc.CanFight)
                {
                    Console.Write("│ ");
                    Console.BackgroundColor = ((npc.CanTalk && currentNpcChoice == 1) || (!npc.CanTalk && currentNpcChoice == 0)) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($">Fight");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  │\n");
                }
                if (npc.CanTrade)
                {
                    Console.Write("│ ");
                    Console.BackgroundColor = ((npc.CanTalk && npc.CanFight && currentNpcChoice == 2) || (!npc.CanTalk && npc.CanFight && currentNpcChoice == 1) || (!npc.CanTalk && !npc.CanFight && currentNpcChoice == 0)) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($">Trade");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  │\n");
                }
                Console.Write("└─────────┘\n");
                Console.Write("Up/Down = Move around\nEnter = Use selected action\nESC = Go back");

                ConsoleKey key;
                do
                {
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        currentNpcChoice--;
                    }
                    if (key == ConsoleKey.DownArrow)
                    {
                        currentNpcChoice++;
                    }
                    if (key == ConsoleKey.Enter)
                    {
                        if (npc.CanTalk && npc.CanFight && npc.CanTrade){
                            if (currentNpcChoice == 0) { Talk(npc); }
                            if (currentNpcChoice == 1) { return Fight(GamePlayer, npc); }
                            if (currentNpcChoice == 2) { Trade(GamePlayer, npc); }
                        }
                        if (!npc.CanTalk && npc.CanFight && npc.CanTrade){
                            if (currentNpcChoice == 0) { return Fight(GamePlayer, npc); }
                            if (currentNpcChoice == 1) { Trade(GamePlayer, npc); }
                        }
                        if (npc.CanTalk && !npc.CanFight && npc.CanTrade){
                            if (currentNpcChoice == 0) { Talk(npc); }
                            if (currentNpcChoice == 1) { Trade(GamePlayer, npc); }
                        }
                        if (npc.CanTalk && npc.CanFight && !npc.CanTrade){
                            if (currentNpcChoice == 0) { Talk(npc); }
                            if (currentNpcChoice == 1) { return Fight(GamePlayer, npc); }
                        }
                        if (npc.CanTalk && !npc.CanFight && !npc.CanTrade){
                            if (currentNpcChoice == 0) { Talk(npc); }
                        }
                        if (!npc.CanTalk && npc.CanFight && !npc.CanTrade){
                            if (currentNpcChoice == 0) { return Fight(GamePlayer, npc); }
                        }
                        if (!npc.CanTalk && !npc.CanFight && npc.CanTrade){
                            if (currentNpcChoice == 0) { Trade(GamePlayer, npc); }
                        }
                        currentNpcChoice = 0;
                    }
                    if (key == ConsoleKey.Escape)
                    {
                        return true;
                    }
                    currentNpcChoice = Math.Clamp(currentNpcChoice, 0, ((npc.CanTalk ? 1 : 0) + (npc.CanFight ? 1 : 0) + (npc.CanTrade ? 1 : 0))-1);
                } while (key != ConsoleKey.Enter && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.Escape);
            }
        }

        while (true){
            int longestText = 10;
            for (int i = 0; i < GameWorld.CurrentSubLocation.Npcs.Count; i++)
            {
                Npc npc = GameWorld.CurrentSubLocation.Npcs[i];
                if (longestText < $">{npc.Type.ToString()} {npc.Name}".Length) { longestText = ('>'+npc.Type.ToString()+' '+npc.Name).Length; }
            }
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{GameWorld.CurrentSubLocation.Art}\n\n");
            Console.Write($"┌─Around You{new string('─', Math.Max(0, longestText-10))}─┐\n");
            for (int i = 0; i < GameWorld.CurrentSubLocation.Npcs.Count+3; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                if (i < GameWorld.CurrentSubLocation.Npcs.Count)
                {
                    Npc npc = GameWorld.CurrentSubLocation.Npcs[i];
                    Console.Write("│ ");
                    Console.BackgroundColor = (currentChoice == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($">{npc.Type.ToString().First()+npc.Type.ToString().ToLower().Substring(1)} {npc.Name}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($" {new string(' ', Math.Max(0, longestText-('>'+npc.Type.ToString()+' '+npc.Name).Length))}│\n");
                }
                if (i == GameWorld.CurrentSubLocation.Npcs.Count)
                {
                    Console.Write($"├─Options{new string('─', Math.Max(0, longestText-7))}─┤\n│ ");
                    Console.BackgroundColor = (currentChoice == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($">Travel");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{new string(' ', Math.Max(0, longestText-7))} │\n");
                }
                if (i == GameWorld.CurrentSubLocation.Npcs.Count+1)
                {
                    Console.Write("│ ");
                    Console.BackgroundColor = (currentChoice == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($">Use Items");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{new string(' ', Math.Max(0, longestText-10))} │\n");
                }
                if (i == GameWorld.CurrentSubLocation.Npcs.Count+2)
                {
                    Console.Write("│ ");
                    Console.BackgroundColor = (currentChoice == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($">Quests");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{new string(' ', Math.Max(0, longestText-7))} │\n");
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"└─{new string('─', Math.Max(0, longestText))}─┘\n\n");
            Console.Write("Up/Down = Move around\nEnter = Use select");

            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
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
                    if (currentChoice >= 0 && currentChoice < GameWorld.CurrentSubLocation.Npcs.Count)
                    {
                        // open sub menu
                        if (!NpcChoice())
                        {
                            return;
                        }
                    }else{
                        if (currentChoice == GameWorld.CurrentSubLocation.Npcs.Count) {
                            Travel(GamePlayer);
                        } else if (currentChoice == GameWorld.CurrentSubLocation.Npcs.Count) {
                            UseItem(GamePlayer, true);
                        } else if (currentChoice == GameWorld.CurrentSubLocation.Npcs.Count+1) {
                            UseItem(GamePlayer, true);
                        } else if (currentChoice == GameWorld.CurrentSubLocation.Npcs.Count+2) {
                            ShowQuests();
                        }
                    }
                    currentChoice = 0;
                }
                currentChoice = Math.Clamp(currentChoice, 0, GameWorld.CurrentSubLocation.Npcs.Count+2);
            } while (key != ConsoleKey.Enter && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow);
        }
    }
}

// Your current location: Bar in Town
// ┌─Around You──┐ -> ┌─Actions─┐
// │ >Human John │    │ >Talk   │
// │ >Bird Peter │    │ >Fight  │
// │ >Bird Peter │    │ >Trade  │
// │ >Dwarf Ork  │    └─────────┘
// ├─Options─────┤
// │ >Travel     │
// │ >Use Items  │
// └─────────────┘

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
//    \                          /    /             \ /
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
