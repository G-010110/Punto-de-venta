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
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuarioCorrecto = "admin";
            string passwordCorrecto = "1234";
            string usuarioCorrecto2 = "usuario";

            if (txtUsuario.Text == "admin" && txtPassword.Text == "1234")
            {    
                this.Hide();
                VentanaAdmin vm = new VentanaAdmin();
                vm.FormClosed += (s, args) => this.Show(); 
                vm.Show();
            }
            if (txtUsuario.Text == "usuario" && txtPassword.Text == "1234")
            {
                this.Hide();
                Form1 fm = new Form1();
                
                fm.FormClosed += (s, args) => this.Show(); 
                fm.Show();

            }
        }
    }
}
