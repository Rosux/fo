using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        WriteCenter($"\n\n   ▄████████  ▄██████▄ \n  ███    ███ ███    ███\n  ███    █▀  ███    ███\n ▄███▄▄▄     ███    ███\n▀▀███▀▀▀     ███    ███\n  ███        ███    ███\n  ███        ███    ███\n  ███         ▀██████▀   ");
        Console.ForegroundColor = ConsoleColor.White;
        WriteCenter("\n(GOTY Edition)\n\n");


        Dialogue peter = new Dialogue();
        peter.AddNode("1", "Hey! I'm peter griffin.", new List<Option>(){
            new Option("Hi peter...", "1.1"),
            new Option("Not true!", "1.2"),
        });
        peter.AddNode("1.2", "It's true, I am really peter griffin.", new List<Option>());
        peter.AddNode("1.1", "Bye.", new List<Option>());


        Talk(peter);
    }

    static void Talk(Dialogue dialogueTree)
    {
        int currentChoice = 0;
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"{dialogueTree.CurrentNode.Text}");
            if (dialogueTree.GetChoices().Count == 0)
            {
                break;
            }
            // print options
            foreach (KeyValuePair<int, string> text in dialogueTree.GetChoices())
            {
                if (text.Key == currentChoice) { Console.BackgroundColor = ConsoleColor.White; }
                Console.WriteLine($">{text.Key+1}: {text.Value}");
                Console.BackgroundColor = ConsoleColor.Black;
            }

            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                {
                    currentChoice -= 1;
                }
                currentChoice = Clamp(currentChoice, 0, dialogueTree.GetChoices().Count-1);
            } while (key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.Enter);

            // int option = Convert.ToInt32(Console.ReadLine())-1;
            // if (option >= 0 && option < dialogueTree.GetChoices().Count)
            // {
            //     dialogueTree.Step(option);
            // }
        }
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