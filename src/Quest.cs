public class Quest
{
    public string Name;
    public bool Completed = false;
    public QuestType QuestType;
    public KillType KillType;
    public ItemType ItemType;
    public int Amount;
    public int AmountQuest;

    public Quest(string Name, QuestType QuestType, KillType KillType, ItemType ItemType,int Amount, int AmountQuest)
    {
        this.Name = Name;
        this.QuestType = QuestType;
        this.KillType = KillType;
        this.ItemType = ItemType;
        this.Amount = Amount;
        this.AmountQuest = AmountQuest;
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
        for (int i = player.OngoingQuests.Count - 1; i >= 0; i--)
        {
            Quest quest = player.OngoingQuests[i];

            if (!quest.Completed)
            {
                bool completed = CheckCompletion(quest, player);

                if (completed)
                {
                    quest.Completed = true;
                    Console.WriteLine($"Quest '{quest.Name}' completed for player '{player.Name}'.");
                    player.OngoingQuests.RemoveAt(i);
                }
            }
        }
}
    private static bool CheckCompletion(Quest quest, Player player)
    {
        if (quest.Amount == quest.AmountQuest)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }

}
