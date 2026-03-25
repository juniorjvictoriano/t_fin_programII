using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActividadIIIDBWinForm
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void Abrirformulario(object formhijo)
        {
            if (this.panel1.Controls.Count > 0)
                this.panel1.Controls.RemoveAt(0);
            Form fh = formhijo as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fh);
            this.panel1.Tag = fh;
            fh.Show();
        }

        private void personasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // NOTA: De esta forma se hace para que el formulario se abra dentro del principal.
            // Debes configurar la propiedad IsMdiContainer = true.
        }


        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void módulosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Abrirformulario(new Productos());
        }

        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
            Abrirformulario(new Productos());
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
