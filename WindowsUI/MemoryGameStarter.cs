using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WindowsUI
{
    public class MemoryGameStarter
    {
        public static void Run()
        {
            GameSettingForm gameSetting = new GameSettingForm();

            if (gameSetting.ShowDialog() == DialogResult.OK)
            {
                MemoryGameForm MemoryGame = new MemoryGameForm(gameSetting);
                MemoryGame.ShowDialog();
            }
        }
    }
}