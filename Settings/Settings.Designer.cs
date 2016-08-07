namespace Start_mpr {
    partial class Settings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Шрифт");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Цвет");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Вид", new System.Windows.Forms.TreeNode[] {
            treeNode21,
            treeNode22});
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Звук");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Еще что то");
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.tree = new System.Windows.Forms.TreeView();
            this.layoutFont = new System.Windows.Forms.TableLayoutPanel();
            this.labelFontFont = new System.Windows.Forms.Label();
            this.labelStyle = new System.Windows.Forms.Label();
            this.labelFontSize = new System.Windows.Forms.Label();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.comboFontStyle = new System.Windows.Forms.ComboBox();
            this.comboFontFont = new System.Windows.Forms.ComboBox();
            this.FontExample = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ABC = new System.Windows.Forms.Label();
            this.btApply = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.layoutFont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            this.FontExample.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btOk, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btCancel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.tree, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.layoutFont, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btApply, 5, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.68588F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.31412F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(664, 347);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btOk
            // 
            this.btOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOk.Location = new System.Drawing.Point(183, 314);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(94, 30);
            this.btOk.TabIndex = 4;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btCancel.Location = new System.Drawing.Point(283, 314);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(94, 30);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Margin = new System.Windows.Forms.Padding(0);
            this.tree.Name = "tree";
            treeNode21.Name = "cNodeFont";
            treeNode21.Text = "Шрифт";
            treeNode22.Name = "cNodeColor";
            treeNode22.Text = "Цвет";
            treeNode23.Name = "nodeView";
            treeNode23.Text = "Вид";
            treeNode24.Name = "nodeSound";
            treeNode24.Text = "Звук";
            treeNode25.Name = "Node0";
            treeNode25.Text = "Еще что то";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode24,
            treeNode25});
            this.tableLayoutPanel1.SetRowSpan(this.tree, 3);
            this.tree.Size = new System.Drawing.Size(180, 347);
            this.tree.TabIndex = 0;
            // 
            // layoutFont
            // 
            this.layoutFont.BackColor = System.Drawing.Color.LightGray;
            this.layoutFont.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.layoutFont, 4);
            this.layoutFont.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFont.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFont.Controls.Add(this.labelFontFont, 0, 3);
            this.layoutFont.Controls.Add(this.labelStyle, 0, 2);
            this.layoutFont.Controls.Add(this.labelFontSize, 0, 1);
            this.layoutFont.Controls.Add(this.numFontSize, 1, 1);
            this.layoutFont.Controls.Add(this.comboFontStyle, 1, 2);
            this.layoutFont.Controls.Add(this.comboFontFont, 1, 3);
            this.layoutFont.Controls.Add(this.FontExample, 0, 5);
            this.layoutFont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFont.Location = new System.Drawing.Point(180, 0);
            this.layoutFont.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFont.Name = "layoutFont";
            this.layoutFont.RowCount = 6;
            this.tableLayoutPanel1.SetRowSpan(this.layoutFont, 2);
            this.layoutFont.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutFont.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutFont.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutFont.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutFont.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutFont.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutFont.Size = new System.Drawing.Size(484, 311);
            this.layoutFont.TabIndex = 1;
            this.layoutFont.Visible = false;
            // 
            // labelFontFont
            // 
            this.labelFontFont.AutoSize = true;
            this.labelFontFont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFontFont.Location = new System.Drawing.Point(3, 80);
            this.labelFontFont.Name = "labelFontFont";
            this.labelFontFont.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.labelFontFont.Size = new System.Drawing.Size(236, 30);
            this.labelFontFont.TabIndex = 4;
            this.labelFontFont.Text = "Шрифт:";
            this.labelFontFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStyle
            // 
            this.labelStyle.AutoSize = true;
            this.labelStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStyle.Location = new System.Drawing.Point(3, 50);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.labelStyle.Size = new System.Drawing.Size(236, 30);
            this.labelStyle.TabIndex = 2;
            this.labelStyle.Text = "Стиль";
            this.labelStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFontSize
            // 
            this.labelFontSize.AutoSize = true;
            this.labelFontSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFontSize.Location = new System.Drawing.Point(3, 20);
            this.labelFontSize.Name = "labelFontSize";
            this.labelFontSize.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.labelFontSize.Size = new System.Drawing.Size(236, 30);
            this.labelFontSize.TabIndex = 0;
            this.labelFontSize.Text = "Размер";
            this.labelFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numFontSize
            // 
            this.numFontSize.Location = new System.Drawing.Point(245, 23);
            this.numFontSize.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numFontSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(120, 20);
            this.numFontSize.TabIndex = 1;
            this.numFontSize.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // comboFontStyle
            // 
            this.comboFontStyle.FormattingEnabled = true;
            this.comboFontStyle.Location = new System.Drawing.Point(245, 53);
            this.comboFontStyle.Name = "comboFontStyle";
            this.comboFontStyle.Size = new System.Drawing.Size(121, 21);
            this.comboFontStyle.TabIndex = 3;
            // 
            // comboFontFont
            // 
            this.comboFontFont.FormattingEnabled = true;
            this.comboFontFont.Location = new System.Drawing.Point(245, 83);
            this.comboFontFont.Name = "comboFontFont";
            this.comboFontFont.Size = new System.Drawing.Size(121, 21);
            this.comboFontFont.TabIndex = 5;
            // 
            // FontExample
            // 
            this.layoutFont.SetColumnSpan(this.FontExample, 2);
            this.FontExample.Controls.Add(this.tableLayoutPanel2);
            this.FontExample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontExample.Location = new System.Drawing.Point(30, 143);
            this.FontExample.Margin = new System.Windows.Forms.Padding(30, 3, 30, 30);
            this.FontExample.Name = "FontExample";
            this.FontExample.Size = new System.Drawing.Size(424, 138);
            this.FontExample.TabIndex = 6;
            this.FontExample.TabStop = false;
            this.FontExample.Text = "Font example";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ABC, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(418, 119);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // ABC
            // 
            this.ABC.AutoSize = true;
            this.ABC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ABC.Location = new System.Drawing.Point(3, 0);
            this.ABC.Name = "ABC";
            this.ABC.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.ABC.Size = new System.Drawing.Size(412, 119);
            this.ABC.TabIndex = 0;
            this.ABC.Text = "ABC";
            this.ABC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btApply
            // 
            this.btApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btApply.Location = new System.Drawing.Point(567, 314);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(94, 30);
            this.btApply.TabIndex = 2;
            this.btApply.Text = "Apply";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 347);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(500, 39);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.layoutFont.ResumeLayout(false);
            this.layoutFont.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            this.FontExample.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.TableLayoutPanel layoutFont;
        private System.Windows.Forms.Button btApply;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label labelFontSize;
        private System.Windows.Forms.NumericUpDown numFontSize;
        private System.Windows.Forms.Label labelStyle;
        private System.Windows.Forms.ComboBox comboFontStyle;
        private System.Windows.Forms.Label labelFontFont;
        private System.Windows.Forms.ComboBox comboFontFont;
        private System.Windows.Forms.GroupBox FontExample;
        private System.Windows.Forms.Label ABC;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}

