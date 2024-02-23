public class Quest
{
    public string Name;
    public bool Completed = false;
    public QuestType QuestType;
    public KillType KillType;
    public ItemType ItemType;
    public int Amount;
    public int Kills;

    public Quest(string Name, QuestType QuestType, KillType KillType, ItemType ItemType,int Amount, int Kills)
    {
        this.Name = Name;
        this.QuestType = QuestType;
        this.KillType = KillType;
        this.ItemType = ItemType;
        this.Amount = Amount;
        this.Kills = Kills;
    }

    public void Complete()
    {
    }

    public void Start(Player player)
    {
        Console.WriteLine($"Quest '{Name}' of type '{QuestType}' started for player '{player.Name}'.");
        player.OngoingQuests.Add(this); 
    }

    public static void UpdateQuests(Player player)
    {
        foreach (Quest quest in player.OngoingQuests)
        {
            if (!quest.Completed)
            {
                bool completed = CheckCompletion(quest, player);

                if (completed)
                {
                    quest.Completed = true;
                    Console.WriteLine($"Quest '{quest.Name}' completed for player '{player.Name}'.");
                    player.OngoingQuests.Remove(quest);
                }
            }
        }
    }
    private static bool CheckCompletion(Quest quest, Player player)
    {
        Console.WriteLine($"{quest.Amount}, {quest.Kills}");
        if (quest.Amount == quest.Kills)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }
}
