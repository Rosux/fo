public class Inventory
{
    public List<object> Items;

    public Inventory(List<object> items=null)
    {
        if (items == null)
        {
            this.Items = new List<object>();
        }
        else
        {
            this.Items = items;
        }
    }

    public void Sell(int Index, Stats stats)
    {
        if (Index < 0 || Index >= this.Items.Count){ return; }
        stats.Gold += GetWorth(this.Items[Index]);
        this.Items.RemoveAt(Index);
    }

    private int GetWorth(object Item)
    {
        switch ((Item as dynamic).Type)
        {
            case ItemType.WEAPON:
                Weapon weapon = (Weapon)Item;
                return weapon.Worth;
            case ItemType.ARMOR:
                Armor armor = (Armor)Item;
                return armor.Worth;
            case ItemType.USABLE:
                Usable usable = (Usable)Item;
                return usable.Worth;
        }
        return 0;
    }

    public bool Add(object Item)
    {
        if (this.Items.Count >= 10)
        {
            return false;
        }
        this.Items.Add(Item);
        return true;
    }

    public int GetArmorPoints()
    {
        if (!Contains(ItemType.ARMOR)){ return 0; }
        int totalArmorPoints = 0;
        foreach (var i in this.Items)
        {
            switch ((i as dynamic).Type)
            {
                case ItemType.ARMOR:
                    totalArmorPoints += ((Armor)i).Armorstat;
                    break;
            }
        }
        return totalArmorPoints;
    }

    public int GetWeaponDamage()
    {
        if (!Contains(ItemType.WEAPON)){ return 0; }
        int totalWeaponDamage = 0;
        foreach (var i in this.Items)
        {
            switch ((i as dynamic).Type)
            {
                case ItemType.WEAPON:
                    totalWeaponDamage += ((Weapon)i).Damage;
                    break;
            }
        }
        return totalWeaponDamage;
    }

    private bool Contains(ItemType Type)
    {
        foreach (var i in this.Items)
        {
            switch ((i as dynamic).Type)
            {
                case ItemType.WEAPON:
                    Weapon weapon = (Weapon)i;
                    if (weapon.Type == Type)
                    {
                        return true;
                    }
                    break;
                case ItemType.ARMOR:
                    Armor armor = (Armor)i;
                    if (armor.Type == Type)
                    {
                        return true;
                    }
                    break;
                case ItemType.USABLE:
                    Usable usable = (Usable)i;
                    if (usable.Type == Type)
                    {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}

// public class HelloWorld
// {
//     public static void Main(string[] args)
//     {
//         List<object> items = new List<object>(){new Weapon(), new Armor()};
        
//         foreach (var i in items)
//         {
//             switch ((i as dynamic).Type)
//             {
//                 case ItemType.WEAPON:
//                     Weapon weapon = (Weapon)i;
//                     Console.WriteLine(weapon.Name);
//                     break;
//                 case ItemType.ARMOR:
//                     Armor armor = (Armor)i;
//                     Console.WriteLine(armor.Name);
//                     break;
//                 case ItemType.USABLE:
//                     Armor usable = (Armor)i;
//                     Console.WriteLine(usable.Name);
//                     break;
//             }
//         }
//     }
// }