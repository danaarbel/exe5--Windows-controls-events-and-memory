using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Player
    {
        private readonly string m_Name;
        private int m_Points;
        private bool m_Turn;

        public Player(string i_NameOfPlayer)
        {
            m_Name = i_NameOfPlayer;
            m_Points = 0;
            m_Turn = false;
        }

        public bool Turn
        {
            get { return m_Turn; }
            set { m_Turn = value; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public int Points
        {
            get { return m_Points; }
        }

        public void AddPointToPlayer()
        {
            m_Points++;
        }

        public void ZeroingPoints()
        {
            m_Points = 0;
        }

        public void Restart()
        {
            ZeroingPoints();
            Turn = false;
        }
    }
}