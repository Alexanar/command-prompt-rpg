using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemDictionary
{
    public Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();
    public List<Item> inventory = new List<Item>();
    public PlayerControl player;
    //equipment slots
    public bool weaponEquipped { get; set; }
    public bool armorEquipped { get; set; }
    public Item[] weaponSlot = new Item[1];
    public Item[] armorSlot = new Item[1];

    public ItemDictionary(PlayerControl player_)
    {
        player = player_;
        FillDictionary();
    }
    //adding items
    public void FillDictionary()
    {
        itemDictionary.Add("wooden sword", new WoodenSword(player, this));
    }
}

/********
* ITEMS *
********/
public class WoodenSword : Item
{
    public int minimumDamage { get; private set; } = 2;
    public int maximumDamage { get; private set; } = 5;
    public WoodenSword(PlayerControl player, ItemDictionary itemDict)
    {
        playerObj = player;
        itemDictObj = itemDict;
        name = "wooden sword";
        ItemType itemType = ItemType.Weapon;
        description = "An old wooden sword used for training";
        stackable = false;
    }

    public override void AddToInventory()
    {
        itemDictObj.inventory.Add(this);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Looted " + name);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public override void OnEquip()
    {
        //when item in slot, add item to inventory
        if (itemDictObj.weaponSlot[0] != null)
        {
            itemDictObj.weaponSlot[0].OnUnequip();
        }
        itemDictObj.weaponSlot[0] = this;
    }
    public override void OnUnequip()
    {
        AddToInventory();
    }
}
