using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.FourInARow
{
    internal class GameManager
    {
        private string m_FirstPlayerName;

        public string FirstPlayerName
        {
            get { return m_FirstPlayerName; }
        }

        private string m_SecondPlayerName;

        public string SecondPlayerName
        {
            get { return m_SecondPlayerName; }
        }

        private int m_FirstPlayerScore;

        public int FirstPlayerScore
        {
            get { return m_FirstPlayerScore; }
        }

        private int m_SecondPlayerScore;

        public int SecondPlayerScore
        {
            get { return m_SecondPlayerScore; }
        }

        private bool m_IsFirstPlayerTurn;
        private bool m_IsOpponentComputer;
        private GameBoard m_CurrentGameBoard;

        public int NumOfRows
        {
            get { return m_CurrentGameBoard.GetRowSize(); }
        }

        public int NumOfColumns
        {
            get { return m_CurrentGameBoard.GetColumnSize(); }
        }

        public char GetCell(int i_Row, int i_Col)
        {
            return m_CurrentGameBoard.GetCell(i_Row, i_Col);
        }

        public int GetColumnTop(int i_IndexOfColumn)
        {
            return m_CurrentGameBoard.GetColumnTop(i_IndexOfColumn);
        }

        public GameManager(
            string i_FirstPlayerName,
            string i_SecondPlayerName,
            bool i_IsOpponentComputer,
            int i_NumOfRows,
            int i_NumOfCols)
        {
            m_FirstPlayerName = i_FirstPlayerName;
            m_SecondPlayerName = i_SecondPlayerName;
            m_IsOpponentComputer = i_IsOpponentComputer;
            m_CurrentGameBoard = new GameBoard(i_NumOfRows, i_NumOfCols);
            m_IsFirstPlayerTurn = true;
            m_FirstPlayerScore = 0;
            m_SecondPlayerScore = 0;
        }

        internal bool InsertToCol(int i_ColToInsert)
        {
            bool toReturn = false;
            if (!this.m_CurrentGameBoard.IsColumnFull(i_ColToInsert))
            {
                m_CurrentGameBoard.InsertToColumn(i_ColToInsert, m_IsFirstPlayerTurn);
                m_IsFirstPlayerTurn = !m_IsFirstPlayerTurn;
                toReturn = true;
            }

            return toReturn;
        }

        internal void ReinitializeBoard()
        {
            m_CurrentGameBoard.InitiaizeNewBoard();
        } 

        internal string GetWinningPlayer()
        {
            string endOfGameMessage = string.Empty;
            string winningPlayer = ((GameBoard.eWinner)this.m_CurrentGameBoard.GetCurrentWinner()).ToString();
            ///update score status
            switch (this.m_CurrentGameBoard.GetCurrentWinner())
            {
                case GameBoard.eWinner.FirstPlayer:
                    this.m_FirstPlayerScore++;
                    endOfGameMessage = string.Format("{0}! you win!", m_FirstPlayerName);
                    break;
                case GameBoard.eWinner.SecondPlayer:
                    this.m_SecondPlayerScore++;
                    endOfGameMessage = string.Format("{0}! you win!", m_SecondPlayerName);
                    break;
                default:
                    endOfGameMessage = "board is full. game over!";
                    break;
            }

            return endOfGameMessage;
        }

        public int DecideNextMove()
        {
            int computerNextMove = -1;

            // Check if it's game over, and if not, decide whether to make a computer move
            if (!IsGameOver())
            {
                if (this.m_IsOpponentComputer)
                {
                    computerNextMove = makeComputerMove();
                }
            }

            return computerNextMove;
        }

        private int makeComputerMove()
        {
            bool validComputerMove = false;
            int computerMove = 0;

            ///AI - look for the next move that guarantees a win, else choose 
            ///a valid random move
            bool isNextMoveWin = canComputerWinInNextTry(ref computerMove);
            if (!isNextMoveWin)
            {
                if (!canComputerBlockPlayerFromWinning(ref computerMove))
                {
                    ///choose a random move for the computer until it is a legal move
                    while (!validComputerMove)
                    {
                        Random rnd = new Random();
                        computerMove = rnd.Next(1, this.m_CurrentGameBoard.GetColumnSize() + 1);
                        if (!this.m_CurrentGameBoard.IsColumnFull(computerMove - 1))
                        {
                            validComputerMove = true;
                        }
                    }
                }
            }

            this.m_CurrentGameBoard.InsertToColumn(computerMove - 1, this.m_IsFirstPlayerTurn);

            ///change the current gamer
            this.m_IsFirstPlayerTurn = !this.m_IsFirstPlayerTurn;
            return computerMove - 1;
        }

        private bool canComputerBlockPlayerFromWinning(ref int i_ComputerMove)
        {
            bool isNextBlocking = false;
            bool v_FirstPlayerSign = true;
            GameBoard tempGameBoard = null;
            for (int i = 1; i <= this.m_CurrentGameBoard.GetColumnSize(); i++)
            {
                tempGameBoard = m_CurrentGameBoard.CloneGame();
                i_ComputerMove = i;
                if (!tempGameBoard.IsColumnFull(i_ComputerMove - 1))
                {
                    tempGameBoard.InsertToColumn(i_ComputerMove - 1, v_FirstPlayerSign);
                    if (tempGameBoard.IsOver())
                    {
                        isNextBlocking = true;
                        break;
                    }
                }
            }

            return isNextBlocking;
        }

        private bool canComputerWinInNextTry(ref int i_ComputerMove)
        {
            bool isNextMoveWin = false;
            GameBoard tempGameBoard = null;
            for (int i = 1; i <= this.m_CurrentGameBoard.GetColumnSize(); i++)
            {
                tempGameBoard = m_CurrentGameBoard.CloneGame();
                i_ComputerMove = i;
                if (!tempGameBoard.IsColumnFull(i_ComputerMove - 1))
                {
                    tempGameBoard.InsertToColumn(i_ComputerMove - 1, this.m_IsFirstPlayerTurn);
                    if (tempGameBoard.IsOver())
                    {
                        isNextMoveWin = true;
                        break;
                    }
                }
            }

            return isNextMoveWin;
        }

        public bool IsGameOver()
        {
            return this.m_CurrentGameBoard.IsOver();
        }
    }
}
