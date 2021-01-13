using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TetrisWinForms;
using Inertia.Realtime;

public class Game : Script
{
    public static Game Instance { get; private set; }

    public const int BlockWidth = 12;
    public const int BlockHeight = 16;

    private const float GravityTime = .75f;
    
    private static Type[] m_tetriminoTypes = new Type[] {
        typeof(TetriminoO),
        typeof(TetriminoI),
        typeof(TetriminoT),
        typeof(TetriminoL),
        typeof(TetriminoJ),
        typeof(TetriminoZ),
        typeof(TetriminoS)
    };

    public List<Tetrimino> Tetriminos;

    private Block[,] m_grid;
    private Tetrimino m_currentTetrimino;
    private Random m_random;
    private float m_gravityTime;

    protected override void OnAwake(ScriptArgumentsCollection args)
    {
        Instance = this;

        m_grid = new Block[BlockWidth, BlockHeight];
        Tetriminos = new List<Tetrimino>();
        m_random = new Random();
    }

    protected override void OnUpdate()
    {
        if (m_currentTetrimino == null)
            GetNewTetrimino();

        m_gravityTime += DeltaTime;
        if (m_gravityTime >= GravityTime)
        {
            m_gravityTime -= GravityTime;

            try
            {
                if (!m_currentTetrimino.TryMoveDown(m_grid)) {
                    m_currentTetrimino = null;

                    var lines = new List<Block>();
                    var inB = new List<Block>();

                    for (var y = BlockHeight - 1; y >= 0; y--)
                    {
                        inB.Clear();

                        for (var x = 0; x < BlockWidth; x++)
                        {
                            if (m_grid[x, y] == null)
                                break;

                            inB.Add(m_grid[x, y]);

                            if (x == BlockWidth - 1)
                                lines.AddRange(inB);
                        }
                    }

                    if (lines.Count > 0)
                    {
                        var lineCount = lines.Count / BlockWidth;

                        foreach (var block in lines)
                        {
                            block.parent.Blocks.Remove(block);
                            m_grid[block.gridX, block.gridY] = null;

                            if (block.parent.Blocks.Count == 0)
                                Tetriminos.Remove(block.parent);
                        }

                        foreach (var tetrimino in Tetriminos)
                        {
                            tetrimino.UpdateBlocks(0, lineCount, m_grid);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error in GameLogic: " + ex.ToString()); }
        }
    }

    public void OnkeyDown(Keys key)
    {
        if (m_currentTetrimino == null)
            return;

        switch (key)
        {
            case Keys.Up:
                m_currentTetrimino.TryRotateBlocks(m_grid);
                break;
            case Keys.Down:
                m_currentTetrimino.TryMoveDown(m_grid);
                break;
            case Keys.Right:
                m_currentTetrimino.TryMoveSide(1, m_grid);
                break;
            case Keys.Left:
                m_currentTetrimino.TryMoveSide(-1, m_grid);
                break;
            case Keys.Space:
                while (true)
                {
                    if (!m_currentTetrimino.TryMoveDown(m_grid))
                        break;
                }
                break;
        }
    }

    private void GetNewTetrimino()
    {
        var type = m_tetriminoTypes[m_random.Next(0, m_tetriminoTypes.Length)];
        m_currentTetrimino = Activator.CreateInstance(type) as Tetrimino;

        var s = m_currentTetrimino.GetDefaultSize();
        var p = new Point(m_random.Next(s.Width, TetrisMain.Instance.Size.Width - s.Width), 0);
        var dx = (p.X / Block.DefaultSize.Width) * Block.DefaultSize.Width;

        var go = false;

        foreach (var block in m_currentTetrimino.Blocks)
        {
            go = !block.CalculDefaultParameters(new Point(dx, 0), m_grid);

            if (go)
                break;
        }

        if (go)
        {
            var r = MessageBox.Show("Game over! Do you want to replay ?", "GAME OVER", MessageBoxButtons.OKCancel);
            if (r == DialogResult.OK)
            {
                foreach (var tetrimo in Tetriminos)
                    tetrimo.ResetBlocks(m_grid, null);

                Tetriminos.Clear();
                m_grid = new Block[BlockWidth, BlockHeight];
                m_gravityTime = 0;
                m_currentTetrimino = null;
            }
            else
                TetrisMain.Instance.Invoke(new Action(() => TetrisMain.Instance.Close()));
        }
        else
            Tetriminos.Add(m_currentTetrimino);
    }
}