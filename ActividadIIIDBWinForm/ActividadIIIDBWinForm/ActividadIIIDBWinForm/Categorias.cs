using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using ActividadIIIDBWinForm.Modelo;

namespace ActividadIIIDBWinForm
{
    public partial class Categorias : Form
    {
        private SQLClientesEntities _context;

        public Categorias()
        {
            InitializeComponent();
            this.Load += Categorias_Load;
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
                    .Include(p => p.Categorias)
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

        private void Categorias_Load(object sender, EventArgs e)
        {
            _context = new SQLClientesEntities();
            CargarDatos();
        }

        private void btnCargar_Click(object sender, EventArgs e) => CargarDatos();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarTexto(txtNombre.Text, "Nombre del cliente")) return;

            try
            {
                var nuevo = new ActividadIIIDBWinForm.Modelo.Clientes
                {
                    NombreCompleto = txtNombre.Text.Trim()
                };

                _context.Clientes.Add(nuevo);
                int filas = _context.SaveChanges();

                if (filas > 0)
                {
                    MessageBox.Show("Cliente insertado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNombre.Clear();
                }

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar cliente:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!TryParseInt(txtID.Text, "ID", out int clienteID)) return;

            var confirm = MessageBox.Show(
                $"¿Deseas eliminar el cliente con ID {clienteID}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var cliente = _context.Clientes
                    .FirstOrDefault(c => c.ClienteID == clienteID);

                if (cliente == null)
                {
                    MessageBox.Show("No se encontró un cliente con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _context.Clientes.Remove(cliente);
                int filas = _context.SaveChanges();

                if (filas > 0)
                {
                    MessageBox.Show("Cliente eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Clear();
                }

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cliente:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!TryParseInt(txtIDActualizar.Text, "ID", out int clienteID)) return;
            if (!ValidarTexto(txtNombreActualizado.Text, "Nombre del cliente")) return;

            try
            {
                var cliente = _context.Clientes
                    .FirstOrDefault(c => c.ClienteID == clienteID);

                if (cliente == null)
                {
                    MessageBox.Show("No se encontró un cliente con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cliente.NombreCompleto = txtNombreActualizado.Text.Trim();

                int filas = _context.SaveChanges();

                if (filas > 0)
                {
                    MessageBox.Show("Cliente actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIDActualizar.Clear();
                    txtNombreActualizado.Clear();
                }

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar cliente:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _context?.Dispose();
            base.OnFormClosed(e);
        }

        private void Categorias_Load_1(object sender, EventArgs e)
        {

        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}