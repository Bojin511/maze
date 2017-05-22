using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace maze
{

	public class Maze
	{
		private Node[][] mazeMatrix;
		private string inputFile;
		private Dimension startPoint, endPoint, mazeDimension;
		private bool mazeSolved;

		public enum NodeType
		{
			NODE_START,
			NODE_END,
			NODE_PASSAGE,
			NODE_PATH,
			NODE_WALL
		}

		private const string STRING_WALL = "#";
		private const string STRING_START = "S";
		private const string STRING_PATH = "X";
		private const string STRING_PASSAGE = " ";
		private const string STRING_END = "E";
		private const string STRING_NO_SOLUTION = "No solution is possible";



		/// <summary>
		/// Class which models field in the maze.
		/// </summary>
		public class Node
		{
			private readonly Maze outerInstance;

			public int distance;
			public Node parent;
			public Maze.NodeType type;
			internal Dimension position = new Dimension();

			public Node(Maze outerInstance, NodeType type)
			{
				this.outerInstance = outerInstance;
				this.type = type;

				distance = -1;
				parent = null;
			}

			public virtual int Width
			{
				set
				{
					this.position.width = value;
				}
				get
				{
					return this.position.width;
				}
			}


			public virtual int Height
			{
				set
				{
					this.position.height = value;
				}
				get
				{
					return this.position.height;
				}
			}

		}


		/// <summary>
		/// Consume input file and process it. </summary>
		/// <exception cref="IOException"> </exception>
		public virtual void consumeInput()
		{
			System.IO.StreamReader reader = new System.IO.StreamReader(inputFile);

			int lineNo = 0;
			int matrixLine = 0;
			string line = reader.ReadLine();

			while (!string.ReferenceEquals(line, null))
			{
				if (lineNo == 0)
				{
					MazeDimension = line;
				}
				else if (lineNo == 1)
				{
					StartPoint = line;
				}
				else if (lineNo == 2)
				{
					EndPoint = line;
				}
				else
				{
					lineToNode(line, matrixLine);
					matrixLine++;
				}

				line = reader.ReadLine();
				lineNo++;
			}

			reader.Close();
		}


		public virtual string InputFile
		{
			set
			{
				this.inputFile = value;
			}
		}


		/// <summary>
		/// Consume input and set start point coordinates. </summary>
		/// <param name="startLine"> </param>
		private string StartPoint
		{
			set
			{
				string[] dimension = value.Split("\\s+", true);
				// height x width
				startPoint = new Dimension(int.Parse(dimension[1]), int.Parse(dimension[0]));
			}
		}


		/// <summary>
		///  Consume input and set end point coordinates. </summary>
		/// <param name="endLine"> </param>
		private string EndPoint
		{
			set
			{
				string[] dimension = value.Split("\\s+", true);
				// height x width:
				endPoint = new Dimension(int.Parse(dimension[1]), int.Parse(dimension[0]));
			}
		}


		/// <summary>
		///  Consume input line and set maze dimension. </summary>
		/// <param name="line"> </param>
		private string MazeDimension
		{
			set
			{
				string[] dimension = value.Split("\\s+", true);
				// height x width
				mazeDimension = new Dimension(int.Parse(dimension[1]), int.Parse(dimension[0]));
    
				mazeMatrix = RectangularArrays.ReturnRectangularNodeArray(mazeDimension.height, mazeDimension.width);
			}
		}


		/// <summary>
		/// Consume input line of maze matrix and convert it to nodes.
		/// </summary>
		/// <param name="line"> </param>
		/// <param name="matrixLine"> </param>
		private void lineToNode(string line, int matrixLine)
		{
			if (matrixLine < mazeDimension.height)
			{
				string[] nodesLine = line.Split("\\s+", true);

				for (int matrixColumn = 0; matrixColumn < nodesLine.Length; matrixColumn++)
				{
					Node node;
					Dimension currentPosition = new Dimension(matrixColumn, matrixLine);

					if (currentPosition.Equals(startPoint))
					{
						node = new Node(this, NodeType.NODE_START);
					}
					else if (currentPosition.Equals(endPoint))
					{
						node = new Node(this, NodeType.NODE_END);
					}
					else
					{
						int matrixValue = int.Parse(nodesLine[matrixColumn]);

						switch (matrixValue)
						{
							case 0:
								node = new Node(this, NodeType.NODE_PASSAGE);
								break;
							case 1:
						default:
								node = new Node(this, NodeType.NODE_WALL);
							break;
						}
					}

					node.Height = matrixLine;
					node.Width = matrixColumn;
					mazeMatrix[matrixLine][matrixColumn] = node;
				}
			}
		}


		/// <summary>
		/// Find End node using Breadth First Search Algorithm.
		/// </summary>
		public virtual void breadthFirstSearch()
		{
			LinkedList<Node> queue = new LinkedList<Node>();
			Node start = mazeMatrix[startPoint.height][startPoint.width];

			start.distance = 0;
			queue.AddLast(start);

			while (queue.Count > 0)
			{
                Node currentNode = queue.First();
                queue.RemoveFirst();

				IList<Node> adjacentNodes = getAdjacentNodes(currentNode);

                foreach (Node adjacentNode in adjacentNodes)
                {
                    if (adjacentNode.distance < 0)
                    {
                        adjacentNode.distance = currentNode.distance + 1;
                        adjacentNode.parent = currentNode;
                        queue.AddLast(adjacentNode);
                        if (adjacentNode.type == NodeType.NODE_END)
                        {
                            queue.Clear();
                            mazeSolved = true;
                            setPath();
                        }
                    }
                }
			}
		}


		/// 
		/// <param name="node"> </param>
		/// <returns> ArrayList of adjacent nodes </returns>
		private IList<Node> getAdjacentNodes(Node node)
		{
			IList<Node> adjacentNodes = new List<Node>();

			// get N node
			if (node.Height > 0)
			{
				Node adjacentNodeN = mazeMatrix[node.Height - 1][node.Width];
				if (adjacentNodeN.type != NodeType.NODE_WALL)
				{
					adjacentNodes.Add(adjacentNodeN);
				}
			}
			// get W node
			if (node.Width < mazeDimension.width - 1)
			{
				Node adjacentNodeW = mazeMatrix[node.Height][node.Width + 1];
				if (adjacentNodeW.type != NodeType.NODE_WALL)
				{
					adjacentNodes.Add(adjacentNodeW);
				}
			}
			// get S node
			if (node.Height < mazeDimension.height - 1)
			{
				Node adjacentNodeS = mazeMatrix[node.Height + 1][node.Width];
				if (adjacentNodeS.type != NodeType.NODE_WALL)
				{
					adjacentNodes.Add(adjacentNodeS);
				}
			}
			// get E node
			if (node.Width > 0)
			{
				Node adjacentNodeE = mazeMatrix[node.Height][node.Width - 1];
				if (adjacentNodeE.type != NodeType.NODE_WALL)
				{
					adjacentNodes.Add(adjacentNodeE);
				}
			}

			return adjacentNodes;
		}


		private void setPath()
		{
			Node node = mazeMatrix[endPoint.height][endPoint.width];

			while (node.parent.type != NodeType.NODE_START)
			{
				if (node.parent.type == NodeType.NODE_PASSAGE)
				{
					node.parent.type = NodeType.NODE_PATH;
					node = node.parent;
				}
			}
		}


		/// <summary>
		/// Outputs solution to stdout.
		/// </summary>
		public virtual void printSolvedMaze()
		{
			if (mazeSolved)
			{
                foreach (Node[] nodeLine in mazeMatrix)
                {
                    string line = "";

                    foreach (Node node in nodeLine)
                    {
                        switch (node.type)
                        {
                            case maze.Maze.NodeType.NODE_WALL:
                                line += STRING_WALL;
                                break;
                            case maze.Maze.NodeType.NODE_START:
                                line += STRING_START;
                                break;
                            case maze.Maze.NodeType.NODE_PATH:
                                line += STRING_PATH;
                                break;
                            case maze.Maze.NodeType.NODE_PASSAGE:
                                line += STRING_PASSAGE;
                                break;
                            case maze.Maze.NodeType.NODE_END:
                                line += STRING_END;
                                break;
                            default:
                                break;
                        }
                    }

                    var fileInfo = new System.IO.FileInfo(inputFile);
                    string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    string path = string.Format(@"c:\users\bojin\documents\visual studio 2010\Projects\maze\maze\output\{0}-{1}.txt",fileName, DateTime.Now.ToString("yyyyMMdd-hhMMss"));

                    string appendText = line + Environment.NewLine;
                    File.AppendAllText(path, appendText, Encoding.UTF8);

                    Console.WriteLine(line);

                }
			}
			else
			{
				Console.WriteLine(STRING_NO_SOLUTION);
			}
		}
	}

}