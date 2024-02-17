public class Npc
{
    public string Name;
    public Inventory Inventory;
    public Stats Stats;
    public List<Quest> Quests;
    public Dialogue Dialogue;
    
    public Npc(string Name)
    {
        this.Name = Name;
    }

}