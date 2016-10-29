using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerControl
{

    //reads
    string enteredText;
    //get map
    private Map MapRef;
    //combat
    private CombatManager combatManagerRef;
    //get reference to the map
    public ItemDictionary inventory;
    //combat
    private int stepsForCombat;
    private int stepAmount;
    Random stepRandom = new Random();
    //players
    public Player player;
    public List<Player> partyList = new List<Player>();

    //setup constructor
    public PlayerControl(Map map, CombatManager combatManager)
    {
        MapRef = map;
        combatManagerRef = combatManager;
        inventory = new ItemDictionary(this);
        player = new Player(combatManagerRef, inventory);
        player.AddToParty(this);

        //setup combat number
        StepsForCombat();
    }

    //**********************************
    // COMMANDS OUT OF COMBAT
    //**********************************
    public void MovementCommands()
    {
        //available commands
        List<string> availableCommands = new List<string>
                { "NORTH", "SOUTH", "EAST", "WEST", "N", "S", "E", "W", "ENTER", "E", "SAVE", "C", "CHARACTER"};

        //check specialtile logic and if combat when coming out of combat
        if (player.cameOutOfCombat)
        {
            //showmap
            MapRef.DisplayMap(MapRef.MapGridData());
            MapRef.SpecialTiles();
            player.cameOutOfCombat = false;
        }
        //make sure string is right string
        enteredText = ReadTextBreak(enteredText, availableCommands, GameManager.IsMap());


        //when string is a valid direction do movement
        switch (enteredText.ToUpper().Trim())
        {
            //movement cases
            case "NORTH":
            case "N":
                if (MapRef.CanMove(-1, 0))
                {
                    MapRef.PlayerPositionRowChange(-1);
                    MapRef.DisplayMap(MapRef.MapGridData());
                    CombatChecker();
                    break;
                }
                Console.WriteLine("The way is blocked");
                break;

            case "SOUTH":
            case "S":
                if (MapRef.CanMove(1, 0))
                {
                    MapRef.PlayerPositionRowChange(1);
                    MapRef.DisplayMap(MapRef.MapGridData());
                    CombatChecker();
                    break;
                }
                Console.WriteLine("The way is blocked");
                break;
            case "EAST":
            case "E":
                if (MapRef.CanMove(0, 1))
                {
                    MapRef.PlayerPositionColChange(1);
                    MapRef.DisplayMap(MapRef.MapGridData());
                    CombatChecker();
                    break;
                }
                Console.WriteLine("The way is blocked");
                break;
            case "WEST":
            case "W":
                if (MapRef.CanMove(0, -1))
                {
                    MapRef.PlayerPositionColChange(-1);
                    MapRef.DisplayMap(MapRef.MapGridData());
                    CombatChecker();
                    break;
                }
                Console.WriteLine("The way is blocked");
                break;
            //special cases
            //end of level
            case "ENTER":
                if (MapRef.CompareTiles(MapRef.playerPositionRow, MapRef.endPositionRow, MapRef.playerPositionCol, MapRef.endPositionCol))
                {
                    ClearWriteLine("You have completed the level!");
                    MapRef.CreateGrid(MapRef.currentLevel + 1, "Welcome to Level: " + (MapRef.currentLevel + 1));
                    //reset steps for combat
                    StepsForCombat();
                    break;
                }
                Console.WriteLine("Invalid Command");
                break;
            case "SAVE":
                Program.SaveGame(MapRef, player);
                break;
            case "C":
            case "CHARACTER":
                GameManager.pInterface();
                break;
        }
        //special tiles
        MapRef.SpecialTiles();

    }


    //**********************************
    // COMMANDS IN COMBAT
    //**********************************
    public void CombatCommands()
    {

        //available commands
        List<string> availableCommands = new List<string>
                { "ATTACK", "MAGIC", "ITEM", "DEFEND"};
        //available targets
        List<string> availableTargets = new List<string>
                { "1", "2", "3", "4", "5", "6"};

        //checkers
        combatManagerRef.HealthCheck();

        //lose game
        if (!combatManagerRef.playerList.Any() && GameManager.IsCombat())
        {
            Console.WriteLine("Your party has died.");
            Console.ReadKey();
            Environment.Exit(0);
        }


        //when is player turn
        if (player.GetTurn() && GameManager.IsCombat())
        {
            combatManagerRef.ShowEnemies();
            combatManagerRef.ShowFriendly();
            Console.WriteLine("It's your turn!");
            //make sure string is right string
            enteredText = ReadTextBreak(enteredText, availableCommands, GameManager.IsCombat());

            //when string is valid, do the command
            switch (enteredText.ToUpper().Trim())
            {
                case "ATTACK":
                    player.Attack();
                    break;
                case "MAGIC":
                    player.Magic();
                    break;
                case "ITEM":
                    player.Item();
                    break;
                case "DEFEND":
                    player.Defend();
                    break;
            }
        }
        else
        {
            combatManagerRef.EnemyTurn();
        }
    }


    //**********************************
    // COMMANDS IN INTERFACE
    //**********************************

    public void InterfaceCommands()
    {
        ClearWriteLine("You are now in the interface. Available commands - INVENTORY, STATS, BACK");
        //available commands
        List<string> availableCommands = new List<string>
                { "INVENTORY", "I", "STATS", "BACK", "B"};
        //make sure string is right string
        enteredText = ReadTextBreak(enteredText, availableCommands, GameManager.IsPInterface());

        //when string is valid, do the command
        switch (enteredText.ToUpper().Trim())
        {
            case "INVENTORY":
            case "I":
                //list inventory items and list index
                Console.WriteLine("Items in inventory :");
                for (int i = 0; i < inventory.inventory.Count; i++)
                {
                    Console.WriteLine(inventory.inventory[i] + " " + inventory.inventory[i].name);
                }
                List<string> inventoryCommands = new List<string>
                { "EQUIP", "UNEQUIP", "USE", "BACK", "B"};
                //new commands for in inventory screen
                enteredText = ReadTextBreak(enteredText, inventoryCommands, GameManager.IsPInterface());
                switch (enteredText.ToUpper().Trim())
                {
                    case "EQUIP":
                    case "USE":
                    case "UNEQUIP":
                    case "BACK":
                    case "B":
                        break;
                }
                break;
            case "STATS":
                break;
            case "BACK":
            case "B":
                GameManager.Map();
                MapRef.DisplayMap(MapRef.MapGridData());
                break;
        }
    }

    //**********************************
    // VARIOUS FUNCTIONS
    //**********************************

    //test two strings together
    public static bool contains(string testString, List<string> compareTo)
    {
        if (compareTo.Contains(testString, StringComparer.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    //tests a string against a compared string until they are equal
    public static string ReadText(string stringVar, List<string> comparedText)
    {
        do
        {
            stringVar = Console.ReadLine();
            if (!contains(stringVar, comparedText))
            {
                Console.WriteLine("Invalid Command");
            }
        } while (!contains(stringVar, comparedText));
        return stringVar;
    }

    //tests a string against a compared string until they are equal with breakout
    public static string ReadTextBreak(string stringVar, List<string> comparedText, bool breakout)
    {
        do
        {
            if (!breakout)
            {
                continue;
            }
            stringVar = Console.ReadLine();
            if (!contains(stringVar, comparedText))
            {
                Console.WriteLine("Invalid Command");
            }
        } while (!contains(stringVar, comparedText) && !breakout);
        return stringVar;
    }

    //clears console and writes line
    public static void ClearWriteLine(string text)
    {
        Console.Clear();
        Console.WriteLine(text);
    }


    //set combat number
    public void StepsForCombat()
    {
        stepAmount = 0;
        stepsForCombat = (3 + stepRandom.Next(0, 6));
    }
    public void CombatChecker()
    {
        stepAmount++;
        if (stepAmount >= stepsForCombat)
        {
            ClearWriteLine("Ambushed!");
            //reset steps for combat
            StepsForCombat();
            //add enemies
            LevelEnemies();
            //add player
            player.AddToManager();
            //start combat
            GameManager.Combat();
            //setup turn order
            combatManagerRef.SetupTurns();
        }
    }
    private void LevelEnemies()
    {
        if (MapRef.currentLevel == 1)
        {
            EnemyWolf wolf1 = new EnemyWolf(combatManagerRef, inventory);
            EnemyWolf wolf2 = new EnemyWolf(combatManagerRef, inventory);
        }
        if (MapRef.currentLevel == 2)
        {
            EnemyWolf wolf1 = new EnemyWolf(combatManagerRef, inventory);
            EnemyWolf wolf2 = new EnemyWolf(combatManagerRef, inventory);
            EnemyWolf wolf3 = new EnemyWolf(combatManagerRef, inventory);
            EnemyWolf wolf4 = new EnemyWolf(combatManagerRef, inventory);
        }
        if (MapRef.currentLevel == 3)
        {

        }
    }



}
