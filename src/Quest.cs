public class Quest
{
    public string Name;
    public bool Completed = false;
    public QuestType QuestType;
    public NpcType KillType;
    public ItemType ItemType;
    public int RequiredKillAmount;
    public string ItemName;
    public int CurrentKills;
    public Npc Npc;
    public Action? Callback;


    public Quest(string Name, QuestType QuestType, ItemType ItemType, string ItemName, Npc Npc, Action callback=null)
    {
        this.Name = Name;
        this.QuestType = QuestType;
        this.ItemType = ItemType;
        this.ItemName = ItemName;
        this.Npc = Npc;
        this.Callback = callback;
    }

    public Quest(string Name, QuestType QuestType, NpcType KillType, int RequiredKillAmount, Action callback=null)
    {
        this.Name = Name;
        this.QuestType = QuestType;
        this.KillType = KillType;
        this.RequiredKillAmount = RequiredKillAmount;
        this.CurrentKills = 0;
        this.Callback = callback;
    }

    public void Complete(Player player)
    {
        player.OngoingQuests.Remove(this);
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
