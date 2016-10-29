using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameManager
{
    //status of game state
    public enum PlayerStatus { Map, Combat, PInterface }
    public static PlayerStatus status;

    public GameManager()
    {
        status = PlayerStatus.Map;
    }

    public static void Map()
    {
        status = PlayerStatus.Map;
    }

    public static void Combat()
    {
        status = PlayerStatus.Combat;
    }

    public static void pInterface()
    {
        status = PlayerStatus.PInterface;
    }
    //test bools for status of game
    public static bool IsMap()
    {
        if (status == PlayerStatus.Map)
        {
            return true;
        }
        return false;
    }
    public static bool IsCombat()
    {
        if (status == PlayerStatus.Combat)
        {
            return true;
        }
        return false;
    }
    public static bool IsPInterface()
    {
        if (status == PlayerStatus.PInterface)
        {
            return true;
        }
        return false;
    }
}

