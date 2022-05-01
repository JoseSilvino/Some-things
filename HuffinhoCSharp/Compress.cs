namespace HuffinhoCSharp{
    public class Compress: Structures.HuffmanCode{
        private static readonly Compress INSTANCE = new();
        public static Compress Instance { get { return INSTANCE; } }
        public override void dohuffman(ref string filepath, ProgressBar bar){
            try{

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
                while (infile.Position < infile.Length){
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
                int trash = (to_shift + 1) % 8;
                if (trash != 0) outfile.WriteByte(w_byte);
                MessageBox.Show("Compression Complete");
                outfile.Position = 0;
                int t_size = 0;
                Structures.HuffTree.tsize(tree, ref t_size);
                byte[] header = new byte[2];
                header[0] = (byte)(trash << 5 | t_size >> 8);
                header[1] = (byte)(t_size);
                outfile.WriteByte(header[0]);
                outfile.WriteByte(header[1]);
                outfile.Close();
                infile.Close();
                bar.Value = 0;
                filepath = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}