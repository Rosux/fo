public class SubLocation
{
    public string Name; 
    public List<Npc> Npcs; // holds the npcs
    public string Art;
    public Action EnterCallback;
    public Action ExitCallback;
    
    public SubLocation(string Name, List<Npc> Npcs, string Art="", Action EnterCallback=null, Action ExitCallback=null)
    {
        this.Name = Name;
        this.Npcs = Npcs;
        this.Art = Art;
        this.EnterCallback = EnterCallback;
        this.ExitCallback = ExitCallback;
    }
}