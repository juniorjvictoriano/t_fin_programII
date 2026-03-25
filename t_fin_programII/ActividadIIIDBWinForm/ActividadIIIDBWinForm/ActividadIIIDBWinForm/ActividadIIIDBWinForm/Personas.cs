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
    public partial class Personas : Form
    {
        public Personas()
        {
            InitializeComponent();
        }


        private void btnCargar_Click(object sender, EventArgs e)
        {
            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=GestionPersonas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryPersonas = "SELECT * FROM personas;";

                using(SqlCommand cmd = new SqlCommand(queryPersonas, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgPersonas.DataSource = dt;
                    }
                }

                connection.Close();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validaciones para evitar insertar datos erroneos.

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El nombre está incorrecto o vacio.");
                return;
            }

            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                MessageBox.Show("El apellido está incorrecto o vacio.");
                return;
            }

            if (string.IsNullOrEmpty(txtFechaNacimiento.Text))
            {
                MessageBox.Show("La fecha de nacimiento está incorrecta o vacia.");
                return;
            }
            if (string.IsNullOrEmpty(cmbSexo.SelectedItem.ToString()))
            {
                MessageBox.Show("El sexo está incorrecto o vacio.");
                return;
            }
            if (string.IsNullOrEmpty(txtNacionalidad.Text))
            {
                MessageBox.Show("La nacionalidad está incorrecta o vacia.");
                return;
            }

            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=GestionPersonas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryInsertarPersonas = @"INSERT INTO personas (nombre, apellido, fecha_nacimiento, genero, nacionalidad)
                                           VALUES ('"+txtNombre.Text+"','"+txtApellido.Text+"'," +
                                                   "'"+txtFechaNacimiento.Text+"','"+cmbSexo.SelectedItem+"'," +
                                                   "'"+txtNacionalidad.Text+"')";

                using (SqlCommand cmd = new SqlCommand(queryInsertarPersonas, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se ha insertado a la persona en la base de datos.");
                    }
                }

                connection.Close();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Debe introducir un ID válido.");
                return;
            }

            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=GestionPersonas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryEliminarPersona = @"DELETE FROM personas WHERE id_persona = '"+ txtID.Text +"'";

                using (SqlCommand cmd = new SqlCommand(queryEliminarPersona, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se ha eliminado a la persona en la base de datos.");
                    }
                }

                connection.Close();
            }
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

            if (string.IsNullOrEmpty(txtApellidoActualizado.Text))
            {
                MessageBox.Show("El apellido está incorrecto o vacio.");
                return;
            }

            if (string.IsNullOrEmpty(txtFechaNacimientoActualizada.Text))
            {
                MessageBox.Show("La fecha de nacimiento está incorrecta o vacia.");
                return;
            }
            if (string.IsNullOrEmpty(cmbSexoActualizado.SelectedItem.ToString()))
            {
                MessageBox.Show("El sexo está incorrecto o vacio.");
                return;
            }
            if (string.IsNullOrEmpty(txtNacionalidadActualizado.Text))
            {
                MessageBox.Show("La nacionalidad está incorrecta o vacia.");
                return;
            }

            // TODO:  Debes cambiar esta variable connectionString para que pueda conectarse a tu base de datos.
            string connectionString = @"Data Source=5CD0537YBH;Initial Catalog=GestionPersonas;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryActualizarPersonas = @"UPDATE personas 
                                                    SET 
                                                        nombre = '" + txtNombreActualizado.Text + "', " +
                                                        "apellido = '" + txtApellidoActualizado.Text + "',  " +
                                                        "fecha_nacimiento = '" + txtFechaNacimientoActualizada.Text + "', " +
                                                        "genero = '" + cmbSexoActualizado.SelectedItem + "', " +
                                                        "nacionalidad = '" + txtNacionalidadActualizado.Text + "'" +
                                                    "WHERE id_persona = '" + txtIDActualizar.Text + "'";

                using (SqlCommand cmd = new SqlCommand(queryActualizarPersonas, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se ha actualizado a la persona en la base de datos.");
                    }
                }

                connection.Close();
            }
        }

        
    }
}
