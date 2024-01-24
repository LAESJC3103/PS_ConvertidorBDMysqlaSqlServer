using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;

namespace PSConvertidorBD_Mysql_a_SqlServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //LAP-ALEKSEI\SQLEXPRESS siria  1
            DescromprimirZip(@"Interop.ADOX.zip", Application.StartupPath);
            ComboBoxLlenaDsn(cmbConexionBD);
            comboBox1.SelectedIndex = 0;// comboBox1.Items[0];       
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\EstBDs.mdb"))
            {
                MessageBox.Show("Es necesario agregar el archivo 'EstBDs.mdb'");
                System.Windows.Forms.Application.Exit();
            }
        }

        private Boolean DescromprimirZip(string ArchivoZip, string RutaGuardar)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(ArchivoZip))
                {
                    zip.ExtractAll(RutaGuardar);
                    zip.Dispose();
                }

                return true;
            }
            catch
            {
                return false;

            }
        }


        #region Combo DSN
        private void ComboBoxLlenaDsn(ComboBox Combo)
        {
            System.Collections.SortedList dsnManejador = ObtieneDSNs();
            for (int i = 0; i < dsnManejador.Count; i++)
            {
                string sName = (string)dsnManejador.GetKey(i);
                Combo.Items.Add(sName);

            }
        }
        private enum DataSourceType { System, User }
        public static System.Collections.SortedList ObtieneDSNs()
        {
            System.Collections.SortedList dsnList = new System.Collections.SortedList();

            // get user dsn's
            #region Currentuser
            Microsoft.Win32.RegistryKey reg = (Microsoft.Win32.Registry.CurrentUser).OpenSubKey("Software");
            if (reg != null)
            {
                reg = reg.OpenSubKey("ODBC");
                if (reg != null)
                {
                    reg = reg.OpenSubKey("ODBC.INI");
                    if (reg != null)
                    {
                        reg = reg.OpenSubKey("ODBC Data Sources");
                        if (reg != null)
                        {
                            // Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
                            foreach (string sName in reg.GetValueNames())
                            {
                                if (mysql(sName, false) == true)
                                {
                                    dsnList.Add(sName, DataSourceType.User);
                                }
                            }
                        }
                        try
                        {
                            reg.Close();
                        }
                        catch
                        { /* ignore this exception if we couldn't close */
                        }
                    }
                }
            }
            #endregion


            #region LocalMAchine
            //reg = (Microsoft.Win32.Registry.LocalMachine).OpenSubKey("Software");
            //if (reg != null)
            //{
            //    reg = reg.OpenSubKey("ODBC");
            //    if (reg != null)
            //    {
            //        reg = reg.OpenSubKey("ODBC.INI");
            //        if (reg != null)
            //        {
            //            reg = reg.OpenSubKey("ODBC Data Sources");
            //            if (reg != null)
            //            {
            //                // Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
            //                foreach (string sName in reg.GetValueNames())
            //                {
            //                    if (mysql(sName, true) == true)
            //                    {
            //                        try
            //                        {
            //                            dsnList.Add(sName, DataSourceType.System);
            //                        }
            //                        catch (SyntaxErrorException err)
            //                        {

            //                        }
            //                    }
            //                }
            //            }
            //            try
            //            {
            //                reg.Close();
            //            }
            //            catch
            //            { /* ignore this exception if we couldn't close */
            //            }
            //        }
            //    }
            //}
            #endregion
            //dsnList.Add("Error", "Error");
            //dsnList.Add("Error2", "Error2");
            //dsnList.Add("Error3", "Error3");
            return dsnList;
        }
        public static bool mysql(string clave, bool local)
        {
            string[] arreglo;
            int contador;
            bool accion = false;
            RegistryKey llaveDeRegistro;
            //if (local)
            //{
            //    llaveDeRegistro = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI\" + clave, true);
            //}
            //else
            //{
                llaveDeRegistro = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI\" + clave, true);
            //}
            if (llaveDeRegistro != null)
            {
                string llave = Convert.ToString(llaveDeRegistro.GetValue("Driver"));
                arreglo = llave.Split(Convert.ToChar(@"\"));

                contador = arreglo.Length;

                for (int i = 0; i < contador; i++)
                {
                    if (arreglo[i].Length > 6)
                    {
                        if (arreglo[i].Substring(0, 6) == "myodbc")
                        {
                            accion = true;
                        }
                    }
                }
            }
            return accion;
        }
        #endregion

        #region SqlServer combo
        private string[] instanciasInstaladas()
        {
            RegistryKey rk;
            rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server", false);
            string[] s;
            s = ((string[])rk.GetValue("InstalledInstances"));
            return s;
        }

       
        #endregion
        DataBase.clsBaseDatos BD;
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtInstancia.Text.Length > 0)
            {
                BD = new DataBase.clsBaseDatos();
                ejecuta();
            }
            else
            {
                MessageBox.Show("Faltan datos del servidor sql server.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        string nombre = "";
        private void ejecuta()
        {
            bool bandera = false; string mensaje = "";
            lbltitulo.Text = "";            
            #region nuevo o trasferencia
            if (chkSoloEstructuraBD.Checked)
            {
                if (txtNombreBD.Text.Length > 0)
                {
                    nombre = txtNombreBD.Text;
                    BD.sNombreBD = txtNombreBD.Text;
                    bandera = true;
                }
                else
                {
                    mensaje = "Favor de escribir el nombre de la base de datos.";
                    bandera = false;
                }
            }
            else
            {
                if (cmbConexionBD.Text.Length > 0)
                {
                    BD.ConexionGB(cmbConexionBD.Text);
                    if (BD.ValidaEstructura())
                    {
                        nombre = BD.sNombreBD;
                        bandera = true;
                    }
                    else
                    {
                        nombre = "";
                        mensaje = "El DSN no pertenece a una base de datos global.";
                        bandera = false;
                    }
                }
                else
                {
                    mensaje = "Favor de seleccionar una base de datos.";
                    bandera = false;
                }
            }
            #endregion
            if (bandera)
            {     
                if (BD.MismaVersion() || chkSoloEstructuraBD.Checked)
                {
                    #region Crea Base de dato
                    progressBar1.Value = 0;
                    progressBar1.PerformStep();
                    bool badera = false;
                    if (comboBox1.SelectedIndex == 0)
                    {
                        bandera = false;
                    }
                    else
                    {
                        bandera = true;
                    }
                    BD.activaConexionSqlsever(txtInstancia.Text, txtUsuario.Text, txtPassword.Text, txtNombreDB.Text, bandera);
                    if (true)
                    {
                        progressBar1.Value = 100;
                        progressBar1.PerformStep();
                        if (BD.CreaBaseDatos(progressBar1, chkSoloEstructuraBD.Checked, lbltitulo,txtOrigen,txtDestino))
                        {
                            if (chkInicializadb.Checked) { BD.InicializaBaseDatos(); }                            
                            BD.modificaSucursalAlmacen(txtSucursal.Text,txtDesSucursal.Text,txtAlmacen.Text,txtDesAlmacen.Text,txtRutaPaquete.Text,txtUrlWebService.Text);
                            BD.defaultTablasAC();
                            BD.deshabilitaCajas();
                            progressBar1.Value = progressBar1.Maximum;
                            progressBar1.PerformStep();
                            MessageBox.Show("Base de datos creada.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            BD = null;
                            BD = new DataBase.clsBaseDatos();
                            BD.borrarBaseDatosSqlServer(txtInstancia.Text, nombre);                            
                            MessageBox.Show("No se pudo crear la base de datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                    #endregion
                    }
                    else
                    {
                        MessageBox.Show("La base de datos ya existe.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("La estructura de la base de datos no coincide.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            progressBar1.Value = progressBar1.Maximum;
            progressBar1.PerformStep();
            lbltitulo.Text = "Proceso terminado.";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtNombreBD.Text = "";
            if (chkSoloEstructuraBD.Checked)
            {
                lblConexionBD.Enabled = false;
                cmbConexionBD.Enabled = false;
                label4.Enabled = true;

                txtNombreBD.Enabled = true;
                chkBdDestino.Enabled = false;

                chkBdDestino.Checked = false;
            }
            else
            {
                lblConexionBD.Enabled = true;
                cmbConexionBD.Enabled = true;
                label4.Enabled = false;
                txtNombreBD.Enabled = false;
                chkBdDestino.Enabled = true;
                chkBdDestino.Checked = false; 
            }
        }

        private void btnSalirMenu_MouseEnter(object sender, EventArgs e)
        {
            btnSalirMenu.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.salirMetroOver));
        }

        private void btnSalirMenu_MouseLeave(object sender, EventArgs e)
        {
            btnSalirMenu.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.salirMetro));
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            btnGenerar.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.botonMetro52Over));
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            btnGenerar.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.botonMetro52));
        }

        private void btnSalirMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ovalShape1_MouseEnter(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '\0';
        }

        private void ovalShape1_MouseLeave(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }

        private void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            var bl = !string.IsNullOrEmpty(txtSucursal.Text) && !string.IsNullOrEmpty(txtAlmacen.Text) &&
                !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(cmbConexionBD.Text) &&
                !string.IsNullOrEmpty(txtInstancia.Text) && !string.IsNullOrEmpty(txtPassword.Text) &&
                !string.IsNullOrEmpty(txtDesSucursal.Text) && !string.IsNullOrEmpty(txtDesAlmacen.Text);

            btnGenerar.Enabled = bl;
        }

        private void txtAlmacen_TextChanged(object sender, EventArgs e)
        {
            var bl = !string.IsNullOrEmpty(txtSucursal.Text) && !string.IsNullOrEmpty(txtAlmacen.Text) &&
                !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(cmbConexionBD.Text) &&
                !string.IsNullOrEmpty(txtInstancia.Text) && !string.IsNullOrEmpty(txtPassword.Text) &&
                !string.IsNullOrEmpty(txtDesSucursal.Text) && !string.IsNullOrEmpty(txtDesAlmacen.Text);

            btnGenerar.Enabled = bl;
        }

        private void cmbConexionBD_SelectedIndexChanged(object sender, EventArgs e)
        {
            var bl = !string.IsNullOrEmpty(txtSucursal.Text) && !string.IsNullOrEmpty(txtAlmacen.Text) &&
                !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(cmbConexionBD.Text) &&
                !string.IsNullOrEmpty(txtInstancia.Text) && !string.IsNullOrEmpty(txtPassword.Text) &&
                !string.IsNullOrEmpty(txtDesSucursal.Text) && !string.IsNullOrEmpty(txtDesAlmacen.Text);

            btnGenerar.Enabled = bl;
        }

        private void txtDesSucursal_TextChanged(object sender, EventArgs e)
        {
            var bl = !string.IsNullOrEmpty(txtSucursal.Text) && !string.IsNullOrEmpty(txtAlmacen.Text) &&
                !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(cmbConexionBD.Text) &&
                !string.IsNullOrEmpty(txtInstancia.Text) && !string.IsNullOrEmpty(txtPassword.Text) &&
                !string.IsNullOrEmpty(txtDesSucursal.Text) && !string.IsNullOrEmpty(txtDesAlmacen.Text);

            btnGenerar.Enabled = bl;
        }

        private void txtDesAlmacen_TextChanged(object sender, EventArgs e)
        {
            var bl = !string.IsNullOrEmpty(txtSucursal.Text) && !string.IsNullOrEmpty(txtAlmacen.Text) &&
               !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(cmbConexionBD.Text) &&
               !string.IsNullOrEmpty(txtInstancia.Text) && !string.IsNullOrEmpty(txtPassword.Text) &&
               !string.IsNullOrEmpty(txtDesSucursal.Text) && !string.IsNullOrEmpty(txtDesAlmacen.Text);

            btnGenerar.Enabled = bl;
        }

        private void btnCriterios_Click(object sender, EventArgs e)
        {
            pnlCriterios.Visible = !pnlCriterios.Visible;
            if (pnlCriterios.Visible) 
                btnCriterios.Text = "( - ) Ocultar Criterios";
            else
                btnCriterios.Text = "( + ) Mostrar Criterios";

        }

        private void txtNombreDB_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                txtRutaPaquete.Text = @"C:\inetpub\wwwroot\" + textBox.Text;
            }         
        }
    }
}
