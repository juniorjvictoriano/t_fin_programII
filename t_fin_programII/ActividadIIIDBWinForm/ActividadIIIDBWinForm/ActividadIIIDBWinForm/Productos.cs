using ActividadIIIDBWinForm.Modelo; // namespace correcto del modelo auto-generado
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ActividadIIIDBWinForm
{
    public partial class Productos : Form
    {

        private SQLClientesEntities _context;
        private string connectionString;

        public Productos()
        {
            InitializeComponent();
            this.Load += Productos_Load;
        }

        private bool ValidarTexto(string valor, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                MessageBox.Show($"El campo '{campo}' es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool TryParseInt(string valor, string campo, out int resultado)
        {
            if (!int.TryParse(valor, out resultado))
            {
                MessageBox.Show($"'{campo}' debe ser un número entero válido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool TryParseDecimal(string valor, string campo, out decimal resultado)
        {
            if (!decimal.TryParse(valor, out resultado))
            {
                MessageBox.Show($"'{campo}' debe ser un número válido (ej: 9.99).", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void CargarDatos()
        {
            try
            {
                // Categorias es propiedad de navegación virtual en Productos.cs del modelo
                var lista = _context.Productos
                    .Select(p => new
                    {
                        ID = p.ProductoID,
                        Nombre = p.NombreProducto,
                        Descripcion = p.Descripcion,
                        Precio = p.Precio,
                        Stock = p.Stock,
                        Categoria = p.Categorias.NombreCategoria  // navegación virtual
                    })
                    .ToList();

                dgProductos.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarCmbCategorias()
        {
            try
            {
                // CategoriaID y NombreCategoria — propiedades reales de Categorias.cs
                var categorias = _context.Categorias
                    .Select(c => new
                    {
                        ID = c.CategoriaID,
                        Nombre = c.NombreCategoria
                    })
                    .ToList();

                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "ID";

                // .ToList() crea una lista independiente para evitar conflicto de binding
                cmbCategoriaActualizado.DataSource = categorias.ToList();
                cmbCategoriaActualizado.DisplayMember = "Nombre";
                cmbCategoriaActualizado.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            _context = new SQLClientesEntities(); // usa "name=SQLClientesEntities" del App.config
            CargarCmbCategorias();
            CargarDatos();
        }

        private void btnCargar_Click(object sender, EventArgs e) => CargarDatos();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarTexto(txtNombre.Text, "Nombre")) return;
            if (!ValidarTexto(txtDescripcion.Text, "Descripción")) return;

            if (!TryParseInt(txtStock.Text, "Stock", out int stock)) return;
            if (!TryParseDecimal(txtPrecio.Text, "Precio", out decimal precio)) return;

            if (cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una categoría válida.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var nuevo = new ActividadIIIDBWinForm.Modelo.Productos
                {
                    NombreProducto = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    Stock = stock,
                    CategoriaID = (int?)Convert.ToInt32(cmbCategoria.SelectedValue), // Nullable<int>
                    Precio = precio
                };

                _context.Productos.Add(nuevo);
                int filas = _context.SaveChanges();

                if (filas > 0)
                    MessageBox.Show("Producto insertado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar producto:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!TryParseInt(txtID.Text, "ID", out int productoID)) return;

            var confirm = MessageBox.Show(
                $"¿Deseas eliminar el producto con ID {productoID}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                // ProductoID — propiedad real confirmada en Productos.cs del modelo
                var producto = _context.Productos
                    .FirstOrDefault(p => p.ProductoID == productoID);

                if (producto == null)
                {
                    MessageBox.Show("No se encontró un producto con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _context.Productos.Remove(producto);
                int filas = _context.SaveChanges();

                if (filas > 0)
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar producto:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!TryParseInt(txtIDActualizar.Text, "ID", out int productoID)) return;

            if (!ValidarTexto(txtNombreActualizado.Text, "Nombre")) return;
            if (!ValidarTexto(txtDescripcionActualizado.Text, "Descripción")) return;

            if (!TryParseInt(txtStockActualizado.Text, "Stock", out int stock)) return;
            if (!TryParseDecimal(txtPrecioActualizado.Text, "Precio", out decimal precio)) return;

            if (cmbCategoriaActualizado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una categoría válida.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var producto = _context.Productos
                    .FirstOrDefault(p => p.ProductoID == productoID);

                if (producto == null)
                {
                    MessageBox.Show("No se encontró un producto con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // EF rastrea el objeto — solo modificamos propiedades y SaveChanges genera el UPDATE
                producto.NombreProducto = txtNombreActualizado.Text.Trim();
                producto.Descripcion = txtDescripcionActualizado.Text.Trim();
                producto.Stock = stock;
                producto.CategoriaID = (int?)Convert.ToInt32(cmbCategoriaActualizado.SelectedValue); // Nullable<int>
                producto.Precio = precio;

                int filas = _context.SaveChanges();

                if (filas > 0)
                    MessageBox.Show("Producto actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _context?.Dispose();
            base.OnFormClosed(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportarACSV();
        }

        private void ExportarACSV()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files|*.csv";
            saveFileDialog.Title = "Guardar archivo CSV";
            saveFileDialog.FileName = $"Reporte_Productos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Obtener la cadena de conexión desde el contexto de Entity Framework
                    string connectionString = _context.Database.Connection.ConnectionString;

                    // Consulta para exportar productos (más relevante para tu formulario de productos)
                    string query = @"
                SELECT 
                    p.ProductoID,
                    p.NombreProducto,
                    p.Descripcion,
                    p.Precio,
                    p.Stock,
                    c.NombreCategoria,
                    (SELECT COUNT(*) FROM DetallesFactura df WHERE df.ProductoID = p.ProductoID) as VecesVendido,
                    ISNULL((
                        SELECT SUM(df.Cantidad * df.Precio) 
                        FROM DetallesFactura df 
                        WHERE df.ProductoID = p.ProductoID
                    ), 0) as TotalVentas
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaID = c.CategoriaID
                ORDER BY p.ProductoID";

                    DataTable dt = new DataTable();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                        {
                            da.Fill(dt);
                        }
                    }

                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Escribir encabezados
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sw.Write(dt.Columns[i].ColumnName);
                            if (i < dt.Columns.Count - 1)
                                sw.Write(",");
                        }
                        sw.WriteLine();

                        // Escribir datos
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                // Manejar valores nulos y caracteres especiales para CSV
                                string valor = row[i]?.ToString() ?? "";

                                // Si el valor contiene comas o comillas, encapsular entre comillas
                                if (valor.Contains(",") || valor.Contains("\"") || valor.Contains("\n"))
                                {
                                    valor = valor.Replace("\"", "\"\"");
                                    valor = $"\"{valor}\"";
                                }

                                sw.Write(valor);
                                if (i < dt.Columns.Count - 1)
                                    sw.Write(",");
                            }
                            sw.WriteLine();
                        }
                    }

                    MessageBox.Show($"Archivo CSV exportado exitosamente a:\n{saveFileDialog.FileName}\n\n" +
                        $"Registros exportados: {dt.Rows.Count}",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}