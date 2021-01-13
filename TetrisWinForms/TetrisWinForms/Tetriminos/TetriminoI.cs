using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TetriminoI : Tetrimino
{
    private bool m_vertical;

    public TetriminoI() : base(Color.Cyan)
    {
    }

    public override void FillDefaultBlocks()
    {
        Blocks.AddRange(new Block[] {
            new Block(this, 0, 0),
            new Block(this, 1, 0),
            new Block(this, 2, 0),
            new Block(this, 3, 0)
        });
    }

    public override Size GetDefaultSize()
    {
        return m_vertical ?
            new Size(Block.DefaultSize.Width * 4, Block.DefaultSize.Height) :
            new Size(Block.DefaultSize.Width, Block.DefaultSize.Height * 4);
    }

    public override void TryRotateBlocks(Block[,] grid)
    {
        var vertical = !m_vertical;

        Block[] b = null;

        if (!vertical)
        {
            b = new Block[] {
                new Block(this, 0, 0),
                new Block(this, 1, 0),
                new Block(this, 2, 0),
                new Block(this, 3, 0)
            };
        }
        else
        {
            b = new Block[] {
                new Block(this, 0, 0),
                new Block(this, 0, 1),
                new Block(this, 0, 2),
                new Block(this, 0, 3)
            };
        }

        foreach (var block in b)
        {
            if (!block.CalculDefaultParameters(Blocks[0].ScenePoint, grid, false))
                return;
        }

        m_vertical = vertical;
        ResetBlocks(grid, b);
    }
}