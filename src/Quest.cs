public class Quest
{
    public string Name;
    public bool Completed = false;
    public QuestType QuestType;
    public KillType KillType;
    public ItemType ItemType;
    public int RequiredKillAmount;
    public string ItemName;
    public int CurrentKills;
    public Npc Npc;


    public Quest(string Name, QuestType QuestType, ItemType ItemType, string ItemName, Npc Npc)
    {
        this.Name = Name;
        this.QuestType = QuestType;
        this.ItemType = ItemType;
        this.ItemName = ItemName;
        this.Npc = Npc;
    }

    public Quest(string Name, QuestType QuestType, KillType KillType, int RequiredKillAmount)
    {
        this.Name = Name;
        this.QuestType = QuestType;
        this.KillType = KillType;
        this.RequiredKillAmount = RequiredKillAmount;
        this.CurrentKills = 0;
    }

    public void Complete(Player player)
    {
        player.OngoingQuests.Remove(this);
    }

    public void Start(Player player)
    {
        Console.WriteLine($"Quest '{Name}' of type '{QuestType}' started for player '{player.Name}'.");
        player.OngoingQuests.Add(this); 
    }

    public bool CheckCompletion()
    {
        if (this.QuestType == QuestType.FETCH){
            return this.Npc.Inventory.HasItem(this.ItemName);
        }else{
            return CurrentKills >= RequiredKillAmount;
        }
    }
}
