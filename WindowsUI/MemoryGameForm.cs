using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.IO;
using Logic;

namespace WindowsUI
{
    public class MemoryGameForm : Form
    {
        private enum eClickMode
        {
            ClickedMouse,
            UnClickedMouse
        }

        private const int k_ButtonSquareSize = 45;
        private const int k_Space = 7;
        private const int k_LeftStart = 10;
        private readonly GameManager r_GameManager;
        private readonly Button[,] r_Board;
        private readonly Color r_GrayColor = Color.FromArgb(200, 200, 200);
        private readonly List<Image> r_ListOfImages;
        private Label labelcurrentPlayer = new Label();
        private Label labelFirstPlayerName = new Label();
        private Label labelSecondPlayerName = new Label();
        private Button firstMove;
        private eClickMode m_MouseClickStatus;

        public MemoryGameForm(GameSettingForm i_GameSettingForm)
        {
            Text = "Memory Game";
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            Player firstPlayer = new Player(i_GameSettingForm.FirstPlayerName);
            Player secondPlayer = new Player(i_GameSettingForm.SecondPlayerName);
            r_GameManager = new GameManager(i_GameSettingForm.BoardColumns, i_GameSettingForm.BoardRows, firstPlayer, secondPlayer);
            r_GameManager.FirstPlayer.Turn = true;
            r_GameManager.GameOver += gameManager_GameOver;
            r_GameManager.BoardChanged += gameManager_BoardChanged;
            r_Board = new Button[i_GameSettingForm.BoardRows, i_GameSettingForm.BoardColumns];
            int widthBoard = (i_GameSettingForm.BoardColumns * k_ButtonSquareSize) + ((i_GameSettingForm.BoardColumns - 1) * k_Space);
            int heightBoard = (i_GameSettingForm.BoardRows * k_ButtonSquareSize) + ((i_GameSettingForm.BoardRows - 1) * k_Space);
            Size = new Size(widthBoard + 35, heightBoard + 120);
            r_ListOfImages = buildListOfImages();
            m_MouseClickStatus = eClickMode.UnClickedMouse;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            initializeControls();
        }

        private void initializeControls()
        {
            labelcurrentPlayer.Size = new Size(150, 20);
            labelcurrentPlayer.Location = new Point(k_LeftStart, (r_GameManager.Board.Row * k_ButtonSquareSize) + ((r_GameManager.Board.Row - 1) * k_Space) + 20);
            labelcurrentPlayer.AutoSize = true;
            updateCurrentPlayerLabel();
            labelFirstPlayerName.Size = new Size(150, 20);
            labelFirstPlayerName.Location = new Point(k_LeftStart, (r_GameManager.Board.Row * k_ButtonSquareSize) + ((r_GameManager.Board.Row - 1) * k_Space) + 40);
            labelFirstPlayerName.AutoSize = true;
            labelFirstPlayerName.BackColor = r_GameManager.ColorFirstPlayer;
            labelSecondPlayerName.Size = new Size(150, 20);
            labelSecondPlayerName.Location = new Point(k_LeftStart, (r_GameManager.Board.Row * k_ButtonSquareSize) + ((r_GameManager.Board.Row - 1) * k_Space) + 60);
            labelSecondPlayerName.AutoSize = true;
            labelSecondPlayerName.BackColor = r_GameManager.ColorSecondPlayer;
            updatePlayersLabel();
            this.Controls.AddRange(
               new Control[]
               {
                   labelcurrentPlayer,
                   labelFirstPlayerName,
                   labelSecondPlayerName
               });

            buildBoard();
        }

        private void buildBoard()
        {
            int currentX;
            int currentY;
            for (int currentRow = 0; currentRow < r_GameManager.Board.Row; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < r_GameManager.Board.Column; currentColumn++)
                {
                    r_Board[currentRow, currentColumn] = new Button();
                    r_Board[currentRow, currentColumn].Size = new Size(k_ButtonSquareSize, k_ButtonSquareSize);
                    currentX = k_LeftStart + (currentColumn * k_ButtonSquareSize) + (currentColumn * k_Space);
                    currentY = k_LeftStart + (currentRow * k_ButtonSquareSize) + (currentRow * k_Space);
                    r_Board[currentRow, currentColumn].Location = new Point(currentX, currentY);
                    r_Board[currentRow, currentColumn].BackColor = r_GrayColor;
                    r_Board[currentRow, currentColumn].Name = string.Format("{0}{1}", currentRow, currentColumn);
                    Controls.Add(r_Board[currentRow, currentColumn]);
                    r_Board[currentRow, currentColumn].Click += new EventHandler(button_Click);
                }
            }
        }

        private List<Image> buildListOfImages()
        {
            List<Image> listOfImages = new List<Image>();
            int sizeOfImagesList = r_GameManager.Board.NumberOfCells / 2;
            List<string> listOfImagesUrl = new List<string>();
            while (listOfImages.Count < sizeOfImagesList)
            {
                WebRequest webRequest = WebRequest.Create("https://picsum.photos/80");
                WebResponse webResponse = webRequest.GetResponse();
                using (webResponse)
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    string currentUrl = webResponse.ResponseUri.ToString();
                    bool containUrl = listOfImagesUrl.Contains(currentUrl);
                    if (!containUrl)
                    {
                        listOfImagesUrl.Add(currentUrl);
                        Image currentImage = Image.FromStream(responseStream);
                        listOfImages.Add(currentImage);
                    }
                }
            }

            return listOfImages;
        }

        private void gameManager_BoardChanged(int i_Row, int i_Column)
        {
            Button button = r_Board[i_Row, i_Column];
            char imageChar = char.Parse(r_GameManager.Board.ValueInRealMatrix(i_Row, i_Column));
            button.Image = r_ListOfImages[imageChar - 'A'];
            button.Refresh();
            r_GameManager.Board.FlipUpCardInBoard(i_Row, i_Column);
        }

        private void gameManager_GameOver()
        {
            bool isTie = EndGame.IsTie(r_GameManager.FirstPlayer, r_GameManager.SecondPlayer);
            bool playAgain;
            if (isTie)
            {
                playAgain = showMessage(string.Format("It's a tie!{0}Every player got: {1} points", Environment.NewLine, r_GameManager.FirstPlayer.Points));
            }
            else
            {
                Player winner = EndGame.WinnerOfTheGame(r_GameManager.FirstPlayer, r_GameManager.SecondPlayer);
                playAgain = showMessage(string.Format(
                @"The winner is: {0}
{1}: {2} points
{3}: {4} points
",
                winner.Name,
                r_GameManager.FirstPlayer.Name,
                r_GameManager.FirstPlayer.Points,
                r_GameManager.SecondPlayer.Name,
                r_GameManager.SecondPlayer.Points));
            }

            if (playAgain)
            {
                Controls.Clear();
                restart();
                initializeControls();
                Update();
            }
            else
            {
                Close();
            }
        }

        private void restart()
        {
            r_GameManager.FirstPlayer.Restart();
            r_GameManager.SecondPlayer.Restart();
            r_GameManager.CurrentPlayer = r_GameManager.FirstPlayer;
            r_GameManager.FirstPlayer.Turn = true;
            int boardRows = r_GameManager.Board.Row;
            int boarsColumn = r_GameManager.Board.Column;
            r_GameManager.Board = new Board(boardRows, boarsColumn);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text.Equals(string.Empty))
            {
                int buttonRow = OperationWithCards.ExtractRow(button.Name);
                int buttonColumn = OperationWithCards.ExtractColumn(button.Name);
                r_GameManager.MakeMove(buttonRow, buttonColumn);
                if (m_MouseClickStatus.Equals(eClickMode.UnClickedMouse))
                {
                    firstMove = button;
                    firstPartOfMove();
                    computerTurn();
                }
                else
                {
                    secondPartOfMove(button);
                    r_GameManager.CheckIfGameEnded();
                    computerTurn();
                }
            }
        }

        private void computerTurn()
        {
            System.Threading.Thread.Sleep(1000);
            if (r_GameManager.Board.NumberOfCells != (r_GameManager.FirstPlayer.Points + r_GameManager.SecondPlayer.Points) * 2)
            {
                if (r_GameManager.FirstPlayer.Turn == false && r_GameManager.SecondPlayer.Name.Equals("- computer -"))
                {
                    string buttonLocation = r_GameManager.ComputerClick();
                    int row = OperationWithCards.ExtractRow(buttonLocation);
                    int column = OperationWithCards.ExtractColumn(buttonLocation);
                    takeFocus(r_Board[row, column]);
                    r_Board[row, column].PerformClick();
                }
            }
        }

        private void firstPartOfMove()
        {
            m_MouseClickStatus = eClickMode.ClickedMouse;
        }

        private void secondPartOfMove(Button i_Button)
        {
            bool isMatch = OperationWithCards.CheckIfMatch(i_Button.Name, firstMove.Name, r_GameManager.Board);
            if (isMatch)
            {
                r_GameManager.CurrentPlayer.AddPointToPlayer();
                updatePlayersLabel();
            }
            else
            {
                System.Threading.Thread.Sleep(2000);
                firstMove.Image = null;
                i_Button.Image = null;
                Refresh();
                r_GameManager.Board.FlipDownCardInBoard(OperationWithCards.ExtractRow(firstMove.Name), OperationWithCards.ExtractColumn(firstMove.Name));
                r_GameManager.Board.FlipDownCardInBoard(OperationWithCards.ExtractRow(i_Button.Name), OperationWithCards.ExtractColumn(i_Button.Name));
                r_GameManager.SwitchTurn();
                updateCurrentPlayerLabel();
            }

            m_MouseClickStatus = eClickMode.UnClickedMouse;
            takeFocus(null);
        }

        private void takeFocus(Control i_Control)
        {
            ActiveControl = i_Control;
            Refresh();
        }

        private void updatePlayersLabel()
        {
            labelFirstPlayerName.Text = string.Format("{0}: {1} Pairs", r_GameManager.FirstPlayer.Name, r_GameManager.FirstPlayer.Points);
            labelSecondPlayerName.Text = string.Format("{0}: {1} Pairs", r_GameManager.SecondPlayer.Name, r_GameManager.SecondPlayer.Points);
        }

        private void updateCurrentPlayerLabel()
        {
            labelcurrentPlayer.Text = string.Format("Current Player: {0}", r_GameManager.CurrentPlayer.Name);
            labelcurrentPlayer.BackColor = r_GameManager.ColorOfCurrentPlayer();
        }

        private bool showMessage(string i_Message)
        {
            string message = string.Format("{0}{1}Another Round?", i_Message, Environment.NewLine);
            return MessageBox.Show(
            message,
            "Memory Game",
           MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
    }
}