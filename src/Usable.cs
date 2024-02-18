public class Usable{
    public ItemType Type = ItemType.USABLE;
    public int Amount;
    public string Name { get; set; }
    public int Worth;

    public Usable(int Amount, string Name, int Worth){
        this.Type = ItemType.USABLE;
        this.Amount = Amount;
        this.Name= Name;
        this.Worth = Worth;
    }
}