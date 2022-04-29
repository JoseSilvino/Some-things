
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.*;
import java.awt.image.*;
import javax.imageio.ImageIO;
import javax.swing.*;
import java.io.File;
import java.io.IOException;

import Teste2;
import javafx.scene.layout.BackgroundImage;
import javafx.scene.layout.BackgroundPosition;
import javafx.scene.layout.BackgroundRepeat;
import javafx.scene.layout.BackgroundSize;

public class Teste {
    public static class Myframe extends JFrame {
        public Myframe() {
            super("Huffinho");
            init_components();
        }

        private JButton compress, decompress, exit;
        private BufferedImage ty;

        private void init_components() {
            setDefaultCloseOperation(EXIT_ON_CLOSE);
            setPreferredSize(new Dimension(400, 300));
            init();
            pack();
        }

        private JFileChooser chooser;

        private void init() {
            try {
                ty = ImageIO.read(new File("D:\\Users\\jssni\\Desktop\\TesteHuff\\typhlosion.png"));
                setIconImage(ty);
            } catch (IOException e) {
                System.err.println(e.getMessage());
            }
            chooser = new JFileChooser();
            setSize(400, 300);
            JButton compress, decompress, exit;
            compress = new JButton("Compress");
            decompress = new JButton("Decompress");
            exit = new JButton("Exit");
            setContentPane(new JPanel() {
                @Override
                public void paint(Graphics g) {
                    super.paint(g);
                    try {
                        BufferedImage bg = new BufferedImage(getWidth(), getHeight(), BufferedImage.TYPE_INT_RGB);
                        Graphics2D g2 = bg.createGraphics();
                        g2.drawImage(ty, 0, 0, getWidth(), getHeight(), null);
                        g.drawImage(bg, 0, 0, getWidth(), getHeight(), null);
                        int w = this.getWidth();
                        int h = this.getHeight();
                        compress.setLocation(w / 2 - compress.getWidth() / 3 - decompress.getWidth(),
                                h / 2 - compress.getHeight() / 2);
                        decompress.setLocation(w / 2 + decompress.getWidth() / 3, h / 2 - decompress.getHeight() / 2);
                        exit.setLocation(w - (int) (exit.getWidth() + 1), h - (int) (exit.getHeight() + 1));
                        compress.repaint();
                        decompress.repaint();
                        exit.repaint();
                    } catch (Exception e) {
                        System.err.println(e.getMessage());
                    }
                }
            });
            setLayout(null);
            int w = this.getWidth();
            int h = this.getHeight();
            compress.setSize(110, 23);
            decompress.setSize(110, 23);
            exit.setSize(50, 23);
            compress.setLocation(w / 2 - compress.getWidth() / 2 - decompress.getWidth(), h / 2 - compress.getHeight());
            decompress.setLocation(w / 2 + decompress.getWidth() / 2, h / 2 - decompress.getHeight());
            exit.setLocation(w - (int) (exit.getWidth() * 1.325), h - (int) (exit.getHeight() * 2.725));
            compress.addActionListener(this::CompressPressed);
            decompress.addActionListener(this::DecompressPressed);
            exit.addActionListener(this::exit);
            add(compress);
            add(decompress);
            add(exit);
        }

        void CompressPressed(ActionEvent e) {
            int dialog = chooser.showOpenDialog(this);
            if (dialog == JFileChooser.OPEN_DIALOG) {
                Teste2.Compress(chooser.getSelectedFile(), this);
            }
        }

        void DecompressPressed(ActionEvent e) {
            int dialog = chooser.showOpenDialog(this);
            if (dialog == JFileChooser.OPEN_DIALOG) {
                Teste2.Decompress(chooser.getSelectedFile(), this);
            }
        }

        void exit(ActionEvent e) {
            System.exit(0);
        }
    }

    public static void main(String[] args) {
        try {
            String OS = System.getProperty("os.name").toLowerCase();
            if (OS.contains("win"))
                UIManager.setLookAndFeel("com.sun.java.swing.plaf.windows.WindowsLookAndFeel");
            else
                UIManager.setLookAndFeel("javax.swing.plaf.nimbus.NimbusLookAndFeel");
        } catch (UnsupportedLookAndFeelException | ClassNotFoundException | InstantiationException
                | IllegalAccessException ex) {
            System.err.println(ex);
        }
        EventQueue.invokeLater(new Runnable() {
            @Override
            public void run() {
                new Myframe().setVisible(true);
            }
        });
    }
}