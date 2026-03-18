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
        int contador = 1;
        private void btnCobrar_Click(object sender, EventArgs e)
        {
            string rutaBaseP = Path.Combine(
    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
    "LOGS");
            string ruta = rutaBaseP + "\\Ticket" + contador + ".pdf";
            contador++;

            PdfWriter writer = new PdfWriter(ruta);
            PdfDocument pdf = new PdfDocument(writer);

            // Tamaño tipo ticket
            Document document = new Document(pdf, new iText.Kernel.Geom.PageSize(200, 400));
            document.SetMargins(10, 10, 10, 10);

            // 🏪 Encabezado
            document.Add(new Paragraph("TIENDA DEL BIENESTAR")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
            document.Add(new Paragraph("RFC: YYY010101YYY")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(8));
            document.Add(new Paragraph("TAMAZUNCHALE, SLP. CARRETERA TAMAZUNCHALE-SAN MARTIN")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(8));

            document.Add(new Paragraph("---------------------------------------------"));

            // 📅 Fecha y hora
            document.Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(8));

            document.Add(new Paragraph("---------------------------------------------"));

            // 🛒 Tabla de productos
            Table table = new Table(3).SetWidth(UnitValue.CreatePercentValue(100));
            table.AddCell(new Paragraph("Prod").SetFontSize(8));
            table.AddCell(new Paragraph("Cant").SetFontSize(8));
            table.AddCell(new Paragraph("Precio").SetFontSize(8));

            double total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string producto = row.Cells[1].Value.ToString();
                int cantidad = Convert.ToInt32(row.Cells[3].Value);
                double precio = Convert.ToDouble(row.Cells[2].Value);

                double subtotal = cantidad * precio;
                total += subtotal;

                table.AddCell(new Paragraph(producto).SetFontSize(8));
                table.AddCell(new Paragraph(cantidad.ToString()).SetFontSize(8));
                table.AddCell(new Paragraph(subtotal.ToString("0.00")).SetFontSize(8));
            }

            document.Add(table);

            document.Add(new Paragraph("---------------------------------------------"));

            // 💰 Total
            document.Add(new Paragraph($"TOTAL: {total:0.00}")
                .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10));

            document.Add(new Paragraph("---------------------------------------------"));

            // 🙏 Mensaje final
            document.Add(new Paragraph("¡GRACIAS POR SU COMPRA!")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

            document.Close();
            dataGridView1.Rows.Clear();
            MessageBox.Show("Ticket generado");

            Process.Start(new ProcessStartInfo
            {
                FileName = ruta,
                UseShellExecute = true
            });
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
                        Importee = importees
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