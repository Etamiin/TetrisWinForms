using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class TetriminoO : Tetrimino
{
    public TetriminoO() : base(Color.Yellow)
    {
    }

    public override void FillDefaultBlocks()
    {
        Blocks.AddRange(new Block[] {
            new Block(this, 0, 0),
            new Block(this, 1, 0),
            new Block(this, 0, 1),
            new Block(this, 1, 1)
        });
    }

    public override Size GetDefaultSize()
    {
        return new Size(Block.DefaultSize.Width * 2, Block.DefaultSize.Height * 2);
    }

    public override void TryRotateBlocks(Block[,] grid)
    {
    }
}