using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Attributes
{
    public string name { get; set; }
    public int level { get; set; }
    public int experience { get; set; }
    public int experienceToLevel { get; set; }
    //stats base
    public int strengthBase { get; private set; }
    public int dexterityBase { get; private set; }
    public int constitutionBase { get; private set; }
    public int magicBase { get; private set; }
    //stats leveled
    public int strengthLeveled { get; private set; }
    public int dexterityLeveled { get; private set; }
    public int constitutionLeveled { get; private set; }
    public int magicLeveled { get; private set; }
    //stats total
    public int strengthTotal { get; set; }
    public int dexterityTotal { get; set; }
    public int constitutionTotal { get; set; }
    public int magicTotal { get; set; }
    //derived stats
    public int healthMax { get; set; }
    public int manaMax { get; set; }
    private int m_healthVar;
    public int healthVar { get { return m_healthVar; } set { m_healthVar = Program.Clamp(value, 0, healthMax); }}
    private int m_manaVar;
    public int manaVar { get { return m_manaVar; } set { m_manaVar = Program.Clamp(value, 0, manaMax); } }
    //from equipment if player (enemy has set amounts)
    public int defense { get; set; }
    public int magicDefense { get; set; }
    public int resistance { get; set; }
    //stat scaling "major"
    public int strengthScaling { get; set; } = 1;
    public int dexterityScaling { get; set; } = 1;
    public int constitutionScaling { get; set; } = 1;
    public int magicScaling { get; set; } = 1;
    public int healthScaling { get; private set; } = 5;
    public int manaScaling { get; private set; } = 3;
    //bonuses (from equipment/buffs)
    public int strengthBonus { get; set; }
    public int dexterityBonus { get; set; }
    public int constitutionBonus { get; set; }
    public int magicBonus { get; set; }
    public int weaponDamMin { get; set; }
    public int weaponDamMax { get; set; }
    //turnspeed
    public int turnSpeed { get; set; }
    public int turnSpeedMax { get; set; }


    //constructor set base attributes for player
    public Attributes(int level_, int strength_, int dexterity_, int constitution_, int magic_)
    {
        level = level_;
        strengthBase = strength_;
        dexterityBase = dexterity_;
        constitutionBase = constitution_;
        magicBase = magic_;
        AttributeUpdate();
    }

    //constructor set base attributes for enemy
    public Attributes(int level_, int strength_, int dexterity_, int constitution_, int magic_, int defense_, int magicDefense_, int resistance_)
    {
        level = level_;
        strengthBase = strength_;
        dexterityBase = dexterity_;
        constitutionBase = constitution_;
        magicBase = magic_;
        //secondary
        defense = defense_;
        magicDefense = magicDefense_;
        resistance = resistance_;
        AttributeUpdate();

    }

    //set scaling values based of major or minor
    public void MajorAttributes(bool strMajor, bool dexMajor, bool conMajor, bool magMajor)
    {
        int scaling = 2;
        if (strMajor)
        {
            strengthScaling = scaling;
        }
        if (dexMajor)
        {
            constitutionScaling = scaling;
        }
        if (conMajor)
        {
            dexterityScaling = scaling;
        }
        if (magMajor)
        {
            magicScaling = scaling;
        }
    }

    //advance level and update stats
    public int LevelUp()
    {
        level++;
        AttributeUpdate();
        Console.WriteLine(name + " has reached level " + level);
        return level;
    }

    //add experience to player
    public int ExperienceAdd(int experienceAmount)
    {
        experience += experienceAmount;
        //reset experience when hit set amount, then add level
        if (experience >= experienceToLevel)
        {
            LevelUp();
            experience = 0;
            return experience;
        }
        return experience;
    }

    //update attributes according to scaling
    public void AttributeUpdate()
    {
        //calc base from scaling and level
        strengthLeveled = strengthBase + (level * strengthScaling);
        dexterityLeveled = dexterityBase + (level * dexterityScaling);
        constitutionLeveled = constitutionBase + (level * constitutionScaling);
        magicLeveled = magicBase + (level * magicScaling);
        //totals
        strengthTotal = strengthLeveled + strengthBonus;
        dexterityTotal = dexterityLeveled + dexterityBonus;
        constitutionTotal = constitutionLeveled + constitutionBonus;
        magicTotal = magicLeveled + magicBonus;
        turnSpeed = (dexterityTotal);
        //set variable turnspeed combat value
        //Resources derived from stats
        healthMax = (constitutionTotal * healthScaling);
        manaMax = (magicTotal * manaScaling);
        //experiencetotal
        experienceToLevel = ((int)Math.Pow(level,2) * 4 + (10*level)); //level exp forumula
    }
    public void SetResource()
    {
        healthMax = (constitutionTotal * healthScaling);
        manaMax = (magicTotal * manaScaling);
        //resource set to max
        healthVar = healthMax;
        manaVar = manaMax;
        turnSpeedMax = turnSpeed;
    }

    public int TurnSpeedSubtract(int furtherSubtraction)
    {
        turnSpeed += -(5 + furtherSubtraction);
        return turnSpeed;
    }


}
