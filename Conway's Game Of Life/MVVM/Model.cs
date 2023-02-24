using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Conway_s_Game_Of_Life
{
    class Model
    {

        private int[,] _newBoard;

        public Model(int x, int y)
        {
            _newBoard = new int[x, y];
        }

        public void InitNewBoard(int x, int y)
        {
            _newBoard = new int[x, y];
            Array.Clear(_newBoard, 0, _newBoard.Length);
        }

        public void ClearBoard()
        {
            Array.Clear(_newBoard, 0, _newBoard.Length);
        }

        public void SetBoard(int x, int y, int set)
        {
            _newBoard[x,y] = set;
        }

        public int GetBoardItem(int x, int y)
        {
            return _newBoard[x, y];
        }

        public int[,] GetBoard()
        {
            return _newBoard;
        }

        public int[,] GetStep()
        {
            int[,] oldBoard = (int[,]) _newBoard.Clone();
            Array.Clear(_newBoard, 0, _newBoard.Length);

            for (int i = 0; i < oldBoard.GetLength(0); i++)
            {
                for (int j = 0; j < oldBoard.GetLength(1); j++)
                {
                    if (oldBoard[i, j] == 0)
                    {
                        //currently dead
                        _newBoard[i, j] = CheckIsStillDead(oldBoard, i, j);
                    } else
                    {
                        //currently alive
                        _newBoard[i, j] = CheckIsStillAlive(oldBoard, i, j);
                    }
                }
            }

            return _newBoard;
        }

        private int CheckIsStillDead(int[,] board, int i, int j)
        {
            int neighborCount = GetNeighborCount(board, i, j);
            if (neighborCount == 3)
                return 1;

            return 0;
        }

        private int CheckIsStillAlive(int[,] board, int i, int j)
        {
            int neighborCount = GetNeighborCount(board, i, j);
            if (neighborCount == 2 || neighborCount == 3)
                return 1;

            return 0;
        }

        private int GetNeighborCount(int[,] board, int i, int j)
        {
            int count = 0;
            count += (j + 1 < board.GetLength(1)) ? board[i, j + 1] : 0;
            count += (j - 1 > 0) ? board[i, j - 1] : 0;
            count += (i + 1 < board.GetLength(0) && j + 1 < board.GetLength(1)) ? board[i + 1, j + 1] : 0;
            count += (i + 1 < board.GetLength(0) && j - 1 > 0) ? board[i + 1, j - 1] : 0;
            count += (i - 1 > 0 && j + 1 < board.GetLength(1)) ? board[i - 1, j + 1] : 0;
            count += (i - 1 > 0 && j - 1 > 0) ? board[i - 1, j - 1] : 0;
            count += (i + 1 < board.GetLength(0)) ? board[i + 1, j] : 0;
            count += (i - 1 > 0) ? board[i - 1, j] : 0;

            return count;
        }

        public static void PrintBoard(int[,] board)
        {
            Debug.Write("\n");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Debug.Write(board[i, j] + " ");
                }
                Debug.Write("\n");
            }
            Debug.Write("\n");
        }
    }
}
