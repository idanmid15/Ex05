using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05.FourInARow
{
    public partial class GameBoardForm : Form
    {
        private const byte k_MarginSpace = 6;
        private const byte k_CellSize = 30;
        private const byte k_SpaceBetweenButtons = 6;
        private const byte k_SpaceForResult = 50;
        private List<Button> m_FirstRowButtons;
        private Button[,] m_ButtonMatrix;
        private GameManager m_GameManager;

        internal GameBoardForm(GameManager i_GameManager)
        {
            InitializeComponent();
            m_GameManager = i_GameManager;
            int NumOfCols = this.m_GameManager.NumOfColumns;
            int NumOfRows = this.m_GameManager.NumOfRows;
            int widthSize = 3 * k_MarginSpace + NumOfCols * (k_CellSize + k_SpaceBetweenButtons);
            int heighthSize = 3 * k_MarginSpace + (NumOfRows + 1) * (k_CellSize + k_SpaceBetweenButtons) + k_SpaceForResult;
            this.Size = new Size(widthSize, heighthSize);
            drawLabeledButtons(NumOfCols);
            drawGameBoard();
            drawGameScore();
        }

        private void drawGameScore()
        {
            Label firstPlayerLabel = new Label();
            firstPlayerLabel.Text = string.Format("{0} : {1}", this.m_GameManager.FirstPlayerName, this.m_GameManager.FirstPlayerScore);
            firstPlayerLabel.Location = new Point(0, this.Height - (10 * k_MarginSpace));
            this.Controls.Add(firstPlayerLabel);

            Label secondPlayerLabel = new Label();
            secondPlayerLabel.Text = string.Format("{0} : {1}", this.m_GameManager.SecondPlayerName, this.m_GameManager.SecondPlayerScore);
            secondPlayerLabel.Location = new Point(firstPlayerLabel.Width + k_SpaceBetweenButtons, this.Height - (10 * k_MarginSpace));
            this.Controls.Add(secondPlayerLabel);
        }

        private void drawGameBoard()
        {
            m_ButtonMatrix = new Button[m_GameManager.NumOfRows, m_GameManager.NumOfColumns];
            int currentYLocation = 0;
            int currentXLocation = 0;
            for (int y = 0; y < m_GameManager.NumOfRows; y++)
            {
                for (int x = 0; x < m_GameManager.NumOfColumns; x++)
                {
                    m_ButtonMatrix[y, x] = new Button();
                    m_ButtonMatrix[y, x].Width = k_CellSize;
                    m_ButtonMatrix[y, x].Height = k_CellSize;
                    m_ButtonMatrix[y, x].Text = m_GameManager.GetCell(y, x) + "";
                    currentYLocation = m_FirstRowButtons[0].Height + y * (k_CellSize + k_SpaceBetweenButtons) + k_MarginSpace;
                    currentXLocation = x * (k_CellSize + k_SpaceBetweenButtons) + k_MarginSpace;
                    m_ButtonMatrix[y, x].Location = new Point(currentXLocation, currentYLocation);
                    this.Controls.Add(m_ButtonMatrix[y, x]);
                }
            }
        }

        private void drawLabeledButtons(int i_NumOfColumns)
        {
            m_FirstRowButtons = new List<Button>();
            Button currentButton = null;
            for (int i = 0; i < i_NumOfColumns; i++)
            {
                currentButton = new Button();
                currentButton.Text = (i + 1).ToString();
                currentButton.Width = k_CellSize;
                currentButton.Location = new Point(k_MarginSpace + (i * (k_CellSize + k_SpaceBetweenButtons)), k_MarginSpace);
                currentButton.Click += new System.EventHandler(InsertButton_Click);
                m_FirstRowButtons.Add(currentButton);

                this.Controls.Add(m_FirstRowButtons[i]);
            }
        }

        private void reDraw()
        {
            this.Hide();
            m_GameManager.ReinitializeBoard();
            new GameBoardForm(m_GameManager).ShowDialog();
            this.Close();
        }

        private void InsertButton_Click(object i_Sender, EventArgs e)
        {
            if (i_Sender is Button)
            {
                Button currentClickedButton = i_Sender as Button;
                int colToInsert = int.Parse(currentClickedButton.Text) - 1;
                int colHeight = m_GameManager.GetColumnTop(colToInsert);

                //if it's a valid insertion move
                if (m_GameManager.InsertToCol(colToInsert))
                {
                    this.m_ButtonMatrix[colHeight, colToInsert].Text = m_GameManager.GetCell(colHeight, colToInsert) + "";
                    int computerMove = this.m_GameManager.DecideNextMove();
                    if (computerMove != -1 && computerMove != -5)
                    {
                        colHeight = m_GameManager.GetColumnTop(computerMove) + 1;
                        this.m_ButtonMatrix[colHeight, computerMove].Text = m_GameManager.GetCell(colHeight, computerMove) + "";
                    }

                    if (this.m_GameManager.IsGameOver())
                    {
                        displayAnotherRoundMessageBox();
                    }
                }
            }
        }

        private void displayAnotherRoundMessageBox()
        {
            DialogResult result = MessageBox.Show(string.Format("{0}{1}Another Round?", this.m_GameManager.GetWinningPlayer(), Environment.NewLine)
                                    , "Game Ended!", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                this.Close();
            }
            else
            {
                reDraw();
            }
        }
    }
}
