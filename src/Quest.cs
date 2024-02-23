public class Quest
{
    public string Name;
    public bool Completed = false;
    public QuestType Type;
    public KillType KillType;
    public int kills;

    public Quest(string name, QuestType type)
    {
        Name = name;
        Type = type;
    }

    public void Complete()
    {
        Completed = true;
        // Additional logic for completing the quest
    }

    public void Start(Player player)
    {
        Console.WriteLine($"Quest '{Name}' of type '{Type}' started for player '{player.Name}'.");
        player.OngoingQuests.Add(this); 
    }

    public static void UpdateQuests(Player player)
    {
        foreach (Quest quest in player.OngoingQuests)
        {
            if (!quest.Completed)
            {
                // Update quest progress
            }
        }
    }
}