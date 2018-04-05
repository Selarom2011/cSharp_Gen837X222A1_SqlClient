namespace Gen837X222A1
{
    partial class frmGen837
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnViewTables = new System.Windows.Forms.Button();
            this.btnAddDelRec = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(70, 51);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(118, 49);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnViewTables
            // 
            this.btnViewTables.Location = new System.Drawing.Point(70, 203);
            this.btnViewTables.Name = "btnViewTables";
            this.btnViewTables.Size = new System.Drawing.Size(127, 49);
            this.btnViewTables.TabIndex = 1;
            this.btnViewTables.Text = "View Tables";
            this.btnViewTables.UseVisualStyleBackColor = true;
            this.btnViewTables.Click += new System.EventHandler(this.btnViewTables_Click);
            // 
            // btnAddDelRec
            // 
            this.btnAddDelRec.Location = new System.Drawing.Point(70, 124);
            this.btnAddDelRec.Name = "btnAddDelRec";
            this.btnAddDelRec.Size = new System.Drawing.Size(119, 48);
            this.btnAddDelRec.TabIndex = 3;
            this.btnAddDelRec.Text = "Add/Delete Record";
            this.btnAddDelRec.UseVisualStyleBackColor = true;
            this.btnAddDelRec.Click += new System.EventHandler(this.btnAddDelRec_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Generates an EDI file with data from SQL tables";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Adds a record to SQL tables with sample data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "View records in SQL tables";
            // 
            // frmGen837
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 294);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddDelRec);
            this.Controls.Add(this.btnViewTables);
            this.Controls.Add(this.btnGenerate);
            this.Name = "frmGen837";
            this.Text = "Generates 837 5010X222A1 EDI file with data from a SQL database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnViewTables;
        private System.Windows.Forms.Button btnAddDelRec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

