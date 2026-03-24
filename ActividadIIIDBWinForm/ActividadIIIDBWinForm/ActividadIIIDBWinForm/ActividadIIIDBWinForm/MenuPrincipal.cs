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

        private void personasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // NOTA: De esta forma se hace para que el formulario se abra dentro del principal.
            // Debes configurar la propiedad IsMdiContainer = true.

            Personas frm = new Personas();
            frm.MdiParent = this;
            frm.Show();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos frm = new Productos();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
