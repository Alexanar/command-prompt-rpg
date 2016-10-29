using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

public class MagicDictionary
{
    public delegate void spell();
    public delegate void castDamageSpell(List<CombatEntity> target, Attributes attributes, string spellName, float damage, bool singleTarget, int targetNumber = 0);
    public Dictionary<string, Delegate> spellDictionary = new Dictionary<string, Delegate>();
    private List<string> availableTargets = new List<string> { "1", "2", "3", "4", "5", "6" };
    public CombatEntity casterObj;
    public CombatManager combatManager;
    public enum SpellType
    {
        DamageTarget,
        DamageAll,
        HealingTarget,
        HealingAll,
        BuffTarget,
        BuffAll
    }

    public MagicDictionary(CombatEntity caster, CombatManager combatManager_)
    {
        casterObj = caster;
        combatManager = combatManager_;
    }

    /***************
    ** SPELL LIST **
    ***************/

    //offensive aoe spell
    public void ConeOfCold()
    {
        //spell stats
        string spellName = "cone of cold";
        int manaCost = 10;
        SpellType spellType = SpellType.DamageAll;
        float damage = CombatEntity.DamageAmount((int)Math.Round(casterObj.attributes.magicTotal * 0.25f), (int)Math.Round(casterObj.attributes.magicTotal * 1.0f), casterObj.attributes.magicTotal, 1.0f);
        //cast spell if in combat, else learn the spell
        if (GameManager.IsCombat())
        {
            SpellTemplate(spellName, damage, manaCost, spellType);
        }
        else
        {
            //learn spell if not already learned
            try
            {
                spellDictionary.Add(spellName, new spell(ConeOfCold));
                Console.WriteLine("You have learned the spell " + spellName);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("You already know the spell " + spellName);
            }
        }
    }

    //offensive target spell
    public void Fireball()
    {
        string spellName = "fireball";
        int manaCost = 5;
        SpellType spellType = SpellType.DamageTarget;
        float damage = CombatEntity.DamageAmount((int)Math.Round(casterObj.attributes.magicTotal * 0.5f), (int)Math.Round(casterObj.attributes.magicTotal * 1.5f), casterObj.attributes.magicTotal, 1.0f);
        //cast spell if in combat, else learn the spell
        if (GameManager.IsCombat())
        {
            SpellTemplate(spellName, damage, manaCost, spellType);
        }
        else
        {
            //learn spell
            try
            {
                spellDictionary.Add(spellName, new spell(Fireball));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("You already know the spell " + spellName);
            }
        }
    }

    //defensive heal spell
    public void Heal()
    {
        string spellName = "heal";
        int manaCost = 5;
        SpellType spellType = SpellType.HealingTarget;
        float healing = CombatEntity.DamageAmount((int)Math.Round(casterObj.attributes.magicTotal * 0.2f), (int)Math.Round(casterObj.attributes.magicTotal * 0.8f), casterObj.attributes.magicTotal, 1.0f);
        if (GameManager.IsCombat())
        {
            SpellTemplate(spellName, healing, manaCost, spellType);
        }
        else
        {
            //learn spell
            try
            {
                spellDictionary.Add(spellName, new spell(Heal));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("You already know the spell " + spellName);
            }
        }
    }

    /***************************
    ** SPELL TEMPLATE METHODS **
    ***************************/

    //damaging spell template
    public void SpellTemplate(string spellName, float damageOrHealing, int manaCost, SpellType spellType)
    {
        if (spellType == SpellType.DamageAll)
        {
            if (casterObj.attributes.manaVar >= manaCost)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(casterObj.attributes.name + " cast " + spellName);
                Console.ForegroundColor = ConsoleColor.Gray; //return foreground color to default
                foreach (CombatEntity tar in combatManager.enemyList)
                {
                    tar.TakeDamage(damageOrHealing);
                    Console.WriteLine(tar.attributes.name + " was dealt " + damageOrHealing + " spelldamage by " + casterObj.attributes.name);
                }
                casterObj.attributes.manaVar -= manaCost;
                casterObj.attributes.TurnSpeedSubtract(-2);
                casterObj.EndTurn();
            }
            else
            {
                Console.WriteLine("Not enough mana to cast spell!");
            }
        }
        if (spellType == SpellType.DamageTarget)
        {
            if (casterObj.attributes.manaVar >= manaCost)
            {
                string enteredText = null;
                Console.WriteLine("Choose target for " + spellName);
                enteredText = PlayerControl.ReadText(enteredText, availableTargets);
                //minus 1 to fit in list spots
                int enemyNumber = int.Parse(enteredText) - 1;
                //check if target is in eenemy list
                if (enemyNumber <= combatManager.enemyList.Count - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(casterObj.attributes.name + " cast " + spellName);
                    Console.ForegroundColor = ConsoleColor.Gray; //return foreground color to default
                    combatManager.enemyList[enemyNumber].TakeDamage(damageOrHealing);
                    Console.WriteLine(combatManager.enemyList[enemyNumber].attributes.name + " was dealt " + damageOrHealing + " spelldamage by " + casterObj.attributes.name);
                    casterObj.attributes.manaVar -= manaCost;
                    casterObj.attributes.TurnSpeedSubtract(-1);
                    casterObj.EndTurn();
                }
                else
                {
                    Console.WriteLine("Invalid Target");
                }
            }
            else
            {
                Console.WriteLine("Not enough mana to cast spell!");
            }
        }
        if (spellType == SpellType.HealingTarget)
        {
            if (casterObj.attributes.manaVar >= manaCost)
            {
                string enteredText = null;
                Console.WriteLine("Choose target for " + spellName);
                enteredText = PlayerControl.ReadText(enteredText, availableTargets);
                //minus 1 to fit in list spots
                int friendNumber = int.Parse(enteredText) - 1;
                //check if target is in eenemy list
                if (friendNumber <= combatManager.playerList.Count - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(casterObj.attributes.name + " cast " + spellName);
                    Console.ForegroundColor = ConsoleColor.Gray; //return foreground color to default
                    combatManager.playerList[friendNumber].TakeDamage(-damageOrHealing);
                    Console.WriteLine(combatManager.playerList[friendNumber].attributes.name + " was given " + damageOrHealing + " points of healing by " + casterObj.attributes.name);
                    casterObj.attributes.manaVar -= manaCost;
                    casterObj.attributes.TurnSpeedSubtract(-1);
                    casterObj.EndTurn();
                }
                else
                {
                    Console.WriteLine("Invalid Target");
                }
            }
            else
            {
                Console.WriteLine("Not enough mana to cast spell!");
            }
        }
    }


}