public class Armor{
    public ItemType Type;
    public string Name { get; set; }
    public Armor(){
        this.Type = ItemType.ARMOR;
        this.Name="ARMOR!";
    }
}