using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class OperationWithCards
    {
        public static int ExtractColumn(string i_Input)
        {
            return int.Parse(i_Input.Substring(1, 1));
        }

        public static int ExtractRow(string i_Input)
        {
            return int.Parse(i_Input.Substring(0, 1));
        }

        public static int NumberOfHidenCards(Player i_PlayerNumberOne, Player i_PlayerNumberTwo, Board i_Board)
        {
            int totalPoints = i_PlayerNumberOne.Points + i_PlayerNumberTwo.Points;
            int totalPairs = i_Board.NumberOfCells / 2;

            return totalPairs - totalPoints;
        }

        public static bool CheckIfMatch(string i_FirstLocation, string i_SecondLocation, Board i_Board)
        {
            bool isMatch = false;
            string valueOfCardOne = i_Board.ValueInRealMatrix(ExtractRow(i_FirstLocation), ExtractColumn(i_FirstLocation));
            string valueOfCardTwo = i_Board.ValueInRealMatrix(ExtractRow(i_SecondLocation), ExtractColumn(i_SecondLocation));
            if (valueOfCardOne.Equals(valueOfCardTwo))
            {
                isMatch = true;
            }

            return isMatch;
        }
    }
}