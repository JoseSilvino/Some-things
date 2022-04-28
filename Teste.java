
import java.awt.EventQueue;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.*;

import javax.swing.*;

import Teste2;
public class Teste{
    public static class Myframe extends JFrame{
        public Myframe(){
            super("Teste");
            init_components();
        }
        private void init_components(){
            setDefaultCloseOperation(EXIT_ON_CLOSE);
            setPreferredSize(new Dimension(400,300));
            setContentPane(new Mypane());
            pack();
        }
    }
    public static class Mypane extends JDesktopPane{
        Mypane(){
            init();
        }
        private void init(){
            setSize(400, 300);
            JButton compress,decompress,exit;
            compress = new JButton("Compress");
            decompress = new JButton("Decompress");
            exit = new JButton("Exit");
            int w = this.getWidth();
            int h = this.getHeight();
            compress.setSize(110,23);
            decompress.setSize(110, 23);
            exit.setSize(50,30);
            compress.setLocation(w/2-compress.getWidth()/2-decompress.getWidth(),h/2-compress.getHeight());
            decompress.setLocation(w/2+decompress.getWidth()/2,h/2-decompress.getHeight());
            exit.setLocation(w-(int)(exit.getWidth()*1.2), h-exit.getHeight()*2);
            compress.addActionListener(this::CompressPressed);
            decompress.addActionListener(this::DecompressPressed);
            exit.addActionListener(this::exit);
            add(compress);
            add(decompress);
            add(exit);
        }
        void CompressPressed(ActionEvent e){
            JOptionPane.showInternalMessageDialog(this, "ComB", "title", JOptionPane.INFORMATION_MESSAGE);
        }
        void DecompressPressed(ActionEvent e){
            Teste2.HuffNode hn = new Teste2.HuffNode('*');
            JOptionPane.showInternalMessageDialog(this, hn.toString(), "title", JOptionPane.INFORMATION_MESSAGE);
        }
        void exit(ActionEvent e){
            System.exit(0);
        }
    }
    public static void main(String[] args) {
        try {
            String OS = System.getProperty("os.name").toLowerCase();
            if(OS.contains("win"))
            UIManager.setLookAndFeel("com.sun.java.swing.plaf.windows.WindowsLookAndFeel");
            else
            UIManager.setLookAndFeel("javax.swing.plaf.nimbus.NimbusLookAndFeel");
       } catch (UnsupportedLookAndFeelException | ClassNotFoundException | InstantiationException | IllegalAccessException ex) {
        System.err.println(ex);
        }
        EventQueue.invokeLater(new Runnable(){
            @Override
            public void run() {
                new Myframe().setVisible(true);
            }
        });
    }
}