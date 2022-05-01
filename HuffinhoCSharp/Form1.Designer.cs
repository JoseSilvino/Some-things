namespace HuffinhoCSharp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        private string filepath;
        private OpenFileDialog openFileDialog;
        private Button snder = null;
        private bool cancel = false;
        private void dialogMethod() {
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK){
                filepath = openFileDialog.FileName;
                snder = null;
                cancel = false;
            }
            else cancel= true;
        }
        private void dialogThreadCreate() {
            Thread dialogThread = new Thread(new ThreadStart(dialogMethod));
            dialogThread.SetApartmentState(ApartmentState.STA);
            dialogThread.Start();
        }
        
        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 150);
            this.MaximizeBox = false;
            this.Text = "Huffinho";
            this.Icon = new Icon("Cs.ico");
            this.c = new Button();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            d = new Button();
            e = new Button();
            bar = new ProgressBar();
            bar.Style = ProgressBarStyle.Blocks;
            bar.Size = new Size(100, 23);
            c.Size = new System.Drawing.Size(80, 23);
            d.Size = new System.Drawing.Size(80, 23);
            e.Size = new System.Drawing.Size(60, 23);
            c.Text = "Compress";
            d.Text = "Decompress";
            e.Text = "Exit";
            c.Location = new System.Drawing.Point(this.Width / 2 - c.Width / 2 - d.Width, this.Height / 2 - c.Height*2);
            d.Location = new System.Drawing.Point(this.Width / 2 + d.Width / 2, this.Height / 2 - d.Height*2);
            e.Location = new System.Drawing.Point(this.Width - e.Width - 16, this.Height - e.Width);
            bar.Location = new Point(this.Width / 2-bar.Width/2, this.Height / 2 + bar.Height/2);
            bar.Maximum = 100;
            c.Click += new EventHandler(compressClick);
            d.Click += new EventHandler(decompressClick);
            e.Click += new EventHandler(exitClick);
            Paint += new PaintEventHandler(to_paint);
            this.Controls.Add(c);
            this.Controls.Add(d);
            this.Controls.Add(e);
            this.Controls.Add(bar);
            openFileDialog = new();
            openFileDialog.InitialDirectory = "C:\\";
        }
        void to_paint(object sender,PaintEventArgs e){
            e.Graphics.DrawIcon(this.Icon,new Rectangle(0,0,this.Width-16,this.Height-38));
            Font font = new Font("Arial", 10, GraphicsUnit.Pixel);
            Rectangle rec = new Rectangle(this.Width / 2 - 80, 0, 160, 12);
            e.Graphics.DrawRectangle(Pens.AntiqueWhite,rec);
            e.Graphics.DrawString("O que quer fazer com o arquivo?", font, Brushes.Black,rec);
            rec.Y = bar.Location.Y - 13;
            rec.Width = 55;
            rec.X = bar.Location.X + 25;
            e.Graphics.DrawRectangle(Pens.NavajoWhite, rec);
            e.Graphics.DrawString("Progresso:", font, Brushes.Black, rec);

        }
        private void exitClick(object sender,EventArgs e){
            Dispose(true);
        }
        
        private void decompressClick(object sender, EventArgs e){
            snder = (Button)sender;
            dialogThreadCreate();
            while (snder != null && !cancel) ;
            if (!cancel) Decompress.Instance.dohuffman(ref filepath, bar);

        }
        private void compressClick(object sender, EventArgs e) {
            snder = (Button)sender;
            dialogThreadCreate();
            while (snder != null && !cancel) ;
            if (!cancel) Compress.Instance.dohuffman(ref filepath, bar);
        }
        private ProgressBar bar;
        private Button c, d, e;
        #endregion
    }
}