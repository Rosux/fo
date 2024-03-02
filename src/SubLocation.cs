public class SubLocation
{
    public string Name; 
    public List<Npc> Npcs; // holds the npcs
    public string Art;
    
    public SubLocation(string Name, List<Npc> Npcs, string art="")
    {
        this.Name = Name;
        this.Npcs = Npcs;
        this.Art = art;
    }
}