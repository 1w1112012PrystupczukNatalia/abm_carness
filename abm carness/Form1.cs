using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Text;


namespace abm_carness
{
    public partial class Form1 : Form

    {

        SqlConnection conexion = new SqlConnection();
        SqlCommand comando = new SqlCommand();
        SqlDataReader lector;

        List<Productos> lPro = new List<Productos>();
        bool nuevo;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarListaProductos();
            CargarTipo();
            CargarSubtipo();
            habilitar(false);
        }
        private void conectarBD()
        {
            // conexion.ConnectionString = @"";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;

        }
        private void CargarListaProductos()
        {
            conectarBD();
            comando.CommandText = "select * from productos";
            lector = comando.ExecuteReader();
            while (lector.Read())
            {
                Productos p = new Productos();
                if (!lector.IsDBNull(0)) { p.pCodigo = lector.GetInt32(0); }
                if (!lector.IsDBNull(1)) { p.pPrecio = lector.GetDouble(1); }
                if (!lector.IsDBNull(2)) { p.pStock = lector.GetInt32(2); }
                if (!lector.IsDBNull(3)) { p.pTipo = lector.GetInt32(3); }
                if (!lector.IsDBNull(4)) { p.pSubtipo = lector.GetInt32(4); }


                lPro.Add(p);
                conexion.Close();
                lector.Close();
                lstProducto.Items.Clear();
                for (int i = 0; i < lPro.Count; i++)
                {
                    lstProducto.Items.Add(lPro[i].ToString());
                }


            }


             }
            private void CargarTipo()
            {
                DataTable tabla = new DataTable();
                conectarBD();
                comando.CommandText = "select * from Tipos_productos";
                tabla.Load(comando.ExecuteReader());
                conexion.Close();
                cboTipoProducto.DataSource = tabla;
                cboTipoProducto.DisplayMember = tabla.Columns[1].ColumnName;





            }
        private void CargarSubtipo()
        {
            DataTable tabla = new DataTable();
            conectarBD();
            comando.CommandText = "select * from Subtipos_productos";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            cboSubtipo.DataSource = tabla;
            cboSubtipo.DisplayMember = tabla.Columns[1].ColumnName;

        }

        private void habilitar(bool x)
        {
            txtCodigo.Enabled = x;  
            txtPrecio.Enabled = x;
            txtStock.Enabled = x;
            cboTipoProducto.Enabled = x;
            cboTipoProducto.Enabled = x;
           
            btnGrabar.Enabled = x;
            btnCancelar.Enabled = x;   //x false 
            btnNuevo.Enabled = !x;     // !x true
            btnEditar.Enabled = !x;
            btnBorrar.Enabled = !x;
            btnSalir.Enabled = !x;
            lstProducto.Enabled = !x;

        }

        private bool ValidadDatos()
        {




            if (txtCodigo.Text == "")
            {
                MessageBox.Show("DEBE INGRESAR EL CODIGO..");
                txtCodigo.Focus();
                return false;
            }

            if (txtPrecio.Text == "")
            {
                MessageBox.Show("DEBE INGRESAR PRECIO..");
                txtPrecio.Focus();
                return false;


            }

            if (txtStock.Text == "")
            {
                MessageBox.Show("DEBE INGRESAR EL STOCK..");
                txtStock.Focus();
                return false;
            }




            if (cboTipoProducto.SelectedIndex == 1)
            {
                MessageBox.Show("DEBE INGRESAR TIPO...");
                cboTipoProducto.Focus();
                return false;

            }

            if (cboSubtipo.SelectedIndex == 1)
            {
                MessageBox.Show("DEBE INGRESAR SUBTIPO...");
               cboSubtipo.Focus();
                return false;

            }


            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            nuevo = true;
            
            habilitar(true);


        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            habilitar(true);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            habilitar(true);

            if (MessageBox.Show("Esta seguro que desea eliminar este producto?", "Eliminando",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                conectarBD();
            {
                Productos pp = new Productos();
                pp.pCodigo = Convert.ToInt32(txtCodigo.Text);

                comando.CommandText = "Delete producto where codigo=" + pp.pCodigo;

                comando.ExecuteNonQuery();
                conexion.Close();
                lector.Close();
                lPro.Clear();
                CargarListaProductos();
            }

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (ValidadDatos())
            {
                Productos pro = new Productos();
                pro.pCodigo = Convert.ToInt32(txtCodigo.Text);
                pro.pPrecio = Convert.ToDouble(txtPrecio.Text);
                pro.pStock = Convert.ToInt32(txtStock.Text);
             
                pro.pTipo = cboTipoProducto.SelectedIndex + 1;
                pro.pSubtipo = cboSubtipo.SelectedIndex + 1;

               
                conectarBD();
                if (nuevo)
                {



                    comando.CommandText = "INSERT INTO PRODUCTOS (id_producto, precio, stock, tipo_producto) values ('" + pro.pCodigo + "', " +
                                                                                                               pro.pPrecio +","+
                                                                                                                pro.pStock + ", " +
                                                                                                                pro.pTipo + ", " +
                                                                                                                pro.pSubtipo + "')";
                    
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    lector.Close();
                    lPro.Clear();
                     CargarListaProductos();


                }

                else
                {
                    txtCodigo.Enabled = false;
                    comando.CommandText = "UPDATE PRODUCTOS SET id_producto= '" + pro.pCodigo + "', " +
                                                                  "precio= " + pro.pPrecio + ", " +
                                                                  "stock= " + pro.pStock + ", " +
                                                                  "tipo_producto= " + pro.pTipo + "," +
                                                                  
                                                           "' WHERE codigo=" + pro.pCodigo;

                    comando.ExecuteNonQuery();
                    conexion.Close();
                    lector.Close();
                    lPro.Clear();
                     CargarListaProductos();
                }
                habilitar(false);
                nuevo = false;
            }
        }
        private void CargarText()
        {
            Productos pc = new Productos();
            txtCodigo.Text = Convert.ToString(lPro[lstProducto.SelectedIndex].pCodigo);
            txtPrecio.Text = Convert.ToString(lPro[lstProducto.SelectedIndex].pPrecio);
            txtStock.Text = Convert.ToString(lPro[lstProducto.SelectedIndex].pStock);
            cboTipoProducto.SelectedIndex = lPro[lstProducto.SelectedIndex].pTipo - 1;

        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            habilitar(false);
            nuevo = false;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarText();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Esta seguro que desea salir?", "Saliendo",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question,
                  MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;

        }
    }
}

