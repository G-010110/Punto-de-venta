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
            string usuarioCorrecto3 = "visitante";

            if (txtUsuario.Text == "admin" && txtPassword.Text == "1234")
            {    
                this.Hide();
                MenuAdmin vm = new MenuAdmin();
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
            if (txtUsuario.Text == "visitante" && txtPassword.Text == "1234")
            {
                this.Hide();
                Visitantee fm = new Visitantee();
/////////////
                fm.FormClosed += (s, args) => this.Show();
                fm.Show();

            }
        }
    }
}
