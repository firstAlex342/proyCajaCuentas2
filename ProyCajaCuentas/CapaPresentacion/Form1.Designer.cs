namespace CapaPresentacion
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
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
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.cuentaPorPagarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevaCuentaPorPagarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aportacionesACuentaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.socioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarSocioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarProductoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pagoDeProductoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoPagoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelContenedor = new System.Windows.Forms.Panel();
            this.buscarPagosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cuentaPorPagarToolStripMenuItem,
            this.socioToolStripMenuItem,
            this.productoToolStripMenuItem,
            this.pagoDeProductoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // cuentaPorPagarToolStripMenuItem
            // 
            this.cuentaPorPagarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevaCuentaPorPagarToolStripMenuItem,
            this.aportacionesACuentaToolStripMenuItem});
            this.cuentaPorPagarToolStripMenuItem.Name = "cuentaPorPagarToolStripMenuItem";
            this.cuentaPorPagarToolStripMenuItem.Size = new System.Drawing.Size(111, 20);
            this.cuentaPorPagarToolStripMenuItem.Text = "Cuenta por pagar";
            // 
            // nuevaCuentaPorPagarToolStripMenuItem
            // 
            this.nuevaCuentaPorPagarToolStripMenuItem.Name = "nuevaCuentaPorPagarToolStripMenuItem";
            this.nuevaCuentaPorPagarToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.nuevaCuentaPorPagarToolStripMenuItem.Text = "Productos A pagar en cuenta";
            this.nuevaCuentaPorPagarToolStripMenuItem.Click += new System.EventHandler(this.nuevaCuentaPorPagarToolStripMenuItem_Click);
            // 
            // aportacionesACuentaToolStripMenuItem
            // 
            this.aportacionesACuentaToolStripMenuItem.Name = "aportacionesACuentaToolStripMenuItem";
            this.aportacionesACuentaToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.aportacionesACuentaToolStripMenuItem.Text = "Aportaciones a producto en cuenta";
            this.aportacionesACuentaToolStripMenuItem.Click += new System.EventHandler(this.aportacionesACuentaToolStripMenuItem_Click);
            // 
            // socioToolStripMenuItem
            // 
            this.socioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editarSocioToolStripMenuItem});
            this.socioToolStripMenuItem.Name = "socioToolStripMenuItem";
            this.socioToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.socioToolStripMenuItem.Text = "Socio";
            // 
            // editarSocioToolStripMenuItem
            // 
            this.editarSocioToolStripMenuItem.Name = "editarSocioToolStripMenuItem";
            this.editarSocioToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.editarSocioToolStripMenuItem.Text = "Editar Socio";
            this.editarSocioToolStripMenuItem.Click += new System.EventHandler(this.editarSocioToolStripMenuItem_Click);
            // 
            // productoToolStripMenuItem
            // 
            this.productoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editarProductoToolStripMenuItem});
            this.productoToolStripMenuItem.Name = "productoToolStripMenuItem";
            this.productoToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.productoToolStripMenuItem.Text = "Producto";
            // 
            // editarProductoToolStripMenuItem
            // 
            this.editarProductoToolStripMenuItem.Name = "editarProductoToolStripMenuItem";
            this.editarProductoToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.editarProductoToolStripMenuItem.Text = "Editar Producto";
            this.editarProductoToolStripMenuItem.Click += new System.EventHandler(this.editarProductoToolStripMenuItem_Click);
            // 
            // pagoDeProductoToolStripMenuItem
            // 
            this.pagoDeProductoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoPagoToolStripMenuItem,
            this.buscarPagosToolStripMenuItem});
            this.pagoDeProductoToolStripMenuItem.Name = "pagoDeProductoToolStripMenuItem";
            this.pagoDeProductoToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.pagoDeProductoToolStripMenuItem.Text = "Pago de Producto";
            // 
            // nuevoPagoToolStripMenuItem
            // 
            this.nuevoPagoToolStripMenuItem.Name = "nuevoPagoToolStripMenuItem";
            this.nuevoPagoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.nuevoPagoToolStripMenuItem.Text = "Nuevo pago";
            this.nuevoPagoToolStripMenuItem.Click += new System.EventHandler(this.nuevoPagoToolStripMenuItem_Click);
            // 
            // panelContenedor
            // 
            this.panelContenedor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContenedor.BackColor = System.Drawing.Color.White;
            this.panelContenedor.Location = new System.Drawing.Point(0, 27);
            this.panelContenedor.Name = "panelContenedor";
            this.panelContenedor.Size = new System.Drawing.Size(800, 423);
            this.panelContenedor.TabIndex = 1;
            // 
            // buscarPagosToolStripMenuItem
            // 
            this.buscarPagosToolStripMenuItem.Name = "buscarPagosToolStripMenuItem";
            this.buscarPagosToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.buscarPagosToolStripMenuItem.Text = "Buscar pagos";
            this.buscarPagosToolStripMenuItem.Click += new System.EventHandler(this.buscarPagosToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelContenedor);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cuentaPorPagarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevaCuentaPorPagarToolStripMenuItem;
        private System.Windows.Forms.Panel panelContenedor;
        private System.Windows.Forms.ToolStripMenuItem aportacionesACuentaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem socioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editarSocioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editarProductoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pagoDeProductoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoPagoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buscarPagosToolStripMenuItem;
    }
}

