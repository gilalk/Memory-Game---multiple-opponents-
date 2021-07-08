using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            // creating list of opponents for the game

            Console.WriteLine("Enter number of opponents: ");
            int n = int.Parse(Console.ReadLine());
            string[] opponents = Opponents(n);

            // choosing random member to choose size of game board

            Random rn = new Random();
            int chooser = rn.Next(1, n);
            int[] points = new int[n];  // counting user points
            int totalPoints = 0;
            int mxVal;


            // player will insert number and create matrix and cards
            Console.WriteLine($"{opponents[chooser]} Enter even number between 2-8: ");
            int num = int.Parse(Console.ReadLine());

            int[,] matrix = CreateMatrix(num);

            int[,] myGameMat = FillMatrix(matrix);

            int numOfCouples = (num * num) / 2;

            // create the board matrix of 'X'
            object[,] tableMat = new object[num, num];
            for (int j = 0; j < tableMat.GetLength(0); j++)
            {
                for (int k = 0; k < tableMat.GetLength(1); k++)
                {
                    tableMat[j, k] = 'X';
                }
            }

            // few variables to use later
            int[] Rows = new int[2];  // keep the chosen rows
            int[] Cols = new int[2];  // keep the chosen cols
            int[] choice = new int[2];  // keep the value in chosen place
            int i = 0;  // variable for the loop
            bool weAreOn = true; // boolian variable for main loop
            bool myTurn = true;

            while (weAreOn)
            {
                for (int j = 0; j < opponents.Length; j++)
                {
                    myTurn = true;

                    while (myTurn)
                    {
                        Console.WriteLine($"It's {opponents[j]}'s turn");
                        while (i < 2 && totalPoints < numOfCouples)
                        {
                            Console.WriteLine("Choose row: ");
                            Rows[i] = int.Parse(Console.ReadLine());
                            Console.WriteLine("Choose col: ");
                            Cols[i] = int.Parse(Console.ReadLine());

                            if (ChoosingCards(Rows[i], Cols[i], myGameMat) == 777)
                            {
                                Console.WriteLine("This place does not exist, try again!");
                                continue;
                            }
                            else if (ChoosingCards(Rows[i], Cols[i], myGameMat) == 0)
                            {
                                Console.WriteLine("This card has been taken! try again");
                                continue;
                            }
                            else
                            {
                                choice[i] = ChoosingCards(Rows[i], Cols[i], myGameMat);
                                tableMat[Rows[i], Cols[i]] = choice[i];
                                PrintTableMat(tableMat);
                                i++;
                            }
                        }
                        i = 0;
                        if (totalPoints >= numOfCouples)  // check if the game ended
                        {
                            myTurn = false;
                            weAreOn = false;
                        }

                        if (choice[0] == choice[1])  // check if the opponent was succeed
                        {
                            Console.WriteLine("Excellent! 1 point!");
                            points[j]++;
                            totalPoints += points[j];
                            myGameMat[Rows[0], Cols[0]] = 0;
                            myGameMat[Rows[1], Cols[1]] = 0;
                            tableMat[Rows[0], Cols[0]] = 0;
                            tableMat[Rows[1], Cols[1]] = 0;
                            PrintTableMat(tableMat);
                        }
                        else
                        {
                            Console.WriteLine("Almost!\nAgain..");
                            tableMat[Rows[0], Cols[0]] = 'X';
                            tableMat[Rows[1], Cols[1]] = 'X';
                            PrintTableMat(tableMat);
                            myTurn = false;
                        }
                    }
                    Console.WriteLine("Press enter to continue..");
                    Console.ReadLine();
                }
            }

            // announce on the winner

            mxVal = FindTheBiggest(points);

            int index = Array.IndexOf(points, mxVal);
            string winner = opponents[index];
            Console.WriteLine($"The winner is:\n{winner}!!\nHooray!!");
        }


        #region all functions
        /// <summary>
        /// Create array with opponents names
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string[] Opponents(int num)
        {
            string[] opp = new string[num];

            for (int i = 0; i < num; i++)
            {
                Console.WriteLine($"Enter opponent number {i + 1} name: ");
                opp[i] = Console.ReadLine();
            }
            return opp;
        }


        /// <summary>
        /// Fill matrix with random couple numbers between 1 to num
        /// </summary>
        /// <param name="num"></param>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static int[,] FillMatrix(int[,] mat)
        {
            Random rndCards = new Random();
            int num = mat.GetLength(0);
            int counter = 0;
            int couples = (num * num) / 2;
            for (int i = 1; i <= couples; i++)
            {
                while (counter < 2)
                {
                    int row = rndCards.Next(num);
                    int col = rndCards.Next(num);
                    if (mat[row, col] == 0)
                    {
                        mat[row, col] = i;
                        counter++;
                    }
                }
                counter = 0;
            }

            return mat;
        }


        /// <summary>
        /// Create matrix of 'X' just for being the game table
        /// </summary>
        /// <param name="num"></param>
        /// <returns> 'X' matrix </returns>
        public static void PrintTableMat(object[,] tableMat)
        {

            for (int i = 0; i < tableMat.GetLength(0); i++)
            {
                for (int j = 0; j < tableMat.GetLength(1); j++)
                {
                    Console.Write(tableMat[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }


        /// <summary>
        /// create matrix (num x num) without values
        /// </summary>
        /// <param name="num"></param>
        /// <returns> matrix (num x num) </returns>
        public static int[,] CreateMatrix(int num)
        {
            int[,] matrix;

            while (true)
            {
                if (num > 0 && num <= 8 && num % 2 == 0)
                {
                    matrix = new int[num, num];
                    break;
                }
            }
            return matrix;

        }

        /// <summary>
        /// insert row, col and matrix and get value in this place
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="mat"></param>
        /// <returns> value by index </returns>
        public static int ChoosingCards(int row, int col, int[,] mat)
        {

            int lim = mat.GetLength(0);

            if (row >= lim || col >= lim)
            {
                return 777;
            }
            else
            {
                return mat[row, col];
            }

        }

        public static int FindTheBiggest(int[] arr)
        {
            int maxi = 0;
            for (int k = 0; k < arr.Length; k++)
            {
                if (arr[k] > maxi)
                {
                    maxi = arr[k];
                }
            }
            return maxi;
        }


        #endregion
    }
}
