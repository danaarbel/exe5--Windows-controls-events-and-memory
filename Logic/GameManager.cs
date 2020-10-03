using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Logic
{
    public class GameManager
    {
        private readonly Player r_FirstPlayer;
        private readonly Player r_SecondPlayer;
        private readonly Color r_ColorFirstPlayer = Color.FromArgb(191, 255, 191);
        private readonly Color r_ColorSecondPlayer = Color.FromArgb(192, 192, 255);
        private Player m_CurrentPlayer;
        private Board m_Board;

        public event GameOverEventHandler GameOver;

        public event BoardChangeEventHandler BoardChanged;

        public GameManager(int i_BoardColumns, int i_BoardRows, Player i_FirstPlayer, Player i_SecondPlayer)
        {
            m_Board = new Board(i_BoardRows, i_BoardColumns);
            r_FirstPlayer = i_FirstPlayer;
            r_SecondPlayer = i_SecondPlayer;
            m_CurrentPlayer = i_FirstPlayer;
        }

        public Color ColorOfCurrentPlayer()
        {
            Color backColor;
            if (m_CurrentPlayer.Name.Equals(r_FirstPlayer.Name))
            {
                backColor = r_ColorFirstPlayer;
            }
            else
            {
                backColor = r_ColorSecondPlayer;
            }

            return backColor;
        }

        public void SwitchTurn()
        {
            if (r_FirstPlayer.Turn)
            {
                r_FirstPlayer.Turn = false;
                r_SecondPlayer.Turn = true;
                m_CurrentPlayer = r_SecondPlayer;
            }
            else
            {
                r_FirstPlayer.Turn = true;
                r_SecondPlayer.Turn = false;
                m_CurrentPlayer = r_FirstPlayer;
            }
        }

        public string ComputerClick()
        {
            Random random = new Random();
            int rowButton = random.Next(0, m_Board.Row);
            int columnButton = random.Next(0, m_Board.Column);
            while (!m_Board.IsEmptyCell(rowButton, columnButton))
            {
                rowButton = random.Next(0, m_Board.Row);
                columnButton = random.Next(0, m_Board.Column);
            }

            string buttonLocation = string.Format("{0}{1}", rowButton, columnButton);

            return buttonLocation;
        }

        public void MakeMove(int i_Row, int i_Column)
        {
            OnBoardChange(i_Row, i_Column);
        }

        public void CheckIfGameEnded()
        {
            int numberOfHidenCards = OperationWithCards.NumberOfHidenCards(r_FirstPlayer, r_SecondPlayer, m_Board);
            if (numberOfHidenCards == 0)
            {
                OnGameOver();
            }
        }

        protected virtual void OnBoardChange(int i_Row, int i_Column)
        {
            if (BoardChanged != null)
            {
                BoardChanged.Invoke(i_Row, i_Column);
            }
        }

        protected virtual void OnGameOver()
        {
            if (GameOver != null)
            {
                GameOver.Invoke();
            }
        }

        public Player FirstPlayer
        {
            get { return r_FirstPlayer; }
        }

        public Player SecondPlayer
        {
            get { return r_SecondPlayer; }
        }

        public Player CurrentPlayer
        {
            get { return m_CurrentPlayer; }
            set { m_CurrentPlayer = value; }
        }

        public Board Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }

        public Color ColorFirstPlayer
        {
            get { return r_ColorFirstPlayer; }
        }

        public Color ColorSecondPlayer
        {
            get { return r_ColorSecondPlayer; }
        }
    }
}