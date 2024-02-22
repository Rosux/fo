public class Npc
{
    public string Name;
    public Inventory Inventory;
    public Stats Stats;
    public List<Quest> Quests;
    public Dialogue Dialogue;
    
    public Npc(string Name, Inventory Inventory, Stats Stats, List<Quest> Quests, Dialogue Dialogue)
    {
        this.Name = Name;
        this.Inventory = Inventory;
        this.Stats = Stats;
        this.Quests = Quests;
        this.Dialogue = Dialogue;
    }

}