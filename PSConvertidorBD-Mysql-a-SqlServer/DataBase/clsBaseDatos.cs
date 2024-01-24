using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.ProviderBase;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace PSConvertidorBD_Mysql_a_SqlServer.DataBase
{
    class clsBaseDatos
    {
        #region mySQL
        private DbConnection dbConexionGB;
        public string sNombreBD;
        public string servidorSQL;
        public string nombreBDSQL;
        public string usuarioSql;
        public string passwordSql;
        #region Conexiones
        public void ConexionGB(string conexion)
        {
            string DSN = "DSN=" + conexion;
            dbConexionGB = null;
            DbProviderFactory ProveedorBD = null;
            ProveedorBD = DbProviderFactories.GetFactory("System.Data.Odbc");
            dbConexionGB = ProveedorBD.CreateConnection();
            dbConexionGB.ConnectionString = DSN;
            try
            {
                dbConexionGB.Open();
                sNombreBD = dbConexionGB.Database.ToString();
                dbConexionGB.Close();
            }
            catch (Exception e)
            {
                dbConexionGB.Close();
                MessageBox.Show(e.ToString(), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        #endregion

        private void abreConexion()
        {
            dbConexionGB.Open();
        }
        private void cerrarConexion()
        {
            dbConexionGB.Close();
        }

        //Funcinón para ejecutar Insert, Delete y Update
        public bool ejecutaMySQL(string cadena)
        {
            bool respuesta = false;
            if (cadena.Length > 0)
            {
                DbCommand GrabaConfiguracion = null;
                GrabaConfiguracion = dbConexionGB.CreateCommand();
                DbDataReader resultado;
                GrabaConfiguracion.CommandText = cadena;
                abreConexion();
                try
                {
                    GrabaConfiguracion.ExecuteNonQuery();
                    cerrarConexion();
                    respuesta = true;
                }
                catch (Exception e)
                {
                    cerrarConexion();
                    MessageBox.Show(e.ToString(), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    respuesta = false;
                }
            }

            return respuesta;
        }
        //Para obtener los datos para llenado de DataTable
        public DataTable datatableMySQL(string sentencia)
        {
            DbDataAdapter AdaptadorDeDatos;
            DataTable TablaDeDatos;
            DbProviderFactory ProveedorBD = null;
            ProveedorBD = DbProviderFactories.GetFactory("System.Data.Odbc");

            DbCommand ObtieneDatos = ProveedorBD.CreateCommand();
            ObtieneDatos.CommandText = sentencia;

            ObtieneDatos.Connection = dbConexionGB;
           
            AdaptadorDeDatos = ProveedorBD.CreateDataAdapter();
            AdaptadorDeDatos.SelectCommand = ObtieneDatos;

            TablaDeDatos = new DataTable();
            AdaptadorDeDatos.Fill(TablaDeDatos);
            return TablaDeDatos;

        }
        //Función para buscar un dato en de registros
        public string stringMySQL(string sentencia)
        {
            string seleccion = "";
            DbCommand busqueda = null;
            busqueda = dbConexionGB.CreateCommand();

            DbDataReader resultado;
            busqueda.CommandText = sentencia;
            abreConexion();
            try
            {
                resultado = busqueda.ExecuteReader(); // ejecuta sentencia

                while (resultado.Read())
                {
                    seleccion = resultado.GetValue(0).ToString();

                }
                cerrarConexion();
            }
            catch (Exception e)
            {
                cerrarConexion();
                MessageBox.Show(e.ToString(), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return seleccion;
        }
        #endregion

        #region Access
        //Conexion para Delfín
        private OleDbConnection ConexionEstBDs()
        {
            System.Data.OleDb.OleDbConnection cnn = new System.Data.OleDb.OleDbConnection();
            cnn.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\EstBDs.mdb";
            return cnn;
        }
       
        //Para obtener los datos para llenado de DataTable Delfín OLEDBC
        private DataTable ObtieneDatosParaDataTableDelfin(string sentencia, OleDbConnection conexion)
        {
            DbDataAdapter AdaptadorDeDatos;
            DataTable TablaDeDatos;
            DbProviderFactory ProveedorBD = null;
            ProveedorBD = DbProviderFactories.GetFactory("System.Data.OleDb");

            DbCommand ObtieneDatos = ProveedorBD.CreateCommand();
            ObtieneDatos.CommandText = sentencia;
            ObtieneDatos.Connection = conexion;

            AdaptadorDeDatos = ProveedorBD.CreateDataAdapter();
            AdaptadorDeDatos.SelectCommand = ObtieneDatos;

            TablaDeDatos = new DataTable();
            AdaptadorDeDatos.Fill(TablaDeDatos);
            return TablaDeDatos;

        }
        // Cuentra registros en una tabla de OLEDB
        private string CuentaRegistrosOLE(string sentencia, OleDbConnection conexion)
        {
            string dato = "";
            DbCommand BuscaId = conexion.CreateCommand();
            DbDataReader resultado;
            BuscaId.CommandText = sentencia;
            conexion.Open();
            resultado = BuscaId.ExecuteReader();
            while (resultado.Read())
            {
                dato = resultado.GetValue(0).ToString();
            }
            conexion.Close();

            return dato;
        }
        // Cuentra registros INSERT, DELETE, UPDATE
        private bool modificaRegistrosOLE(string sentencia, OleDbConnection conexion)
        {
            try
            {
                DbCommand BuscaId = conexion.CreateCommand();
                DbDataReader resultado;
                BuscaId.CommandText = sentencia;
                conexion.Open();
                BuscaId.ExecuteNonQuery();
                conexion.Close();
                return true;
            }
            catch (SyntaxErrorException err)
            {
                conexion.Close();
                return false;
            }
        }

        private OleDbConnection dbDelfin;
        public void activaConexionAcces()
        {
            dbDelfin = ConexionEstBDs();
        }

        public DataTable datatableDelfin(string sentencia)
        {
            return ObtieneDatosParaDataTableDelfin(sentencia, dbDelfin);
        }

        public string stringDelfin(string sentencia)
        {
            return CuentaRegistrosOLE(sentencia, dbDelfin);
        }

        public bool ejecutaDelfin(string sentencia)
        {
            return modificaRegistrosOLE(sentencia, dbDelfin);
        }
        #endregion

        #region SqlServer
        private SqlConnection ConexionSqlServer, ConexionSqlServer1;
        public void activaConexionSqlsever(string servidor, string user, string pass,string bd, bool esWindows)
        {            
            if (esWindows)
            {
                ConexionSqlServer = new SqlConnection("Data Source=" + servidor + ";Integrated Security=SSPI;");//User Id=" + user + "; Password=" + pass + ";");//("Data Source="+ servidor +";Integrated Security=True;");
                ConexionSqlServer1 = new SqlConnection("Data Source=" + servidor + ";Initial Catalog=" + bd + ";Integrated Security=SSPI;");//User Id=" + user + "; Password=" + pass + ";");
            }
            else
            {
                servidorSQL = servidor; usuarioSql = user; passwordSql = pass;
                ConexionSqlServer = new SqlConnection("Data Source=" + servidor + ";User Id=" + user + "; Password=" + pass + ";");//("Data Source="+ servidor +";Integrated Security=True;");
                ConexionSqlServer1 = new SqlConnection("Data Source=" + servidor + ";Initial Catalog=" + bd + ";User Id=" + user + "; Password=" + pass + ";");
            }

            //ConexionSqlServer1 = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog = AC_CREMERIA2000;Integrated Security=True;");
        }

        public bool creaBaseDatosSqlServer(bool creaBaseDatos)
        {
            try
            {
                if (creaBaseDatos)
                {
                    string sentencia = "create database " + sNombreBD; //+ sNombreBD;
                    SqlCommand myCommand = new SqlCommand(sentencia, ConexionSqlServer);
                    ConexionSqlServer.Open();
                    myCommand.ExecuteNonQuery();
                    ConexionSqlServer.Close();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message.ToString());
                ConexionSqlServer.Close();
                return true;
            }
        }

        public bool borrarBaseDatosSqlServer(string servidor, string sNombreBD)
        {
            SqlConnection ConexionSqlServer2 = new SqlConnection("Data Source=" + servidor + ";Integrated Security=True;");
            string sentencia = "drop database " + sNombreBD; //+ sNombreBD;
            sentencia = @"ALTER DATABASE " + sNombreBD + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + sNombreBD + "]";
            SqlCommand myCommand = new SqlCommand(sentencia, ConexionSqlServer2);
            try
            {
                ConexionSqlServer2.Open();
                myCommand.ExecuteNonQuery();
                ConexionSqlServer2.Close();
                return true;
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message.ToString());

                ConexionSqlServer2.Close();
                return false;
            }
        }

        public bool ejecutaSqlServer(string sentencia)
        {
            SqlCommand myCommand = new SqlCommand(@sentencia, ConexionSqlServer1);
            try
            {
                ConexionSqlServer1.Open();
                myCommand.ExecuteNonQuery();
                ConexionSqlServer1.Close();
                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString()  + Environment.NewLine + sentencia);
                ConexionSqlServer1.Close();
                return false;
            }
        }

        public bool ejecutaSqlServerNoME(string sentencia)
        {
            SqlCommand myCommand = new SqlCommand(@sentencia, ConexionSqlServer1);
            try
            {
                ConexionSqlServer1.Open();
                myCommand.ExecuteNonQuery();
                ConexionSqlServer1.Close();
                return true;
            }
            catch (Exception err)
            {                
                ConexionSqlServer1.Close();
                return false;
            }
        }

        public DataTable datatableBD(string sentencia)
        {
            DbDataAdapter AdaptadorDeDatos;
            DataTable TablaDeDatos = new DataTable();
            DbProviderFactory ProveedorBD = null;
            string defineProvider = "System.Data.SqlClient";
            try
            {
                ProveedorBD = DbProviderFactories.GetFactory(defineProvider);

                DbCommand ObtieneDatos = null;
                ObtieneDatos = ProveedorBD.CreateCommand();
                ObtieneDatos.CommandText = sentencia;
                ObtieneDatos.Connection = ConexionSqlServer1; 

                AdaptadorDeDatos = ProveedorBD.CreateDataAdapter();
                AdaptadorDeDatos.SelectCommand = ObtieneDatos;

                TablaDeDatos = new DataTable();
                AdaptadorDeDatos.Fill(TablaDeDatos);
            }
            catch (Exception e)
            {
                //mensaje = e.Message + " --- " + e.InnerException + " --- " + sentencia;
            }
            finally
            {
               // cerrarConexion();
            }
            return TablaDeDatos;
        }



        #endregion

        #region variablesG
        DataTable rstTablas, rstCampos, rstIndice, rstCampoIndex;
        private  ADOX.Index idxNvaIndex = new ADOX.Index();
#endregion

        public bool CreaBaseDatos(ProgressBar barras, bool DatosDefaul, Label mensaje,TextBox txtOrigen,TextBox txtDestino)
        {
            #region Variable
            string sNomTabla = "";
            string sNumBDTabla = "";
            int iNumTabla = 0;
            string sSqlCrear = "";
            string sNomCampo;
            string sNomIndice;
            string sMensaj;
            int lNumIndice;
            int lNumCampo;
            string sValores;
            string sNumRegis;
            int iTipoCam;

            int iNumTablas;
            int iNumCampos;

            DataTable rstShowDatabase;
            DataTable rstShowTables;
            DataTable rstCamposTbl;
            DataTable rstShowIndices;
            DataTable rstDeBusquedas;
            //Dim fldAgregaCampo   As New adox.Column

            string sUniqueIdx;
            string sPrimaryIdx;
            bool bExisteBD;
            bool resultado = false;
            string sqlFiltroBDs;
            #endregion
           
            int barra = 100, incrementa = 0;
            #region BD
            activaConexionAcces();
            mensaje.Location = new System.Drawing.Point(35, 547);
            mensaje.BackColor = System.Drawing.Color.Transparent;          
            sqlFiltroBDs = "BD NOT IN(5,10,12)";// 11 es la AC
            rstTablas = datatableDelfin("SELECT NUMERO,NOMBRE,BD FROM tblTablas WHERE " + sqlFiltroBDs + " ORDER BY NOMBRE;");
            
            barras.Maximum = (barra + rstTablas.Rows.Count + 100);
            foreach (DataRow rowrstTablas in rstTablas.Rows)
            {
                #region Tablas
                iNumTabla = Convert.ToInt32(rowrstTablas["Numero"].ToString());
                sNomTabla = rowrstTablas["Nombre"].ToString();
                sNumBDTabla = rowrstTablas["BD"].ToString();
                mensaje.Text = "Creando la tabla: " + sNomTabla; mensaje.Refresh();              


                rstCampos = datatableDelfin("SELECT * FROM tblCamposTabla WHERE TABLA=" + iNumTabla + " ORDER BY NUMERO;");
                foreach (DataRow rowrstCampos in rstCampos.Rows)
                {                   
                    sNomCampo = rowrstCampos["Nombre"].ToString().Trim();
                    sNomCampo = sNomCampo + " " + TipoDeDatoSQL(sNomCampo,Convert.ToInt32(rowrstCampos["Tipo"].ToString()), Convert.ToInt32(rowrstCampos["Valor"].ToString()));
                    sSqlCrear = sSqlCrear + "," + sNomCampo;
                }

                sSqlCrear = sSqlCrear.Substring(1);

                //sSqlCrear.Replace("(13,4)", "(17,4)")

                string[] arrLineasOrigen = txtOrigen.Text.Split((char)13);
                string[] arrLineasDestino = txtDestino.Text.Split((char)13);

                int iLineasOrigen = arrLineasOrigen.Count();
                int iLineasDestino = arrLineasDestino.Count();

                if (iLineasOrigen == iLineasDestino)
                {
                    for (int i = 0; i < iLineasOrigen; i++)
                    {
                        if (arrLineasOrigen[i] != String.Empty && arrLineasDestino[i] != String.Empty) {
                            sSqlCrear = sSqlCrear.Replace(arrLineasOrigen[i], arrLineasDestino[i]);
                        }
                    }

                    ejecutaSqlServer("if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '" + sNomTabla + "')" +
                                        " drop table " + sNomTabla);//DROP TABLE " + sNomTabla);
                    resultado = ejecutaSqlServer("CREATE TABLE " + sNomTabla + " (" + sSqlCrear + ");");
                }
                else { resultado = false; }
                if (resultado == true)
                {
                    sSqlCrear = "";
                    //-----------------
                    rstIndice = datatableDelfin("SELECT * FROM tblIndices WHERE TABLA=" + iNumTabla + ";");

                    foreach (DataRow rowrstIndice in rstIndice.Rows)
                    {
                        #region Indice
                        lNumIndice = Convert.ToInt32(rowrstIndice["Numero"].ToString());
                        sNomIndice = rowrstIndice["Nombre"].ToString();
                        idxNvaIndex.Name = sNomIndice;
                        idxNvaIndex.IndexNulls = ADOX.AllowNullsEnum.adIndexNullsIgnore;

                        if (Convert.ToBoolean(rowrstIndice["PRINCIPAL"].ToString()))
                        {
                            sPrimaryIdx = "PRIMARY";
                        }
                        else
                        {
                            sPrimaryIdx = "";
                        }

                        if (Convert.ToBoolean(rowrstIndice["UNICA"].ToString()))
                        {
                            sUniqueIdx = "UNIQUE";
                        }
                        else
                        {
                            sUniqueIdx = "";
                        }

                        sNomCampo = "";
                        rstCampoIndex = datatableDelfin("SELECT CAMPO FROM tblCamposIndice WHERE INDICE=" + lNumIndice + ";");

                        foreach (DataRow rowrstCampoIndex in rstCampoIndex.Rows)
                        {
                            lNumCampo = Convert.ToInt32(rowrstCampoIndex["CAMPO"].ToString());
                            rstCampos = datatableDelfin("SELECT NOMBRE FROM tblCamposTabla WHERE NUMERO=" + lNumCampo + ";");
                            foreach (DataRow rowrstCampos in rstCampos.Rows)
                            {
                                sNomCampo = sNomCampo + "," + rowrstCampos["Nombre"].ToString().Trim();
                            }
                        }
                        sNomCampo = sNomCampo.Substring(1);

                        if (sPrimaryIdx == "" && sUniqueIdx == "")
                        {
                            resultado = ejecutaSqlServer("CREATE INDEX " + sNomIndice + " ON " + sNomTabla + " (" + sNomCampo + ");");
                            if (resultado == false) { break; }
                        }
                        else if (sPrimaryIdx != "" && sUniqueIdx != "") {
                            //resultado = ejecutaSqlServer("ALTER TABLE " + sNomTabla + " ADD PRIMARY KEY(" + sNomCampo + ")");
                            //resultado = ejecutaSqlServer("CREATE " + sPrimaryIdx + " INDEX " + sNomIndice + " ON " + sNomTabla + " (" + sNomCampo + ");");
                            //if (resultado == false) { break; }
                            resultado = ejecutaSqlServer("CREATE " + sUniqueIdx + " INDEX " + sNomIndice + " ON " + sNomTabla + " (" + sNomCampo + ");");
                            if (resultado == false) { break; }
                        }
                        else if(sPrimaryIdx != "" && sUniqueIdx == "")
                        {
                            //resultado = ejecutaSqlServer("ALTER TABLE " + sNomTabla + " ADD PRIMARY KEY(" + sNomCampo + ")");
                            //if (resultado == false) { break; }
                        }
                        else if (sPrimaryIdx == "" && sUniqueIdx != "") {
                            resultado = ejecutaSqlServer("CREATE " + sUniqueIdx + " INDEX " + sNomIndice + " ON " + sNomTabla + " (" + sNomCampo + ");");
                            if (resultado == false) { break; }
                        }                        
                        //resultado = ejecutaSqlServer("CREATE " + sUniqueIdx + " INDEX " + sNomIndice + " ON " + sNomTabla + " (" + sNomCampo + ");");
                        //if (resultado == false)
                        //{
                        //  break;
                        //}
                        #endregion
                    }
                    //-------------------------------

                    if (DatosDefaul && resultado)
                    {
                        #region NuevosDatos
                        DataTable rstTablasAdd;
                        DataTable rstNumRegistro;
                        DataTable rstDefaultCampos;

                        rstTablasAdd = datatableDelfin("SELECT NUMERO,NOMBRE FROM tblTablas WHERE NUMERO=" + iNumTabla + ";");
                        foreach (DataRow rowrstTablasAdd in rstTablasAdd.Rows)
                        {
                            #region Default
                            iNumTabla = Convert.ToInt32(rowrstTablasAdd["Numero"].ToString());
                            sNomTabla = rowrstTablasAdd["Nombre"].ToString();
                            sNomCampo = ""; sValores = "";
                            if (sNomTabla != "tblCatAlmacenes" && sNomTabla != "tblPVConfigura")
                            {
                                rstNumRegistro = datatableDelfin("SELECT DISTINCT REGISTRO FROM (tblTablas INNER JOIN tblCamposTabla ON tblTablas.Numero = tblCamposTabla.Tabla) INNER JOIN tblDefaultCampos ON tblCamposTabla.Numero = tblDefaultCampos.Campo WHERE tblTablas.Numero=" + iNumTabla + ";");
                                foreach (DataRow rowrstNumRegistro in rstNumRegistro.Rows)
                                {
                                    sNumRegis = rowrstNumRegistro["Registro"].ToString();
                                    sNomCampo = ""; sValores = "";
                                    rstDefaultCampos = datatableDelfin("SELECT * FROM (tblTablas INNER JOIN tblCamposTabla ON tblTablas.Numero = tblCamposTabla.Tabla) INNER JOIN tblDefaultCampos ON tblCamposTabla.Numero = tblDefaultCampos.Campo WHERE tblTablas.Numero=" + iNumTabla + " AND REGISTRO='" + sNumRegis + "';");
                                    foreach (DataRow rowrstDefaultCampos in rstDefaultCampos.Rows)
                                    {
                                        sNomCampo = sNomCampo + "," + rowrstDefaultCampos["tblCamposTabla.Nombre"].ToString().Trim(); //Trim$(.Fields("tblCamposTabla.Nombre").Value);
                                        if (rowrstDefaultCampos["tblDefaultCampos.Valor"].ToString().IndexOf("000") > -1)
                                        {
                                            sValores = sValores + ",'" + rowrstDefaultCampos["tblDefaultCampos.Valor"].ToString() + "'";
                                        }
                                        else
                                        {
                                            if (IsNumeric(rowrstDefaultCampos["tblDefaultCampos.Valor"].ToString()))
                                            {
                                                if (rowrstDefaultCampos["tblCamposTabla.Nombre"].ToString() == "AI" || rowrstDefaultCampos["tblCamposTabla.Nombre"].ToString() == "FOL_FOLIO" || rowrstDefaultCampos["tblCamposTabla.Nombre"].ToString() == "CVE_CFDI")
                                                {
                                                    sValores = sValores + ",'" + rowrstDefaultCampos["tblDefaultCampos.Valor"].ToString() + "'";
                                                }
                                                else
                                                {
                                                    sValores = sValores + "," + rowrstDefaultCampos["tblDefaultCampos.Valor"].ToString();
                                                }

                                            }
                                            else
                                            {
                                                sValores = sValores + ",'" + rowrstDefaultCampos["tblDefaultCampos.Valor"].ToString() + "'";
                                            }
                                        }
                                    }
                                    sNomCampo = sNomCampo.Substring(1);
                                    sValores = sValores.Substring(1);
                                    if (sNomCampo != "" && sValores != "")
                                    {
                                      resultado =  ejecutaSqlServer("INSERT INTO " + sNomTabla + " (" + sNomCampo + ") VALUES (" + sValores + ");");
                                    }
                                    if (resultado == false)
                                    {
                                        break;
                                    }
                                }
                            }
                            #endregion
                            if (resultado == false)
                            {
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region DatosMysql
                        resultado = AgregaDatos(sNomTabla,sNumBDTabla);
                        #endregion
                    }
                #endregion
                    barra = barra + 1;
                    barras.Value = barra;
                    barras.PerformStep();
                    WaitSeconds(1);
                }
                if (resultado == false)
                {
                    //borrarBaseDatosSqlServer();
                    break;
                }
            }
            #endregion
            //Leer Archivo 28
            //if (resultado)
            //{
            //    StreamReader sr = new StreamReader(Application.StartupPath + "\\script.sql");
            //    string line = sr.ReadToEnd();
            //    Regex regex = new Regex(@"GO\r\n");
            //    string[] funcion = regex.Split(line);
            //    mensaje.Text = "Creando Store Procedure";
            //    foreach (string row in funcion)
            //    {
            //        if (row.Length > 0)
            //        {
            //            ejecutaSqlServer(row);
            //        }
            //    }
            //}

            return resultado;
        }
        public bool InicializaBaseDatos()
        {
            bool resultado = false;          
            DataTable dtValidaTabla = new DataTable();

            resultado = ejecutaSqlServer("DELETE FROM tblBitacoraAntibioticos;");
            resultado = ejecutaSqlServer("DELETE FROM tblBitacoraCDSO;");
            resultado = ejecutaSqlServer("DELETE FROM tblBitacoraFactWeb;");
            resultado = ejecutaSqlServer("DELETE FROM tblPedimentosVtas;");            

            //dtValidaTabla = datatableMySQL("SHOW TABLES LIKE 'tblDp_Replicar';");
            //if (dtValidaTabla.Rows.Count > 0)
            //{
            //    resultado = ejecutaSqlServer("DELETE FROM tblDp_Replicar;");
            //}   

            resultado = ejecutaSqlServer("DELETE FROM tblGralAlmacen;");
            resultado = ejecutaSqlServer("DELETE FROM tblRenAlmacen;");
            resultado = ejecutaSqlServer("DELETE FROM tblCostos;");
            resultado = ejecutaSqlServer("DELETE FROM tblPromedios;");
            resultado = ejecutaSqlServer("DELETE FROM tblAdicionalCapas;");
            
            dtValidaTabla = datatableMySQL("SELECT * FROM tblExiPorAlmacen;");
            if (dtValidaTabla.Rows.Count > 0)
            {            
                resultado = ejecutaSqlServer("UPDATE tblExiPorAlmacen SET EXI_ALM=0;");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblCatArticulos;");
            if (dtValidaTabla.Rows.Count > 0)
            {    
                resultado = ejecutaSqlServer("UPDATE tblCatArticulos SET EXI_ACT=0;");
            }                      

            resultado = ejecutaSqlServer("DELETE FROM tblEncInvFisico;");
            resultado = ejecutaSqlServer("DELETE FROM tblRenInvFisico;");
            resultado = ejecutaSqlServer("DELETE FROM tblEncCargosAbonos;");
            resultado = ejecutaSqlServer("DELETE FROM tblRenCargosAbonos;");

            dtValidaTabla = datatableMySQL("SELECT * FROM tblCatClientes;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblCatClientes SET SAL_CLI=0;");
            }
            

            resultado = ejecutaSqlServer("DELETE FROM tblGralVentas;");
            resultado = ejecutaSqlServer("DELETE FROM tblRenVentas;");
            resultado = ejecutaSqlServer("DELETE FROM tblFacturasEnc;");
            resultado = ejecutaSqlServer("DELETE FROM tblFacturasRen;");

            resultado = ejecutaSqlServer("DELETE FROM tblRenDevolucion;");
            resultado = ejecutaSqlServer("DELETE FROM tblEncPedidos;");
            resultado = ejecutaSqlServer("DELETE FROM tblRenPedidos;");
            resultado = ejecutaSqlServer("DELETE FROM tblNotasPorFactura;");
            resultado = ejecutaSqlServer("DELETE FROM tblRenMesas;");
            resultado = ejecutaSqlServer("DELETE FROM tblComandas;");
            resultado = ejecutaSqlServer("DELETE FROM tblModifComanda;");


            dtValidaTabla = datatableMySQL("SELECT * FROM tblMesas;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblMesas SET ESTADO=0,NUM_COM=1,ABIERTA=0,CUENTAS=0,ADJ_MES='',NUM_PER=0, NOMBRE='';");
            }

            resultado = ejecutaSqlServer("DELETE FROM tblPersxMesa;");
            resultado = ejecutaSqlServer("DELETE FROM tblAuxCaja;");
            resultado = ejecutaSqlServer("DELETE FROM tblCortes;");
            resultado = ejecutaSqlServer("DELETE FROM tblArqueo;");

            dtValidaTabla = datatableMySQL("SELECT * FROM tblCampRedondeo;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblCampRedondeo SET IMPORTE=0;");
            }
            
            dtValidaTabla = datatableMySQL("SELECT * FROM tblCajas;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblCajas SET TUR_CAJ=1;");
            }
            
            resultado = ejecutaSqlServer("DELETE FROM tblEncuestaResp;");
            resultado = ejecutaSqlServer("DELETE FROM tblApartadosEnc;");

            resultado = ejecutaSqlServer("DELETE FROM tblApartadosRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblFacturaElectronica;");
            resultado = ejecutaSqlServer("DELETE FROM tblFEPendientes;");
            resultado = ejecutaSqlServer("DELETE FROM tblFEDescripciones;");
            resultado = ejecutaSqlServer("DELETE FROM tblFacturasServ;");
            resultado = ejecutaSqlServer("DELETE FROM tblBitacora;");
            resultado = ejecutaSqlServer("DELETE FROM tblBitacoraPV;");
            resultado = ejecutaSqlServer("DELETE FROM tblBitacoraTA;");

            resultado = ejecutaSqlServer("DELETE FROM tblBitacoraComanda;");
            resultado = ejecutaSqlServer("DELETE FROM tblBitacoraTC;");

            dtValidaTabla = datatableMySQL("SELECT * FROM tblCatProveedor;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblCatProveedor SET SAL_PROV=0;");
            }
            
            resultado = ejecutaSqlServer("DELETE FROM tblCxPEnc;");
            resultado = ejecutaSqlServer("DELETE FROM tblCxPRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblCotizacionesEnc;");
            resultado = ejecutaSqlServer("DELETE FROM tblCotizacionesRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblDevolucionEnc;");

            resultado = ejecutaSqlServer("DELETE FROM tblDevolucionRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblOCOMEnc;");
            resultado = ejecutaSqlServer("DELETE FROM tblOCOMRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblComprasEnc;");
            resultado = ejecutaSqlServer("DELETE FROM tblComprasRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblRequisicionesEnc;");
            resultado = ejecutaSqlServer("DELETE FROM tblRequisicionesRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblCargosDocs;");

            resultado = ejecutaSqlServer("DELETE FROM tblDefPresupuesto;");
            resultado = ejecutaSqlServer("DELETE FROM tblDefPresupuestoRen;");

            dtValidaTabla = datatableMySQL("SELECT * FROM tblCatCtasBancos;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblCatCtasBancos SET SAL_ACTUAL=0, SAL_INICIAL=0;");
            }

            resultado = ejecutaSqlServer("DELETE FROM tblGastosEnc;");
            resultado = ejecutaSqlServer("DELETE FROM tblGastosRen;");
            resultado = ejecutaSqlServer("DELETE FROM tblIngreEgre;");
            resultado = ejecutaSqlServer("DELETE FROM tblReposicionCaja;");
            resultado = ejecutaSqlServer("DELETE FROM tblReposicionCajaMov;");

            resultado = ejecutaSqlServer("DELETE FROM tblConciliaciones;");
            resultado = ejecutaSqlServer("DELETE FROM tblConciliaEnc;");

            resultado = ejecutaSqlServer("DELETE FROM tblEncPedidosRest;");
            resultado = ejecutaSqlServer("DELETE FROM tblRenPedidosRest;");
            resultado = ejecutaSqlServer("DELETE FROM tblPropinas;");         

            dtValidaTabla = datatableMySQL("SELECT * FROM tblEmpresa;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblEmpresa SET FOLIO_NV ='0000000001';");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblEmpresa;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblEmpresa SET FOLIO_FACT ='0000000001';");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblTipoConcepto;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblTipoConcepto SET FOL_TIP ='0000000001', FOL_EXTRA ='-000000001';");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblConceptos;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblConceptos SET FOL_CON ='0000000001', FOL_EXTRA ='-000000001'");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblFolioGralesCom;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblFolioGralesCom SET FOL_FOL ='0000000001', FOL_EXTRA ='-000000001';");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblConceptosCom;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblConceptosCom SET FOL_CON ='0000000001', FOL_EXTRA ='-000000001';");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblFolioGralesGyT;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblFolioGralesGyT SET FOL_FOL ='0000000001', FOL_EXTRA ='-000000001';");
            }

            dtValidaTabla = datatableMySQL("SELECT * FROM tblConceptosGyT;");
            if (dtValidaTabla.Rows.Count > 0)
            {
                resultado = ejecutaSqlServer("UPDATE tblConceptosGyT SET FOL_CON ='0000000001', FOL_EXTRA ='-000000001';");
            }
            
            return true;
        }
        public bool modificaSucursalAlmacen(string codSucursal,string desSucursal,string codAlmacen,string desAlmacen,string rutaPaquete,string urlWebservice)
        {
            bool resultado = false;
            DataTable dtValidaTabla = new DataTable();

            try
            {
                //INSERT UPDATE EN tblSucursales
                resultado = ejecutaSqlServer("UPDATE tblSucursales SET ES_PRINCIPAL ='0';");
                dtValidaTabla = datatableMySQL("SELECT * FROM tblSucursales WHERE COD_SUCU ='" + codSucursal + "';");

                if (dtValidaTabla.Rows.Count > 0)
                {
                    resultado = ejecutaSqlServer("UPDATE tblSucursales SET DES_SUCU ='" + desSucursal + "',ES_PRINCIPAL ='1' WHERE COD_SUCU ='" + codSucursal + "';");
                }
                else
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblSucursales (COD_SUCU,DES_SUCU,ES_PRINCIPAL) VALUES ('" + codSucursal + "','" + desSucursal + "','1');");
                }

                //INSERT UPDATE EN tblCatAlmacenes
                dtValidaTabla = datatableMySQL("SELECT * FROM tblCatAlmacenes WHERE COD_ALM  ='" + codAlmacen + "';");
                if (dtValidaTabla.Rows.Count > 0)
                {
                    resultado = ejecutaSqlServer("UPDATE tblCatAlmacenes SET DES_ALM ='" + desAlmacen + "',COD_TIP ='1',COD_SUCU ='" + codSucursal + "',SIN_EXIST='1',PROT_MAX='0',AP_ALM='0' WHERE COD_ALM  ='" + codAlmacen + "';");
                }
                else
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblCatAlmacenes (COD_ALM,DES_ALM,COD_TIP,COD_SUCU,SIN_EXIST,PROT_MAX,AP_ALM,INV_CICLICO,DIA_CICLICO,CANT_CICLICO,PORC_SINMOV,ULTIMO_CICLICO,SAL_SINEXIST,SUBCUENTA) VALUES ('" + codAlmacen + "','" + desAlmacen + "',1,'" + codSucursal + "',1,0,0,0,0,0,0,'18000101',1,'');");
                }

                //INSERT UPDATE EN tblParametrosGenerales
                dtValidaTabla = datatableMySQL("SELECT * FROM tblParametrosGenerales;");
                if (dtValidaTabla.Rows.Count > 0)
                {
                    resultado = ejecutaSqlServer("UPDATE tblParametrosGenerales SET COD_SUCU  ='" + codSucursal + "'");
                }
                else
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblParametrosGenerales (COD_SUCU,FACT_MOSTRA,FACT_POREMP,NOTA_FACTURA,CAMP_REDN,CAD_ACT,DIAS_CAD,HORAS_CAD,LIM_RENVTA,PROM_TOLERA,MOD_FECPED,CP_RESTPRES,LDV_CLIENTE,LDA,LDC_TIPODOC,LDC_MONEDA,LDC_SUCURSAL,LDC_PRESUP,TC_SERVICIO,TC_IDSITIO,TC_KEY,TC_EMPRESA,TA_ACT,TA_AGR,TA_IMPTO,TA_GRUPO,TA_CADENA,TA_TIENDA,TA_URL,TA_FTP,CFD_FACT,CONX_CXC,BD_CXC,CONX_AC,BD_AC,CONX_DP,BD_DP,TC_URL,FE_DESCTO,FE_ORIGEN,FE_DESCUND,CP_PREVIO,CP_RECPREV,LOT_SAL,FE_LOT,FE_CAD,PRECIO_BASE,PRECIO_DECIMAL,PRECIO_TRUNCAR,REP_NOMEMP,REP_NOMCOM,PV_PRINTREG,PV_COSTOREP,FE_FOLIONUM,FE_RUTAXML,FE_VENCERT,FE_WEBSERVURL,FE_EMPRESA,FE_DOMINIO,FE_USUARIO,FE_CONTRASENA,FE_EMAILPV,FE_MENSAJE,ACT_PRECOM,PREXLISTA,UND_PRECOM,ACT_CIN,CONX_CIN,OBLIGA_BASC,FE_LLAVE,FE_CERTIFICADO,FE_ZONAHORA,FE_PAC,FE_LLAVEPWD,FE_PFX,CNX_PSE,IMPTO_SER,FF_MESERO,FF_NUMPERS,FE_CANCELASAT,FF_SINMESERO,FF_ORDENTOT,PSE_CODPROV,ME_CODPROV) VALUES ('" + codSucursal + "',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'','','',0,0,0,0,0,0,'https:\\h2h.m3rcurio.com','',0,'Provider=SQLOLEDB.1; Password= " + passwordSql + "; Persist Security Info=True; User ID= " + usuarioSql + " Initial Catalog= " + ConexionSqlServer1.Database.ToString() + "; Data Source= " + ConexionSqlServer1.DataSource.ToString() + "','" + ConexionSqlServer1.Database.ToString() + "','','','','','',1,0,0,0,0,0,0,0,0,4,0,1,1,1,0,0,''," + DateTime.Now.ToString("yyyy-MM-dd") + ",''," + "'','','','','','',0,0,0,0,'',0,'','','',0,'','','',0,0,0,0,0,0,0,0);");
                }

                dtValidaTabla = datatableMySQL("SHOW TABLES LIKE 'tblAC_Config';");
                if (dtValidaTabla.Rows.Count > 0)
                {
                    resultado = ejecutaSqlServer("UPDATE tblAC_Config SET Ruta_Paquetes  ='" + rutaPaquete + "',Cnx_WebService ='" + urlWebservice + "'");
                }
                else
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Config (Cnx_BDCentral,Cnx_BDDependencia,Ruta_Paquetes,Cen_Parametros,Cen_Usuarios,Cen_Folios,Cen_Monedas,Cen_Impuestos,Cen_FormasPago,Cen_Vendedores,Cen_Almacenes,Act_Formatos,Act_Parametros,Act_Precios,Act_Catalogos,Act_Perfiles,DisparadorManual,Cnx_WebService) VALUES ('','','" + rutaPaquete + "',0,0,0,0,0,0,0,0,0,1,1,1,1,1,'" + urlWebservice + "');");
                }

                dtValidaTabla = datatableMySQL("SHOW TABLES LIKE 'tblAC_Zonas';");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Zonas (Id_Zona,NombreZona) VALUES (1,'UNICA');");
                }

                dtValidaTabla = datatableMySQL("SHOW TABLES LIKE 'tblAC_UnidadesNegocio';");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_UnidadesNegocio (Id_UnidadNegocio,NombreUnidadNegocio,Id_Zona) VALUES (1,'UEN UNICA',1);");
                }

                dtValidaTabla = datatableMySQL("SHOW TABLES LIKE 'tblAC_Sedes';");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Sedes (Id_Sede,NombreSede,Id_UnidadNegocio) VALUES (1,'SEDE UNICA',1);");
                }
            }
            catch (Exception Ex)
            {

            }

            return true;                                                                                                                                     
        }

        public bool defaultTablasAC()
        {
            bool resultado = false;
            DataTable dtValidaTabla = new DataTable();

            //DELETE EN tblAC_Perfiles,tblAC_DpDisparador 
            resultado = ejecutaSqlServer("DELETE FROM tblAC_Perfiles;");
            resultado = ejecutaSqlServer("DELETE FROM tblAC_DpDisparador;");
            resultado = ejecutaSqlServer("DELETE FROM tblAC_PlantillaMenus;");

            //INSERT EN tblAC_Perfiles,tblAC_DpDisparador
            resultado = ejecutaSqlServer("INSERT INTO tblAC_Perfiles (Id_Perfil,Descripcion_Perfil) VALUES (0, 'SUPERUSUARIO'), (1, 'EJECUTIVO'), (2,'SUPERVISOR'), (3,'OPERATIVO'), (4,'ADMINISTRADOR')");
            resultado = ejecutaSqlServer("INSERT INTO tblAC_DpDisparador (EventoPV,PorTiempo,PorDia,PorSemana,EnLinea,Evento_Activo) VALUES (3,0,'01:00:00',0,0,0);");

            //INSERT EN tblAC_PlantillaMenus
            resultado = ejecutaSqlServer("INSERT tblAC_PlantillaMenus (Id_Perfil,Orden_Menu,Id_Opcion,Nombre_Opcion,EsRama_De) VALUES (0, 101, 'btn', 'CATALOGOS', 0), (0, 102, 'btn', 'ACTUALIZA_DP', 0),(0, 103, 'btn', 'DEPENDENCIAS', 0),(0, 104, 'btn', 'CONFIGURACION_AC', 0),(0, 105, 'btn', 'Subir_Paquete', 0),(0, 106, 'btn', 'Consultas', 0),(0, 1011, 'btn', 'Articulos', 0),(0, 1012, 'btn', 'Clientes', 0),(0, 1013, 'btn', 'Restricciones', 0),(0, 1014, 'btn', 'Precios', 0),(0, 1015, 'btn', 'Paquetes', 0),(0, 1016, 'btn', 'Reg_Fiscales', 0),(0, 1017, 'btn', 'Perfiles', 0),(0, 1021, 'btn', 'Centralizacion', 0),(0, 1022, 'btn', 'Generar_Paquetes', 0),(0, 1023, 'btn', 'Consultar_Paquetes', 0),(0, 1031, 'btn', 'Zona', 0),(0, 1032, 'btn', 'Unidad_de_negocio', 0),(0, 1033, 'btn', 'Sede', 0),(0, 1034, 'btn', 'Dependencia', 0),(0, 1035, 'btn', 'Politicas_DP', 0), " +
                "(0, 1041, 'btn', 'Usuarios', 0)," +
                "(0, 1042, 'btn', 'Probar_Conexion', 0)," +
                "(0, 1043, 'btn', 'Parametros', 0)," +
                "(0, 1061, 'btn', 'Paquetes', 0)," +
                "(0, 1062, 'btn', 'Dependencias', 0)," +
                "(0, 10110, 'btn', 'Adm_Productos', 0)," +
                "(0, 10111, 'btn', 'Agrupaciones', 0)," +
                "(0, 10112, 'btn', 'Unidades', 0)," +
                "(0, 10113, 'btn', 'Impuestos', 0)," +
                "(0, 10114, 'btn', 'Listas', 0)," +
                "(0, 10120, 'btn', 'Adm_Clientes', 0)," +
                "(0, 10121, 'btn', 'Agrupaciones', 0)," +
                "(0, 10122, 'btn', 'Cd_Edo_Pais', 0)," +
                "(0, 10130, 'btn', 'Puntos', 0)," +
                "(0, 10131, 'btn', 'Ventas', 0)," +
                "(0, 10132, 'btn', 'Formas_Pago', 0)," +
                "(0, 10140, 'btn', 'Precios', 0)," +
                "(0, 10141, 'btn', 'Ofertas', 0)," +
                "(0, 10220, 'btn', 'Genera_Paquetes_de_Articulos', 0)," +
                "(0, 10221, 'btn', 'Genera_Paquetes_de_Clientes', 0)," +
                "(0, 10222, 'btn', 'Genera_Paquetes_de_Precios', 0)," +
                "(0, 10223, 'btn', 'Genera_Paquetes_de_Politicas', 0)," +
                "(0, 10224, 'btn', 'Consulta_Paquete', 0)," +
                "(0, 10225, 'btn', 'Limpia_Paquete', 0)," +
                "(0, 10226, 'btn', 'Envia_Paquete', 0)," +
                "(2, 105, 'btn', 'Subir_Paquete', 0)," +
                "(2, 106, 'btn', 'Consultas', 0)," +
                "(2, 1061, 'btn', 'Paquetes', 0)," +
                "(2, 1062, 'btn', 'Dependencias', 0)," +
                "(3, 101, 'btn', 'CATALOGOS', 0)," +
                "(3, 102, 'btn', 'ACTUALIZA_DP', 0)," +
                "(3, 103, 'btn', 'DEPENDENCIAS', 0)," +
                "(3, 105, 'btn', 'Subir_Paquete', 0)," +
                "(3, 1011, 'btn', 'Articulos', 0)," +
                "(3, 1012, 'btn', 'Clientes', 0)," +
                "(3, 1013, 'btn', 'Restricciones', 0)," +
                "(3, 1014, 'btn', 'Precios', 0)," +
                "(3, 1015, 'btn', 'Paquetes', 0)," +
                "(3, 1016, 'btn', 'Reg_Fiscales', 0)," +
                "(3, 1017, 'btn', 'Perfiles', 0)," +
                "(3, 1021, 'btn', 'Centralizacion', 0)," +
                "(3, 1022, 'btn', 'Generar_Paquetes', 0)," +
                "(3, 1023, 'btn', 'Consultar_Paquetes', 0)," +
                "(3, 1031, 'btn', 'Zona', 0)," +
                "(3, 1032, 'btn', 'Unidad_de_negocio', 0)," +
                "(3, 1033, 'btn', 'Sede', 0)," +
                "(3, 1034, 'btn', 'Dependencia', 0)," +
                "(3, 1035, 'btn', 'Politicas_DP', 0)," +
                "(3, 10110, 'btn', 'Adm_Productos', 0)," +
                "(3, 10111, 'btn', 'Agrupaciones', 0)," +
                "(3, 10112, 'btn', 'Unidades', 0)," +
                "(3, 10113, 'btn', 'Impuestos', 0)," +
                "(3, 10114, 'btn', 'Listas', 0)," +
                "(3, 10120, 'btn', 'Adm_Clientes', 0)," +
                "(3, 10121, 'btn', 'Agrupaciones', 0)," +
                "(3, 10122, 'btn', 'Cd_Edo_Pais', 0)," +
                "(3, 10130, 'btn', 'Puntos', 0)," +
                "(3, 10131, 'btn', 'Ventas', 0)," +
                "(3, 10132, 'btn', 'Formas_Pago', 0)," +
                "(3, 10140, 'btn', 'Precios', 0)," +
                "(3, 10141, 'btn', 'Ofertas', 0)," +
                "(3, 10220, 'btn', 'Genera_Paquetes_de_Articulos', 0)," +
                "(3, 10221, 'btn', 'Genera_Paquetes_de_Clientes', 0)," +
                "(3, 10222, 'btn', 'Genera_Paquetes_de_Precios', 0)," +
                "(3, 10223, 'btn', 'Genera_Paquetes_de_Politicas', 0)," +
                "(3, 10224, 'btn', 'Consulta_Paquete', 0)," +
                "(3, 10225, 'btn', 'Limpia_Paquete', 0)," +
                "(3, 10226, 'btn', 'Envia_Paquete', 0)," +
                "(4, 101, 'btn', 'CATALOGOS', 0)," +
                "(4, 102, 'btn', 'ACTUALIZA_DP', 0)," +
                "(4, 103, 'btn', 'DEPENDENCIAS', 0)," +
                "(4, 104, 'btn', 'CONFIGURACION_AC', 0)," +
                "(4, 105, 'btn', 'Subir_Paquete', 0)," +
                "(4, 106, 'btn', 'Consultas', 0)," +
                "(4, 1011, 'btn', 'Articulos', 0)," +
                "(4, 1012, 'btn', 'Clientes', 0)," +
                "(4, 1013, 'btn', 'Restricciones', 0)," +
                "(4, 1014, 'btn', 'Precios', 0)," +
                "(4, 1015, 'btn', 'Paquetes', 0)," +
                "(4, 1016, 'btn', 'Reg_Fiscales', 0)," +
                "(4, 1017, 'btn', 'Perfiles', 0)," +
                "(4, 1021, 'btn', 'Centralizacion', 0)," +
                "(4, 1022, 'btn', 'Generar_Paquetes', 0)," +
                "(4, 1023, 'btn', 'Consultar_Paquetes', 0)," +
                "(4, 1031, 'btn', 'Zona', 0)," +
                "(4, 1032, 'btn', 'Unidad_de_negocio', 0)," +
                "(4, 1033, 'btn', 'Sede', 0)," +
                "(4, 1034, 'btn', 'Dependencia', 0)," +
                "(4, 1035, 'btn', 'Politicas_DP', 0)," +
                "(4, 1041, 'btn', 'Usuarios', 0)," +
                "(4, 1042, 'btn', 'Probar_Conexion', 0)," +
                "(4, 1043, 'btn', 'Parametros', 0)," +
                "(4, 1061, 'btn', 'Paquetes', 0)," +
                "(4, 1062, 'btn', 'Dependencias', 0)," +
                "(4, 10110, 'btn', 'Adm_Productos', 0)," +
                "(4, 10111, 'btn', 'Agrupaciones', 0)," +
                "(4, 10112, 'btn', 'Unidades', 0)," +
                "(4, 10113, 'btn', 'Impuestos', 0)," +
                "(4, 10114, 'btn', 'Listas', 0)," +
                "(4, 10120, 'btn', 'Adm_Clientes', 0)," +
                "(4, 10121, 'btn', 'Agrupaciones', 0)," +
                "(4, 10122, 'btn', 'Cd_Edo_Pais', 0)," +
                "(4, 10130, 'btn', 'Puntos', 0)," +
                "(4, 10131, 'btn', 'Ventas', 0)," +
                "(4, 10132, 'btn', 'Formas_Pago', 0)," +
                "(4, 10140, 'btn', 'Precios', 0)," +
                "(4, 10141, 'btn', 'Ofertas', 0)," +
                "(4, 10220, 'btn', 'Genera_Paquetes_de_Articulos', 0)," +
                "(4, 10221, 'btn', 'Genera_Paquetes_de_Clientes', 0)," +
                "(4, 10222, 'btn', 'Genera_Paquetes_de_Precios', 0)," +
                "(4, 10223, 'btn', 'Genera_Paquetes_de_Politicas', 0)," +
                "(4, 10224, 'btn', 'Consulta_Paquete', 0)," +
                "(4, 10225, 'btn', 'Limpia_Paquete', 0)," +
                "(4, 10226, 'btn', 'Envia_Paquete', 0)");

            //INSERT UPDATE EN tblAC_Usuarios
            dtValidaTabla = datatableMySQL("SHOW TABLES LIKE 'tblAC_Usuarios';");
            if (dtValidaTabla.Rows.Count > 0)
            {
                dtValidaTabla = datatableMySQL("SELECT * FROM tblAC_Usuarios WHERE Id_Usuario ='PACIFIC'");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('PACIFIC', 0, 'PACIFIC', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                }

                dtValidaTabla = datatableMySQL("SELECT * FROM tblAC_Usuarios WHERE Id_Usuario ='EJECUTIVO'");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('EJECUTIVO', 1, 'EJECUTIVO', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                }

                dtValidaTabla = datatableMySQL("SELECT * FROM tblAC_Usuarios WHERE Id_Usuario ='SUPERVISOR'");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('SUPERVISOR', 2, 'SUPERVISOR', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                }

                dtValidaTabla = datatableMySQL("SELECT * FROM tblAC_Usuarios WHERE Id_Usuario ='OPERATIVO'");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('OPERATIVO', 3, 'OPERATIVO', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                }

                dtValidaTabla = datatableMySQL("SELECT * FROM tblAC_Usuarios WHERE Id_Usuario ='ADMIN'");
                if (dtValidaTabla.Rows.Count <= 0)
                {
                    resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('ADMIN', 4, 'ADMINISTRADOR', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                }
              

            }
            else
            {
                resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('PACIFIC', 0, 'PACIFIC', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('EJECUTIVO', 1, 'EJECUTIVO', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('SUPERVISOR', 2, 'SUPERVISOR', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('OPERATIVO', 3, 'OPERATIVO', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
                resultado = ejecutaSqlServer("INSERT INTO tblAC_Usuarios (Id_Usuario, Perfil_Usuario, Nombre_Usuario, Passw_Usuario, FechaAlta_Usuario, Estatus_Usuario) VALUES ('ADMIN', 4, 'ADMINISTRADOR', 'nEdTnexxwyY=', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 0)");
            }
            //ESTATUS DE USUARIOS DESBLOQUEADOS
            resultado = ejecutaSqlServer("Update tblAC_Usuarios set Estatus_Usuario = 0");

            //SE AGREGO ESTA LINEA PARA MODIFICAR LA COLUMNA DE tblUndCosPreArt EL DIA  20200106
            //resultado = ejecutaSqlServer("ALTER TABLE tblUndCosPreArt ALTER COLUMN PCIO_PUNTOS SET DEFAULT 0;");

            dtValidaTabla = datatableBD("SELECT  obj_Constraint.NAME AS 'constraint' " +
                                           "FROM   sys.objects obj_table " +
                                           "JOIN sys.objects obj_Constraint " +
                                           "ON obj_table.object_id = obj_Constraint.parent_object_id " +
                                           "JOIN sys.sysconstraints constraints " +
                                           "ON constraints.constid = obj_Constraint.object_id " +
                                           "JOIN sys.columns columns " +
                                           "ON columns.object_id = obj_table.object_id " +
                                           "AND columns.column_id = constraints.colid " +
                                           "WHERE obj_table.NAME = 'tblUndCosPreArt' and columns.NAME = 'PCIO_PUNTOS' AND obj_Constraint.type = 'D'");

            if (dtValidaTabla.Rows.Count > 0)
            {                
                resultado = ejecutaSqlServer("ALTER table tblUndCosPreArt drop constraint  "+ dtValidaTabla.Rows[0]["constraint"] + "");

                resultado = ejecutaSqlServer("ALTER TABLE tblUndCosPreArt ADD DEFAULT 0 FOR PCIO_PUNTOS;");
            }
            else
            {
                resultado = ejecutaSqlServer("ALTER TABLE tblUndCosPreArt ADD DEFAULT 0 FOR PCIO_PUNTOS;");
            }

            dtValidaTabla = datatableBD("SELECT COD_CLI,ISNULL(DP_CLI,-1) AS DP_CLI ,ISNULL(RIF_CFDI,-1) AS RIF_CFDI,ISNULL(ID_CTAPAGO,-1) AS ID_CTAPAGO  FROM tblCatClientes;");

            foreach (DataRow drRow in dtValidaTabla.Rows)
            {               
                if (drRow["DP_CLI"].ToString() == "-1")
                {
                    ejecutaSqlServer("UPDATE tblCatClientes SET DP_CLI ='' WHERE COD_CLI ='" + drRow["COD_CLI"].ToString() + "';");
                }

                if (drRow["RIF_CFDI"].ToString() == "-1")
                {
                    ejecutaSqlServer("UPDATE tblCatClientes SET RIF_CFDI ='' WHERE COD_CLI ='" + drRow["COD_CLI"].ToString() + "';");
                }

                if (drRow["ID_CTAPAGO"].ToString() == "-1")
                {
                    ejecutaSqlServer("UPDATE tblCatClientes SET ID_CTAPAGO = 0 WHERE COD_CLI ='" + drRow["COD_CLI"].ToString() + "';");
                }

            }

            //ACTUALIZA FECHA DEFAULT PEDIMENTO TABLAS
            resultado = ejecutaSqlServer("UPDATE tblAdicionalCapas SET FEC_PEDIM='1800-01-01' WHERE FEC_PEDIM IS NULL;");
            resultado = ejecutaSqlServer("UPDATE tblAdicionalCapas SET FEC_PEDIM='1800-01-01' WHERE FEC_PEDIM < '1800-01-01';");
            resultado = ejecutaSqlServer("UPDATE tblCostos SET FEC_PEDIM='1800-01-01' WHERE FEC_PEDIM IS NULL;");
            resultado = ejecutaSqlServer("UPDATE tblCostos SET FEC_PEDIM='1800-01-01' WHERE FEC_PEDIM < '1800-01-01';");

            resultado = ejecutaSqlServer("UPDATE tblRenAlmacen SET FEC_PEDIM='1800-01-01' WHERE FEC_PEDIM IS NULL;");
            resultado = ejecutaSqlServer("UPDATE tblRenAlmacen SET FEC_PEDIM='1800-01-01' WHERE FEC_PEDIM < '1800-01-01';");



            //ASIGNAR DEFAULT EN SCHEMA PARA LAS SIGUIENTES TABLAS
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncCargosAbonos ADD DEFAULT 0 FOR IMP_RETISR;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncCargosAbonos ADD DEFAULT 0 FOR BASE_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncCargosAbonos ADD DEFAULT 0 FOR IMP_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncCargosAbonos ADD DEFAULT 0 FOR BASE_RETISR;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasEnc ADD DEFAULT 0 FOR PORC_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasEnc ADD DEFAULT 0 FOR IMP_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasEnc ADD DEFAULT 0 FOR PORC_RETISR;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasEnc ADD DEFAULT 0 FOR IMP_RETISR;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasEnc ADD DEFAULT '1800-01-01' FOR FEC_CANC;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasEnc ADD DEFAULT '' FOR SUSTIT_FACT;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasRen ADD DEFAULT 0 FOR IMP0_ART;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasRen ADD DEFAULT 0 FOR IMP0_REG;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasRen ADD DEFAULT 0 FOR COD0_IMP;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasRen ADD DEFAULT 0 FOR IMP_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasRen ADD DEFAULT 0 FOR IMP_RETISR;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblFacturasRen ADD DEFAULT 0 FOR FE_OBJIMP;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblCostos ADD DEFAULT '1800-01-01' FOR FEC_PEDIM;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblGralVentas ADD DEFAULT 0 FOR CLIENTE_MOS;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblGralVentas ADD DEFAULT 0 FOR COD0_IMP;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblGralVentas ADD DEFAULT 0 FOR IMP0_ART;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblGralVentas ADD DEFAULT 0 FOR IMP0_REG;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblRenPedidos ADD DEFAULT 0 FOR NUM_REN;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR PORC_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR IMP_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR PORC_RETISR;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR IMP_RETISR;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR IMP0_ART;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR IMP0_REG;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR COD0_IMP;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR IMP_RETIVA;");
            resultado = ejecutaSqlServerNoME("ALTER TABLE tblEncDevolucion ADD DEFAULT 0 FOR IMP_RETISR;");
                        
            return true;
        } 
        public bool deshabilitaCajas()
        {
            bool resultado = false;
            resultado = ejecutaSqlServer("UPDATE tblCajas SET VEN_CAJ=1;");
            return true;
        }
        private static void WaitSeconds(double nSecs)
        {
            // Esperar los segundos indicados

            // Crear la cadena para convertir en TimeSpan
            string s = "0.00:00:" + nSecs.ToString().Replace(",", ".");
            TimeSpan ts = TimeSpan.Parse(s);

            // Añadirle la diferencia a la hora actual
            DateTime t1 = DateTime.Now.Add(ts);

            // Esta asignación solo es necesaria
            // si la comprobación se hace al principio del bucle
            DateTime t2 = DateTime.Now;

            // Mientras no haya pasado el tiempo indicado
            while (t2 < t1)
            {
                // Un respiro para el sitema
                System.Windows.Forms.Application.DoEvents();
                // Asignar la hora actual
                t2 = DateTime.Now;
            }
        }

        private Boolean IsNumeric(string valor)
        {
            int result;
            return int.TryParse(valor, out result);
        }

        enum TipoDeCampoPS
        {
              tdBoleano = 0,
              tdByte = 1,
              tdInteger = 2,
              tdLongInteger = 3,
              tdCurrency = 4,
              tdSingle = 5,
              tdDouble = 6,
              tdFecha = 7,
              tdString = 9,
              tdMemo = 11,
              tdHora = 12,
              tdAutoNumerico = 13,
              tdLongText = 14,
              tdImage = 15
        };

        private string TipoDeDatoSQL(string NombreCampo,int Tipo, int LongitudTexto)
        {
            string TipoDeDatoSQL1 = "";
            switch (Tipo)
            {
                case (int)TipoDeCampoPS.tdBoleano: TipoDeDatoSQL1 = "SMALLINT NOT NULL"; break;
                case (int)TipoDeCampoPS.tdByte: TipoDeDatoSQL1 = "TINYINT NOT NULL"; break;
                case (int)TipoDeCampoPS.tdCurrency: TipoDeDatoSQL1 = "DECIMAL(13,4) NOT NULL"; break;
                case (int)TipoDeCampoPS.tdDouble: TipoDeDatoSQL1 = "REAL NOT NULL"; break;
                case (int)TipoDeCampoPS.tdFecha: TipoDeDatoSQL1 = "DATE NOT NULL"; break;
                case (int)TipoDeCampoPS.tdHora: TipoDeDatoSQL1 = "TIME NULL"; break;
                case (int)TipoDeCampoPS.tdInteger: TipoDeDatoSQL1 = "SMALLINT NOT NULL"; break;
                case (int)TipoDeCampoPS.tdLongInteger: TipoDeDatoSQL1 = "BIGINT NOT NULL"; break;
                case (int)TipoDeCampoPS.tdMemo: TipoDeDatoSQL1 = "TEXT NOT NULL"; break;
                case (int)TipoDeCampoPS.tdSingle: TipoDeDatoSQL1 = "REAL NOT NULL"; break;
                case (int)TipoDeCampoPS.tdString: TipoDeDatoSQL1 = "VARCHAR(" + LongitudTexto + ") NOT NULL"; break;
                case (int)TipoDeCampoPS.tdAutoNumerico: TipoDeDatoSQL1 = "BIGINT NOT NULL IDENTITY(1,1) UNIQUE"; break;
                case (int)TipoDeCampoPS.tdLongText: TipoDeDatoSQL1 = "NVARCHAR(MAX) NOT NULL"; break;
                case (int)TipoDeCampoPS.tdImage: TipoDeDatoSQL1 = "NVARCHAR(MAX) NOT NULL"; break;
                default:
                    if(Tipo == 10) { TipoDeDatoSQL1 = "DATETIME NOT NULL"; }                    
                    break;
            }

            //SE AGREGO LA VALIDACIÓN PARA FORZAR EL TIPO DE DATOS PARA LOS CAMPOS QUE INICIEN CON XML_ Y IMGBIT_ EL DIA 2020/07/07 POR JC Y MONICA
            //LA VALIDACION NombreCampo.Length > 20 ES PARA CAMPOS CON MENOS LONGITUD DE CARACTERES ENTREN A LA CONDICION DEL Substring
            if (NombreCampo.Length >= 4 && NombreCampo.Substring(0, 4) == "XML_") {
                TipoDeDatoSQL1 = "NVARCHAR(MAX) NOT NULL";
            }

            //DEFINIR ESTOS CAMPOS XML COMO NULL
            if (NombreCampo == "XML_ISTMO")
            {
                TipoDeDatoSQL1 = "NVARCHAR(MAX) NULL";
            }

            if (NombreCampo == "XML_COFEPRIS")
            {
                TipoDeDatoSQL1 = "NVARCHAR(MAX) NULL";
            }
            ///

            else if (NombreCampo.Length >= 7 && NombreCampo.Substring(0, 7) == "IMGBIT_") {
                TipoDeDatoSQL1 = "NVARCHAR(MAX) NOT NULL";
            }
            
            return TipoDeDatoSQL1;
        }

        private bool AgregaDatos(string tabla,string Bd)
        {
            DataTable tablaSql = new DataTable(); bool bandera = false;
            DataTable dtValidaTabla = datatableMySQL("SHOW TABLES LIKE '"+ tabla + "'");            
            if (dtValidaTabla.Rows.Count <= 0) { return true; }
                     
            DataTable Datos = datatableMySQL("SELECT * FROM " + tabla);
            foreach (DataRow row in Datos.Rows)
            {
                #region fecha
                if (Datos.Columns.Contains("FEC_INI"))
                {
                    if (row["FEC_INI"].ToString().Length == 0)
                    {
                        row["FEC_INI"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_SAL"))
                {
                    if (row["FEC_SAL"].ToString().Length == 0)
                    {
                        row["FEC_SAL"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_CAD"))
                {
                    if (row["FEC_CAD"].ToString().Length == 0)
                    {
                        row["FEC_CAD"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_CIERRE"))
                {
                    if (row["FEC_CIERRE"].ToString().Length == 0)
                    {
                        row["FEC_CIERRE"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_FIN"))
                {
                    if (row["FEC_FIN"].ToString().Length == 0)
                    {
                        row["FEC_FIN"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_OFR"))
                {
                    if (row["FEC_OFR"].ToString().Length == 0)
                    {
                        row["FEC_OFR"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_UCO"))
                {
                    if (row["FEC_UCO"].ToString().Length == 0)
                    {
                        row["FEC_UCO"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_VEN"))
                {
                    if (row["FEC_VEN"].ToString().Length == 0)
                    {
                        row["FEC_VEN"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FEC_ING"))
                {
                    if (row["FEC_ING"].ToString().Length == 0)
                    {
                        row["FEC_ING"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_UCP"))
                {
                    if (row["FEC_UCP"].ToString().Length == 0)
                    {
                        row["FEC_UCP"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_MOV"))
                {
                    if (row["FEC_MOV"].ToString().Length == 0)
                    {
                        row["FEC_MOV"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_DOC"))
                {
                    if (row["FEC_DOC"].ToString().Length == 0)
                    {
                        row["FEC_DOC"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_REG"))
                {
                    if (row["FEC_REG"].ToString().Length == 0)
                    {
                        row["FEC_REG"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_PROM"))
                {
                    if (row["FEC_PROM"].ToString().Length == 0)
                    {
                        row["FEC_PROM"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_UVEN"))
                {
                    if (row["FEC_UVEN"].ToString().Length == 0)
                    {
                        row["FEC_UVEN"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_UPAG"))
                {
                    if (row["FEC_UPAG"].ToString().Length == 0)
                    {
                        row["FEC_UPAG"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_LCRE"))
                {
                    if (row["FEC_LCRE"].ToString().Length == 0)
                    {
                        row["FEC_LCRE"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_EVENTO"))
                {
                    if (row["FEC_EVENTO"].ToString().Length == 0)
                    {
                        row["FEC_EVENTO"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FE_VENCERT"))
                {
                    if (row["FE_VENCERT"].ToString().Length == 0)
                    {
                        row["FE_VENCERT"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_APE"))
                {
                    if (row["FEC_APE"].ToString().Length == 0)
                    {
                        row["FEC_APE"] = Convert.ToDateTime("1800-01-01");
                    }
                }
                if (Datos.Columns.Contains("FECHA_COM"))
                {
                    if (row["FECHA_COM"].ToString().Length == 0)
                    {
                        row["FECHA_COM"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_RESERVA"))
                {
                    if (row["FEC_RESERVA"].ToString().Length == 0)
                    {
                        row["FEC_RESERVA"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FECHA_PV"))
                {
                    if (row["FECHA_PV"].ToString().Length == 0)
                    {
                        row["FECHA_PV"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FECHA_MOVTC"))
                {
                    if (row["FECHA_MOVTC"].ToString().Length == 0)
                    {
                        row["FECHA_MOVTC"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FECHA_COR"))
                {
                    if (row["FECHA_COR"].ToString().Length == 0)
                    {
                        row["FECHA_COR"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_PED"))
                {
                    if (row["FEC_PED"].ToString().Length == 0)
                    {
                        row["FEC_PED"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_FAC"))
                {
                    if (row["FEC_FAC"].ToString().Length == 0)
                    {
                        row["FEC_FAC"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_VENC"))
                {
                    if (row["FEC_VENC"].ToString().Length == 0)
                    {
                        row["FEC_VENC"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_LIQ"))
                {
                    if (row["FEC_LIQ"].ToString().Length == 0)
                    {
                        row["FEC_LIQ"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_SERV"))
                {
                    if (row["FEC_SERV"].ToString().Length == 0)
                    {
                        row["FEC_SERV"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FECHA"))
                {
                    if (row["FECHA"].ToString().Length == 0)
                    {
                        row["FECHA"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FECHA_VEN"))
                {
                    if (row["FECHA_VEN"].ToString().Length == 0)
                    {
                        row["FECHA_VEN"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_UCOM"))
                {
                    if (row["FEC_UCOM"].ToString().Length == 0)
                    {
                        row["FEC_UCOM"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_UCOM"))
                {
                    if (row["FEC_UCOM"].ToString().Length == 0)
                    {
                        row["FEC_UCOM"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_VENCIM"))
                {
                    if (row["FEC_VENCIM"].ToString().Length == 0)
                    {
                        row["FEC_VENCIM"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("PED_FEC"))
                {
                    if (row["PED_FEC"].ToString().Length == 0)
                    {
                        row["PED_FEC"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_COTI"))
                {
                    if (row["FEC_COTI"].ToString().Length == 0)
                    {
                        row["FEC_COTI"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_OCOM"))
                {
                    if (row["FEC_OCOM"].ToString().Length == 0)
                    {
                        row["FEC_OCOM"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_PREV"))
                {
                    if (row["FEC_PREV"].ToString().Length == 0)
                    {
                        row["FEC_PREV"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_REC"))
                {
                    if (row["FEC_REC"].ToString().Length == 0)
                    {
                        row["FEC_REC"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_REQ"))
                {
                    if (row["FEC_REQ"].ToString().Length == 0)
                    {
                        row["FEC_REQ"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_SALINI"))
                {
                    if (row["FEC_SALINI"].ToString().Length == 0)
                    {
                        row["FEC_SALINI"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_APLI"))
                {
                    if (row["FEC_APLI"].ToString().Length == 0)
                    {
                        row["FEC_APLI"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_MOVIM"))
                {
                    if (row["FEC_MOVIM"].ToString().Length == 0)
                    {
                        row["FEC_MOVIM"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_GENERA"))
                {
                    if (row["FEC_GENERA"].ToString().Length == 0)
                    {
                        row["FEC_GENERA"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FECHA_CON"))
                {
                    if (row["FECHA_CON"].ToString().Length == 0)
                    {
                        row["FECHA_CON"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FECHA_MOV"))
                {
                    if (row["FECHA_MOV"].ToString().Length == 0)
                    {
                        row["FECHA_MOV"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_POLIZA"))
                {
                    if (row["FEC_POLIZA"].ToString().Length == 0)
                    {
                        row["FEC_POLIZA"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FEC_ANTERIOR"))
                {
                    if (row["FEC_ANTERIOR"].ToString().Length == 0)
                    {
                        row["FEC_ANTERIOR"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("Nac_Paciente"))
                {
                    if (row["Nac_Paciente"].ToString().Length == 0)
                    {
                        row["Nac_Paciente"] = Convert.ToDateTime("1800-01-01");

                    }
                }

                if (Datos.Columns.Contains("Fch_Apertura"))
                {
                    if (row["Fch_Apertura"].ToString().Length == 0)
                    {
                        row["Fch_Apertura"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("Fch_Consulta"))
                {
                    if (row["Fch_Consulta"].ToString().Length == 0)
                    {
                        row["Fch_Consulta"] = Convert.ToDateTime("1800-01-01");

                    }
                }

                if (Datos.Columns.Contains("FechaHora"))
                {
                    if (row["FechaHora"].ToString().Length == 0)
                    {
                        row["FechaHora"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("Fecha"))
                {
                    if (row["Fecha"].ToString().Length == 0)
                    {
                        row["Fecha"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("Fecha_Crea"))
                {
                    if (row["Fecha_Crea"].ToString().Length == 0)
                    {
                        row["Fecha_Crea"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("Fecha_Procesa"))
                {
                    if (row["Fecha_Procesa"].ToString().Length == 0)
                    {
                        row["Fecha_Procesa"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("FechaAlta_Usuario"))
                {
                    if (row["FechaAlta_Usuario"].ToString().Length == 0)
                    {
                        row["FechaAlta_Usuario"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                if (Datos.Columns.Contains("Fecha_AltaUsuario"))
                {
                    if (row["Fecha_AltaUsuario"].ToString().Length == 0)
                    {
                        row["Fecha_AltaUsuario"] = Convert.ToDateTime("1800-01-01");

                    }
                }
                #endregion

                #region CAMPOS CON VALOR DEFAULT 
                #endregion
            }

            if (Datos.Rows.Count > 0)
            {
                tablaSql = datatableBD("Select * FROM " + tabla);
                bandera = false; int cols = 0; 
                foreach (DataColumn colum in tablaSql.Columns)
                {
                    int col = 0; 
                    for (int i = 0; i < Datos.Columns.Count; i++)
                    {
                        if (colum.ColumnName == Datos.Columns[i].ColumnName)
                        {
                            col = i;
                            break;
                        }
                    }
                    if (bandera)
                    {
                        for (int i = 0; i < Datos.Rows.Count; i++)
                        {
                            tablaSql.Rows[i][cols] = Datos.Rows[i][col].ToString() == "-1" ? 1 : Datos.Rows[i][col];
                        }
                    }
                    else
                    {
                        bandera = true; DataRow row;
                        for (int i = 0; i < Datos.Rows.Count; i++)
                        {
                            row = tablaSql.NewRow(); tablaSql.Rows.Add(row);
                            tablaSql.Rows[i][cols] = Datos.Rows[i][col];
                        }

                    }

                    cols ++;
                }
                
            }


            ConexionSqlServer1.Open();
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConexionSqlServer1))
            {
                bulkCopy.BulkCopyTimeout = 86400;
                bulkCopy.DestinationTableName = tabla;
                try
                {
                    // Write from the source to the destination.
                    if (bandera)
                        bulkCopy.WriteToServer(tablaSql);
                    else
                        bulkCopy.WriteToServer(Datos);
                    ConexionSqlServer1.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    ConexionSqlServer1.Close();
                    return false;
                }
            }
        }

        public bool MismaVersion()
        {
            try
            {

                string versionMysql, versionAcces;
                activaConexionAcces();

                versionAcces = stringDelfin("SELECT VER FROM tblVersion");
                versionMysql = stringMySQL("SELECT VER FROM tblVersion");               

                if (versionMysql == versionAcces)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception err) {

                MessageBox.Show("Error:" + err.Message.ToString());
                return false;

            }
        }

        public bool ValidaEstructura()
        {
            string versionMysql;

            versionMysql = stringMySQL("SELECT COD_SUCU FROM tblparametrosgenerales;");
            if (versionMysql.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
