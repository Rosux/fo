public class Armor{
    public ItemType Type = ItemType.ARMOR;
    public int Armorstat;
    public string Name { get; set; }
    public int Worth;
    public Armor(int Armorstat, string Name, int Worth){
        this.Type = ItemType.ARMOR;
        this.Armorstat = Armorstat;
        this.Name= Name;
        this.Worth = Worth;
    }
}