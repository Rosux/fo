public class Item
{
    public ItemType Type;
    public string Name;
    public int Damage;
    public int Price;
    public int Defence;

    public Item(ItemType type, params object[] p)
    {
        this.Type = type;
        if (type == ItemType.WEAPON)
        {
            this.Name = (string)p[0];
            this.Damage = (int)p[1];
            this.Price = (int)p[2];
        }
        else if (type == ItemType.ARMOR)
        {
            this.Name = (string)p[0];
            this.Defence = (int)p[1];
        }
        else if (type == ItemType.USABLE)
        {
            this.Name = (string)p[0];
        }
    }

    public static Item Weapon(string Name, int Damage, int Price)
    {
        return new Item(ItemType.WEAPON, Name, Damage, Price);
    }

    public static Item Armor(string Name, int Defence)
    {
        return new Item(ItemType.ARMOR, Name, Defence);
    }

    public static Item Usable(string Name)
    {
        return new Item(ItemType.USABLE, Name);
    }

    // to create new weapon/armor/usable do
    // `Item sword = Item.Weapon("sword", 15, 35);`
    // `Item helmet = Item.Armor("helmet", 56);`
    // `Item potion = Item.Usable("magic-potion");`
    // list of items (like an inventory):
    // List<Item> inventory = new List<Item>();
}