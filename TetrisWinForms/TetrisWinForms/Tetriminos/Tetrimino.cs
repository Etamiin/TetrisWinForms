using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public abstract class Tetrimino
{
    public Color Color { get; set; }
    public readonly List<Block> Blocks;

    public Tetrimino(Color color)
    {
        Color = color;
        Blocks = new List<Block>(4);

        FillDefaultBlocks();
    }

    public abstract void FillDefaultBlocks();
    public abstract Size GetDefaultSize();
    public abstract void TryRotateBlocks(Block[,] grid);

    public bool TryMoveDown(Block[,] grid)
    {
        for (var i = Blocks.Count - 1; i >= 0; i--)
        {
            var block = Blocks[i];
            if (block == null)
                continue;

            var ny = block.gridY + 1;
            if (ny >= grid.GetLength(1) || ny < 0 ||
                block.gridX < 0 || block.gridX >= grid.GetLength(0) ||
                grid[block.gridX, ny] != null && grid[block.gridX, ny].parent != block.parent)
                return false;
        }

        UpdateBlocks(0, 1, grid);
        return true;
    }
    public bool TryMoveSide(int side, Block[,] grid)
    {
        for (var i = Blocks.Count - 1; i >= 0; i--)
        {
            var block = Blocks[i];
            if (block == null)
                continue;

            var nx = block.gridX + side;
            if (nx < 0 || nx >= grid.GetLength(0) || grid[nx, block.gridY] != null && grid[nx, block.gridY].parent != block.parent)
                return false;
        }

        UpdateBlocks(side, 0, grid);
        return true;
    }

    public void UpdateBlocks(int x, int y, Block[,] grid)
    {
        for (var i = Blocks.Count - 1; i >= 0; i--)
        {
            var block = Blocks[i];

            grid[block.gridX, block.gridY] = null;

            if (x != 0)
            {
                var nx = block.gridX + x;

                if (nx >= 0 && nx < grid.GetLength(0))
                    block.gridX = nx;
            }

            if (y != 0)
            {
                var ny = block.gridY + y;

                if (ny >= 0 && ny < grid.GetLength(1))
                    block.gridY = ny;
            }

            grid[block.gridX, block.gridY] = block;

            var gridPoint = new Point(
                block.gridX * Block.DefaultSize.Width,
                block.gridY * Block.DefaultSize.Height);

            block.ScenePoint = gridPoint;
        }
    }
    public void ResetBlocks(Block[,] grid, Block[] newBlocks)
    {
        foreach (var block in Blocks)
        {
            grid[block.gridX, block.gridY] = null;
        }

        Blocks.Clear();

        if (newBlocks != null)
        {
            Blocks.AddRange(newBlocks);

            foreach (var block in Blocks)
            {
                grid[block.gridX, block.gridY] = block;
            }
        }
    }

    protected void ChangeBlockParameters(Block[,] grid, Block block, int offsetX, int offsetY, Point defaultPoint)
    {
        if (block.parent != this)
            return;

        var lox = block.offsetX;
        var loy = block.offsetY;

        block.offsetX = offsetX;
        block.offsetY = offsetY;

        if (!block.CalculDefaultParameters(defaultPoint, grid))
        {
            block.offsetX = lox;
            block.offsetY = loy;
        }
    }
}