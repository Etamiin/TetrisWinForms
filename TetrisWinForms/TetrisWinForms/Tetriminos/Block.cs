using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class Block
{
    public static Size DefaultSize => new Size(25, 25);

    public Point ScenePoint { get; set; }

    public readonly Tetrimino parent;
    public int offsetX { get; set; }
    public int offsetY { get; set; }
    public int gridX { get; set; }
    public int gridY { get; set; }

    public Block(Tetrimino parent, int offsetX, int offsetY)
    {
        this.parent = parent;
        this.offsetX = offsetX;
        this.offsetY = offsetY;
    }

    public bool CalculDefaultParameters(Point defaultPoint, Block[,] grid, bool updateGrid = true)
    {
        ScenePoint = new Point(
            defaultPoint.X + DefaultSize.Width * offsetX,
            defaultPoint.Y + DefaultSize.Height * offsetY);

        var lx = gridX;
        var ly = gridY;
        
        gridX = ScenePoint.X / DefaultSize.Width;
        gridY = ScenePoint.Y / DefaultSize.Height;

        if (gridX < 0 || gridX >= grid.GetLength(0) || gridY < 0 || gridY >= grid.GetLength(1) || grid[gridX, gridY] != null && grid[gridX, gridY].parent != parent)
        {
            gridX = lx;
            gridY = ly;

            return false;
        }

        if (updateGrid)
        {
            grid[lx, ly] = null;
            grid[gridX, gridY] = this;
        }

        return true;
    }
}
