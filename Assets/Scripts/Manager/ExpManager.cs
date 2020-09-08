using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class ExpManager
{
    public static int CalculateExp(Environment e)
    {
        int Plvl = Player.MyInstPL.myLevel;
        int baseXP = (Plvl * 5) + 45;

        int grayLevel = CalculateGray();
        int totalXP = 1;
        if (e.myLevel >= Plvl)
        {
            totalXP = (int)(baseXP * (1 + 0.05 * (e.myLevel - Plvl)));
        }
        else if (e.myLevel > grayLevel)
        {
            totalXP = (baseXP) * (1 - (Plvl - e.myLevel) / zeroDiff());
        }
        return totalXP;
    }

    private static int zeroDiff()
    {
        int Plvl = Player.MyInstPL.myLevel;
        if (Plvl <= 7)
        {
            return 5;
        }
        if (Plvl >= 8 && Plvl <= 9)
        {
            return 6;
        }
        if (Plvl >= 10 && Plvl <= 11)
        {
            return 7;
        }
        if (Plvl >= 12 && Plvl <= 15)
        {
            return 8;
        }
        if (Plvl >= 16 && Plvl <= 19)
        {
            return 9;
        }
        if (Plvl >= 20 && Plvl <= 24)
        {
            return 10;
        }
        return 11;
    }

    public static int CalculateGray()
    {
        int Plvl = Player.MyInstPL.myLevel;
        if (Plvl <= 5)
        {
            return 0;
        }
        else if (Plvl >= 6 && Plvl <= 19)
        {
            return Plvl - (Plvl / 10) - 5;
        }
        else if (Plvl == 20)
        {
            return Plvl - 10;
        }
        else if (Plvl >= 21 && Plvl <= 24)
        {
            return Plvl - (Plvl / 5) - 1;
        }
        return Plvl - 9;
    }
}