/*
 * CSCI 641 Project 3
 * @author  Ashwani Kumar (ak8647)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Model;

namespace SudokuSolver.ViewModel
{
    class SudokuViewModel
    {
        private int[,] sudoku;
        private int[,] sudokuGame;
        public SudokuViewModel(string difficultyLevel)
        {
            SudokuModel obj = new SudokuModel(9, difficultyLevel);
            Sudoku = obj.Sudoku;
            sudokuGame = obj.SudokuGame;
        }

        public int[,] Sudoku { get => sudoku; set => sudoku = value; }
        public int[,] SudokuGame { get => sudokuGame; set => sudokuGame = value; }
    }
}
