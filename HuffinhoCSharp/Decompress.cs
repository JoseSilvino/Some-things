namespace HuffinhoCSharp{
    public class Decompress : HuffinhoCSharp.Structures.HuffmanCode {
        private static readonly Decompress INSTANCE = new Decompress();
        public static Decompress Instance { get { return INSTANCE; } }
        private Structures.HuffTree  createTree(FileStream file, byte b){
            Structures.HuffTree tree = new Structures.HuffTree(b);
            if (b == '\\') tree.Byte = (byte)file.ReadByte();
            if (b == '*'){
                tree.Left = createTree(file, (byte)file.ReadByte());
                tree.Right = createTree(file, (byte)file.ReadByte());
            }
            return tree;
        }
        public override void dohuffman(ref string filepath,ProgressBar bar){
            try{
                FileStream infile = new FileStream(filepath, FileMode.Open);
                FileStream outfile = new FileStream(filepath.Substring(0, filepath.Length - 5), FileMode.Create);
                filepath = "";
                byte f_byte = (byte)infile.ReadByte();
                byte s_byte = (byte)infile.ReadByte();
                ushort trash_size, tree_size;
                trash_size = ((ushort)(f_byte >> 5));
                tree_size = ((byte)(((f_byte << 8) << 3) >> 3));
                tree_size |= s_byte;
                Structures.HuffTree tree = createTree(infile, (byte)infile.ReadByte());
                Structures.HuffTree? tmptree = tree;
                while (infile.Position < infile.Length){
                    byte b = (byte)infile.ReadByte();
                    if (infile.Position < infile.Length){
                        for (int pos = 7; pos >= 0; pos--){
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                            if (tmptree.isLeaf()){
                                outfile.WriteByte(tmptree.Byte);
                                tmptree = tree;
                            }
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
                            if (Program.isBitSet((ushort)b, pos)) tmptree = tmptree.Right;
                            else tmptree = tmptree.Left;
                        }
                    }
                    else{
                        for (int pos = 7; pos >= trash_size; pos--){
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                            if (tmptree.isLeaf()){
                                outfile.WriteByte(tmptree.Byte);
                                tmptree = tree;
                            }
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
                            if (Program.isBitSet((ushort)b, pos)) tmptree = tmptree.Right;
                            else tmptree = tmptree.Left;
                        }
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                        if (tmptree.isLeaf()){
                            outfile.WriteByte(tmptree.Byte);
                            tmptree = tree;
                        }
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
                    }
                    bar.Value = (int)(infile.Position * 100 / infile.Length);
                }
                MessageBox.Show("Decompress Complete");
                bar.Value = 0;
                outfile.Close();
                infile.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}