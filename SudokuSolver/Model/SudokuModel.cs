/*
 * CSCI 641 Project 3
 * @author  Ashwani Kumar (ak8647)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    /// <summary>
    /// The main model class which implements a Sudoku puzzle. It contains the main backtrack
    /// algorithm for generating a random Sudoku puzzle every time. 
    /// 
    /// </summary>
    class SudokuModel
    {
        private int size;
        private string difficultyType;
        private int numOfBlanks;
        private int[,] sudoku;
        private int[,] sudokuGame;

        public int[,] Sudoku { get => sudoku; set => sudoku = value; }
        public int[,] SudokuGame { get => sudokuGame; set => sudokuGame = value; }

        /// <summary>
        /// Constructor for the SudokuModel which takes 2 arguments - size and 
        /// difficulty type.
        /// </summary>
        /// <param name="size">Size of Sudoku</param>
        /// <param name="difficultyType">difficultyType of the generated Sudou Puzzle</param>
        public SudokuModel(int size, string difficultyType)
        {
            this.size = size;
            this.difficultyType = difficultyType;
            SingletonSudoku.Size = size;
            SolveSudoku(0, 0, SingletonSudoku.GetSingletonSudokuBoard());
            Sudoku = SingletonSudoku.GetSingletonSudokuBoard().Clone() as int[,];
            numOfBlanks = GetBlankSpaceCount(difficultyType);
            CreateSudokuPuzzle(SingletonSudoku.GetSingletonSudokuBoard());
            SudokuGame = SingletonSudoku.GetSingletonSudokuBoard();
        }

        /// <summary>
        /// Contains the main backtrack logic for creating a solved sudoku puzzle.
        /// </summary>
        /// <param name="row">row index of Sudoku board</param>
        /// <param name="col">column index of Sudoku board</param>
        /// <param name="sudokuBoard">2D singleton int array of Sudoku board</param>
        /// <returns></returns>
        public Boolean SolveSudoku(int row, int col, int[,] sudokuBoard)
        {
            for (int i = row; i < size; i++, col = 0)
                for (int j = col; j < size; j++)
                    if (sudokuBoard[i, j] == 0)
                    {
                        foreach (int val in PossibleValues())
                        {
                            if (IsValid(i, j, val, sudokuBoard))
                            {
                                sudokuBoard[i, j] = val;
                                if (SolveSudoku(i, j + 1, sudokuBoard))
                                    return true;
                                sudokuBoard[i, j] = 0;
                            }
                        }
                        return false;
                    }
            return true;
        }

        /// <summary>
        /// Generates a random set of values ranging from 1 to
        /// the size of the sudoku board.
        /// </summary>
        /// <returns>a HashSet object containing random 1 to n numbers, where n is 
        /// the size of the sudoku</returns>
        public HashSet<int> PossibleValues()
        {
            HashSet<int> numberSet = new HashSet<int>();
            Random rand = new Random();
            while (numberSet.Count != size)
            {
                numberSet.Add(rand.Next(1, size + 1));
            }
            return numberSet;
        }

        /// <summary>
        /// Checks where a value could be placed at cell - [i,j], where i is row index
        /// and j is col index in the sudoku board
        /// </summary>
        /// <param name="i">i is the row index</param>
        /// <param name="j">j is the column index</param>
        /// <param name="val">value to be place at cell - [i, j]</param>
        /// <param name="sudokuBoard">2D singleton int array of Sudoku board</param>
        /// <returns></returns>
        public Boolean IsValid(int i, int j, int val, int[,] sudokuBoard)
        {
            return CheckRow(i, val, sudokuBoard)
                && CheckCol(j, val, sudokuBoard)
                && CheckRegion(i, j, val, sudokuBoard);
        }

        /// <summary>
        /// Checks whether a value can be placed in a particular row of sudoku.
        /// </summary>
        /// <param name="row">the row index to be checked</param>
        /// <param name="val">value to be placed at the specific row</param>
        /// <param name="sudokuBoard">2D singleton int array of Sudoku board</param>
        /// <returns></returns>
        public Boolean CheckRow(int row, int val, int[,] sudokuBoard)
        {
            for (int col = 0; col < size; col++)
                if (sudokuBoard[row, col] == val)
                    return false;
            return true;
        }

        /// <summary>
        /// Checks whether a value can be placed in a particular column of sudoku.
        /// </summary>
        /// <param name="col">the column index to be checked</param>
        /// <param name="val">value to be placed at the specific column</param>
        /// <param name="sudokuBoard">2D singleton int array of Sudoku board</param>
        /// <returns></returns>
        public Boolean CheckCol(int col, int val, int[,] sudokuBoard)
        {
            for (int row = 0; row < size; row++)
                if (sudokuBoard[row, col] == val)
                    return false;
            return true;
        }

        /// <summary>
        /// Checks where a value could be placed within a particular grid containing the  
        /// cell - [i,j], where i is row index and j is col index in the sudoku board
        /// </summary>
        /// <param name="i">i is the row index</param>
        /// <param name="j">j is the column index</param>
        /// <param name="val">value to be place at cell - [i, j]</param>
        /// <param name="sudokuBoard">2D singleton int array of Sudoku board</param>
        public Boolean CheckRegion(int i, int j, int val, int[,] sudokuBoard)
        {
            int gridSize = (int)Math.Sqrt(size);
            int rowIndex = ((int)i / gridSize) * gridSize;
            int colIndex = ((int)j / gridSize) * gridSize;

            for (int row = rowIndex; row < rowIndex + gridSize; row++)
                for (int col = colIndex; col < colIndex + gridSize; col++)
                    if (sudokuBoard[row, col] == val)
                        return false;
            return true;
        }

        /// <summary>
        /// Calculates the count of blankspaces for generating a Sudoku puzzle based on
        /// the value of difficultyType
        /// </summary>
        /// <param name="difficultyType">difficultyType for puzzle generation</param>
        /// <returns>count of blankspaces in the puzzle</returns>
        public int GetBlankSpaceCount(string difficultyType)
        {
            difficultyType = difficultyType.ToLower();
            return difficultyType == "easy" ? (int)(size * size * 0.4) :
                difficultyType == "medium" ? (int)(size * size * 0.55) : (int)(size * size * 0.75);
        }

        /// <summary>
        /// Creates a sudoku puzzled based on the value of blank spaces count. It randomly 
        /// blanks out cell with the puzzle till numOfBlanks becomes 0.
        /// </summary>
        /// <param name="sudokuBoard">2D singleton int array of Sudoku board</param>
        public void CreateSudokuPuzzle(int[,] sudokuBoard)
        {
            Random rand = new Random();
            for (int count = numOfBlanks; count >= 0;)
            {
                int row = rand.Next(0, size);
                int col = rand.Next(0, size);

                if (sudokuBoard[row, col] != 0)
                {
                    sudokuBoard[row, col] = 0;
                    count--;
                }
            }
        }
    }
}
