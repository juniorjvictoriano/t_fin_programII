using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActividadIIIDBWinForm
{
    public partial class Productos : Form
    {
        public Productos()
        {
            InitializeComponent();
        }

        private void cargarDatos()
        {
            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=Ventas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryProductos = @"SELECT p.ProductoID, p.NombreProducto, p.Descripcion, p.Precio, p.Stock, c.NombreCategoria
	                                                FROM Productos p
		                                                INNER JOIN  Categorias c
			                                                ON p.CategoriaID = c.CategoriaID;";

                using (SqlCommand cmd = new SqlCommand(queryProductos, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgProductos.DataSource = dt;
                    }
                }

                connection.Close();
            }
        }

        private void cargarCmbCategorias()
        {
            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=Ventas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryCategorias = "SELECT * FROM Categorias;";

                using (SqlCommand cmd = new SqlCommand(queryCategorias, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cmbCategoria.DataSource = dt;
                        cmbCategoria.DisplayMember = "NombreCategoria";
                        cmbCategoria.ValueMember = "CategoriaID";

                        cmbCategoriaActualizado.DataSource = dt;
                        cmbCategoriaActualizado.DisplayMember = "NombreCategoria";
                        cmbCategoriaActualizado.ValueMember = "CategoriaID";
                    }
                }

                connection.Close();
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            this.cargarDatos();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validaciones para evitar insertar datos erroneos.

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El nombre está incorrecto o vacio.");
                return;
            }

            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción está incorrecto o vacio.");
                return;
            }

            if (string.IsNullOrEmpty(txtStock.Text))
            {
                MessageBox.Show("El stock está incorrecta o vacia.");
                return;
            }
            if (string.IsNullOrEmpty(cmbCategoria.SelectedValue.ToString()))
            {
                MessageBox.Show("La categoria está incorrecto o vacio.");
                return;
            }
            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                MessageBox.Show("El precio está incorrecta o vacia.");
                return;
            }

            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=Ventas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryInsertarProductos = @"INSERT INTO Productos (NombreProducto, Descripcion, Stock, CategoriaID, Precio)
                                           VALUES ('"+txtNombre.Text+"','"+txtDescripcion.Text+"'," +
                                                   "'"+txtStock.Text+"','"+cmbCategoria.SelectedValue+"'," +
                                                   "'"+txtPrecio.Text+"')";

                using (SqlCommand cmd = new SqlCommand(queryInsertarProductos, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se ha insertado el producto en la base de datos.");
                    }
                }

                connection.Close();
            }

            this.cargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Debe introducir un ID válido.");
                return;
            }

            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=Ventas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryEliminarProducto = @"DELETE FROM Productos WHERE ProductoID = '"+ txtID.Text +"'";

                using (SqlCommand cmd = new SqlCommand(queryEliminarProducto, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se ha eliminado el producto en la base de datos.");
                    }
                }

                connection.Close();
            }

            this.cargarDatos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // Validaciones para evitar actualizar datos erroneos.

            if (string.IsNullOrEmpty(txtIDActualizar.Text))
            {
                MessageBox.Show("Debe introducir un ID válido.");
                return;
            }

            if (string.IsNullOrEmpty(txtNombreActualizado.Text))
            {
                MessageBox.Show("El nombre está incorrecto o vacio.");
                return;
            }

            if (string.IsNullOrEmpty(txtDescripcionActualizado.Text))
            {
                MessageBox.Show("La descripción está incorrecto o vacio.");
                return;
            }

            if (string.IsNullOrEmpty(txtStockActualizado.Text))
            {
                MessageBox.Show("El stock de nacimiento está incorrecta o vacia.");
                return;
            }
            if (string.IsNullOrEmpty(cmbCategoriaActualizado.SelectedValue.ToString()))
            {
                MessageBox.Show("La categoria está incorrecto o vacio.");
                return;
            }
            if (string.IsNullOrEmpty(txtPrecioActualizado.Text))
            {
                MessageBox.Show("El precio está incorrecta o vacia.");
                return;
            }

            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=Ventas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryActualizarProductos = @"UPDATE Productos 
                                                    SET 
                                                        NombreProducto = '" + txtNombreActualizado.Text + "', " +
                                                        "Descripcion = '" + txtDescripcionActualizado.Text + "',  " +
                                                        "Stock = '" + txtStockActualizado.Text + "', " +
                                                        "CategoriaID = '" + cmbCategoriaActualizado.SelectedValue + "', " +
                                                        "Precio = '" + txtPrecioActualizado.Text + "'" +
                                                    "WHERE ProductoID = '" + txtIDActualizar.Text + "'";

                using (SqlCommand cmd = new SqlCommand(queryActualizarProductos, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se ha actualizado el producto en la base de datos.");
                    }
                }

                connection.Close();
            }

            this.cargarDatos();
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            this.cargarCmbCategorias();
            this.cargarDatos();
        }
    }
}
