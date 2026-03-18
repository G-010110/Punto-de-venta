using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml;
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
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace CAJA
{
    public partial class VentanaAdmin : Form
    {
        public VentanaAdmin()
        {
            InitializeComponent();
            CargarExcel();
            CargarPDFs();
        }

        public void CargarExcel()
        {
            string ruta = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
           "BaseDatos",
           "DB_INVENTARIO_PRODUCTOS.xlsx");
            DataTable dt = new DataTable();

            using (var workbook = new XLWorkbook(ruta))
            {
                var worksheet = workbook.Worksheet(1);

                // Columnas
                bool primeraFila = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (primeraFila)
                    {
                        foreach (var cell in row.Cells())
                            dt.Columns.Add(cell.Value.ToString());

                        primeraFila = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;

                        foreach (var cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }
            }

            dataGridView1.DataSource = dt;
        }
        public void GuardarExcel()
        {
            string ruta = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
           "BaseDatos",
           "DB_INVENTARIO_PRODUCTOS.xlsx");

            var dt = (DataTable)dataGridView1.DataSource;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Productos");

                // Encabezados
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                }

                // Datos
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = dt.Rows[i][j]?.ToString();
                    }
                }

                workbook.SaveAs(ruta);
            }

            MessageBox.Show("Guardado correctamente");
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarExcel();
        }
        public void CargarPDFs()
        {
            string carpeta = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
            "LOGS");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string[] archivos = Directory.GetFiles(carpeta, "*.pdf");

            listView1.Items.Clear();

            foreach (var archivo in archivos)
            {
                listView1.Items.Add(Path.GetFileName(archivo));
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            string carpeta = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
            "LOGS");

            string archivo = listView1.SelectedItems[0].Text;

            string rutaCompleta = Path.Combine(carpeta, archivo);

            Process.Start(new ProcessStartInfo
            {
                FileName = rutaCompleta,
                UseShellExecute = true
            });
        }

        private void VentanaAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
          

        }
    }
}
