public class SubLocation
{
    public string Name; 
    public List<Npc> Npcs; // holds the npcs
    public string Art;
    public Action<SubLocation>? EnterCallback;
    public Action<SubLocation>? ExitCallback;
    
    public SubLocation(string Name, List<Npc> Npcs, string Art="", Action<SubLocation> EnterCallback=null, Action<SubLocation> ExitCallback=null)
    {
        this.Name = Name;
        this.Npcs = Npcs;
        this.Art = Art;
        this.EnterCallback = EnterCallback;
        this.ExitCallback = ExitCallback;
    }

    public void AddNpc(Npc newNpc)
    {
        this.Npcs.Add(newNpc);
    }

    public void RemoveNpc(Npc npc)
    {
        this.Npcs.Remove(npc);
    }

    public void RemoveNpc(string npcName)
    {
        for (int i = 0; i < this.Npcs.Count; i++)
        {
            if (this.Npcs[i].Name == npcName)
            {
                RemoveNpc(this.Npcs[i]);
            }
        }
    }

    public Npc? GetNpc(string npcName)
    {
        for (int i = 0; i < this.Npcs.Count; i++)
        {
            if (this.Npcs[i].Name == npcName)
            {
                return this.Npcs[i];
            }
        }
        return null;
    }
}