using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAJA
{
    public partial class Form1 : Form
    {
        //Atributos de la caja 
        string id, descricion, pU, cant, unidad, importees;
        ListaEsperaV le = new ListaEsperaV();
        int no_sesion = 1;
        //Diccionario de sesiones 
        Dictionary<string, BindingList<Registro>> sesiones = new Dictionary<string, BindingList<Registro>>();
        BindingList<Registro> sesionactual;
        private void button3_Click(object sender, EventArgs e)
        {
            le.Show();
        }

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            sesiones["Sesion1"] = new BindingList<Registro>();
            sesionactual = sesiones["Sesion1"];
            //dataGridView1.AutoGenerateColumns = true;
            listView1.Items.Add("Sesion1");
            dataGridView1.DataSource = sesionactual;
        }
        void CalcularTotal()
        {
            float total = 0;

            foreach (Registro r in sesionactual)
            {
                total += float.Parse(r.Importee);
            }

            txtTotal.Text = "Total: $ " + total.ToString("0.00");
        }
        private void button2_Click(object sender, EventArgs e)
        {

            no_sesion++;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string nombreSesion = listView1.SelectedItems[0].Text;
                sesionactual = sesiones[nombreSesion];
                dataGridView1.DataSource = sesionactual;
                CalcularTotal();
            }
        }

        public void generarTicket()
        {
            float total = 0;

            foreach (Registro r in sesionactual)
            {
                total += float.Parse(r.Importee);
            }

            txtTotal.Text = "Total: $ " + total.ToString("0.00");
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            no_sesion++;
            string nombreSesion = "Sesion" + no_sesion;
            sesiones[nombreSesion] = new BindingList<Registro>();
            sesionactual = sesiones[nombreSesion];
            dataGridView1.DataSource = sesionactual;
            listView1.Items.Add(nombreSesion);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CalcularTotal();
            dataGridView1.Rows.Clear();
            MessageBox.Show("GRACIAS POR SU COMPRA, EL TOTAL FUE DE: " + txtTotal.Text);
        }

        private void txtCant_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pU) && !string.IsNullOrEmpty(txtCant.Text))
            {
                cant = txtCant.Text;

                float precio = float.Parse(pU);
                float cantidad = float.Parse(cant);

                importees = (precio * cantidad).ToString();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAgregarp_Click(object sender, EventArgs e)
        {
            importees = ((float.Parse(pU)) * (float.Parse(cant))) + "";
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(descricion))
            {
                sesionactual.Add(new Registro
                {
                    Id = id,
                    Nombre = descricion,
                    pUnitarioo = pU,
                    Cantidaaad = cant,
                    un = unidad,
                    Importee = importees
                });
                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Primero busca un producto válido antes de agregar.");
            }

        }
        string busqueda;
        //C:\Users\010110\Desktop\SEMESTRE 4\Topicos avanzados de programacion\Unidad 2\CAJA\BaseDatos\DB_INVENTARIO_PRODUCTOS.xlsx
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            busqueda=txtBusqueda.Text;
            txtCant.Text = 1+"";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open("C:\\Users\\010110\\Desktop\\SEMESTRE 4\\Topicos avanzados de programacion\\Unidad 2\\CAJA\\BaseDatos\\DB_INVENTARIO_PRODUCTOS.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    DataTable tabla = result.Tables[0]; 

                    
                    //recorrer filas
                    foreach (DataRow fila in tabla.Rows)
                    {
                        if (fila[0].ToString() == busqueda) 
                        {
                            //Cargar los datos en memoria
                            id = fila[0].ToString();
                            descricion = fila[1].ToString();
                            pU = fila[2].ToString();
                            cant = txtCant.Text;
                            unidad = fila[3].ToString();
                            
                            //Mostrar datos en stage 
                            txtDescr.Text = descricion;
                            txtUni.Text = "$"+pU+" p/u";
                        }
                    }
                }
            }
        }
        class Registro
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
            public string pUnitarioo { get; set; }
            public string Cantidaaad { get; set; }
            public string un { get; set; }
            public string Importee { get; set; }
        }

    }
}
