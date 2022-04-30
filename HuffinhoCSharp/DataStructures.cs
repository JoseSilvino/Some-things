namespace HuffinhoCSharp{
    namespace Structures{
        ///<summary>
        /// Implementação de Hash Table no Huffman
        ///</summary>
        public class HashTable {
            private readonly Element[] table;
            /// <summary>
            /// Construtor da classe
            /// </summary>

            public HashTable(){
                table = new Element[256];
            }
            /// <summary>
            /// Getter e Setter da Hash Table
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public Element this[int i] { get { return table[i]; }set { table[i] = value; } }
        }
        /// <summary>
        /// Implementação da árvore binária de Huffman
        /// </summary>
        public class HuffTree{
            private byte b;
            private System.UInt64 frequency;
            private HuffTree left,right;
            /// <summary>
            /// Construtor da classe
            /// </summary>
            /// <param name="b"></param>
            /// <param name="frequency"></param>
            /// <param name="left"></param>
            /// <param name="right"></param>
            public HuffTree(byte b,UInt64 frequency,HuffTree left,HuffTree right){
                this.b = b;
                this.frequency = frequency;
                this.left = left;
                this.right = right;
            }
            /// <summary>
            /// Construtor da classe sem os nós filhos
            /// </summary>
            /// <param name="b"></param>
            /// <param name="frequency"></param>
            public HuffTree(byte b, UInt64 frequency){
                this.b = b;
                this.frequency=frequency;
                this.left = null;
                this.right = null;
            }
            /// <summary>
            /// Construtor da classe somente com o byte
            /// </summary>
            /// <param name="b"></param>
            public HuffTree(byte b){
                this.b = b;
                this.frequency = 0;
                this.left=null;
                this.right=null;
            }
            /// <summary>
            /// Getter e Setter para o byte
            /// </summary>
            public byte Byte { get { return b; } set { b = value; } }
            /// <summary>
            /// Getter e Setter para a frequência
            /// </summary>
            public UInt64 Frequency { get { return frequency; } set { frequency = value; } }
            /// <summary>
            /// Getter e Setter para o nó filho da esquerda
            /// </summary>
            public HuffTree Left { get { return left; } set { left = value; } }
            /// <summary>
            /// Getter e Setter para o nó filho da direita
            /// </summary>
            public HuffTree Right { get { return right; } set { right = value; } }
            /// <summary>
            /// Função que retorna se o nó é uma folha
            /// </summary>
            /// <returns>Retorna true se é uma folha ou false se não</returns>
            public bool isLeaf() { return left == null && right == null; }
            /// <summary>
            /// Função que consegue o tamanho da árvore
            /// </summary>
            /// <param name="tree"></param>
            /// <param name="size"></param>
            public static void tsize(HuffTree tree,ref int size){
                if (tree != null){
                    size++;
                    if(tree.leaf_abar_mul()) size++;
                    tsize(tree.left,ref size);
                    tsize(tree.right,ref size);
                }
            }
            /// <summary>
            /// Função que verifica se o nó é uma folha '*' ou '\\'
            /// </summary>
            /// <returns>retorna de o nó simboliza '*' ou '\\' no arquivo original</returns>
            public bool leaf_abar_mul(){
                return isLeaf() && (b=='*'||b=='\\');
            }
            public override string ToString(){
                string s = "";
                if (leaf_abar_mul()) s += '\\';
                s += (char)Byte;
                if(left != null) s += left.ToString();
                if (right != null) s += right.ToString();
                return s;
            }
            /// <summary>
            /// Função que coloca os novos valores dos bytes na Hash Table
            /// </summary>
            /// <param name="ht"></param>
            /// <param name="h"></param>
            /// <param name="bytev"></param>
            public void newByteValues(HashTable ht, byte h, ushort bytev) {
                if (isLeaf()) {
                    ht[b] = new Element(bytev, (byte)(h - 1)); 
                }
                if (left != null) left.newByteValues(ht, (byte)(h + 1), (ushort)(bytev << 1));
                if (right != null) right.newByteValues(ht, (byte)(h + 1), (ushort)((bytev << 1)+1));
            }
            /// <summary>
            /// Operador '<'
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool operator <(HuffTree a,HuffTree b) { return a.frequency < b.frequency; }
            /// <summary>
            /// Operador '>'
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool operator >( HuffTree a,HuffTree b) { return a.frequency > b.frequency; }
        }
        /// <summary>
        /// Implementação do elemento que fica guardado na Hash table
        /// </summary>
        public class Element {
            ushort huff_value;
            private byte size;
            public Element(ushort huff_value, byte size){
                this.huff_value = huff_value;
                this.size = size;
            }
            /// <summary>
            /// Getter e Setter do novo valor do byte no arquivo comprimido
            /// </summary>
            public ushort HuffValue { get { return huff_value; } set { huff_value = value; } }
            /// <summary>
            /// Tamanho ocupado pelo novo valor do byte dentro do short
            /// </summary>
            public byte Size { get { return size; } set { size = value; } }
            public override string ToString() { return "|" + huff_value.ToString() + " " + size.ToString() + "|"; }
        }
        /// <summary>
        /// Implementação da fila de prioridade no algoritmo de Huffman
        /// </summary>
        public class Heap{
            private HuffTree[] heap;
            private int heap_size;
            private readonly int max_size = 256;
            /// <summary>
            /// Construtor da classe
            /// </summary>
            public Heap(){
                heap = new HuffTree[257];
            }
            /// <summary>
            /// Getter e Setter da heap
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public HuffTree this[int index] { get { return heap[index]; } set { heap[index] = value; } }
            /// <summary>
            /// Getter e Setter do tamanho atual da heap
            /// </summary>
            public int Size { get { return heap_size; } }
            private int parent(int i) { return i / 2; }
            private int left(int i) { return i * 2; }
            private int right(int i) { return i * 2 + 1; }
            private void swap(int a,int b){
                var aux = heap[a];
                heap[a] = heap[b];
                heap[b] = aux;
            }
            /// <summary>
            /// Implementação do min heapify
            /// </summary>
            /// <param name="i"></param>
            public void heapify(int i){
                int low = i, left_c = left(i), right_c = right(i);
                if (left_c <= heap_size && heap[left_c] < heap[i]) low = left_c;
                if (right_c <= heap_size && heap[right_c] < heap[low]) low = right_c;
                if (i != low)
                {
                    swap(i, low);
                    heapify(low);
                }
            }
            /// <summary>
            /// Dequeue da fila de prioridade
            /// </summary>
            /// <returns></returns>
            public HuffTree dequeue(){
                if (heap_size != 0) {
                    HuffTree deq = heap[1];
                    heap[1] = heap[heap_size--];
                    heapify(1);
                    return deq;
                }
                return null;
            }
            /// <summary>
            /// Enqueue da fila de prioridade
            /// </summary>
            /// <param name="t"></param>
            public void enqueue(HuffTree t)
            {
                if (heap_size < max_size) {
                    heap[++heap_size] = t;
                    int key = heap_size;
                    int kparent = parent(key);
                    while (kparent >= 1 && (heap[key] < heap[kparent])){
                        swap(key, kparent);
                        key = kparent;
                        kparent = parent(key);
                    }
                }
            }
            /// <summary>
            /// Cria uma árvore de Huffman a partir da fila de prioridade
            /// </summary>
            /// <returns></returns>
            public HuffTree HTFromHeap(){
                HuffTree a, b, aux;
                while (Size > 1){
                    a = dequeue();
                    b = dequeue();
                    aux = new HuffTree((byte)'*', a.Frequency + b.Frequency, a, b);
                    enqueue(aux);
                }
                return dequeue();
            }
            public override string ToString(){
                string s = "[";
                for (int i = 1; i < heap_size; i++){
                    s += "{ " + i + ' ' + heap[i].Byte + " , " + heap[i].Frequency + "}";
                    if (i < heap_size - 1) s += ',';
                }
                return s+']';
            }
        }
    }
}