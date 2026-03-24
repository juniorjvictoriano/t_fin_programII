using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using ActividadIIIDBWinForm.Modelo;

namespace ActividadIIIDBWinForm
{
    public partial class Proveedor : Form
    {
        private SQLClientesEntities _context;

        public Proveedor()
        {
            InitializeComponent();
            this.Load += Proveedor_Load;
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

        private void CargarDatos()
        {
            try
            {
                var lista = _context.Productos
                    .Include(p => p.Categorias) // carga explícita del JOIN
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
                MessageBox.Show($"Error al cargar datos:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Proveedor_Load(object sender, EventArgs e)
        {
            _context = new SQLClientesEntities();
            CargarDatos();
        }

        private void btnCargar_Click(object sender, EventArgs e) => CargarDatos();
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarTexto(txtNombre.Text, "Nombre del proveedor")) return;

            try
            {
                var nuevo = new ActividadIIIDBWinForm.Modelo.Proveedores
                {
                    NombreProveedor = txtNombre.Text.Trim()
                };

                _context.Proveedores.Add(nuevo);
                int filas = _context.SaveChanges();

                if (filas > 0)
                {
                    MessageBox.Show("Proveedor insertado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNombre.Clear();
                }

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar proveedor:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!TryParseInt(txtID.Text, "ID", out int proveedorID)) return;

            var confirm = MessageBox.Show(
                $"¿Deseas eliminar el proveedor con ID {proveedorID}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var proveedor = _context.Proveedores
                    .FirstOrDefault(p => p.ProveedorID == proveedorID);

                if (proveedor == null)
                {
                    MessageBox.Show("No se encontró un proveedor con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _context.Proveedores.Remove(proveedor);
                int filas = _context.SaveChanges();

                if (filas > 0)
                {
                    MessageBox.Show("Proveedor eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Clear();
                }

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar proveedor:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!TryParseInt(txtIDActualizar.Text, "ID", out int proveedorID)) return;
            if (!ValidarTexto(txtNombreActualizado.Text, "Nombre del proveedor")) return;

            try
            {
                var proveedor = _context.Proveedores
                    .FirstOrDefault(p => p.ProveedorID == proveedorID);

                if (proveedor == null)
                {
                    MessageBox.Show("No se encontró un proveedor con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                proveedor.NombreProveedor = txtNombreActualizado.Text.Trim();

                int filas = _context.SaveChanges();

                if (filas > 0)
                {
                    MessageBox.Show("Proveedor actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIDActualizar.Clear();
                    txtNombreActualizado.Clear();
                }

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar proveedor:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // Evento requerido por el diseñador
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _context?.Dispose();
            base.OnFormClosed(e);
        }
    }
}