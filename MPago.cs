using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
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
using Table = iText.Layout.Element.Table;
using Paragraph = iText.Layout.Element.Paragraph;
using Document = iText.Layout.Document;
using TextAlignment = iText.Layout.Properties.TextAlignment;


namespace CAJA
{
    public partial class MPago : Form
    {
        Form1 f;
        public MPago(Form1 f)
        {
            this.f= f;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float cambio = f.pasarTotal-float.Parse(txtDinero.Text);
            generarTicket(cambio);
        }
        int contador = Properties.Settings.Default.Contador;
        public void generarTicket(float cambio)
        {
            string rutaBaseP = Path.Combine(
   Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
   "LOGS");
            string ruta = rutaBaseP + "\\Ticket" + contador + ".pdf";
            contador++;
            Properties.Settings.Default.Contador= contador;
            Properties.Settings.Default.Save();

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
            table.AddCell(new Paragraph("Productos").SetFontSize(8));
            table.AddCell(new Paragraph("Cantidad").SetFontSize(8));
            table.AddCell(new Paragraph("Precio").SetFontSize(8));

            double total = 0;

            foreach (DataGridViewRow row in f.dataGridView1.Rows)
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
            document.Add(new Paragraph($"PAGO CON: {txtDinero.Text:0.00}")
                .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10));
            document.Add(new Paragraph($"SU CAMBIO: {cambio:0.00}")
                .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10));
            document.Add(new Paragraph("---------------------------------------------"));

            // 🙏 Mensaje final
            document.Add(new Paragraph("¡GRACIAS POR SU COMPRA!")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

            document.Close();
            f.dataGridView1.Rows.Clear();
            MessageBox.Show("Ticket generado");

            Process.Start(new ProcessStartInfo
            {
                FileName = ruta,
                UseShellExecute = true
            });
            this.Close();
        }
    }
}
