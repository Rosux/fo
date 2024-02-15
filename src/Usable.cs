public class Usable{
    public ItemType Type;
    public string Name { get; set; }
    public Usable(){
        this.Type = ItemType.USABLE;
        this.Name="Potion!";
    }
}