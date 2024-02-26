public class Usable{
    public ItemType Type = ItemType.USABLE;
    public UseType UseType;
    public int Amount;
    public string Name { get; set; }
    public int Worth;

    public Usable(UseType type, int Amount, string Name, int Worth){
        this.Type = ItemType.USABLE;
        this.UseType = type;
        this.Amount = Amount;
        this.Name= Name;
        this.Worth = Worth;
    }
}