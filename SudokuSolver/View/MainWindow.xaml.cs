/*
 * CSCI 641 Project 3
 * @author  Ashwani Kumar (ak8647)
 */
 
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SudokuSolver.ViewModel;
using Microsoft.Win32;

namespace SudokuSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string difficultyLevel;
        private SudokuViewModel viewModel;
        //private static TextBox revealCell;
        private static Boolean revealFlag = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Iterates through each cell of the sudokuBoard grid. Finds each textboxe and compares
        /// its value with the corresponding value in the sudoku array. If the puzzle is solved, 
        /// then displays a message box saying the puzzle has been solved.
        /// </summary>
        /// <param name="sender">Validate button</param>
        /// <param name="e">RoutedEventArgs object</param>
        private void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            revealFlag = false;
            bool isSolved = true;
            for (int i = 0; i < 81; i++)
            {
                Border obj = sudokuBoard.Children[i] as Border;
                if (obj.Child is TextBox)
                {
                    TextBox child = obj.Child as TextBox;
                    int x = Int32.Parse(child.Uid.Split(',')[0]);
                    int y = Int32.Parse(child.Uid.Split(',')[1]);

                    int val = 0;
                    if (child.Text != "")
                    {
                        val = Int32.Parse(child.Text);
                    }

                    if (val != viewModel.Sudoku[x, y])
                    {
                        isSolved = false;
                        child.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 224));
                    }
                    else
                    {
                        child.Background = new SolidColorBrush(Color.FromArgb(255, 34, 139, 34));
                    }
                }
            }


            if (isSolved)
            {
                MessageBox.Show("Puzzle solved!!");
            }
        }

        /// <summary>
        /// Populates the each cell in the sudokuBoard grid with a dynamically created TextBox or Label control. 
        /// Populates text value of Label control using sudokuGame array. Clears children of sudokuBoard grid
        /// if they already exist.
        /// 
        /// Referred youtube link - https://www.youtube.com/watch?v=eiGOF0roB18 for dynamic creation of controls 
        /// like label and textbox inside the grid control.
        /// </summary>
        /// <param name="sender">New Puzzle button control</param>
        /// <param name="e">RoutedEventArgs object</param>
        private void BtnNewPuzzle_Click(object sender, RoutedEventArgs e)
        {
            revealFlag = false;
            ComboBoxItem currentItem = (ComboBoxItem)combodifficultylevel.SelectedItem;
            difficultyLevel = currentItem.Content.ToString();

            sudokuBoard.Children.Clear();

            viewModel = new SudokuViewModel(difficultyLevel);


            int i = 0, j;
            foreach (var row in sudokuBoard.RowDefinitions)
            {
                j = 0;
                foreach (var col in sudokuBoard.ColumnDefinitions)
                {
                    Border br = new Border();
                    Grid.SetRow(br, i);
                    Grid.SetColumn(br, j);

                    br.Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                    if (viewModel.SudokuGame[i, j] == 0)
                    {
                        TextBox textbox = new TextBox
                        {
                            Uid = i + "," + j,
                            Width = 20,
                            Margin = new Thickness(0, 5, 0, 5),
                            MaxLength = 1,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                        };
                        textbox.PreviewTextInput += ValidateNumber;
                        textbox.PreviewMouseLeftButtonDown += TextBoxClickEvent;
                        br.Child = textbox;
                    }
                    else
                    {
                        Label label = new Label
                        {
                            Content = viewModel.SudokuGame[i, j],
                            Uid = i + "," + j,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        br.Child = label;
                    }
                    br.BorderBrush = Brushes.Black;
                    sudokuBoard.Children.Add(br);
                    j++;
                }
                i++;
            }
            BeautifySudoku();
            sudokuBorder.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
            btnReveal.IsEnabled = true;
            btnValidate.IsEnabled = true;
        }

        /// <summary>
        /// Beautifies the sudokuBoard grid so that each inner region of the board
        /// can get highlighted. Adds thickness to the borders of each cell following
        /// a pattern.
        /// </summary>
        private void BeautifySudoku() {
            int childIndex = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Border br = sudokuBoard.Children[childIndex] as Border;
                    if (i % 3 == 2 && j % 3 == 2)
                    {
                        br.BorderThickness = new Thickness(0, 0, 2, 2);
                    }
                    else if (i % 3 == 2)
                    {
                        br.BorderThickness = new Thickness(0, 0, 0, 2);
                    }
                    else if (j % 3 == 2) {
                        br.BorderThickness = new Thickness(0, 0, 2, 0);
                    }    
                    childIndex += 1;
                }
            }

            for (int i = 0; i < 9; i++) {
                Border br = sudokuBoard.Children[i] as Border;
                double top = br.BorderThickness.Top + 2;
                double left = br.BorderThickness.Left;
                double right = br.BorderThickness.Right;
                double bottom = br.BorderThickness.Bottom;
                br.BorderThickness = new Thickness(left, top, right, bottom);
            }

            for (int i = 0; i < 81; i+= 9)
            {
                Border br = sudokuBoard.Children[i] as Border;
                double top = br.BorderThickness.Top;
                double left = br.BorderThickness.Left + 2;
                double right = br.BorderThickness.Right;
                double bottom = br.BorderThickness.Bottom;
                br.BorderThickness = new Thickness(left, top, right, bottom);
            }
        }
        /// <summary>
        /// Event handler method for PreviewTextInput event of TextBox. The text of 
        /// TextBox is only populated if the given input is a digit ranging from 0 to 9.
        /// </summary>
        /// <param name="sender">TextBox control object</param>
        /// <param name="e">TextCompositionEventArgs object</param>
        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]");
        }
        //public void TextBox_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e) {
        //    revealCell = e.Source as TextBox; ;
        //}

        /// <summary>
        /// Sets the revealFlag.
        /// </summary>
        /// <param name="sender">Reveal Button control</param>
        /// <param name="e">RoutedEventArgs object</param>
        private void BtnReveal_Click(object sender, RoutedEventArgs e)
        {
            revealFlag = true;
        }

        /// <summary>
        /// Event handler method for PreviewMouseLeftButtonDown event of TextBox. If the 
        /// revealFlag is set, then it populates the text of the TextBox on which this event
        /// gets fired. Uses sudoku array value to fill the TextBox with value ranging from 0 to 9.
        /// </summary>
        /// <param name="sender">TextBox control object</param>
        /// <param name="e">TextCompositionEventArgs object</param>
        private void TextBoxClickEvent(object sender, RoutedEventArgs e)
        {
            if (revealFlag)
            {
                TextBox textbox = sender as TextBox;
                int x = Int32.Parse(textbox.Uid.Split(',')[0]);
                int y = Int32.Parse(textbox.Uid.Split(',')[1]);
                textbox.Text = viewModel.Sudoku[x, y].ToString();
                revealFlag = false;
            }
        }
        /// <summary>
        /// Creates a SaveFileDialog object which calls SavePuzzle() method for saving
        /// the sudoku puzzle as a text file.
        /// </summary>
        /// <param name="sender">Save button control</param>
        /// <param name="e">RoutedEventArgs object</param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "SudokuPuzzle",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            //String currDir = Directory.GetCurrentDirectory();
            //dlg.InitialDirectory = currDir;

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                File.WriteAllText(dlg.FileName, SavePuzzle());
            }
        }
        /// <summary>
        /// Saves the sudoku puzzle in a particular format as string object.
        /// </summary>
        /// <returns>String containing the sudoku puzzle</returns>
        private string SavePuzzle()
        {
            string sudokuPuzzleText = "";
            for (int i = 0; i < viewModel.SudokuGame.GetLength(0); i++)
            {
                for (int j = 0; j < viewModel.SudokuGame.GetLength(0); j++)
                {
                    if (viewModel.SudokuGame[i, j] != 0)
                        sudokuPuzzleText += viewModel.SudokuGame[i, j];
                    else
                        sudokuPuzzleText += "X";

                    if (j != 8)
                        sudokuPuzzleText += " ";
                }

                if (i != 8)
                    sudokuPuzzleText += "\n";
            }
            return sudokuPuzzleText;
        }
    }
}
