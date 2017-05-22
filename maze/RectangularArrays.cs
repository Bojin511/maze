
internal static class RectangularArrays
{
    internal static maze.Maze.Node[][] ReturnRectangularNodeArray(int size1, int size2)
    {
        maze.Maze.Node[][] newArray = new maze.Maze.Node[size1][];
        for (int array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new maze.Maze.Node[size2];
        }

        return newArray;
    }
}