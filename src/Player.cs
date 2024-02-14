public class Player
{
    public string Name;
    public Inventory Inventory;
    public Stats Stats;
    public List<Quest> OngoingQuests;

    public Player()
    {
        this.Name = "mike";
        this.Stats = new Stats(100, 12 , 10);
    }

}