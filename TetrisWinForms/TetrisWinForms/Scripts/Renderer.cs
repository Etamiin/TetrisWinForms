using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using TetrisWinForms;
using Inertia.Realtime;

public class Renderer : Script
{
    private BufferedGraphicsContext m_context;
    private BufferedGraphics m_graphicsBuffer;
    private Graphics m_graphics => m_graphicsBuffer.Graphics;
    private int m_fps;
    private int m_lastFps;
    private float m_time;

    protected override void OnAwake(ScriptArgumentsCollection args)
    {
        m_context = BufferedGraphicsManager.Current;
        m_graphicsBuffer = m_context.Allocate(TetrisMain.Instance.CreateGraphics(),
            TetrisMain.Instance.DisplayRectangle);
    }

    protected override void OnUpdate()
    {
        if (m_graphics == null) return;

        var brush = new SolidBrush(Color.White);

        m_time += DeltaTime;

        DrawBackground();
        DrawTetriminos();
        DrawGrid();
        if (m_lastFps != 0)
            DrawFps();

        if (m_time >= 1f)
        {
            m_time -= 1f;
            m_lastFps = m_fps;
            m_fps = 0;
        }

        m_graphicsBuffer.Render();
        m_fps++;

        void DrawBackground()
        {
            brush.Color = Color.FromArgb(255, 75, 75, 75);
            m_graphics.FillRectangle(brush, TetrisMain.Instance.DisplayRectangle);
        }
        void DrawGrid()
        {
            brush.Color = Color.Black;

            var xWidth = TetrisMain.Instance.Size.Width / Game.BlockWidth;
            var yHeight = TetrisMain.Instance.Size.Height / Game.BlockHeight;

            for (var x = 0; x < Game.BlockWidth; x++)
            {
                m_graphics.DrawLine(new Pen(brush), new Point(x * xWidth, 0), new Point(x * xWidth, TetrisMain.Instance.Size.Height));
            }

            for (var y = 0; y < Game.BlockHeight; y++)
            {
                m_graphics.DrawLine(new Pen(brush), new Point(0, y * yHeight), new Point(TetrisMain.Instance.Size.Width, y * yHeight));
            }
        }
        void DrawTetriminos()
        {
            foreach (var tetrimino in Game.Instance.Tetriminos)
            {
                brush.Color = tetrimino.Color;

                foreach (var block in tetrimino.Blocks)
                {
                    m_graphics.FillRectangle(brush, new Rectangle(block.ScenePoint, Block.DefaultSize));
                }
            }
        }
        void DrawFps()
        {
            brush.Color = Color.White;
            m_graphics.DrawString("FPS: " + m_lastFps, new Font("Arial", 14), brush, new Point());
        }
    }
}