using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Text.RegularExpressions;

public static class Program
{
    public static bool notRunning;

    //main line
    [STAThread]
    public static void Main()
    {
        //setup objects
        CombatManager combatManager = new CombatManager();
        Map MapObj = new Map();
        PlayerControl control = new PlayerControl(MapObj, combatManager);
        //title screen text and new/load/exit
        TitleScreen(MapObj, control);




        //program
        while (!notRunning)
        {
            Movement(control);
            Combat(control);
            Inventory(control);
        }

    }

    //movement around map code
    private static void Movement(PlayerControl obj)
    {
        while (GameManager.IsMap())
        {

            obj.MovementCommands();
        }
    }

    //combat code
    private static void Combat(PlayerControl obj)
    {
        while (GameManager.IsCombat())
        {

            obj.CombatCommands();
        }
    }

    //inventory and character code
    private static void Inventory(PlayerControl obj)
    {
        while (GameManager.IsPInterface())
        {
            obj.InterfaceCommands();
        }

    }

    //**************\\
    // TITLE SCREEN \\
    //**************\\
    private static void TitleScreen(Map mapObj, PlayerControl playerObj)
    {
        Console.Clear();
        string enteredText = null;
        List<string> availableCommands = new List<string>
        { "N", "NEWGAME", "NEW", "L", "LOAD", "LOADGAME", "EXIT", "EXITGAME", "QUIT", "E"};
        notRunning = true; //disable game
        Console.Title = "Ara TextRPG";
        Console.ForegroundColor = ConsoleColor.Magenta; //set color
        Console.WriteLine("Welcome teo Ara TextRPG!");
        Console.ForegroundColor = ConsoleColor.Gray; //set color normal
        Console.WriteLine("New Game" + "\nLoad Game" + "\nExit Game");
        Console.ForegroundColor = ConsoleColor.White; //set color white
        Console.WriteLine("Enter Command");
        Console.ForegroundColor = ConsoleColor.Gray; //set color normal
        //commands and corresponding methods
        enteredText = PlayerControl.ReadText(enteredText, availableCommands);
        switch (enteredText.ToUpper().Trim())
        {
            case "N":
            case "NEWGAME":
            case "NEW":
                NewGame(mapObj, playerObj.player);
                break;
            case "L":
            case "LOAD":
            case "LOADGAME":
                LoadGame(mapObj, playerObj.player, playerObj);
                break;
            case "EXIT":
            case "EXITGAME":
            case "QUIT":
            case "E":
                ExitGame();
                break;
        }
    }

    private static void NewGame(Map mapObj, Player playerObj)
    {
        //dowhile name doesn't contain only letters
        do
        {
            Console.Clear();
            Console.WriteLine("What is your name?" + "\n(Must contain letter only)");
            playerObj.attributes.name = Console.ReadLine();
        } while (!Regex.IsMatch(playerObj.attributes.name, @"^[a-zA-Z]+$"));
        //picking attribute majors
        bool pickedMajor = false;
        do
        {
            string enteredText = null;
            string majorOne = null;
            string majorTwo = null;
            //reset scalling
            playerObj.attributes.strengthScaling = 1;
            playerObj.attributes.constitutionScaling = 1;
            playerObj.attributes.dexterityScaling = 1;
            playerObj.attributes.magicScaling = 1;
            List<string> availableCommands = new List<string> { "1", "2", "3", "4" };
            Console.Clear();
            Console.WriteLine("Hello " + playerObj.attributes.name + " Please pick two attributes to major in" +
                "\n(Majored attributes level twice as much)" +
                "\n1: strength (physical damage) \n2: constitution (health), \n3: dexterity (turn speed), \n4: magic (mana/spelldamage)");
            enteredText = PlayerControl.ReadText(enteredText, availableCommands);
            switch (enteredText.ToUpper().Trim())
            {
                case "1":
                    majorOne = "strength";
                    availableCommands.RemoveAt(0);
                    playerObj.attributes.strengthScaling = 2;
                    break;
                case "2":
                    majorOne = "constitution";
                    playerObj.attributes.constitutionScaling = 2;
                    availableCommands.RemoveAt(1);
                    break;
                case "3":
                    majorOne = "dexterity";
                    playerObj.attributes.dexterityScaling = 2;
                    availableCommands.RemoveAt(2);
                    break;
                case "4":
                    majorOne = "magic";
                    playerObj.attributes.magicScaling = 2;
                    availableCommands.RemoveAt(3);
                    break;
            }
            Console.WriteLine(majorOne);
            Console.WriteLine("Pick your second major");
            enteredText = null;
            enteredText = PlayerControl.ReadText(enteredText, availableCommands);
            switch (enteredText.ToUpper().Trim())
            {
                case "1":
                    majorTwo = "strength";
                    playerObj.attributes.strengthScaling = 2;
                    break;
                case "2":
                    majorTwo = "constitution";
                    playerObj.attributes.constitutionScaling = 2;
                    break;
                case "3":
                    majorTwo = "dexterity";
                    playerObj.attributes.dexterityScaling = 2;
                    break;
                case "4":
                    majorTwo = "magic";
                    playerObj.attributes.magicScaling = 2;
                    break;
            }
            Console.WriteLine("Your two major's are " + majorOne + " " + majorTwo);
            Console.WriteLine("Continue with these majors?");
            List<string> continueCommand = new List<string> { "YES", "Y", "NO", "N" };
            enteredText = PlayerControl.ReadText(enteredText, continueCommand);
            switch (enteredText.ToUpper().Trim())
            {
                case "Y":
                case "YES":
                    pickedMajor = true;
                    break;
                case "N":
                case "NO":
                    break;
            }

        } while (!pickedMajor);

        mapObj.CreateGrid(1, "Entered Level 1"); //spawn first area
        playerObj.attributes.AttributeUpdate();
        playerObj.attributes.SetResource();
        notRunning = false; //enable game
    }

    private static void LoadGame(Map mapObj, Player playerObj, PlayerControl ctrl)
    {
        OpenFileDialog filebrowser = new OpenFileDialog(); //new file browser
        filebrowser.Filter = "Text Files (.txt)|*.txt";
        //check dialog is ok, catch file errors
        if (filebrowser.ShowDialog() == DialogResult.OK)
        {
            //try to read file into datas
            // try
            //{
            Stream file = filebrowser.OpenFile();
            StreamReader loadGame = new StreamReader(file);
            while (loadGame.EndOfStream == false)
            {
                //player load
                playerObj.attributes.name = loadGame.ReadLine();
                playerObj.attributes.level = int.Parse(loadGame.ReadLine());
                playerObj.attributes.experience = int.Parse(loadGame.ReadLine());
                playerObj.attributes.strengthScaling = int.Parse(loadGame.ReadLine());
                playerObj.attributes.constitutionScaling = int.Parse(loadGame.ReadLine());
                playerObj.attributes.dexterityScaling = int.Parse(loadGame.ReadLine());
                playerObj.attributes.magicScaling = int.Parse(loadGame.ReadLine());
                playerObj.attributes.healthVar = int.Parse(loadGame.ReadLine());
                playerObj.attributes.manaVar = int.Parse(loadGame.ReadLine());

                //map load
                mapObj.currentLevel = int.Parse(loadGame.ReadLine());
                mapObj.playerPositionCol = int.Parse(loadGame.ReadLine());
                mapObj.playerPositionRow = int.Parse(loadGame.ReadLine());

                mapObj.CreateGrid(mapObj.currentLevel, "Entered Level " + mapObj.currentLevel);
                notRunning = false; //enable game
                Console.WriteLine("Game Loaded");
            }
            file.Close();
            loadGame.Close();
            /*}
            catch (Exception ex)
            {
                TitleScreen(mapObj, ctrl);
                Console.WriteLine("Error reading file");
            }*/
            //cancel back to main screen
        }
        else
        {
            TitleScreen(mapObj, ctrl);
        }
        notRunning = false; //enable game if load succeeds
    }

    private static void ExitGame()
    {
        Environment.Exit(0);
    }

    //save game data
    public static void SaveGame(Map mapObj, Player playerObj)
    {
        SaveFileDialog folderBrowser = new SaveFileDialog(); //new save file browser
        folderBrowser.Filter = "Text Files (.txt)|*.txt";
        //check dialog is ok, catch file errors
        if (folderBrowser.ShowDialog() == DialogResult.OK && folderBrowser.FileName != "")
        {
            try
            {
                StreamWriter saveGame = new StreamWriter(folderBrowser.FileName);
                //player stat
                saveGame.WriteLine(playerObj.attributes.name);
                saveGame.WriteLine(playerObj.attributes.level);
                saveGame.WriteLine(playerObj.attributes.experience);
                saveGame.WriteLine(playerObj.attributes.strengthScaling);
                saveGame.WriteLine(playerObj.attributes.constitutionScaling);
                saveGame.WriteLine(playerObj.attributes.dexterityScaling);
                saveGame.WriteLine(playerObj.attributes.magicScaling);
                saveGame.WriteLine(playerObj.attributes.healthVar);
                saveGame.WriteLine(playerObj.attributes.manaVar);
                //map stat
                saveGame.WriteLine(mapObj.currentLevel);
                saveGame.WriteLine(mapObj.playerPositionCol);
                saveGame.WriteLine(mapObj.playerPositionRow);
                saveGame.Close();
                Console.WriteLine("Game Saved");
            }
            catch
            {
                Console.WriteLine("Failed to Save File!");
            }

        }
        else
        {
            Console.WriteLine("Failed to Save File!");
        }
    }

    //clamp code
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0)
            return min;
        if (value.CompareTo(max) > 0)
            return max;

        return value;
    }


}

