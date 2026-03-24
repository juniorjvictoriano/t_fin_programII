using System;
using System.Linq;
using System.Windows.Forms;
using ActividadIIIDBWinForm.Modelo;

namespace ActividadIIIDBWinForm
{
    public partial class Clientes : Form
    {
        private SQLClientesEntities _context;

        public Clientes()
        {
            InitializeComponent();
            this.Load += Clientes_Load;
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
                var lista = _context.Productos
                    .Select(p => new
                    {
                        ID = p.ProductoID,
                        Nombre = p.NombreProducto,
                        Descripcion = p.Descripcion,
                        Precio = p.Precio,
                        Stock = p.Stock,
                        Categoria = p.Categorias.NombreCategoria
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

        private void Clientes_Load(object sender, EventArgs e)
        {
            _context = new SQLClientesEntities();
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
                    CategoriaID = (int?)Convert.ToInt32(cmbCategoria.SelectedValue),
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
                else
                    MessageBox.Show("No se encontró un producto con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

                producto.NombreProducto = txtNombreActualizado.Text.Trim();
                producto.Descripcion = txtDescripcionActualizado.Text.Trim();
                producto.Stock = stock;
                producto.CategoriaID = (int?)Convert.ToInt32(cmbCategoriaActualizado.SelectedValue);
                producto.Precio = precio;

                int filas = _context.SaveChanges();

                if (filas > 0)
                    MessageBox.Show("Producto actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("No se encontró un producto con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {
            // Evento requerido por el diseñador
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _context?.Dispose();
            base.OnFormClosed(e);
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}