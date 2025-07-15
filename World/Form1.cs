using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World
{
    public partial class Form1 : Form
    {
        public World_ world;
        public Form1()
        {
            InitializeComponent();
            World_.g = this.CreateGraphics();
            World_.imageWorld = new Image[3];
            World_.imageWorld[2] = Image.FromFile(@"WorldNps/bird.png");
            World_.imageWorld[1] = Image.FromFile(@"WorldNps/Bug.png");
            World_.imageWorld[0] = Image.FromFile(@"WorldNps/Plant.png");
            World_.bkcolor = this.BackColor;
            world = new World_(Width, Height);
            timerlife.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            world.Live();
        }
    }
}
