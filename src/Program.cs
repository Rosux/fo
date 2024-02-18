using System;

class Program
{
    static void Main(string[] args)
    {
        Weapon weapon = new Weapon(100, "balzak", 20 );
        Armor armor = new Armor(20, "Shield", 20);
        Usable usable = new Usable(20, "Protection Potion", 10);
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