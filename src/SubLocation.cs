public class SubLocation
{
    public string Name; 
    public List<Npc> Npcs; // holds the npcs
    
    public SubLocation(string Name, List<Npc> Npcs)
    {
        this.Name = Name;
        this.Npcs = Npcs;
    }
    public override string ToString()
    {
        return $"SubLocation: {Name}, NPCs: {Npcs.Count}";
    }
}