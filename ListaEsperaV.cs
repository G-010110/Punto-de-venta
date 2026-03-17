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
    public partial class ListaEsperaV : Form
    {
        
        public ListaEsperaV()
        {
            InitializeComponent();
        }

        private void ListaEsperaV_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string nombreSesion = listView1.SelectedItems[0].Text;
                //f.sesionactual = f.sesiones[nombreSesion];
                //f.dataGridView1.DataSource = f.sesionactual;
            }
        }
    }
}
