using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Item
{
    public PlayerControl playerObj;
    public ItemDictionary itemDictObj;
    public enum ItemType {Armor, Weapon, Consumable, Grimoire, KeyItem }
    public string name { get; set; }
    public string description { get; set; }
    public bool stackable;
    public int numberOfItem;
    public abstract void AddToInventory();
    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }     //weapon/armor only
}
