namespace Downloader
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Gra (Sharpie)");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Edytor map (SME)");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.copobraclb = new System.Windows.Forms.Label();
            this.pobierzbt = new System.Windows.Forms.Button();
            this.foldertb = new System.Windows.Forms.TextBox();
            this.folderbt = new System.Windows.Forms.Button();
            this.gdzielb = new System.Windows.Forms.Label();
            this.pobierzlb = new System.Windows.Forms.Label();
            this.drzewo = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.info = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // copobraclb
            // 
            this.copobraclb.AutoSize = true;
            this.copobraclb.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.copobraclb.Location = new System.Drawing.Point(8, 9);
            this.copobraclb.Name = "copobraclb";
            this.copobraclb.Size = new System.Drawing.Size(208, 20);
            this.copobraclb.TabIndex = 0;
            this.copobraclb.Text = "1. Wybierz co chcesz pobrać:";
            // 
            // pobierzbt
            // 
            this.pobierzbt.Location = new System.Drawing.Point(16, 253);
            this.pobierzbt.Name = "pobierzbt";
            this.pobierzbt.Size = new System.Drawing.Size(93, 34);
            this.pobierzbt.TabIndex = 2;
            this.pobierzbt.Text = "Pobierz!";
            this.pobierzbt.UseVisualStyleBackColor = true;
            this.pobierzbt.Click += new System.EventHandler(this.pobierzbt_Click);
            // 
            // foldertb
            // 
            this.foldertb.Location = new System.Drawing.Point(12, 198);
            this.foldertb.Name = "foldertb";
            this.foldertb.Size = new System.Drawing.Size(221, 20);
            this.foldertb.TabIndex = 3;
            this.foldertb.Enter += new System.EventHandler(this.foldertb_Enter);
            this.foldertb.Leave += new System.EventHandler(this.foldertb_Leave);
            // 
            // folderbt
            // 
            this.folderbt.Location = new System.Drawing.Point(239, 196);
            this.folderbt.Name = "folderbt";
            this.folderbt.Size = new System.Drawing.Size(90, 23);
            this.folderbt.TabIndex = 4;
            this.folderbt.Text = "Wybierz folder ...";
            this.folderbt.UseVisualStyleBackColor = true;
            this.folderbt.Click += new System.EventHandler(this.folderbt_Click);
            // 
            // gdzielb
            // 
            this.gdzielb.AutoSize = true;
            this.gdzielb.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.gdzielb.Location = new System.Drawing.Point(12, 175);
            this.gdzielb.Name = "gdzielb";
            this.gdzielb.Size = new System.Drawing.Size(191, 20);
            this.gdzielb.TabIndex = 5;
            this.gdzielb.Text = "2. Wybierz miejsce zapisu:";
            // 
            // pobierzlb
            // 
            this.pobierzlb.AutoSize = true;
            this.pobierzlb.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.pobierzlb.Location = new System.Drawing.Point(12, 231);
            this.pobierzlb.Name = "pobierzlb";
            this.pobierzlb.Size = new System.Drawing.Size(137, 20);
            this.pobierzlb.TabIndex = 6;
            this.pobierzlb.Text = "3. Kliknij \'Pobierz\':";
            // 
            // drzewo
            // 
            this.drzewo.CheckBoxes = true;
            this.drzewo.ImageIndex = 0;
            this.drzewo.ImageList = this.imageList1;
            this.drzewo.Location = new System.Drawing.Point(12, 32);
            this.drzewo.Name = "drzewo";
            treeNode3.Checked = true;
            treeNode3.ImageKey = "snake.ico";
            treeNode3.Name = "Sharpie.exe";
            treeNode3.SelectedImageKey = "snake.ico";
            treeNode3.Text = "Gra (Sharpie)";
            treeNode3.ToolTipText = "Główna gra.";
            treeNode4.ImageKey = "map.ico";
            treeNode4.Name = "MapEditor.exe";
            treeNode4.SelectedImageKey = "map.ico";
            treeNode4.Text = "Edytor map (SME)";
            treeNode4.ToolTipText = "Umożliwia tworzenie i edycję map do gry.";
            this.drzewo.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            this.drzewo.SelectedImageIndex = 0;
            this.drzewo.ShowRootLines = false;
            this.drzewo.Size = new System.Drawing.Size(317, 110);
            this.drzewo.TabIndex = 7;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "DLL_icon_on_Windows_Vista.png");
            this.imageList1.Images.SetKeyName(1, "snake.ico");
            this.imageList1.Images.SetKeyName(2, "map.ico");
            // 
            // info
            // 
            this.info.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.info.Location = new System.Drawing.Point(10, 145);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(317, 21);
            this.info.TabIndex = 9;
            this.info.Text = "Pobrane zostaną również biblioteki wymagane przez grę lub/i edytor.";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.Location = new System.Drawing.Point(115, 258);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(54, 21);
            this.status.TabIndex = 10;
            this.status.Text = "label1";
            this.status.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 293);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(313, 10);
            this.progressBar1.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 311);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.info);
            this.Controls.Add(this.drzewo);
            this.Controls.Add(this.pobierzlb);
            this.Controls.Add(this.gdzielb);
            this.Controls.Add(this.folderbt);
            this.Controls.Add(this.foldertb);
            this.Controls.Add(this.pobierzbt);
            this.Controls.Add(this.copobraclb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sharpie Downloader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label copobraclb;
        private System.Windows.Forms.Button pobierzbt;
        private System.Windows.Forms.TextBox foldertb;
        private System.Windows.Forms.Button folderbt;
        private System.Windows.Forms.Label gdzielb;
        private System.Windows.Forms.Label pobierzlb;
        private System.Windows.Forms.TreeView drzewo;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label info;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

