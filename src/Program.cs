using System;
using System.Reflection.Metadata;

class Program
{
    static void Main(string[] args)
    {
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
        
        List<Location> locations = InitializeLocations();
        World myWorld = new World(locations);
        Stats playerStats = new Stats(CurrentHealth: 100, MaxHealth: 100, Attack: 20, Defence: 10, Gold: 100);
        myWorld.PrintAllLocations();
        myWorld.Travel(myWorld.Locations[1], playerStats);
        Console.WriteLine($"Current Location: {myWorld.CurrentLocation.Name}");
        Console.WriteLine($"Remaining Gold: {playerStats.Gold}");
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