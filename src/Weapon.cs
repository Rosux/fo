using System;
using System.Collections.Generic;
public class Weapon{
    public ItemType Type;
    public string Name { get; set; }
    public Weapon(){
        this.Type = ItemType.WEAPON;
        this.Name="SWORD!";
    }
}



