namespace Pantallas_Sistema_facturacion
{
    partial class frmInformes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.grpInformes = new System.Windows.Forms.GroupBox();
            this.btnVentasDiarias = new System.Windows.Forms.Button();
            this.btnVentasMensuales = new System.Windows.Forms.Button();
            this.btnProductosMasVendidos = new System.Windows.Forms.Button();
            this.btnClientesFrecuentes = new System.Windows.Forms.Button();
            this.btnInventario = new System.Windows.Forms.Button();
            this.grpExportar = new System.Windows.Forms.GroupBox();
            this.btnExportarPDF = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.panelVistaPrevia = new System.Windows.Forms.Panel();
            this.txtVistaPrevia = new System.Windows.Forms.TextBox();
            this.lblVistaPrevia = new System.Windows.Forms.Label();
            this.panelTitulo.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            this.grpFiltros.SuspendLayout();
            this.grpInformes.SuspendLayout();
            this.grpExportar.SuspendLayout();
            this.panelVistaPrevia.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitulo
            // 
            this.panelTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panelTitulo.Controls.Add(this.lblTitulo);
            this.panelTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitulo.Location = new System.Drawing.Point(0, 0);
            this.panelTitulo.Name = "panelTitulo";
            this.panelTitulo.Size = new System.Drawing.Size(950, 60);
            this.panelTitulo.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(950, 60);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "📊 Módulo de Informes";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelIzquierdo
            // 
            this.panelIzquierdo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelIzquierdo.Controls.Add(this.grpExportar);
            this.panelIzquierdo.Controls.Add(this.grpInformes);
            this.panelIzquierdo.Controls.Add(this.grpFiltros);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 60);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Padding = new System.Windows.Forms.Padding(10);
            this.panelIzquierdo.Size = new System.Drawing.Size(280, 500);
            this.panelIzquierdo.TabIndex = 1;
            // 
            // grpFiltros
            // 
            this.grpFiltros.Controls.Add(this.lblFechaInicio);
            this.grpFiltros.Controls.Add(this.dtpFechaInicio);
            this.grpFiltros.Controls.Add(this.lblFechaFin);
            this.grpFiltros.Controls.Add(this.dtpFechaFin);
            this.grpFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFiltros.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.grpFiltros.Location = new System.Drawing.Point(10, 10);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Size = new System.Drawing.Size(260, 120);
            this.grpFiltros.TabIndex = 0;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "📅 Filtros de Fecha";
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFechaInicio.ForeColor = System.Drawing.Color.Black;
            this.lblFechaInicio.Location = new System.Drawing.Point(15, 30);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(86, 20);
            this.lblFechaInicio.TabIndex = 0;
            this.lblFechaInicio.Text = "Fecha Inicio:";
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(110, 27);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(130, 27);
            this.dtpFechaInicio.TabIndex = 1;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFechaFin.ForeColor = System.Drawing.Color.Black;
            this.lblFechaFin.Location = new System.Drawing.Point(15, 70);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(70, 20);
            this.lblFechaFin.TabIndex = 2;
            this.lblFechaFin.Text = "Fecha Fin:";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(110, 67);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(130, 27);
            this.dtpFechaFin.TabIndex = 3;
            // 
            // grpInformes
            // 
            this.grpInformes.Controls.Add(this.btnVentasDiarias);
            this.grpInformes.Controls.Add(this.btnVentasMensuales);
            this.grpInformes.Controls.Add(this.btnProductosMasVendidos);
            this.grpInformes.Controls.Add(this.btnClientesFrecuentes);
            this.grpInformes.Controls.Add(this.btnInventario);
            this.grpInformes.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpInformes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpInformes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.grpInformes.Location = new System.Drawing.Point(10, 130);
            this.grpInformes.Name = "grpInformes";
            this.grpInformes.Size = new System.Drawing.Size(260, 230);
            this.grpInformes.TabIndex = 1;
            this.grpInformes.TabStop = false;
            this.grpInformes.Text = "📋 Tipos de Informe";
            // 
            // btnVentasDiarias
            // 
            this.btnVentasDiarias.BackColor = System.Drawing.Color.White;
            this.btnVentasDiarias.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVentasDiarias.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnVentasDiarias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVentasDiarias.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnVentasDiarias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnVentasDiarias.Location = new System.Drawing.Point(15, 30);
            this.btnVentasDiarias.Name = "btnVentasDiarias";
            this.btnVentasDiarias.Size = new System.Drawing.Size(225, 35);
            this.btnVentasDiarias.TabIndex = 0;
            this.btnVentasDiarias.Text = "📈 Ventas Diarias";
            this.btnVentasDiarias.UseVisualStyleBackColor = false;
            this.btnVentasDiarias.Click += new System.EventHandler(this.btnVentasDiarias_Click);
            // 
            // btnVentasMensuales
            // 
            this.btnVentasMensuales.BackColor = System.Drawing.Color.White;
            this.btnVentasMensuales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVentasMensuales.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnVentasMensuales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVentasMensuales.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnVentasMensuales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnVentasMensuales.Location = new System.Drawing.Point(15, 70);
            this.btnVentasMensuales.Name = "btnVentasMensuales";
            this.btnVentasMensuales.Size = new System.Drawing.Size(225, 35);
            this.btnVentasMensuales.TabIndex = 1;
            this.btnVentasMensuales.Text = "📅 Ventas Mensuales";
            this.btnVentasMensuales.UseVisualStyleBackColor = false;
            this.btnVentasMensuales.Click += new System.EventHandler(this.btnVentasMensuales_Click);
            // 
            // btnProductosMasVendidos
            // 
            this.btnProductosMasVendidos.BackColor = System.Drawing.Color.White;
            this.btnProductosMasVendidos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProductosMasVendidos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnProductosMasVendidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductosMasVendidos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnProductosMasVendidos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnProductosMasVendidos.Location = new System.Drawing.Point(15, 110);
            this.btnProductosMasVendidos.Name = "btnProductosMasVendidos";
            this.btnProductosMasVendidos.Size = new System.Drawing.Size(225, 35);
            this.btnProductosMasVendidos.TabIndex = 2;
            this.btnProductosMasVendidos.Text = "🏆 Productos Más Vendidos";
            this.btnProductosMasVendidos.UseVisualStyleBackColor = false;
            this.btnProductosMasVendidos.Click += new System.EventHandler(this.btnProductosMasVendidos_Click);
            // 
            // btnClientesFrecuentes
            // 
            this.btnClientesFrecuentes.BackColor = System.Drawing.Color.White;
            this.btnClientesFrecuentes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClientesFrecuentes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnClientesFrecuentes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientesFrecuentes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClientesFrecuentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnClientesFrecuentes.Location = new System.Drawing.Point(15, 150);
            this.btnClientesFrecuentes.Name = "btnClientesFrecuentes";
            this.btnClientesFrecuentes.Size = new System.Drawing.Size(225, 35);
            this.btnClientesFrecuentes.TabIndex = 3;
            this.btnClientesFrecuentes.Text = "👥 Clientes Frecuentes";
            this.btnClientesFrecuentes.UseVisualStyleBackColor = false;
            this.btnClientesFrecuentes.Click += new System.EventHandler(this.btnClientesFrecuentes_Click);
            // 
            // btnInventario
            // 
            this.btnInventario.BackColor = System.Drawing.Color.White;
            this.btnInventario.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInventario.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnInventario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInventario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnInventario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnInventario.Location = new System.Drawing.Point(15, 190);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Size = new System.Drawing.Size(225, 35);
            this.btnInventario.TabIndex = 4;
            this.btnInventario.Text = "📦 Inventario";
            this.btnInventario.UseVisualStyleBackColor = false;
            this.btnInventario.Click += new System.EventHandler(this.btnInventario_Click);
            // 
            // grpExportar
            // 
            this.grpExportar.Controls.Add(this.btnExportarPDF);
            this.grpExportar.Controls.Add(this.btnExportarExcel);
            this.grpExportar.Controls.Add(this.btnImprimir);
            this.grpExportar.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpExportar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpExportar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.grpExportar.Location = new System.Drawing.Point(10, 360);
            this.grpExportar.Name = "grpExportar";
            this.grpExportar.Size = new System.Drawing.Size(260, 130);
            this.grpExportar.TabIndex = 2;
            this.grpExportar.TabStop = false;
            this.grpExportar.Text = "💾 Exportar";
            // 
            // btnExportarPDF
            // 
            this.btnExportarPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnExportarPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarPDF.FlatAppearance.BorderSize = 0;
            this.btnExportarPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPDF.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExportarPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportarPDF.Location = new System.Drawing.Point(15, 30);
            this.btnExportarPDF.Name = "btnExportarPDF";
            this.btnExportarPDF.Size = new System.Drawing.Size(110, 35);
            this.btnExportarPDF.TabIndex = 0;
            this.btnExportarPDF.Text = "📄 PDF";
            this.btnExportarPDF.UseVisualStyleBackColor = false;
            this.btnExportarPDF.Click += new System.EventHandler(this.btnExportarPDF_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnExportarExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarExcel.FlatAppearance.BorderSize = 0;
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExportarExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportarExcel.Location = new System.Drawing.Point(130, 30);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(110, 35);
            this.btnExportarExcel.TabIndex = 1;
            this.btnExportarExcel.Text = "📊 Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImprimir.FlatAppearance.BorderSize = 0;
            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.Location = new System.Drawing.Point(15, 75);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(225, 35);
            this.btnImprimir.TabIndex = 2;
            this.btnImprimir.Text = "🖨️ Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // panelVistaPrevia
            // 
            this.panelVistaPrevia.BackColor = System.Drawing.Color.White;
            this.panelVistaPrevia.Controls.Add(this.txtVistaPrevia);
            this.panelVistaPrevia.Controls.Add(this.lblVistaPrevia);
            this.panelVistaPrevia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVistaPrevia.Location = new System.Drawing.Point(280, 60);
            this.panelVistaPrevia.Name = "panelVistaPrevia";
            this.panelVistaPrevia.Padding = new System.Windows.Forms.Padding(15);
            this.panelVistaPrevia.Size = new System.Drawing.Size(670, 500);
            this.panelVistaPrevia.TabIndex = 2;
            // 
            // lblVistaPrevia
            // 
            this.lblVistaPrevia.AutoSize = true;
            this.lblVistaPrevia.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblVistaPrevia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblVistaPrevia.Location = new System.Drawing.Point(15, 15);
            this.lblVistaPrevia.Name = "lblVistaPrevia";
            this.lblVistaPrevia.Size = new System.Drawing.Size(182, 28);
            this.lblVistaPrevia.TabIndex = 0;
            this.lblVistaPrevia.Text = "👁️ Vista Previa";
            // 
            // txtVistaPrevia
            // 
            this.txtVistaPrevia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtVistaPrevia.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVistaPrevia.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtVistaPrevia.ForeColor = System.Drawing.Color.White;
            this.txtVistaPrevia.Location = new System.Drawing.Point(15, 50);
            this.txtVistaPrevia.Multiline = true;
            this.txtVistaPrevia.Name = "txtVistaPrevia";
            this.txtVistaPrevia.ReadOnly = true;
            this.txtVistaPrevia.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtVistaPrevia.Size = new System.Drawing.Size(640, 435);
            this.txtVistaPrevia.TabIndex = 1;
            this.txtVistaPrevia.Text = "Seleccione un tipo de informe para generar la vista previa...";
            // 
            // frmInformes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 560);
            this.Controls.Add(this.panelVistaPrevia);
            this.Controls.Add(this.panelIzquierdo);
            this.Controls.Add(this.panelTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInformes";
            this.Text = "Informes";
            this.Load += new System.EventHandler(this.frmInformes_Load);
            this.panelTitulo.ResumeLayout(false);
            this.panelIzquierdo.ResumeLayout(false);
            this.grpFiltros.ResumeLayout(false);
            this.grpFiltros.PerformLayout();
            this.grpInformes.ResumeLayout(false);
            this.grpExportar.ResumeLayout(false);
            this.panelVistaPrevia.ResumeLayout(false);
            this.panelVistaPrevia.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.GroupBox grpInformes;
        private System.Windows.Forms.Button btnVentasDiarias;
        private System.Windows.Forms.Button btnVentasMensuales;
        private System.Windows.Forms.Button btnProductosMasVendidos;
        private System.Windows.Forms.Button btnClientesFrecuentes;
        private System.Windows.Forms.Button btnInventario;
        private System.Windows.Forms.GroupBox grpExportar;
        private System.Windows.Forms.Button btnExportarPDF;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Panel panelVistaPrevia;
        private System.Windows.Forms.Label lblVistaPrevia;
        private System.Windows.Forms.TextBox txtVistaPrevia;
    }
}
