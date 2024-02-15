public class Inventory
{
    public List<object> Items;

    // public int GetArmorStats()
    // {
    //     // loop through items and find all armor types
    //     // add them and return as int
    // }
    
    // public int GetAttackStats()
    // {
    //     // loop through items. find all weapons. add the damage and return it
    // }
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