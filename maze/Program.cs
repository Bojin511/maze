using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace maze
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                long startTime = DateTimeHelperClass.CurrentUnixTimeMillis();

                Maze maze = new Maze();
                maze.InputFile = arg;
                maze.consumeInput();
                maze.breadthFirstSearch();
                maze.printSolvedMaze();

                long endTime = DateTimeHelperClass.CurrentUnixTimeMillis();
                long totalTime = endTime - startTime;
                Console.WriteLine("total time: " + totalTime + "ms");
            }

            // Pause to read console output
            Console.ReadLine();
        }
    }
}
