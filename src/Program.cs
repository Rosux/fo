using System;

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
            Console.WriteLine("Press ENTER to start a new game.\nPress I for info about the game.\nPress ESC to exit the game.");
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape)
            {
                return;
            }
            if (key == ConsoleKey.I)
            {
                // print help menu text stuff
                Console.Clear();
                WriteParchment("Fo is a game about stories or something.\n\nYou are free to go anywhere as long\nas you pay the price.\nYou can interact with most people.\n\nGo out there and explore.\n");
                Console.Write("\n\nPress any key to go back.");
                Console.ReadKey();
            }
            if (key == ConsoleKey.Enter)
            {
                GamePlayer = new Player("Player", new Stats(1100, 1, 1, 10000), new Inventory(new List<Object>(){new Weapon("reddit moderator katana", 1190, 50), new Armor("Stupid helmet", 50, 15), new Usable(UseType.HEAL, "Potion of health", 200, 250) }));
                GameWorld = InitializeWorld(GamePlayer);
                MainGameplayLoop();
            }
        } while (true);
        
    }

    private static World InitializeWorld(Player player)
    {
        // LOCATIONS:

        // Location: Town SubLocations: Bar, Fountain, Town_Sqaure, Shop, Hospital 
        // SubLocation Bar = new SubLocation("Bar", new List<Npc>(){}, " _____________________\n|         |_________\n|        [___________\n|          |   |   |\n|    @@   /_\\ /_\\ /_\\\n|   @()@\n|   _/\\_\n| <&,)(V)-,_ ________\n|  ~_) ( [_________ _\n|  (_( _) |          |\n|   \\ \\~  |          |\n|    \\,\\, |          |\n|    /'/'o===========|\n|_,__-'-_,+-----------\n");
        // SubLocation Fountain = new SubLocation("Fountain", new List<Npc>(){}, "       ('          \n      `),       \n   __cD|`.__    \n  !_________!   \n    \\_____/     \n     !___!      \n      | |       \n      | |       \n     _!_!_      \n   /_______\\    \n");
        // SubLocation Town_Square = new SubLocation("Town Square", new List<Npc>(){}, "~         ~~          __\n       _T      .,,.    ~--~ ^^\n ^^   // \\                    ~\n      ][O]    ^^      ,-~ ~\n   /''-I_I         _II____\n__/_  /   \\ ______/ ''   /'\\_,__\n  | II--'''' \\,--:--..,_/,.-[ },\n; '/__\\,.--';|   |[] .-.| O[ _ }\n:' |  | []  -|   ''--:.;[,.'\\,/\n'  |[]|,.--'' '',   ''-,.    |\n  ..    ..-''    ;       ''. '\n");
        // SubLocation Shop = new SubLocation("Shop", new List<Npc>(){}, " __________________________________________________________________________\n|: : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : : |\n| : : : : : : : : : : : : : : : :_________________________: : : : : : : : :|\n|: : : : : : : : : : : : : : : _)                         (_ : : : : : : : |\n| : : : : : : : : : : : : : : )_ :  Club 40 Gift Shoppe :  _( : : : : : : :|\n|: : Elevator  : : : :__________)_________________________(__________  : : |\n| _____________ : _ :/ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ\\: _ :|\n||  _________  | /_\\ '.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.'.Z.' /_\\ |\n|| |    |    | |:=|=: |Flowers * Perfumes_________Lingerie * Candles| :=|=:|\n|| |    |    | | : : :|   ______    _  .'         '.  _    ______   |: : : |\n|| |    |    | |======| .' ,|,  '. /_\\ |           | /_\\ .'  ,|, '. |======|\n|| |    |    |:|Lounge| | ;;;;;  | =|= |           | =|= |  ;;;;; | |Casino|\n|| |    |    | |<---  | |_';;;'_n|     |  n______  |     |$_';;;'_| |  --->|\n|| |    |    | |      | |_|-;-|__|     |-|_______|-|     |__|-;-|_| |      |\n|| |    |    | |      | |________|     |           |     |________| |      |\n|| |    |    | |      |                |           |                |      |\nlc_|____|____|_|______|________________|           |________________|______|\n");
        // SubLocation Hospital = new SubLocation("Hospital", new List<Npc>(){}, "      .---------.\n _    |:: [-=-] |\n| |   |_________|\n|~|\n|_|                    ,;;;;,\n I\\  ,__ ,;;;, __,    ///\\\\\\\\\\\n I |[   / . . \\   }   / '  \\\\||\n I | ) (   _   ) (    \\_= _///\n I |[___'-. .-'___}\\___ )_\\\n I ||~/,'~~~~~,\\~~|'---((  \\\n I \\ //        \\\\ |     \\ \\ \\\n I  \\/         // |     | /-/\n I (/         (/  |     |/||\\\n I  |             |     |    |\n I  |             |     |____/\n I  :-----_o_-----:      || |\n I  | /~~|===|~~\\ |      (( |\n I  ||   |===|   ||      ||_/\n/^\\ '~   '^^^'   ''     ((__|\n");

        // Location Town = new Location("Town", new List<SubLocation>{Bar, Fountain, Town_Square, Shop, Hospital}, 400);
        // // Console.WriteLine(Town);

        // // Location Castle SubLocations: Treasury, Throne Room, Dungeon
        // SubLocation Treasury = new SubLocation("Treasury", new List<Npc>(){}, "|#######====================#######|\n|#(∞)*UNITED STATES OF AMERICA*(∞)#|\n|#**          /===\\   ********  **#|\n|*# [G}      | (') |             #*|\n|#*  ******  | /v\\ |    O N E    *#|\n|#(∞)         \\===/            (∞)#|\n|##=========	∞     ===========##|\n------------------------------------\n");
        // SubLocation Throne_Room = new SubLocation("Throne Room", new List<Npc>(){}, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢠⣤⣶⣾⣿⣿⣿⣿⣷⣶⣤⡄⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⠉⠛⠁⠈⠛⠉⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣇⣀⣀⣀⣀⣸⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⠀⠀⢸⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⡇⠀⠀⠀⠀⠀⠀\n⢠⣤⣤⣤⣤⡄⠸⣿⣿⣿⣿⣿⣿⣿⣿⡇⢠⣤⣤⣤⣤⡄⠀⠀⠀⠀\n⠈⠉⠉⠉⠉⠁⢠⣤⣤⣤⣤⣤⣤⣤⣤⡄⠈⠉⠉⠉⠉⠁⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⠘⠛⠛⠛⠛⠛⠛⠛⠛⠛⠛⠃⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⢸⣿⣿⡇⠸⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠇⢸⣿⣿⡇⠀⠀⠀⠀⠀\n⠀⠈⠛⠛⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠛⠛⠁⠀⠀⠀⠀⠀\n");
        // SubLocation Dungeon = new SubLocation("Dungeon", new List<Npc>(){}, "   _________________________________________________________\n /|     -_-                                             _-  |\\\n/ |_-_- _                                         -_- _-   -| \\   \n  |                            _-  _--                      | \n  |                            ,                            |\n  |      .-'````````'.        '(`        .-'```````'-.      |\n  |    .` |           `.      `)'      .` |           `.    |          \n  |   /   |   ()        \\      U      /   |    ()       \\   |\n  |  |    |    ;         | o   T   o |    |    ;         |  |\n  |  |    |     ;        |  .  |  .  |    |    ;         |  |\n  |  |    |     ;        |   . | .   |    |    ;         |  |\n  |  |    |     ;        |    .|.    |    |    ;         |  |\n  |  |    |____;_________|     |     |    |____;_________|  |  \n  |  |   /  __ ;   -     |     !     |   /     `'() _ -  |  |\n  |  |  / __  ()        -|        -  |  /  __--      -   |  |\n  |  | /        __-- _   |   _- _ -  | /        __--_    |  |\n  |__|/__________________|___________|/__________________|__|\n /                                             _ -        lc \\\n/   -_- _ -             _- _---                       -_-  -_ \\\n");

        // Location Castle = new Location("Castle", new List<SubLocation>{Treasury, Throne_Room, Dungeon}, 500);
        // // Console.WriteLine(Castle);

        // // Location: Mountain SubLocations: Cave, Vulcano
        // SubLocation Cave = new SubLocation("Cave", new List<Npc>(){}, " ********************************************************************************\n*                    /   \\              /'\\       _                              *\n*\\_..           /'.,/     \\_         .,'   \\     / \\_                            *\n*    \\         /            \\      _/       \\_  /    \\     _                     *\n*     \\__,.   /              \\    /           \\/.,   _|  _/ \\                    *\n*          \\_/                \\  /',.,''\\      \\_ \\_/  \\/    \\                   *\n*                           _  \\/   /    ',../',.\\    _/      \\                  *\n*             /           _/m\\  \\  /    |         \\  /.,/'\\   _\\                 *\n*           _/           /MMmm\\  \\_     |          \\/      \\_/  \\                *\n*          /      \\     |MMMMmm|   \\__   \\          \\_       \\   \\_              *\n*                  \\   /MMMMMMm|      \\   \\           \\       \\    \\             *\n*                   \\  |MMMMMMmm\\      \\___            \\_      \\_   \\            *\n*                    \\|MMMMMMMMmm|____.'  /\\_            \\       \\   \\_          *\n*                    /'.,___________...,,'   \\            \\   \\        \\         *\n*                   /       \\          |      \\    |__     \\   \\_       \\        *\n*                 _/        |           \\      \\_     \\     \\    \\       \\_      *\n*                /                               \\     \\     \\_   \\        \\     *\n*                                                 \\     \\      \\   \\__      \\    *\n*                                                  \\     \\_     \\     \\      \\   *\n*                                                   |      \\     \\     \\      \\  *\n*                                                    \\ms          |            \\ *\n ********************************************************************************\n");
        // SubLocation Vulcano = new SubLocation("Vulcano", new List<Npc>(){}, "              (   (( . : (    .)   ) :  )\n                (   ( :  .  :    :  )  ))\n                 ( ( ( (  .  :  . . ) )\n                  ( ( : :  :  )   )  )\n                   ( :(   .   .  ) .'\n                    '. :(   :    )\n                      (   :  . )  )\n                       ')   :   #@##\n                      #',### ' #@  #@\n                     #/ @'#~@#~/\\   #\n                   ##  @@# @##@  `..@,\n                 @#/  #@#   _##     `\\\n               @##;  `#~._.' ##@      \\_\n             .-#/           @#@#@--,_,--\\\n            / `@#@..,     .~###'         `~.\n          _/         `-.-' #@####@          \\\n       __/     &^^       ^#^##~##&&&   %     \\_\n      /       && ^^      @#^##@#%%#@&&&&  ^    \\\n    ~/         &&&    ^^^   ^^   &&&  %%% ^^^   `~._\n .-'   ^^    %%%. &&   ___^     &&   && &&   ^^     \\\n/akg ^^^ ___&&& X & && |n|   ^ ___ %____&& . ^^^^^   `~.\n         |M|       ---- .  ___.|n| /\\___\\  X\n                   |mm| X  |n|X    ||___|             \n");

        // Location Mountain = new Location("Mountain", new List<SubLocation>{Cave, Vulcano}, 300);
        // // Console.WriteLine(Mountain);

        // // Location: Farm SubLocations: River, Woods, Farmhouse
        // SubLocation River = new SubLocation("River", new List<Npc>(){}, "                  _.._\n   _________....-~    ~-.______\n~~~                            ~~~~-----...___________..--------\n                                           |   |     |\n                                           | |   |  ||\n                                           |  |  |   |\n                                           |'. .' .`.|\n___________________________________________|0oOO0oO0o|____________\n -          -         -       -      -    / '  '. ` ` \\    -    -\n      --                  --       --   /    '  . `   ` \\    --\n---            ---          ---       /  '                \\ ---\n     ----               ----        /       ' ' .    ` `    \\  ----\n-----         -----         ----- /   '   '        `      `   \\\n     .-~~-.          ------     /          '    . `     `    `  \\\n      ( .._)-------           /  '    '      '      `\n		    --------/     '     '   '\n");
        // SubLocation Woods = new SubLocation("Woods", new List<Npc>(){}, "              v .   ._, |_  .,\n           `-._\\/  .  \\ /    |/_\n               \\\\  _\\, y | \\//\n         _\\_.___\\\\, \\\\/ -.\\||\n           `7-,--.`._||  / / ,\n           /'     `-. `./ / |/_.'\n                     |    |//\n                     |_    /\n                     |-   |\n                     |   =|\n                     |    |\n--------------------/ ,  . \\--------._\n");
        // SubLocation Farmhouse = new SubLocation("Farmhouse", new List<Npc>(){}, "                            +&-\n                          _.-^-._    .--.\n                       .-'   _   '-. |__|\n                      /     |_|     \\|  |\n                     /               \\  |\n                    /|     _____     |\\ |\n                     |    |==|==|    |  |\n |---|---|---|---|---|    |--|--|    |  |\n |---|---|---|---|---|    |==|==|    |  |\n^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n");

        #region farm house
        Dialogue guard1Dialogue = new Dialogue();
        guard1Dialogue.AddNode("1", "Oh look who it is. Just in time for me to\nend your bloodline. Take this!", new List<Option>(){
            new Option("...", null, "1"),
        });
        Dialogue guard2Dialogue = new Dialogue();
        guard2Dialogue.AddNode("1", "YOU MURDERED A KING'S GUARD!?\nYou're not getting away with this!\nAAAARRRRRGGGGHHH", new List<Option>(){
            new Option("...", null, "1"),
        });
        Npc guard1 = new Npc("King's Guard", NpcType.GUARD, new Stats(200, 1, 1, 34), new Inventory(new List<object>(){
            new Weapon("King's Guard Estoc", 39, 56),
            new Armor("King's Guard Helmet", 23, 41),
            new Armor("King's Guard Chestplate", 31, 44),
            new Armor("King's Guard Leggings", 18, 31),
            new Armor("Leather boots", 7, 11),
        }), false, guard1Dialogue);
        Npc guard2 = new Npc("King's Guard", NpcType.GUARD, new Stats(230, 1, 1, 41), new Inventory(new List<object>(){
            new Weapon("King's Guard Axe", 41, 61),
            new Armor("King's Guard Helmet", 23, 41),
            new Armor("Iron Chestplate", 31, 44),
            new Armor("Leather Pants", 7, 11),
            new Armor("Leather Boots", 7, 11),
        }), false, guard2Dialogue);
        Npc mother = new Npc("Mother", NpcType.HUMAN, new Stats(100, 1, 1, 13), new Inventory(), true, null);

        bool motherAlive = true;
        bool guardsAlive = true;
        SubLocation house = new SubLocation("Farm House", new List<Npc>(){
            mother,
        }, Art.Farmhouse, (SubLocation location)=>{
            // when the player enters the house the guards talk and attack the player
            if(!motherAlive && guardsAlive)
            {
                Talk(guard1);
                Fight(guard1);
                Talk(guard2);
                Fight(guard2);
                guardsAlive = false;
            }
        }, (SubLocation location)=>{
            // when the player leaves 2 guards come and kill mother
            if (motherAlive)
            {
                motherAlive = false;
                location.Name = "Burned Farm House";
                location.RemoveNpc("Mother");
                location.AddNpc(guard1);
                location.AddNpc(guard2);
            }
        });
        #endregion

        #region river
        SubLocation river = new SubLocation("River", new List<Npc>(), Art.River, null, null);
        #endregion

        World w = new World(new List<Location>(){
            new Location("Farm", new List<SubLocation>(){house, river}, 350),
        });
        w.CurrentLocation = w.Locations[0];
        w.CurrentSubLocation = w.Locations[0].SubLocations[0];
        return w;
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
                    if (!chooseLocation && !GameWorld.Locations[currentLocationChoice].Locked && GameWorld.CurrentSubLocation != GameWorld.Locations[currentLocationChoice].SubLocations[currentSubLocationChoice] && (GameWorld.CurrentLocation == GameWorld.Locations[currentLocationChoice] || player.Stats.Pay(GameWorld.Locations[currentLocationChoice].TravelPrice)))
                    {
                        GameAudio.PlayRandomCoin();
                        GameWorld.Travel(GameWorld.Locations[currentLocationChoice], GameWorld.Locations[currentLocationChoice].SubLocations[currentSubLocationChoice]);
                        return;
                    }else if (GameWorld.CurrentSubLocation == GameWorld.Locations[currentLocationChoice].SubLocations[currentSubLocationChoice])
                    {
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

    static bool Fight(Npc npc)
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
        Player player = GamePlayer;
        if (!npc.CanFight) { return true; }
        if (npc.Stats.CurrentHealth <= 0 || player.Stats.CurrentHealth <= 0){ return true; }
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
                WriteParchment($"\nToday our dearest {npc.Name} left us behind.\n\nR.I.P. dearest {npc.Name}.");
            }
            for (int i = 0; i < player.OngoingQuests.Count; i++)
            {
                if (player.OngoingQuests[i].QuestType == QuestType.KILL && player.OngoingQuests[i].KillType == npc.Type)
                {
                    player.OngoingQuests[i].CurrentKills++;
                    if(player.OngoingQuests[i].CheckCompletion())
                    {
                        player.CompletedQuest.Add(player.OngoingQuests[i]);
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
                        currentChoice = 0;
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
                    currentChoice = 0;
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
                        int dropChoice = 0;
                        bool droping = true;
                        while(droping){
                            int longest = 13;
                            if (13 + player.Inventory.GetName(currentChoice).Length > longest) { longest = 13 + player.Inventory.GetName(currentChoice).Length; }
                            Console.Clear();
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($"┌─Drop item \"{player.Inventory.GetName(currentChoice)}\"?{new string('─', Math.Max(0, longest-13-player.Inventory.GetName(currentChoice).Length))}─┐\n│ ");
                            if (dropChoice == 0) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                            Console.Write($">Yes");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($"{new string(' ', longest-4)} │\n│ ");
                            if (dropChoice == 1) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                            Console.Write($">No");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($"{new string(' ', longest-3)} │\n");
                            Console.Write($"└─{new string('─', longest)}─┘\n");
                            ConsoleKey confirmKey;
                            do{
                                confirmKey = Console.ReadKey(true).Key;
                                if (confirmKey == ConsoleKey.UpArrow)
                                {
                                    dropChoice--;
                                }
                                if (confirmKey == ConsoleKey.DownArrow)
                                {
                                    dropChoice++;
                                }
                                if (confirmKey == ConsoleKey.Enter)
                                {
                                    if (dropChoice == 0)
                                    {
                                        player.Inventory.Remove(currentChoice);
                                        GameAudio.PlayRandomDrop();
                                        if (player.Inventory.Items.Count == 0)
                                        {
                                            currentChoice = 0;
                                            drop = false;
                                        }
                                        droping = false;
                                    }
                                    if (dropChoice == 1)
                                    {
                                        droping = false;
                                        break;
                                    }
                                }
                                dropChoice = Math.Clamp(dropChoice, 0, 1);
                            } while (confirmKey != ConsoleKey.Enter && confirmKey != ConsoleKey.UpArrow && confirmKey != ConsoleKey.DownArrow);
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
            int longestQuestName = 9;
            for (int i = 0; i < GamePlayer.OngoingQuests.Count; i++)
            {
                if (GamePlayer.OngoingQuests[i].Name.Length > longestQuestName){ longestQuestName = GamePlayer.OngoingQuests[i].Name.Length; }
            }
            for (int i = 0; i < GamePlayer.CompletedQuest.Count; i++)
            {
                if (GamePlayer.CompletedQuest[i].Name.Length > longestQuestName){ longestQuestName = GamePlayer.CompletedQuest[i].Name.Length; }
            }
            
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
            Console.Write($"├─Completed{new string('─', Math.Max(0, longestQuestName-9))}─┼──────────┤\n");
            for (int i = 0; i < GamePlayer.CompletedQuest.Count; i++)
            {
                Quest q = GamePlayer.CompletedQuest[i];
                string progress = "";
                if (q.QuestType == QuestType.KILL){
                    progress = $"{new string(' ', Math.Max(0, 8-(q.CurrentKills.ToString()+'/'+q.RequiredKillAmount.ToString()).Length))}{q.CurrentKills}/{q.RequiredKillAmount}";
                }else if (q.QuestType == QuestType.FETCH){
                    progress = "     1/1";
                }
                Console.Write("│ ");
                Console.BackgroundColor = (currentChoice == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                Console.Write($"{q.Name}{new string(' ', Math.Max(0, longestQuestName-q.Name.Length))} │ {progress}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" │\n");
            }
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
                Console.Write($"Selected {npc.Type.ToString().First()+npc.Type.ToString().ToLower().Substring(1)} {npc.Name}\n{npc.Stats.CurrentHealth}/{npc.Stats.MaxHealth} HP\n\n┌─Actions─┐\n");
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
                            if (currentNpcChoice == 1) { return Fight(npc); }
                            if (currentNpcChoice == 2) { Trade(GamePlayer, npc); }
                        }
                        if (!npc.CanTalk && npc.CanFight && npc.CanTrade){
                            if (currentNpcChoice == 0) { return Fight(npc); }
                            if (currentNpcChoice == 1) { Trade(GamePlayer, npc); }
                        }
                        if (npc.CanTalk && !npc.CanFight && npc.CanTrade){
                            if (currentNpcChoice == 0) { Talk(npc); }
                            if (currentNpcChoice == 1) { Trade(GamePlayer, npc); }
                        }
                        if (npc.CanTalk && npc.CanFight && !npc.CanTrade){
                            if (currentNpcChoice == 0) { Talk(npc); }
                            if (currentNpcChoice == 1) { return Fight(npc); }
                        }
                        if (npc.CanTalk && !npc.CanFight && !npc.CanTrade){
                            if (currentNpcChoice == 0) { Talk(npc); }
                        }
                        if (!npc.CanTalk && npc.CanFight && !npc.CanTrade){
                            if (currentNpcChoice == 0) { return Fight(npc); }
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
            int longestText = 13;
            for (int i = 0; i < GameWorld.CurrentSubLocation.Npcs.Count; i++)
            {
                Npc npc = GameWorld.CurrentSubLocation.Npcs[i];
                if (npc.Stats.CurrentHealth <= 0) {
                    GameWorld.CurrentSubLocation.Npcs.Remove(npc);
                    GamePlayer.RemoveQuestFromNpc(npc);
                }
                if (longestText < $">{npc.Type.ToString()} {npc.Name}".Length) { longestText = ('>'+npc.Type.ToString()+' '+npc.Name).Length; }
            }
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{GameWorld.CurrentSubLocation.Art}\n\n");
            Console.Write($"┌─Around You{new string('─', Math.Max(0, longestText-10))}─┐\n");
            for (int i = 0; i < GameWorld.CurrentSubLocation.Npcs.Count+3; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                if (GameWorld.CurrentSubLocation.Npcs.Count == 0 && i == 0)
                {
                    Console.Write($"│ No one around{new string(' ', Math.Max(0, longestText-13))} │\n");
                }
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
