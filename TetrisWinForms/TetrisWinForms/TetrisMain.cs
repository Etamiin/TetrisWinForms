using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inertia.Realtime;

namespace TetrisWinForms
{
    public partial class TetrisMain : Form
    {
        public static TetrisMain Instance { get; private set; }

        public TetrisMain()
        {
            InitializeComponent();
        }

        private void TetrisMain_Load(object sender, EventArgs e)
        {
            Instance = this;

            Script.MaxExecutionPerSecond = 200;

            Size = new Size(
                Block.DefaultSize.Width * Game.BlockWidth,
                Block.DefaultSize.Height * Game.BlockHeight);

            Location = new Point(
                Screen.PrimaryScreen.Bounds.Width / 2 - Size.Width / 2,
                Screen.PrimaryScreen.Bounds.Height / 2 - Size.Height / 2);

            var collection = new ScriptCollection();

            collection.Add<Renderer>();
            collection.Add<Game>();

            KeyDown += TetrisMain_KeyDown;

        }

        private void TetrisMain_KeyDown(object sender, KeyEventArgs e)
        {
            Game.Instance.OnkeyDown(e.KeyCode);
        }
    }
}
