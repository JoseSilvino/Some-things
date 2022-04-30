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
        FileDialog chooser;
        string filepath;
        private void dialogMethod() {
            var res = chooser.ShowDialog();
            if (res == DialogResult.OK) { filepath = chooser.FileName; }
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
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Text = "Huffinho";
            this.Icon = new Icon("Cs.ico");
            this.c = new Button();
            d = new Button();
            e = new Button();
            bar = new ProgressBar();
            bar.Style = ProgressBarStyle.Continuous;
            bar.Size = new Size(100, 23);
            c.Size = new System.Drawing.Size(75, 23);
            d.Size = new System.Drawing.Size(75, 23);
            e.Size = new System.Drawing.Size(75, 23);
            c.Text = "Compress";
            d.Text = "Decompress";
            e.Text = "Exit";
            c.Location = new System.Drawing.Point(this.Width / 2 - c.Width / 2 - d.Width, this.Height / 2 - c.Height / 2);
            d.Location = new System.Drawing.Point(this.Width / 2 + d.Width / 2, this.Height / 2 - d.Height / 2);
            e.Location = new System.Drawing.Point(this.Width - e.Width - 16, this.Height - e.Width + 13);
            bar.Location = new Point(this.Width / 2-bar.Width/2, this.Height / 2 + bar.Height);
            bar.Maximum = 100;
            c.Click += new EventHandler(compressClick);
            d.Click += new EventHandler(decompressClick);
            e.Click += new EventHandler(exitClick);
            this.Controls.Add(c);
            this.Controls.Add(d);
            this.Controls.Add(e);
            this.Controls.Add(bar);
            chooser = new OpenFileDialog();
            chooser.Filter = "All Files (*.*)|*.*";
            filepath = "";
        }
        private void exitClick(object sender,EventArgs e){
            Dispose();
        }
        private void print_byte(byte b){
            string s = "";
            for (int i = 7; i >= 0; i--) {
                if (Program.isBitSet((ushort)b, i)) s += '1';
                else s += '0';
            }
            MessageBox.Show(s);
        }
        Structures.HuffTree createTree(FileStream file,byte b){
            Structures.HuffTree tree = new Structures.HuffTree(b);
            if (b == '\\') tree.Byte = (byte)file.ReadByte();
            if (b == '*'){
                tree.Left = createTree(file, (byte)file.ReadByte());
                tree.Right = createTree(file, (byte)file.ReadByte());
            }
            return tree;
        }
        private void decompressClick(object sender, EventArgs e){
            try{
                dialogThreadCreate();
                long seconds = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                while (filepath == "") { if ((DateTime.Now.Ticks / TimeSpan.TicksPerSecond) - seconds == 30) throw new Exception(); }
                FileStream infile = new FileStream(filepath, FileMode.Open);
                FileStream outfile = new FileStream(filepath.Substring(0, filepath.Length - 5), FileMode.Create);
                filepath = "";
                byte f_byte = (byte)infile.ReadByte();
                byte s_byte = (byte)infile.ReadByte();
                ushort trash_size, tree_size;
                trash_size = ((ushort)(f_byte >> 5));
                tree_size = ((byte)(((f_byte << 8)<<3)>>3)) ;
                tree_size |= s_byte;
                Structures.HuffTree tree = createTree(infile, (byte)infile.ReadByte());
                var tmptree = tree;
                while (infile.Position < infile.Length){
                    byte b = (byte)infile.ReadByte();
                    if (infile.Position < infile.Length) {
                        for (int pos = 7; pos >= 0; pos--) {
                            if (tmptree.isLeaf()){
                                outfile.WriteByte(tmptree.Byte);
                                tmptree = tree;
                            }if (Program.isBitSet((ushort)b, pos)) tmptree = tmptree.Right;
                            else tmptree = tmptree.Left;
                        }
                    } else{
                            for (int pos = 7; pos >= trash_size; pos--) {
                            if (tmptree.isLeaf()) {
                                outfile.WriteByte(tmptree.Byte);
                                tmptree = tree;
                            }if(Program.isBitSet((ushort)b,pos)) tmptree= tmptree.Right;
                            else tmptree = tmptree.Left;
                        }if (tmptree.isLeaf()) {
                            outfile.WriteByte(tmptree.Byte);
                            tmptree = tree;
                        }
                    }
                    bar.Value = (int)(infile.Position * 100 / infile.Length);
                }
                MessageBox.Show("Decompress Complete");
                bar.Value = 0;
                outfile.Close();
                infile.Close();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void compressClick(object sender, EventArgs e) {
            try{
                dialogThreadCreate();
                long seconds = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                while (filepath == "") { if ((DateTime.Now.Ticks / TimeSpan.TicksPerSecond) - seconds == 30) throw new Exception(); }
                FileStream infile = new FileStream(filepath, FileMode.Open);
                FileStream outfile = new FileStream(filepath + ".huff", FileMode.Create);
                ulong[] freq = new ulong[256];
                while (infile.Position < infile.Length){
                    byte b = (byte)infile.ReadByte();
                    freq[b]++;
                }
                Structures.Heap hipi = new Structures.Heap();
                for (short b = 0; b < 256; b++) if (freq[b] != 0) hipi.enqueue(new Structures.HuffTree((byte)b, freq[b]));
                Structures.HuffTree tree = hipi.HTFromHeap();
                outfile.WriteByte((byte)0);
                outfile.WriteByte((byte)0);
                string treestr = tree.ToString();
                for (int i = 0; i < treestr.Length; i++) outfile.WriteByte((byte)treestr[i]);
                Structures.HashTable hash = new Structures.HashTable();
                tree.newByteValues(hash, (byte)0, (ushort)0);
                infile.Position = 0;
                byte w_byte = 0;
                byte to_shift = 7;
                while (infile.Position < infile.Length) {
                    byte b = (byte)infile.ReadByte();
                    Structures.Element el = hash[b];
                    ushort n_value = el.HuffValue;
                    short n_tam = (short)el.Size;
                    while (n_tam > -1){
                        if (Program.isBitSet(n_value, n_tam)) w_byte = Program.setBit(w_byte, to_shift);
                        if (to_shift == 0){
                            outfile.WriteByte(w_byte);
                            to_shift = 7;
                            w_byte = 0;
                        }
                        else to_shift--;
                        n_tam--;
                    }
                    bar.Value = (int)(infile.Position * 100 / infile.Length);
                }
                int trash = (to_shift+1) % 8;
                if (trash != 0) outfile.WriteByte(w_byte);
                MessageBox.Show("Compression Complete");
                outfile.Position = 0;
                int t_size = 0;
                Structures.HuffTree.tsize(tree, ref t_size);
                byte[] header = new byte[2];
                header[0] = (byte)(trash << 5 | t_size>>8);
                header[1] = (byte)(t_size);
                outfile.WriteByte(header[0]);
                outfile.WriteByte(header[1]);
                outfile.Close();
                infile.Close();
                bar.Value = 0;
                filepath = "";
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        private ProgressBar bar;
        private Button c, d, e;
        #endregion
    }
}