using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player : CombatEntity
{

    //text input
    private string enteredText;
    private List<string> availableTargets = new List<string> { "1", "2", "3", "4", "5", "6" };
    private List<string> availableSpells = new List<string>();


    enum EquipSlot
    {
        head,
        body,
        legs,
        weapon
    }

    //setup player
    public Player(CombatManager manager, ItemDictionary inventory) : base(manager, inventory)
    {
        attributes = new Attributes(1, 5, 5, 5, 5);
        attributes.SetResource();
        combatManager = manager;
        spellBook = new MagicDictionary(this, combatManager);
        //spell tests
        spellBook.Fireball();
        spellBook.ConeOfCold();
        spellBook.Heal();
    }

    //*********************************
    // COMBAT INTERFACE IMPLEMENTATION
    //*********************************
    public override void AddToManager()
    {
        attributes.AttributeUpdate();
        combatManager.combatEntities.Add(this);
        combatManager.playerList.Add(this);
    }
    //add player to party
    public void AddToParty(PlayerControl playerControl)
    {
        playerControl.partyList.Add(this);
    }
    public override void DoTurn()
    {
        throw new NotImplementedException();
    }


    public override void Attack()
    {
        Console.WriteLine("Choose target");
        enteredText = PlayerControl.ReadText(enteredText, availableTargets);
        //minus 1 to fit in list spots
        int enemyNumber = int.Parse(enteredText) - 1; 
        //check if target is in eenemy list
        if (enemyNumber <= combatManager.enemyList.Count - 1)
        {
            //damage
            float damage = DamageAmount(attributes.weaponDamMin, attributes.weaponDamMax, attributes.strengthTotal, 1.5f);
            //do damage
            PlayerControl.ClearWriteLine(combatManager.enemyList[enemyNumber].GetName() + " was dealt " + damage + " damage by " + attributes.name);
            combatManager.enemyList[enemyNumber].TakeDamage(damage);
            attributes.TurnSpeedSubtract(0);
            //end
            EndTurn();
        }
        else
        {
            Console.WriteLine("Invalid Target");
        }
    }

    public override void Magic()
    {
        availableSpells = spellBook.spellDictionary.Keys.ToList();
        //check if list has any spells
        if (availableSpells.Any())
        {
            //spell list
            Console.WriteLine("Known Spells: ");
            foreach (string spell in availableSpells)
            {
                Console.WriteLine(spell);
            }
            Console.WriteLine("Choose spell");
            enteredText = PlayerControl.ReadText(enteredText, availableSpells);
            enteredText.ToLower();
            spellBook.spellDictionary[enteredText].DynamicInvoke();
        }
        else
        {
            Console.WriteLine("You don't know any magic!");
        }
    }

    public override void Item()
    {
        throw new NotImplementedException();
    }

    public override void Defend()
    {
        throw new NotImplementedException();
    }




}

