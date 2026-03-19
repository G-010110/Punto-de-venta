using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAJA
{
    public partial class MenuAdmin : Form
    {
        public MenuAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VentanaAdmin vm = new VentanaAdmin();
            vm.Show();
        }
    }
}
