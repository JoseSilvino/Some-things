public final class Teste2{
    public static class HuffNode{
        int freq;
        char c;
        HuffNode left,right;
        /**
         * Construtor sem freq,left e right
         * @param c
         */
        public HuffNode(char c){
            this(0, c);
        }
        /**
         * Construtor sem left e right
         * @param freq
         * @param c
         */
        public HuffNode(int freq,char c){
            this(freq,c,null,null);
        }
        /**
         * Construtor
         * @param freq
         * @param c
         * @param left
         * @param right
         */
        public HuffNode(int freq,char c,HuffNode left,HuffNode right){
            this.freq = freq;
            this.c = c;
            this.left = left;
            this.right = right;
        }
        @Override
        public String toString() {
            String s = "[";
            if (this.left!=null) s += this.left.toString();
            s += "{'"+this.c+"', "+this.freq+"}";
            if (this.right!=null) s += this.right.toString();
            return s + ']';
        }
    }
    public static class HashTable{
        HashElement []table;
        public HashTable(){
            this.table = new HashElement[256];
        }
        void setElement(char key,HashElement element){
            int k = (int)key;
            this.table[k] = element;
        }
        /**
        * Coloca o novo valor do byte key na table na posição key
        */
        void setElement(char key,short nv,short ns){
            setElement(key, new HashElement(nv, ns));
        }
        /**
         * 
         * @param key
         * @return Elemento na posição key na table
         */
        HashElement getElement(char key){
            int k = (int) key;
            return this.table[k];
        }
    }
    public static class HashElement{
        short byte_n_value;
        short byte_n_size;
        public HashElement(short nv,short ns){
            this.byte_n_size = ns;
            this.byte_n_value = nv;
        }
    }
    public static class Heap{
        HuffNode[] heap;
        public Heap(){
            heap = new HuffNode[257];
        }
        /**
         * 
         * @param node
         */
        void enqueue(HuffNode node){

        }
        /**
         * 
         * @return
        */
        HuffNode dequeue(){
            return null;
        }
    }
}