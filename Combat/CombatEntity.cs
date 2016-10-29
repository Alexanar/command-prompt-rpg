using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CombatEntity : ICombat
{
    public Attributes attributes;
    public MagicDictionary spellBook;
    public ItemDictionary lootTable;
    //combat variables
    public CombatManager combatManager;
    public static Random random = new Random();
    public int randomNumber;
    private bool isTurn;
    public bool dead { get; set; }
    public int experienceWorth { get; set; }
    //coming out of combat
    public bool cameOutOfCombat { get; set; }

    public CombatEntity(CombatManager manager, ItemDictionary inventory)
    {
        //attributes = new Attributes(1, 1, 1, 1, 1);
        combatManager = manager;
        lootTable = inventory;
    }

    //adds entity to manager and sorts
    public virtual void AddToManager()
    {
        attributes.AttributeUpdate();
        combatManager.combatEntities.Add(this);
        combatManager.enemyList.Add(this);
    }
    public virtual void ItemDrops()
    {

    }
    public int GetSpeed()
    {
        return attributes.turnSpeed;
    }
    public bool SetTurn(bool turn)
    {
        isTurn = turn;
        return isTurn;
    }
    public bool GetTurn()
    {
        return isTurn;
    }
    public void EndTurn()
    {
        isTurn = false;
        combatManager.SetupTurns();
    }


    public abstract void DoTurn();

    //damage calc
    public static float DamageAmount(float min, float max, int attribute, float multiplier)
    {
        int rand = random.Next((int)Math.Round(min), (int)Math.Round(max));
        float damage = (rand + (attribute * multiplier));
        return damage;
    }

    //*********************************
    // COMBAT INTERFACE IMPLEMENTATION
    //*********************************

    public abstract void Attack();

    public abstract void Magic();

    public abstract void Item();

    public abstract void Defend();

    public string GetName()
    {
        return attributes.name;
    }

    public virtual void TakeDamage(float damage)
    {
        attributes.healthVar -= (int)Math.Round(damage);
    }

}

