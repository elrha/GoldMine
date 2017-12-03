using Mines.Controls;
using Mines.Defines;
using Mines.Manager.GameManager;
using Mines.Renderer;
using PerseusCommon.Manager.AssemblyManager;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mines
{
    public partial class Main : Form
    {
        private GameManager gameManager;

        public Main()
        {
            InitializeComponent();

            Config.Initialize(24, 20);

            var playerControls = new List<PlayerControl>();
            for (int i = 0; i < Config.PlayerCount; i++)
                playerControls.Add(new PlayerControl(this, i));

            this.gameManager = new GameManager(new MainRenderer(this, new MainControl(this, () => { return this.gameManager.PauseGame(); }, () => { this.gameManager.StartGame(); }), playerControls));
            this.FormClosing += (object sender, FormClosingEventArgs e) => { this.gameManager.StopGame(); };
        }
    }
}