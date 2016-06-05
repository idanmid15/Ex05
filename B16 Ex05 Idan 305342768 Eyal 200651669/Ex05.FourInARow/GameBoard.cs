namespace Ex05.FourInARow
{
    using System;
    using System.Text;

    public class GameBoard
    {
        public enum eWinner
        {
            FirstPlayer,
            SecondPlayer,
            NoWinner
        }

        private enum eGameSigns
        {
            FirstPlayer,
            SecondPlayer,
            Empty
        }

        private readonly char r_FirstPlayerSign = 'O';
        private readonly char r_SecondPlayerSign = 'X';
        private eWinner m_LastWinner;
        private eGameSigns[,] m_Board;
        private int[] m_ColumnTops;
        private int m_NumOfCellsLeft;

        public GameBoard(int i_NumOfRows, int i_NumOfColumns)
        {
            m_Board = new eGameSigns[i_NumOfRows, i_NumOfColumns];
            m_ColumnTops = new int[i_NumOfColumns];
            m_NumOfCellsLeft = i_NumOfRows * i_NumOfColumns;
            for (int i = 0; i < i_NumOfRows; i++)
            {
                for (int j = 0; j < i_NumOfColumns; j++)
                {
                    m_Board[i, j] = eGameSigns.Empty;
                }
            }

            for (int i = 0; i < i_NumOfColumns; i++)
            {
                m_ColumnTops[i] = i_NumOfRows - 1;
            }
        }

        public GameBoard CloneGame()
        {
            GameBoard newGameBoard = new GameBoard(this.m_Board.GetLength(0), this.m_Board.GetLength(1));
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    newGameBoard.m_Board[i, j] = m_Board[i, j];
                }
            }

            for (int i = 0; i < this.m_ColumnTops.Length; i++)
            {
                newGameBoard.m_ColumnTops[i] = this.m_ColumnTops[i];
            }

            newGameBoard.m_NumOfCellsLeft = this.m_NumOfCellsLeft;
            newGameBoard.m_LastWinner = this.m_LastWinner;
            return newGameBoard;
        }

        public int GetColumnSize()
        {
            return m_Board.GetLength(1);
        }

        public int GetRowSize()
        {
            return m_Board.GetLength(0);
        }

        public char GetCell(int i_Row, int i_Col)
        {
            char toReturn = ' ';
            switch(m_Board[i_Row, i_Col])
            {
                case eGameSigns.FirstPlayer:
                    toReturn = r_FirstPlayerSign;
                    break;
                case eGameSigns.SecondPlayer:
                    toReturn = r_SecondPlayerSign;
                    break;
            }

            return toReturn;
        }

        public int GetColumnTop(int i_IndexOfColumn)
        {
            return m_ColumnTops[i_IndexOfColumn];
        }

        public void InitiaizeNewBoard()
        {
            int heightOfMatrix = m_Board.GetLength(0) - 1;
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    m_Board[i, j] = eGameSigns.Empty;
                }
            }

            for (int i = 0; i < m_ColumnTops.Length; i++)
            {
                m_ColumnTops[i] = heightOfMatrix;
            }

            m_NumOfCellsLeft = m_Board.GetLength(0) * m_Board.GetLength(1);
        }

        public void InsertToColumn(int i_Column, bool i_Sign)
        {
            m_Board[m_ColumnTops[i_Column], i_Column] = i_Sign ? eGameSigns.FirstPlayer : eGameSigns.SecondPlayer;
            m_ColumnTops[i_Column]--;
            m_NumOfCellsLeft--;
        }

        public string ToMatrixString()
        {
            StringBuilder boardString = new StringBuilder();
            boardString.Append("  ");
            for (int j = 0; j < m_Board.GetLength(1); j++)
            {
                boardString.Append(string.Format("{0}   ", j + 1));
            }

            boardString.Append(System.Environment.NewLine);
            for (int i = 0; i < (m_Board.GetLength(0) * 2); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    if (i % 2 == 0)
                    {
                        boardString.Append(string.Format("| {0} ", parseEnumToPlayerSign(m_Board[i / 2, j])));
                    }
                    else
                    {
                        boardString.Append("====");
                    }

                    if (j == m_Board.GetLength(1) - 1)
                    {
                        boardString.Append((i % 2 == 0) ? "|" : "=");
                    }
                }

                boardString.Append(System.Environment.NewLine);
            }

            return boardString.ToString();
        }

        private char parseEnumToPlayerSign(eGameSigns i_EGameSign)
        {
            char toReturn = ' ';
            switch (i_EGameSign)
            {
                case eGameSigns.FirstPlayer:
                    toReturn = r_FirstPlayerSign;
                    break;
                case eGameSigns.SecondPlayer:
                    toReturn = r_SecondPlayerSign;
                    break;
            }

            return toReturn;
        }

        public eWinner GetCurrentWinner()
        {
            return m_LastWinner;
        }

        public bool IsColumnFull(int i_Column)
        {
            return m_ColumnTops[i_Column] == -1;
        }

        private bool isFourInRow()
        {
            int countFourInARow = 0;
            bool foundFour = false;
            eGameSigns victorySign = eGameSigns.Empty;
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1) - 1; j++)
                {
                    if (m_Board[i, j] == m_Board[i, j + 1] && m_Board[i, j] != eGameSigns.Empty)
                    {
                        countFourInARow++;
                    }
                    else
                    {
                        countFourInARow = 0;
                    }

                    if (checkIfFourAndUpdateVictorySign(countFourInARow, ref foundFour, ref victorySign, m_Board[i, j]))
                    {
                        break;
                    }
                }

                if (foundFour)
                {
                    break;
                }

                countFourInARow = 0;
            }

            return updateWinnerIfFound(foundFour, victorySign);
        }

        private bool checkIfFourAndUpdateVictorySign(
            int i_CountFourInARow,
                ref bool io_FoundFour,
                    ref eGameSigns io_VictorySign,
                        eGameSigns i_CurrentSign)
        {
            /// Means we have 3 pairs
            if (i_CountFourInARow == 3)
            {
                io_FoundFour = true;
                io_VictorySign = i_CurrentSign;
            }

            return io_FoundFour;
        }

        private bool updateWinnerIfFound(bool i_FoundFour, eGameSigns i_VictorySign)
        {
            if (i_FoundFour)
            {
                m_LastWinner = i_VictorySign == eGameSigns.FirstPlayer ? eWinner.FirstPlayer : eWinner.SecondPlayer;
            }

            return i_FoundFour;
        }

        private bool isFourInColumn()
        {
            int countFourInAColumn = 0;
            bool foundFour = false;
            eGameSigns victorySign = eGameSigns.Empty;
            for (int i = 0; i < m_Board.GetLength(1); i++)
            {
                for (int j = 0; j < m_Board.GetLength(0) - 1; j++)
                {
                    if (m_Board[j, i] == m_Board[j + 1, i] && m_Board[j, i] != eGameSigns.Empty)
                    {
                        countFourInAColumn++;
                    }
                    else
                    {
                        countFourInAColumn = 0;
                    }

                    if (checkIfFourAndUpdateVictorySign(countFourInAColumn, ref foundFour, ref victorySign, m_Board[j, i]))
                    {
                        break;
                    }
                }

                if (foundFour)
                {
                    break;
                }

                countFourInAColumn = 0;
            }

            return updateWinnerIfFound(foundFour, victorySign);
        }

        private bool isFourInDiagnol()
        {
            int countFourInDiagnol = 0;
            bool foundFour = false;
            const bool v_IsDownward = true;
            eGameSigns victorySign = eGameSigns.Empty;
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    /// Down Diagnol
                    eGameSigns currentDiagnolSign = m_Board[i, j];
                    checkDiagnol(ref countFourInDiagnol, ref foundFour, ref victorySign, i, j, currentDiagnolSign, v_IsDownward);
                    /// Up Diagnol
                    checkDiagnol(ref countFourInDiagnol, ref foundFour, ref victorySign, i, j, currentDiagnolSign, !v_IsDownward);
                    if (foundFour)
                    {
                        break;
                    }
                }

                if (foundFour)
                {
                    break;
                }
            }

            return updateWinnerIfFound(foundFour, victorySign);
        }

        private void checkDiagnol(
            ref int io_CountFourInDiagnol,
                ref bool io_FoundFour,
                    ref eGameSigns io_VictorySign,
                        int i_IndexI,
                            int i_IndexJ,
                                eGameSigns i_CurrentDiagnolSign,
                                    bool i_IsDownDiagnol)
        {
            int rowToCheck = 0;
            for (int k = 1; k < 4; k++)
            {
                rowToCheck = i_IndexI + (i_IsDownDiagnol ? k : -k);
                /// Check if out of bounds
                if (rowToCheck > m_Board.GetLength(0) - 1 || i_IndexJ + k > m_Board.GetLength(1) - 1 || rowToCheck < 0)
                {
                    io_CountFourInDiagnol = 0;
                    break;
                }
                else if (m_Board[rowToCheck, i_IndexJ + k] == i_CurrentDiagnolSign && i_CurrentDiagnolSign != eGameSigns.Empty)
                {
                    io_CountFourInDiagnol++;
                }
                else
                {
                    io_CountFourInDiagnol = 0;
                }

                if (checkIfFourAndUpdateVictorySign(io_CountFourInDiagnol, ref io_FoundFour, ref io_VictorySign, i_CurrentDiagnolSign))
                {
                    break;
                }
            }
        }

        private bool isFull()
        {
            return m_NumOfCellsLeft == 0;
        }

        private bool isThereAWinner()
        {
            return isFourInColumn() || isFourInRow() || isFourInDiagnol();
        }

        public bool IsOver()
        {
            bool fullBoard = isFull();
            bool foundWinner = isThereAWinner();
            if (!foundWinner)
            {
                m_LastWinner = eWinner.NoWinner;
            }

            return foundWinner || fullBoard;
        }
    }
}