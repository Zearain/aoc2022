using System.Collections.Concurrent;
using System.Numerics;

namespace AwesomeSolver.Core.Solvers.Day12;

public sealed class HeightNode
{
    public HeightNode(float x, float y, int height)
    {
        Position = new(x, y);
        Height = height;
    }

    public Vector2 Position { get; private set; }
    public int Height { get; private set; }

    public float FScore { get; set; }
    public int GScore { get; set; }
    public float HScore { get; set; }

    public HeightNode? Parent { get; set; }

    public bool IsWalkableFrom(HeightNode node)
    {
        return !(Math.Abs(node.Position.X - Position.X) > 1 ||
            Math.Abs(node.Position.Y - Position.Y) > 1 ||
            Height - node.Height > 1);
    }
}

public sealed class HeightMap
{
    private readonly int[][] heights;
    private readonly (int x, int y) startPoint;
    private readonly (int x, int y) endPoint;

    public HeightMap(IEnumerable<string> inputLines)
    {
        var mapChar = inputLines.Select(x => x.ToArray()).ToArray();
        var mapGrid = new List<int[]>();
        for (int y = 0; y < mapChar.Length; y++)
        {
            var row = new List<int>();
            for (int x = 0; x < mapChar[y].Length; x++)
            {
                var heightChar = mapChar[y][x];

                if (heightChar == 'S')
                {
                    startPoint = (x, y);
                    heightChar = 'a';
                }

                if (heightChar == 'E')
                {
                    endPoint = (x, y);
                    heightChar = 'z';
                }

                var height = (int)heightChar % 32;
                row.Add(height);
            }
            mapGrid.Add(row.ToArray());
        }
        heights = mapGrid.ToArray();
    }

    private HeightNode[][] HeightNodeMap => heights.Select((row, y) => row.Select((height, x) => new HeightNode(x, y, height)).ToArray()).ToArray();

    public IEnumerable<HeightNode> FindDefaultPath()
    {
        return FindPath(startPoint.x, startPoint.y, endPoint.x, endPoint.y) ?? throw new Exception("Unable to find path");
    }

    public IEnumerable<HeightNode> FindShortestLowToEndPath()
    {
        var startPoints = new List<(int x, int y)>();
        for (int y = 0; y < heights.Length; y++)
        {
            for (int x = 0; x < heights[y].Length; x++)
            {
                if (heights[y][x] == 1) startPoints.Add((x, y));
            }
        }

        var paths = new ConcurrentBag<IEnumerable<HeightNode>>();
        Parallel.ForEach(startPoints, point => {
            var path = FindPath(point.x, point.y, endPoint.x, endPoint.y);
            if (path is {}) paths.Add(path);
        });

        return paths.OrderBy(x => x.Count()).First();
    }

    public IEnumerable<HeightNode>? FindPath(int startX, int startY, int endX, int endY)
    {
        var map = HeightNodeMap;
        var startNode = map[startY][startX];
        var endNode = map[endY][endX];

        var openSet = new PriorityQueue<HeightNode, float>();
        var closedSet = new List<HeightNode>();
        int gScore = 0;
        var currentNode = startNode;

        openSet.Enqueue(startNode, startNode.FScore);
        while(openSet.Count > 0)
        {
            currentNode = openSet.Dequeue();
            closedSet.Add(currentNode);

            if (closedSet.Exists(x => x.Position == endNode.Position))
            {
                break;
            }

            var adjecentNodes = GetWalkableAdjecentNodes(map, currentNode);
            gScore++;
            foreach(var adjecent in adjecentNodes)
            {
                if (closedSet.Exists(x => x.Position == adjecent.Position))
                {
                    continue;
                }

                if (!openSet.UnorderedItems.Any(x => x.Element.Position == adjecent.Position))
                {
                    adjecent.GScore = gScore;
                    adjecent.HScore = Math.Abs(endNode.Position.X - adjecent.Position.X) + Math.Abs(endNode.Position.Y - adjecent.Position.Y);
                    adjecent.FScore = adjecent.GScore + adjecent.HScore;
                    adjecent.Parent = currentNode;

                    openSet.Enqueue(adjecent, adjecent.FScore);
                    continue;
                }

                if (gScore + adjecent.HScore < adjecent.FScore)
                {
                    adjecent.GScore = gScore;
                    adjecent.FScore = adjecent.GScore + adjecent.HScore;
                    adjecent.Parent = currentNode;
                }
            }
        }

        if (!closedSet.Exists(x => x.Position == endNode.Position))
        {
            return null;
        }

        var path = new Stack<HeightNode>();
        var temp = currentNode;
        do
        {
            path.Push(temp);
            temp = temp.Parent;
        } while(temp is not null);

        return path.ToArray();
    }

    private IEnumerable<HeightNode> GetWalkableAdjecentNodes(HeightNode[][] map, HeightNode node)
    {
        var tmp = map.SelectMany(x => x).Where(x => {
            var xDist = Math.Abs(node.Position.X - x.Position.X);
            var yDist = Math.Abs(node.Position.Y - x.Position.Y);

            return ((xDist == 1f && yDist == 0f) || (xDist == 0f && yDist == 1f)) && 
                x.Height - node.Height < 2;
        }).ToArray();
        return  tmp.Length <= 4 ? tmp : throw new ArgumentOutOfRangeException(nameof(node));
    }
}