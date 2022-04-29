import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintStream;
import java.math.BigInteger;
import java.util.Arrays;
import java.util.Scanner;

import javax.swing.JOptionPane;

public final class Teste2 {
    static int tsize = 0;

    public static boolean is_bit_set(short b, int i) {
        int mask = 1 << i;
        boolean res = (mask & b) != 0;
        return res;
    }

    public static byte set_bit(byte b, int i) {
        int mask = 1 << i;
        byte res = (byte) (mask | b);
        return res;
    }

    public static byte[] create_header(int trash, int tsize) {
        byte[] header = new byte[2];
        header[0] = (byte) ((trash << 5) | tsize >> 8);
        header[1] = (byte) tsize;
        return header;
    }

    public static class CouldNotReadFileException extends Exception {
        public CouldNotReadFileException(File f) {
            super("Could not read file " + f.getPath());
        }
    }

    public static void Compress(File f, Teste.Myframe parent) {
        try {
            File new_file = new File(f.toString() + ".huff");
            new_file.createNewFile();
            Scanner input = new Scanner(f);
            PrintStream out = new PrintStream(new_file);
            BigInteger[] freqs = new BigInteger[256];
            for (int i = 0; i < 256; i++)
                freqs[i] = new BigInteger("0");
            BigInteger one = new BigInteger("1");
            input.useDelimiter("");
            if (!input.hasNext()) {
                input.close();
                out.close();
                throw new CouldNotReadFileException(f);
            }
            while (input.hasNext()) {
                byte b = (byte) input.next().charAt(0);
                System.out.println((char) b);
                freqs[(int) b] = freqs[(int) b].add(one);
            }
            Heap h = new Heap();
            for (int i = 0; i < 256; i++)
                if (freqs[i].compareTo(new BigInteger("0")) != 0)
                    h.enqueue(new HuffNode(freqs[i], (byte) i));
            System.out.println("AAAA");
            HuffNode tree = h.HT_from_Heap();
            System.out.println("AAAA");
            HashTable hash = new HashTable();
            hash.set(tree, (byte) 0, (short) 0);
            String to_print = tree.toString();
            System.out.println("AAAA");
            input.close();
            System.out.println("AAAA");
            input = new Scanner(f);
            byte not_shifted = 7;
            byte Final_byte = 0;

            while (input.hasNext()) {
                System.out.println("Entoru no while");
                byte b = (byte) input.next().charAt(0);
                HashElement element = hash.getElement(b);
                short new_value = element.byte_n_value;
                for (byte new_size = element.byte_n_size; new_size >= 0; new_size--) {
                    if (is_bit_set(new_value, new_size))
                        Final_byte = set_bit(Final_byte, not_shifted);
                    if (not_shifted == 0) {
                        to_print += (char) (Final_byte & 0xff);
                        not_shifted = 7;
                        Final_byte = 0;
                    } else
                        not_shifted--;

                }
            }
            int trash = (not_shifted + 1) % 8;
            if (trash != 0)
                to_print += (char) (Final_byte & 0xff);
            JOptionPane.showMessageDialog(parent, "Compressão Finalizada", "Compressão",
                    JOptionPane.INFORMATION_MESSAGE);
            tsize = 0;
            tree.getSize();
            byte[] header = create_header(trash, tsize);
            out.print((char) (header[0] & 0xff));
            out.print((char) (header[1] & 0xff));
            out.print(to_print);
            System.out.println(((char) (header[0] & 0xff)) + ((char) (header[1] & 0xff)) + to_print);
            input.close();
            out.close();
        } catch (Exception e) {
            System.err.println(e.getMessage());
            // System.exit(0);
        }
    }

    public static void printbyte(byte b, String s) {
        System.out.println(s);
        for (int i = 7; i > -1; i--) {
            if (is_bit_set((short) b, i))
                System.out.print('1');
            else
                System.out.print('0');
        }
        System.out.println();
    }

    public static void Decompress(File f, Teste.Myframe parent) {
        try {
            String path = f.getPath();
            File new_file = new File(path.substring(0, path.length() - 5));
            new_file.createNewFile();
            Scanner input = new Scanner(f);
            if (!input.hasNext()) {
                input.close();
                out.close();
                throw new CouldNotReadFileException(f);
            }
            PrintStream out = new PrintStream(new_file);
            input.useDelimiter("");
            long filesize = f.length();
            char c = input.next().charAt(0);
            byte firstbyte = (byte) ((byte) c & 0xff);
            c = input.next().charAt(0);
            byte secondbyte = (byte) ((byte) c & 0xff);
            byte trashsize = (byte) (0b00000111 & (firstbyte >> 5));
            int tree_size = (int) ((0b11111000 & (0b11111000 & (firstbyte << 11) >> 3)) | secondbyte);
            System.out.println(trashsize + " " + tree_size);
            HuffNode tree = HT_from_File(input, (byte) input.next().charAt(0));
            HuffNode tmp_tree = tree;
            String to_out = "";
            short actual;
            int i = 0;
            while (i < filesize - 2 - tree_size) {
                System.out.println(i + " " + (filesize - 3 - tree_size));
                byte b = (byte) input.next().charAt(0);
                actual = 7;
                if (i < filesize - 3 - tree_size)
                    while (actual > -1) {
                        if (tmp_tree.isLeaf()) {
                            to_out += (char) (tmp_tree.b & 0xff);
                            tmp_tree = tree;
                        }
                        if (is_bit_set((short) b, actual))
                            tmp_tree = tmp_tree.right;
                        else
                            tmp_tree = tmp_tree.left;
                        actual--;
                    }
                else {
                    System.out.println("AQUI");
                    while (actual >= trashsize - 1) {
                        System.out.println("WHILE");
                        if (tmp_tree.isLeaf()) {
                            System.err.println("if1");
                            to_out += (char) (tmp_tree.b & 0xff);
                            tmp_tree = tree;
                            out.print(to_out);
                            JOptionPane.showMessageDialog(parent, "Descompressão Finalizada", "Descompressão",
                                    JOptionPane.INFORMATION_MESSAGE);
                            System.out.println(to_out);
                            input.close();
                            out.close();
                        }
                        if (is_bit_set((short) b, actual))
                            tmp_tree = tmp_tree.right;
                        else
                            tmp_tree = tmp_tree.left;
                        actual--;
                    }
                }
                i++;
            }

        } catch (Exception e) {
            System.err.println(e.getMessage());
            // System.exit(0);
        }
    }

    static HuffNode HT_from_File(Scanner input, byte b) {
        HuffNode node = new HuffNode(b);
        if (b == '\\')
            node.b = (byte) input.next().charAt(0);
        if (b == '*') {
            node.left = HT_from_File(input, (byte) input.next().charAt(0));
            node.right = HT_from_File(input, (byte) input.next().charAt(0));
        }
        return node;
    }

    public static class HuffNode {
        BigInteger freq;
        byte b;
        HuffNode left, right;

        /**
         * Construtor sem freq,left e right
         * 
         * @param c
         */
        public HuffNode(byte b) {
            this(0, b);
        }

        public HuffNode(int freq, byte b) {
            this(new BigInteger(Integer.toString(freq)), b);
        }

        public boolean isLeaf() {
            return left == null && right == null;
        }

        /**
         * Construtor sem left e right
         * 
         * @param freq
         * @param c
         */
        public HuffNode(BigInteger freq, byte b) {
            this(freq, b, null, null);
        }

        public boolean is_mul_or_antibar() {
            return isLeaf() && (b == '*' || b == '\\');
        }

        public void getSize() {
            tsize++;
            if (this.is_mul_or_antibar())
                tsize++;
            if (this.left != null)
                this.left.getSize();
            if (this.right != null)
                this.right.getSize();
        }

        /**
         * Construtor
         * 
         * @param freq
         * @param c
         * @param left
         * @param right
         */
        public HuffNode(BigInteger freq, byte b, HuffNode left, HuffNode right) {
            this.freq = freq;
            this.b = b;
            this.left = left;
            this.right = right;
        }

        @Override
        public String toString() {
            String s = "";
            if (isLeaf() && (b == '*' || b == '\\'))
                s += '\\';
            s += (char) (this.b & 0xff);
            if (this.left != null)
                s += this.left.toString();
            if (this.right != null)
                s += this.right.toString();
            return s;
        }
    }

    public static class HashTable {
        HashElement[] table;

        public HashTable() {
            this.table = new HashElement[256];
        }

        void setElement(byte key, HashElement element) {
            int k = (int) key;
            this.table[k] = element;
        }

        /**
         * Coloca o novo valor do byte key na table na posição key
         */
        void setElement(byte key, short nv, byte ns) {
            setElement(key, new HashElement(nv, ns));
        }

        /**
         * 
         * @param key
         * @return Elemento na posição key na table
         */
        HashElement getElement(byte key) {
            int k = (int) key;
            return this.table[k];
        }

        void set(HuffNode ht, byte height, short b) {
            if (ht != null) {
                if (ht.isLeaf())
                    this.setElement(ht.b, b, (byte) (height - 1));
                set(ht.left, (byte) (height + 1), (short) (b << 1));
                set(ht.right, (byte) (height + 1), (short) ((b << 1) + 1));
            }
        }
    }

    public static class HashElement {
        short byte_n_value;
        byte byte_n_size;

        public HashElement(short nv, byte ns) {
            this.byte_n_size = ns;
            this.byte_n_value = nv;
        }
    }

    public static class Heap {
        HuffNode[] heap;
        int size;

        public Heap() {
            heap = new HuffNode[257];
            heap[0] = new HuffNode(0, (byte) 0);
            size = 0;
        }

        int parent_index(int i) {
            return i / 2;
        }

        int left_child(int i) {
            return i * 2;
        }

        int right_child(int i) {
            return i * 2 + 1;
        }

        void swap(int i, int j) {
            HuffNode aux = heap[i];
            heap[i] = heap[j];
            heap[j] = aux;
        }

        /**
         * 
         * @param node
         */
        void enqueue(HuffNode node) {
            if (size <= 257) {
                heap[++size] = node;
                int key = size;
                int parent = parent_index(key);
                while (parent >= 1 && heap[key].freq.compareTo(heap[parent].freq) == -1) {
                    swap(key, parent);
                    key = parent;
                    parent = parent_index(key);
                }
            }
        }

        /**
         * Min Heapify
         * 
         * @param i
         */
        void heapify(int i) {
            int low = i, l = left_child(i), r = right_child(i);
            if (l <= size && heap[l].freq.compareTo(heap[i].freq) == -1)
                low = l;
            if (r <= size && heap[r].freq.compareTo(heap[low].freq) == -1)
                low = r;
            if (heap[i].b != heap[low].b) {
                swap(i, low);
                heapify(low);
            }
        }

        boolean isEmpty() {
            return size == 0;
        }

        /**
         * 
         * @return
         */
        HuffNode dequeue() {
            HuffNode item = null;
            if (!isEmpty()) {
                item = heap[1];
                heap[1] = heap[size];
                size--;
                heapify(1);
            }
            return item;
        }

        HuffNode HT_from_Heap() {
            HuffNode a, b, aux;
            System.out.println("HTFROMHEAP");
            System.out.println(size);
            while (size > 1) {
                System.out.println("HTFROMHEAPWHILE");
                a = dequeue();
                b = dequeue();
                aux = new HuffNode(a.freq.add(b.freq), (byte) '*', a, b);
                enqueue(aux);
            }
            return dequeue();
        }
    }
}