using System;
using System.Collections.Generic;
public class Weapon{
    public ItemType Type = ItemType.WEAPON;
    public int Damage; 
    public string Name { get; set; }
    public int Worth;
    public Weapon(string Name, int Damage, int Worth){
        this.Damage = Damage;
        this.Name= Name;
        this.Worth = Worth;
    }
}



