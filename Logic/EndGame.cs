using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class EndGame
    {
        public static Player WinnerOfTheGame(Player i_PlayerNumberOne, Player i_PlayerNumberTwo)
        {
            Player winner = i_PlayerNumberTwo;
            if (i_PlayerNumberOne.Points > i_PlayerNumberTwo.Points)
            {
                winner = i_PlayerNumberOne;
            }

            return winner;
        }

        public static bool IsTie(Player i_PlayerNumberOne, Player i_PlayerNumberTwo)
        {
            return i_PlayerNumberOne.Points == i_PlayerNumberTwo.Points;
        }
    }
}