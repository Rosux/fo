using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "FO (GOTY Edition)";
        Console.CursorVisible = false;
        WriteLogo();

        Dialogue peter = new Dialogue();
        peter.AddNode("1", "Hey! I'm peter griffin.", new List<Option>(){
            new Option("Hi peter...", "1.1"),
            new Option("Not true!", "1.2"),
        });
        peter.AddNode("1.1", "Bye.", new List<Option>(){
            new Option("...", null, "1")
        });
        peter.AddNode("1.2", "It's true, I am really peter griffin.", new List<Option>(){
            new Option("...", null, "1")
        });

        Talk(peter, "Peter");
        // Console.WriteLine("hmmmm puase");
        // Console.ReadKey();
        // Talk(peter, "Peter");
        // Console.Clear();
        // WriteParchment("Dearest Player.\n\nThank you for playing our game!\n\nFrom,\nTeam FO");
        // Console.ReadKey();
    }

    static void Talk(Dialogue dialogueTree, string npcName="")
    {
        int currentChoice = 0;
        // weird internal method since i cant do `break 2;`
        void printingLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"{npcName}: {dialogueTree.CurrentNode.Text}");
                // print options
                foreach (KeyValuePair<int, string> text in dialogueTree.GetChoices())
                {
                    if (text.Key == currentChoice) { Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.WriteLine($">{text.Key+1}: {text.Value}");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
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
                        currentChoice = 0;
                        if (dialogueTree.Step(currentChoice))
                        {
                            return;
                        }
                        break;
                    }
                    currentChoice = Math.Clamp(currentChoice, 0, Math.Max(0, dialogueTree.GetChoices().Count-1));
                } while (key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.Enter && dialogueTree.GetChoices().Count > 0);
            }
        }
        printingLoop();
        WriteCenter("Press any key to continue...");
        Console.ReadKey(true);
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

        Console.Write($"/ \\----{new string('-', maxLength)}---, \n");
        Console.Write($"\\_,|   {new string(' ', maxLength)}   | \n");
        for (int i = 0; i < lines.Length; i++)
        {
            Console.Write($"   |   {lines[i]}{new String(' ', (maxLength-lines[i].Length)+3)}| \n");
        }
        Console.Write($"   |  ,{new string('-', maxLength)}----,\n");
        Console.Write($"   \\_/_{new string('_', maxLength)}___/ \n");
    }
}

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

// some ending: WriteParchment("Dearest Player.\n\nThank you for playing our game!\n\nFrom,\nTeam FO");