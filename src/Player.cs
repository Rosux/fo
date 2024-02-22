public class Player
{
    public string Name;
    public Inventory Inventory;
    public Stats Stats;
    public List<Quest> OngoingQuests;

    public Player(string Name, Inventory Inventory,Stats Stats, List<Quest> OngoingQuests)
    {
        this.Name = Name;
        this.Inventory = Inventory;
        this.Stats = Stats;
        this.OngoingQuests = OngoingQuests;
    }

}