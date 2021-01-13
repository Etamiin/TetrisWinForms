using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class TetriminoJ : Tetrimino
{
    private int m_rot = 0;

    public TetriminoJ() : base(Color.Blue)
    {
    }

    public override void FillDefaultBlocks()
    {
        Blocks.AddRange(new Block[] {
            new Block(this, 0, 0),
            new Block(this, 1, 0),
            new Block(this, 2, 0),
            new Block(this, 2, 1)
        });
    }

    public override Size GetDefaultSize()
    {
        return m_rot == 0 || m_rot == -180 ?
            new Size(Block.DefaultSize.Width * 3, Block.DefaultSize.Height * 2) :
            new Size(Block.DefaultSize.Width * 2, Block.DefaultSize.Height * 3);
    }

    public override void TryRotateBlocks(Block[,] grid)
    {
        var rot = m_rot - 90;
        if (rot == -360)
            rot = 0;

        Block[] b = null;

        switch (rot)
        {
            case 0:
                b = new Block[] {
                    new Block(this, 0, 0),
                    new Block(this, 1, 0),
                    new Block(this, 2, 0),
                    new Block(this, 2, 1)
                };
                break;
            case -90:
                b = new Block[] {
                    new Block(this, 0, 0),
                    new Block(this, 0, 1),
                    new Block(this, 0, 2),
                    new Block(this, -1, 2)
                };
                break;
            case -180:
                b = new Block[] {
                    new Block(this, 0, 0),
                    new Block(this, 0, 1),
                    new Block(this, 1, 1),
                    new Block(this, 2, 1)
                };
                break;
            case -270:
                b = new Block[] {
                    new Block(this, 0, 0),
                    new Block(this, 1, 0),
                    new Block(this, 0, 1),
                    new Block(this, 0, 2)
                };
                break;
        }

        foreach (var block in b)
        {
            if (!block.CalculDefaultParameters(Blocks[0].ScenePoint, grid, false))
                return;
        }

        m_rot = rot;
        ResetBlocks(grid, b);
    }
}