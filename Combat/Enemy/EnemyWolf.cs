using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnemyWolf : CombatEntity
{

    public EnemyWolf(CombatManager manager, ItemDictionary inventory) : base(manager, inventory)
    {
        //setup enemy stats
        attributes = new Attributes(
            1,      //level
            3,      //strength
            2,      //dexterity
            3,      //constitution
            1,      //magic
            2,      //defense
            1,      //magic defense
            1       //resistance
            );
        experienceWorth = 10;       //experience worth
        attributes.name = "Wolf";
        attributes.AttributeUpdate();
        attributes.SetResource();
        combatManager = manager;
        AddToManager();
    }

    public override void DoTurn()
    {
        //enemy AI
        if (GetTurn())
        {
            Attack();
        }
    }

    //basic attack
    public override void Attack()
    {
        //target of attack
        randomNumber = random.Next(0, combatManager.playerList.Count);
        //damage
        float damage = DamageAmount(attributes.strengthTotal * 0.5f, attributes.strengthTotal * 1f, attributes.strengthTotal, 0.5f);
        //do damage
        combatManager.playerList[randomNumber].TakeDamage(damage);
        Console.WriteLine(combatManager.playerList[randomNumber].GetName() + " was dealt " + damage + " damage by " + attributes.name);
        //delay for next turn
        attributes.TurnSpeedSubtract(3);
        //end
        EndTurn();
    }

    public override void Defend()
    {
        throw new NotImplementedException();
    }

    public override void Item()
    {
        throw new NotImplementedException();
    }

    public override void Magic()
    {
        throw new NotImplementedException();
    }

    public override void ItemDrops()
    {
        int dropNumber = random.Next(0, 101); //(random always -1 on max) really 0-100
        if (dropNumber <= 25)
        {
            lootTable.itemDictionary["wooden sword"].AddToInventory();
        }
        else
        {
            lootTable.itemDictionary["wooden sword"].AddToInventory();
        }

    }

}