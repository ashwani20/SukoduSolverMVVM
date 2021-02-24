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
    /// This is a singleton class which creates a single instance of Sudoku board
    /// </summary>
    class SingletonSudoku
    {
        private static int[,] sudokuBoard = null;
        private static int size;
        private SingletonSudoku() { }
        /// <summary>
        /// Property for size of sudokuBoard
        /// </summary>
        public static int Size { get => size; set => size = value; }

        /// <summary>
        /// This is a static method which creates only 1 instance of 2D Sudoku board.
        /// When called, it first checks if a 2D int sudokuBoard array exists. It it
        /// does, then return the existing 2D array object. Otherwise, it creates a
        /// new instance only 1 time.
        /// </summary>
        /// <returns>static instance of a 2D Sudoku board which of type int</returns>
        public static int[,] GetSingletonSudokuBoard()
        {
            if (sudokuBoard == null)
                sudokuBoard = new int[Size, Size];
            return sudokuBoard;
        }
    }
}
