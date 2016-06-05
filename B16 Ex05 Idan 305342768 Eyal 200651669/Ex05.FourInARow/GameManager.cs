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

        public GameManager(string i_FirstPlayerName,
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
    }
}
