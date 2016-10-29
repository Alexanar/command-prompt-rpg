using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CombatManager
{
    public List<CombatEntity> combatEntities = new List<CombatEntity>();
    public List<CombatEntity> enemyList = new List<CombatEntity>();
    public List<CombatEntity> playerList = new List<CombatEntity>();
    public int experiencePoints { get; set; }

    public void SetupTurns()
    {
        combatEntities = combatEntities.OrderByDescending(entity => entity.GetSpeed()).ToList();
        combatEntities[0].SetTurn(true);
    }
    public void EnemyTurn()
    {
        if (GameManager.IsCombat())
        {
            combatEntities[0].DoTurn();
        }
    }
    //console prints
    public void ShowEnemies()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            Console.WriteLine(i + 1 + ": " + enemyList[i].GetName());
        }
    }
    //show players and health/mana
    public void ShowFriendly()
    {  
        for (int i = 0; i < playerList.Count; i++)
        {
            Console.WriteLine(i + 1 + ": " + playerList[i].GetName()
                + " - Health: " + playerList[i].attributes.healthVar + "/" + playerList[i].attributes.healthMax
                + " Mana: " + playerList[i].attributes.manaVar + "/" + playerList[i].attributes.manaMax);
        }
    }

    //check healths
    public void HealthCheck()
    {
        for (int i = 0; i < combatEntities.Count; i++)
        {
            if (combatEntities[i].attributes.healthVar <= 0)
            {
                Console.WriteLine(combatEntities[i].GetName() + " has died");
                combatEntities[i].ItemDrops();
                experiencePoints += combatEntities[i].experienceWorth;
                combatEntities[i].dead = true;
            }
        }
        enemyList.RemoveAll(x => x.dead == true);
        playerList.RemoveAll(x => x.dead == true);
        combatEntities.RemoveAll(x => x.dead == true);
        EnemyCheck();
    }
    //check if all enemies are killed / add experience
    private void EnemyCheck()
    {
        if (!enemyList.Any())
        {
            foreach (CombatEntity entity in playerList)
            {
                entity.attributes.ExperienceAdd(experiencePoints);
                entity.cameOutOfCombat = true;
            }
            Console.ForegroundColor = ConsoleColor.Green; //set color green
            Console.WriteLine("Gained " + experiencePoints + " experience!");
            Console.ForegroundColor = ConsoleColor.Gray; //set color normal
            Console.WriteLine("Victory!");
            GameManager.Map();
            combatEntities.Clear();
            playerList.Clear();
            experiencePoints = 0;
            Console.ForegroundColor = ConsoleColor.White; //set color normal
            Console.WriteLine("Press any key to continue!");
            Console.ForegroundColor = ConsoleColor.Gray; //set color normal
            Console.ReadKey();
        }
    }
}

