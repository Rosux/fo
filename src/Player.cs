public class Player
{
    public string Name;
    public Inventory Inventory;
    public Stats Stats;
    public List<Quest> OngoingQuests;
    public List<Quest> CompletedQuest;
    

    public Player(string Name = "Player", Stats Stats = null, Inventory Inventory = null, List<Quest> Quests = null)
    {
        this.Name = Name;
        this.Stats = (Stats == null) ? new Stats() : Stats;
        this.Inventory = (Inventory == null) ? new Inventory() : Inventory;
        this.OngoingQuests = (Quests == null) ? new List<Quest>() : Quests;
    }
    public void AddQuest(Quest quest)
    {
        this.OngoingQuests.Add(quest);
    }
}