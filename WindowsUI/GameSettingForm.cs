using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace WindowsUI
{
    public class GameSettingForm : Form
    {
        private TextBox textboxFirstPlayerName = new TextBox();
        private TextBox textboxSecondPlayerName = new TextBox();
        private Label labelFirstPlayerName = new Label();
        private Label labelSecondPlayerName = new Label();
        private Label labelBoardSize = new Label();
        private Button buttonStart = new Button();
        private Button buttonPlayingAgainst = new Button();
        private Button buttonBoardSize = new Button();
        private int m_numberOfClicked = 0;
        private int m_BoardColumns;
        private int m_BoardRows;

        public GameSettingForm()
        {
            Size = new Size(435, 230);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Memory Game - Settings";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initControls();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            DialogResult = DialogResult.OK;
        }

        private void initControls()
        {
            labelFirstPlayerName.Text = "First Player Name:";
            labelFirstPlayerName.Location = new Point(30, 20);
            labelSecondPlayerName.Text = "Second Player Name:";
            labelSecondPlayerName.Size = new Size(120, labelSecondPlayerName.Height);
            labelSecondPlayerName.Location = new Point(30, 45);
            labelBoardSize.Text = "Board Size:";
            labelBoardSize.Location = new Point(30, 75);
            textboxFirstPlayerName.Size = new Size(110, textboxFirstPlayerName.Height);
            int textBoxFirstPlayerNameTop = labelFirstPlayerName.Top + (labelFirstPlayerName.Height / 2);
            textBoxFirstPlayerNameTop -= textboxFirstPlayerName.Height / 2;
            textboxFirstPlayerName.Location = new Point(labelFirstPlayerName.Right + 30, textBoxFirstPlayerNameTop);
            textboxSecondPlayerName.Size = new Size(110, textboxSecondPlayerName.Height);
            int textBoxSecondPlayerNameTop = labelSecondPlayerName.Top + (labelSecondPlayerName.Height / 2);
            textBoxSecondPlayerNameTop -= textboxSecondPlayerName.Height / 2;
            textboxSecondPlayerName.Location = new Point(labelSecondPlayerName.Right + 10, textBoxSecondPlayerNameTop);
            textboxSecondPlayerName.Enabled = false;
            textboxSecondPlayerName.Text = "- computer -";
            buttonStart.Text = "Start!";
            buttonStart.Size = new Size(buttonStart.Width + 10, buttonStart.Height);
            buttonStart.Location = new Point(ClientSize.Width - buttonStart.Width - 25, ClientSize.Height - buttonStart.Height - 20);
            buttonStart.BackColor = Color.FromArgb(0, 192, 0);
            buttonPlayingAgainst.Text = "Against a Friend";
            buttonPlayingAgainst.Size = new Size(110, buttonPlayingAgainst.Height);
            buttonPlayingAgainst.Location = new Point(ClientSize.Width - buttonPlayingAgainst.Width - 25, textBoxSecondPlayerNameTop);
            buttonBoardSize.Text = "4 x 4";
            m_BoardColumns = 4;
            m_BoardRows = 4;
            buttonBoardSize.Size = new Size(120, 75);
            buttonBoardSize.Location = new Point(30, ClientSize.Height - buttonBoardSize.Height - 20);
            buttonBoardSize.BackColor = Color.FromArgb(191, 191, 255);
            Controls.AddRange(
                new Control[]
                {
                textboxFirstPlayerName,
                textboxSecondPlayerName,
                labelFirstPlayerName,
                labelSecondPlayerName,
                labelBoardSize,
                buttonStart,
                buttonPlayingAgainst,
                buttonBoardSize,
                });

            buttonPlayingAgainst.Click += new EventHandler(buttonPlayingAgainst_Click);
            buttonBoardSize.Click += new EventHandler(buttonBoardSize_Click);
            buttonStart.Click += new EventHandler(buttonStart_Click);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            m_numberOfClicked++;
            switch (m_numberOfClicked)
            {
                case 1:
                    buttonBoardSize.Text = "4 x 5";
                    m_BoardColumns = 5;
                    m_BoardRows = 4;
                    break;
                case 2:
                    buttonBoardSize.Text = "4 x 6";
                    m_BoardColumns = 6;
                    m_BoardRows = 4;
                    break;
                case 3:
                    buttonBoardSize.Text = "5 x 4";
                    m_BoardColumns = 4;
                    m_BoardRows = 5;
                    break;
                case 4:
                    buttonBoardSize.Text = "5 x 6";
                    m_BoardColumns = 6;
                    m_BoardRows = 5;
                    break;
                case 5:
                    buttonBoardSize.Text = "6 x 4";
                    m_BoardColumns = 4;
                    m_BoardRows = 6;
                    break;
                case 6:
                    buttonBoardSize.Text = "6 x 5";
                    m_BoardColumns = 5;
                    m_BoardRows = 6;
                    break;
                case 7:
                    buttonBoardSize.Text = "6 x 6";
                    m_BoardColumns = 6;
                    m_BoardRows = 6;
                    break;
                case 8:
                    buttonBoardSize.Text = "4 x 4";
                    m_BoardColumns = 4;
                    m_BoardRows = 4;
                    m_numberOfClicked = 0;
                    break;
            }
        }

        private void buttonPlayingAgainst_Click(object sender, EventArgs e)
        {
            if (!textboxSecondPlayerName.Enabled)
            {
                textboxSecondPlayerName.Enabled = true;
                textboxSecondPlayerName.Text = string.Empty;
                buttonPlayingAgainst.Text = "Against Computer";
            }
            else
            {
                textboxSecondPlayerName.Enabled = false;
                textboxSecondPlayerName.Text = "- computer -";
                buttonPlayingAgainst.Text = "Against a Friend";
            }
        }

        public string FirstPlayerName
        {
            get { return textboxFirstPlayerName.Text; }
        }

        public string SecondPlayerName
        {
            get { return textboxSecondPlayerName.Text; }
        }

        public int BoardColumns
        {
            get { return m_BoardColumns; }
        }

        public int BoardRows
        {
            get { return m_BoardRows; }
        }
    }
}