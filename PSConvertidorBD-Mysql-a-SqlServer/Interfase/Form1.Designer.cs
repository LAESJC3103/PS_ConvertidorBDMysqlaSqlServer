namespace PSConvertidorBD_Mysql_a_SqlServer
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbConexionBD = new System.Windows.Forms.ComboBox();
            this.lblConexionBD = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInstancia = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSoloEstructuraBD = new System.Windows.Forms.CheckBox();
            this.txtNombreBD = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSalirMenu = new System.Windows.Forms.Button();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.ovalShape1 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.lbltitulo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chkBdDestino = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.gbdbglobal = new System.Windows.Forms.GroupBox();
            this.btnCriterios = new System.Windows.Forms.Button();
            this.chkInicializadb = new System.Windows.Forms.CheckBox();
            this.gbInstanciaSQL = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNombreDB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSucursal = new System.Windows.Forms.TextBox();
            this.txtAlmacen = new System.Windows.Forms.TextBox();
            this.txtDesSucursal = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDesAlmacen = new System.Windows.Forms.TextBox();
            this.pnlCriterios = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDestino = new System.Windows.Forms.TextBox();
            this.txtOrigen = new System.Windows.Forms.TextBox();
            this.txtRutaPaquete = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUrlWebService = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbdbglobal.SuspendLayout();
            this.gbInstanciaSQL.SuspendLayout();
            this.pnlCriterios.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbConexionBD
            // 
            this.cmbConexionBD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConexionBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbConexionBD.FormattingEnabled = true;
            this.cmbConexionBD.Location = new System.Drawing.Point(220, 26);
            this.cmbConexionBD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbConexionBD.MaxLength = 255;
            this.cmbConexionBD.Name = "cmbConexionBD";
            this.cmbConexionBD.Size = new System.Drawing.Size(157, 32);
            this.cmbConexionBD.TabIndex = 1;
            this.cmbConexionBD.SelectedIndexChanged += new System.EventHandler(this.cmbConexionBD_SelectedIndexChanged);
            // 
            // lblConexionBD
            // 
            this.lblConexionBD.AutoSize = true;
            this.lblConexionBD.BackColor = System.Drawing.Color.Transparent;
            this.lblConexionBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConexionBD.ForeColor = System.Drawing.Color.White;
            this.lblConexionBD.Location = new System.Drawing.Point(7, 31);
            this.lblConexionBD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConexionBD.Name = "lblConexionBD";
            this.lblConexionBD.Size = new System.Drawing.Size(209, 20);
            this.lblConexionBD.TabIndex = 0;
            this.lblConexionBD.Text = "Conexión a la BD Global:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Instancia a SqlServer:";
            // 
            // txtInstancia
            // 
            this.txtInstancia.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInstancia.Location = new System.Drawing.Point(201, 31);
            this.txtInstancia.Name = "txtInstancia";
            this.txtInstancia.Size = new System.Drawing.Size(335, 29);
            this.txtInstancia.TabIndex = 6;
            this.txtInstancia.Text = "psadmincentral.database.windows.net,1433";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(35, 576);
            this.progressBar1.Maximum = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(360, 52);
            this.progressBar1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(9, 126);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(94, 123);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(192, 29);
            this.txtUsuario.TabIndex = 8;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(391, 123);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(148, 29);
            this.txtPassword.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(293, 129);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password:";
            // 
            // chkSoloEstructuraBD
            // 
            this.chkSoloEstructuraBD.AutoSize = true;
            this.chkSoloEstructuraBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSoloEstructuraBD.ForeColor = System.Drawing.Color.White;
            this.chkSoloEstructuraBD.Location = new System.Drawing.Point(6, 127);
            this.chkSoloEstructuraBD.Name = "chkSoloEstructuraBD";
            this.chkSoloEstructuraBD.Size = new System.Drawing.Size(275, 24);
            this.chkSoloEstructuraBD.TabIndex = 2;
            this.chkSoloEstructuraBD.Text = "Solo estructura Base de Datos";
            this.chkSoloEstructuraBD.UseVisualStyleBackColor = true;
            this.chkSoloEstructuraBD.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtNombreBD
            // 
            this.txtNombreBD.Enabled = false;
            this.txtNombreBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombreBD.Location = new System.Drawing.Point(391, 122);
            this.txtNombreBD.Name = "txtNombreBD";
            this.txtNombreBD.Size = new System.Drawing.Size(140, 29);
            this.txtNombreBD.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Enabled = false;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(288, 127);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Nombre BD:";
            // 
            // btnSalirMenu
            // 
            this.btnSalirMenu.BackgroundImage = global::PSConvertidorBD_Mysql_a_SqlServer.Properties.Resources.salirMetro;
            this.btnSalirMenu.Location = new System.Drawing.Point(402, 576);
            this.btnSalirMenu.Name = "btnSalirMenu";
            this.btnSalirMenu.Size = new System.Drawing.Size(50, 52);
            this.btnSalirMenu.TabIndex = 13;
            this.toolTip1.SetToolTip(this.btnSalirMenu, "Cerrar");
            this.btnSalirMenu.UseVisualStyleBackColor = true;
            this.btnSalirMenu.Click += new System.EventHandler(this.btnSalirMenu_Click);
            this.btnSalirMenu.MouseEnter += new System.EventHandler(this.btnSalirMenu_MouseEnter);
            this.btnSalirMenu.MouseLeave += new System.EventHandler(this.btnSalirMenu_MouseLeave);
            // 
            // btnGenerar
            // 
            this.btnGenerar.BackColor = System.Drawing.Color.Transparent;
            this.btnGenerar.BackgroundImage = global::PSConvertidorBD_Mysql_a_SqlServer.Properties.Resources.botonMetro52;
            this.btnGenerar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGenerar.Enabled = false;
            this.btnGenerar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.ForeColor = System.Drawing.Color.White;
            this.btnGenerar.Location = new System.Drawing.Point(171, 644);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(105, 52);
            this.btnGenerar.TabIndex = 14;
            this.btnGenerar.Text = "Generar";
            this.toolTip1.SetToolTip(this.btnGenerar, "Generar base de datos");
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.button1_Click);
            this.btnGenerar.MouseEnter += new System.EventHandler(this.button1_MouseEnter);
            this.btnGenerar.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.ovalShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(606, 702);
            this.shapeContainer1.TabIndex = 16;
            this.shapeContainer1.TabStop = false;
            // 
            // ovalShape1
            // 
            this.ovalShape1.BackColor = System.Drawing.Color.White;
            this.ovalShape1.BackgroundImage = global::PSConvertidorBD_Mysql_a_SqlServer.Properties.Resources.botonMetro52;
            this.ovalShape1.BorderColor = System.Drawing.Color.Transparent;
            this.ovalShape1.FillColor = System.Drawing.Color.Black;
            this.ovalShape1.FillGradientColor = System.Drawing.Color.Black;
            this.ovalShape1.Location = new System.Drawing.Point(502, 160);
            this.ovalShape1.Name = "ovalShape1";
            this.ovalShape1.Size = new System.Drawing.Size(38, 28);
            this.ovalShape1.Visible = false;
            this.ovalShape1.MouseEnter += new System.EventHandler(this.ovalShape1_MouseEnter);
            this.ovalShape1.MouseLeave += new System.EventHandler(this.ovalShape1_MouseLeave);
            // 
            // lbltitulo
            // 
            this.lbltitulo.AutoSize = true;
            this.lbltitulo.BackColor = System.Drawing.Color.Transparent;
            this.lbltitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitulo.ForeColor = System.Drawing.Color.White;
            this.lbltitulo.Location = new System.Drawing.Point(13, 243);
            this.lbltitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbltitulo.Name = "lbltitulo";
            this.lbltitulo.Size = new System.Drawing.Size(0, 29);
            this.lbltitulo.TabIndex = 17;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(458, 551);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 120);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // chkBdDestino
            // 
            this.chkBdDestino.AutoSize = true;
            this.chkBdDestino.Checked = true;
            this.chkBdDestino.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBdDestino.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBdDestino.ForeColor = System.Drawing.Color.White;
            this.chkBdDestino.Location = new System.Drawing.Point(39, 92);
            this.chkBdDestino.Name = "chkBdDestino";
            this.chkBdDestino.Size = new System.Drawing.Size(269, 24);
            this.chkBdDestino.TabIndex = 19;
            this.chkBdDestino.Text = "Crea Base de Datos (Destino)";
            this.chkBdDestino.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Autenticación de SQL Server",
            "Autenticación de Windows"});
            this.comboBox1.Location = new System.Drawing.Point(13, 175);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox1.MaxLength = 255;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(338, 32);
            this.comboBox1.TabIndex = 20;
            // 
            // gbdbglobal
            // 
            this.gbdbglobal.Controls.Add(this.btnCriterios);
            this.gbdbglobal.Controls.Add(this.lblConexionBD);
            this.gbdbglobal.Controls.Add(this.cmbConexionBD);
            this.gbdbglobal.Controls.Add(this.chkBdDestino);
            this.gbdbglobal.Controls.Add(this.chkSoloEstructuraBD);
            this.gbdbglobal.Controls.Add(this.label4);
            this.gbdbglobal.Controls.Add(this.txtNombreBD);
            this.gbdbglobal.ForeColor = System.Drawing.SystemColors.Menu;
            this.gbdbglobal.Location = new System.Drawing.Point(18, 13);
            this.gbdbglobal.Name = "gbdbglobal";
            this.gbdbglobal.Size = new System.Drawing.Size(572, 86);
            this.gbdbglobal.TabIndex = 21;
            this.gbdbglobal.TabStop = false;
            this.gbdbglobal.Text = "Origen(MySql)";
            // 
            // btnCriterios
            // 
            this.btnCriterios.ForeColor = System.Drawing.SystemColors.InfoText;
            this.btnCriterios.Location = new System.Drawing.Point(409, 26);
            this.btnCriterios.Name = "btnCriterios";
            this.btnCriterios.Size = new System.Drawing.Size(129, 32);
            this.btnCriterios.TabIndex = 31;
            this.btnCriterios.Text = "( + ) Mostrar criterios";
            this.btnCriterios.UseVisualStyleBackColor = true;
            this.btnCriterios.Click += new System.EventHandler(this.btnCriterios_Click);
            // 
            // chkInicializadb
            // 
            this.chkInicializadb.AutoSize = true;
            this.chkInicializadb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInicializadb.ForeColor = System.Drawing.Color.White;
            this.chkInicializadb.Location = new System.Drawing.Point(19, 230);
            this.chkInicializadb.Name = "chkInicializadb";
            this.chkInicializadb.Size = new System.Drawing.Size(223, 24);
            this.chkInicializadb.TabIndex = 20;
            this.chkInicializadb.Text = "Inicializar base de datos";
            this.chkInicializadb.UseVisualStyleBackColor = true;
            // 
            // gbInstanciaSQL
            // 
            this.gbInstanciaSQL.Controls.Add(this.label9);
            this.gbInstanciaSQL.Controls.Add(this.txtNombreDB);
            this.gbInstanciaSQL.Controls.Add(this.chkInicializadb);
            this.gbInstanciaSQL.Controls.Add(this.label1);
            this.gbInstanciaSQL.Controls.Add(this.txtInstancia);
            this.gbInstanciaSQL.Controls.Add(this.comboBox1);
            this.gbInstanciaSQL.Controls.Add(this.label2);
            this.gbInstanciaSQL.Controls.Add(this.txtUsuario);
            this.gbInstanciaSQL.Controls.Add(this.label3);
            this.gbInstanciaSQL.Controls.Add(this.txtPassword);
            this.gbInstanciaSQL.ForeColor = System.Drawing.SystemColors.Menu;
            this.gbInstanciaSQL.Location = new System.Drawing.Point(18, 114);
            this.gbInstanciaSQL.Name = "gbInstanciaSQL";
            this.gbInstanciaSQL.Size = new System.Drawing.Size(572, 270);
            this.gbInstanciaSQL.TabIndex = 22;
            this.gbInstanciaSQL.TabStop = false;
            this.gbInstanciaSQL.Text = "Destino(SQL)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(7, 79);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 20);
            this.label9.TabIndex = 22;
            this.label9.Text = "Nombre BD:";
            // 
            // txtNombreDB
            // 
            this.txtNombreDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtNombreDB.Location = new System.Drawing.Point(117, 74);
            this.txtNombreDB.Name = "txtNombreDB";
            this.txtNombreDB.Size = new System.Drawing.Size(248, 29);
            this.txtNombreDB.TabIndex = 7;
            this.txtNombreDB.TextChanged += new System.EventHandler(this.txtNombreDB_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(21, 394);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 20);
            this.label5.TabIndex = 23;
            this.label5.Text = "Cód.Sucursal:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(21, 439);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 20);
            this.label6.TabIndex = 24;
            this.label6.Text = "Cód.Almacen:";
            // 
            // txtSucursal
            // 
            this.txtSucursal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtSucursal.Location = new System.Drawing.Point(151, 391);
            this.txtSucursal.MaxLength = 3;
            this.txtSucursal.Name = "txtSucursal";
            this.txtSucursal.Size = new System.Drawing.Size(81, 26);
            this.txtSucursal.TabIndex = 10;
            this.txtSucursal.TextChanged += new System.EventHandler(this.txtSucursal_TextChanged);
            // 
            // txtAlmacen
            // 
            this.txtAlmacen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtAlmacen.Location = new System.Drawing.Point(151, 435);
            this.txtAlmacen.MaxLength = 4;
            this.txtAlmacen.Name = "txtAlmacen";
            this.txtAlmacen.Size = new System.Drawing.Size(81, 26);
            this.txtAlmacen.TabIndex = 12;
            this.txtAlmacen.TextChanged += new System.EventHandler(this.txtAlmacen_TextChanged);
            // 
            // txtDesSucursal
            // 
            this.txtDesSucursal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtDesSucursal.Location = new System.Drawing.Point(349, 390);
            this.txtDesSucursal.MaxLength = 30;
            this.txtDesSucursal.Name = "txtDesSucursal";
            this.txtDesSucursal.Size = new System.Drawing.Size(241, 26);
            this.txtDesSucursal.TabIndex = 11;
            this.txtDesSucursal.TextChanged += new System.EventHandler(this.txtDesSucursal_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(238, 395);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 20);
            this.label7.TabIndex = 28;
            this.label7.Text = "Descripción:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(239, 438);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 20);
            this.label8.TabIndex = 29;
            this.label8.Text = "Descripción:";
            // 
            // txtDesAlmacen
            // 
            this.txtDesAlmacen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtDesAlmacen.Location = new System.Drawing.Point(349, 433);
            this.txtDesAlmacen.MaxLength = 30;
            this.txtDesAlmacen.Name = "txtDesAlmacen";
            this.txtDesAlmacen.Size = new System.Drawing.Size(241, 26);
            this.txtDesAlmacen.TabIndex = 13;
            this.txtDesAlmacen.TextChanged += new System.EventHandler(this.txtDesAlmacen_TextChanged);
            // 
            // pnlCriterios
            // 
            this.pnlCriterios.Controls.Add(this.groupBox1);
            this.pnlCriterios.Location = new System.Drawing.Point(13, 105);
            this.pnlCriterios.Name = "pnlCriterios";
            this.pnlCriterios.Size = new System.Drawing.Size(581, 280);
            this.pnlCriterios.TabIndex = 30;
            this.pnlCriterios.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtDestino);
            this.groupBox1.Controls.Add(this.txtOrigen);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Menu;
            this.groupBox1.Location = new System.Drawing.Point(22, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 252);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Criterios";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(318, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(159, 20);
            this.label11.TabIndex = 3;
            this.label11.Text = "Cadena reemplazo";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(43, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(180, 20);
            this.label10.TabIndex = 2;
            this.label10.Text = "Cadena de busqueda";
            // 
            // txtDestino
            // 
            this.txtDestino.Location = new System.Drawing.Point(318, 55);
            this.txtDestino.Multiline = true;
            this.txtDestino.Name = "txtDestino";
            this.txtDestino.Size = new System.Drawing.Size(177, 187);
            this.txtDestino.TabIndex = 1;
            this.txtDestino.Text = "(17,4)";
            // 
            // txtOrigen
            // 
            this.txtOrigen.Location = new System.Drawing.Point(43, 55);
            this.txtOrigen.Multiline = true;
            this.txtOrigen.Name = "txtOrigen";
            this.txtOrigen.Size = new System.Drawing.Size(177, 187);
            this.txtOrigen.TabIndex = 0;
            this.txtOrigen.Text = "(13,4)";
            // 
            // txtRutaPaquete
            // 
            this.txtRutaPaquete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtRutaPaquete.Location = new System.Drawing.Point(151, 472);
            this.txtRutaPaquete.MaxLength = 200;
            this.txtRutaPaquete.Name = "txtRutaPaquete";
            this.txtRutaPaquete.Size = new System.Drawing.Size(439, 26);
            this.txtRutaPaquete.TabIndex = 31;
            this.txtRutaPaquete.Text = "C:\\inetpub\\wwwroot\\AC_PACIFIC";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(21, 476);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(125, 20);
            this.label12.TabIndex = 32;
            this.label12.Text = "Ruta Paquete:";
            // 
            // txtUrlWebService
            // 
            this.txtUrlWebService.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtUrlWebService.Location = new System.Drawing.Point(181, 510);
            this.txtUrlWebService.MaxLength = 200;
            this.txtUrlWebService.Name = "txtUrlWebService";
            this.txtUrlWebService.Size = new System.Drawing.Size(409, 26);
            this.txtUrlWebService.TabIndex = 33;
            this.txtUrlWebService.Text = "http://localhost/AC_PACIFIC/AC_Service.asmx";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(20, 514);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(155, 20);
            this.label13.TabIndex = 34;
            this.label13.Text = "URL  WebService:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(606, 702);
            this.Controls.Add(this.txtUrlWebService);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtRutaPaquete);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pnlCriterios);
            this.Controls.Add(this.txtDesAlmacen);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDesSucursal);
            this.Controls.Add(this.txtAlmacen);
            this.Controls.Add(this.txtSucursal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gbInstanciaSQL);
            this.Controls.Add(this.gbdbglobal);
            this.Controls.Add(this.lbltitulo);
            this.Controls.Add(this.btnSalirMenu);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.shapeContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(570, 400);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Convertidor de base de datos MySQL a SQL Server 9.9.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbdbglobal.ResumeLayout(false);
            this.gbdbglobal.PerformLayout();
            this.gbInstanciaSQL.ResumeLayout(false);
            this.gbInstanciaSQL.PerformLayout();
            this.pnlCriterios.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbConexionBD;
        private System.Windows.Forms.Label lblConexionBD;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInstancia;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkSoloEstructuraBD;
        private System.Windows.Forms.TextBox txtNombreBD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSalirMenu;
        private System.Windows.Forms.ToolTip toolTip1;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private System.Windows.Forms.Label lbltitulo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkBdDestino;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox gbdbglobal;
        private System.Windows.Forms.GroupBox gbInstanciaSQL;
        private System.Windows.Forms.CheckBox chkInicializadb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSucursal;
        private System.Windows.Forms.TextBox txtAlmacen;
        private System.Windows.Forms.TextBox txtDesSucursal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDesAlmacen;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNombreDB;
        private System.Windows.Forms.Panel pnlCriterios;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDestino;
        private System.Windows.Forms.TextBox txtOrigen;
        private System.Windows.Forms.Button btnCriterios;
        private System.Windows.Forms.TextBox txtRutaPaquete;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtUrlWebService;
        private System.Windows.Forms.Label label13;
    }
}

