using ExcelDataReader;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public float pasarTotal = 0;
        void CalcularTotal()
        {
            float total = 0;
           
            foreach (Registro r in sesionactual)
            {
                total += float.Parse(r.Importee);
            }
            pasarTotal = total;
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
        public int contador = 1;
        
        private void btnCobrar_Click(object sender, EventArgs e)
        {
            MPago mp = new MPago(this);
           
            mp.Show();
        }
        
        private void txtTotal_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalcularTotal();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            txtTotal.Text = "$0.00";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            GenerarCorteCaja();
        }
        void GenerarCorteCaja()
        {
            float totalGeneral = 0;
            int totalVentas = sesiones.Count;
            int totalProductos = 0;

            foreach (var sesion in sesiones)
            {
                foreach (Registro r in sesion.Value)
                {
                    totalGeneral += float.Parse(r.Importee);
                    totalProductos += int.Parse(r.Cantidaaad);
                }
            }

            MessageBox.Show(
                "CORTE DE CAJA\n\n" +
                "Ventas: " + totalVentas + "\n" +
                "Productos vendidos: " + totalProductos + "\n" +
                "Total: $" + totalGeneral.ToString("0.00")
            );
        }

        void GenerarPDF(string contenido)
        {
            string ruta = "CorteCaja_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";

            PdfWriter writer = new PdfWriter(ruta);
            PdfDocument pdf = new PdfDocument(writer);
            Document doc = new Document(pdf);

            doc.Add(new Paragraph(contenido));

            doc.Close();

            MessageBox.Show("PDF generado en: " + ruta);
        }
        private void btnAgregarp_Click(object sender, EventArgs e)
        {
            try
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
                        Importee = importees,
                        aggFecha = Fecha
                    });
                    CalcularTotal();
                }
                else
                {
                    MessageBox.Show("Primero busca un producto válido antes de agregar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Por favor ingresa datos...", "Advertencia");
            }
        }
        string busqueda;
        public DateTime Fecha;
        //C:\Users\010110\Desktop\SEMESTRE 4\Topicos avanzados de programacion\Unidad 2\CAJA\BaseDatos\DB_INVENTARIO_PRODUCTOS.xlsx
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            busqueda=txtBusqueda.Text;
            txtCant.Text = 1+"";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string rutaBaseP = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
            "BaseDatos",
            "DB_INVENTARIO_PRODUCTOS.xlsx");

            using (var stream = File.Open(rutaBaseP, FileMode.Open, FileAccess.Read))
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
                            Fecha = DateTime.Now;
                            //Mostrar datos en stage 
                            txtDescr.Text = descricion;
                            txtUni.Text = "$"+pU+" p/u";
                        }
                    }
                }
            }
            ///////////////////////
            void GenerarCortePorDia(DateTime fecha, out string reporte, out float totalGeneral)
            {
                totalGeneral = 0;
                int totalVentas = 0;
                int totalProductos = 0;

                reporte = "===== CORTE DE CAJA =====\n";
                reporte += "Fecha: " + fecha.ToShortDateString() + "\n\n";

                foreach (var sesion in sesiones)
                {
                    float subtotal = 0;
                    bool tieneVentasHoy = false;

                    foreach (Registro r in sesion.Value)
                    {
                        if (r.aggFecha.Date == fecha.Date)
                        {
                            subtotal += float.Parse(r.Importee);
                            totalProductos += int.Parse(r.Cantidaaad);
                            tieneVentasHoy = true;
                        }
                    }

                    if (tieneVentasHoy)
                    {
                        reporte += sesion.Key + " -> $" + subtotal.ToString("0.00") + "\n";
                        totalGeneral += subtotal;
                        totalVentas++;
                    }
                }

                reporte += "\n----------------------\n";
                reporte += "Ventas: " + totalVentas + "\n";
                reporte += "Productos: " + totalProductos + "\n";
                reporte += "TOTAL: $" + totalGeneral.ToString("0.00");
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

            public DateTime aggFecha { get; set; }
        }



    }
}