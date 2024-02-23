public class Npc
{
    public NpcType Type;
    public string Name;
    public Inventory Inventory;
    public Stats Stats;
    public List<Quest> Quests;
    public Dialogue? Dialogue;

    public bool CanTalk = false;
    public bool CanFight = false;
    public bool CanTrade = false;
    
    public Npc(string Name = "NPC", NpcType Type = NpcType.HUMAN, Stats Stats = null, Inventory Inventory = null, bool CanTrade = false, List<Quest> Quests = null, Dialogue Dialogue = null)
    {
        this.Name = Name;
        this.Type = Type;
        if (Stats != null){
            this.Stats = Stats;
            this.CanFight = true;
        }else{
            this.Stats = new Stats();
        }
        this.Inventory = (Inventory == null) ? new Inventory() : Inventory;
        this.CanTrade = CanTrade;
        this.Quests = (Quests == null) ? new List<Quest>() : Quests;
        if (Dialogue != null){
            this.Dialogue = Dialogue;
            this.CanTalk = true;
        }else{
            this.Dialogue = Dialogue;
        }
    }

}