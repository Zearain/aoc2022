bool IsOnAnyLine(List<Line> lines, Point point)
{
    // Divide the plane into a grid of cells with a given size
    const int CELL_SIZE = 10;
    int xMin = (int)Math.Floor(lines.Min(l => Math.Min(l.Point1.X, l.Point2.X)) / CELL_SIZE) * CELL_SIZE;
    int xMax = (int)Math.Ceiling(lines.Max(l => Math.Max(l.Point1.X, l.Point2.X)) / CELL_SIZE) * CELL_SIZE;
    int yMin = (int)Math.Floor(lines.Min(l => Math.Min(l.Point1.Y, l.Point2.Y)) / CELL_SIZE) * CELL_SIZE;
    int yMax = (int)Math.Ceiling(lines.Max(l => Math.Max(l.Point1.Y, l.Point2.Y)) / CELL_SIZE) * CELL_SIZE;

    // Create a dictionary to store the lines in each cell
    Dictionary<Tuple<int, int>, List<Line>> lineCells = new Dictionary<Tuple<int, int>, List<Line>>();
    foreach (Line line in lines)
    {
        int x1 = (int)(line.Point1.X / CELL_SIZE) * CELL_SIZE;
        int y1 = (int)(line.Point1.Y / CELL_SIZE) * CELL_SIZE;
        int x2 = (int)(line.Point2.X / CELL_SIZE) * CELL_SIZE;
        int y2 = (int)(line.Point2.Y / CELL_SIZE) * CELL_SIZE;
        if (!lineCells.ContainsKey(Tuple.Create(x1, y1)))
        {
            lineCells[Tuple.Create(x1, y1)] = new List<Line>();
        }
        lineCells[Tuple.Create(x1, y1)].Add(line);
        if (!lineCells.ContainsKey(Tuple.Create(x2, y2)))
        {
            lineCells[Tuple.Create(x2, y2)] = new List<Line>();
        }
        lineCells[Tuple.Create(x2, y2)].Add(line);
    }

    // Check the lines in the cell that contains the point
    int x = (int)(point.X / CELL_SIZE) * CELL_SIZE;
    int y = (int)(point.Y / CELL_SIZE) * CELL_SIZE;
    if (lineCells.ContainsKey(Tuple.Create(x, y)))
    {
        return lineCells[Tuple.Create(x, y)].Any(line => IsOnLine(line.Point1, line.Point2, point));
    }
    return false;
}
