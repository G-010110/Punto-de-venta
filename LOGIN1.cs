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
    public partial class LOGIN1 : Form
    {
        public LOGIN1()
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

            if (txtUsuario.Text == "admin" && txtPassword.Text == "1234")
            {
                MessageBox.Show("¡Bienvenido al sistema!", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // --- ESTO ES LO QUE DEBES AGREGAR ---
                this.Hide(); // Oculta el formulario de Login actual

               // Form1 pantallaPrincipal = new Form1(); // Crea la instancia
               // pantallaPrincipal.ShowDialog();    //    // Abre el Form1 y se queda esperando

                this.Close(); // Esta línea se ejecuta SOLO cuando cierras el Form1//
            }

        }
    }
}
