using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PointerSearcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static public List<int> File1 = new List<int>();
        static public List<int> File2 = new List<int>();
        static public List<int> File3 = new List<int>();
        static public bool file3 = false;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogue = new OpenFileDialog();
            dialogue.Title = "Open File";
            if (dialogue.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(dialogue.FileName, FileMode.Open);
                int input;
                for (int i = 0; (input = fs.ReadByte()) != -1; i++)
                {
                    File1.Add(input); // add endian support later
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogue = new OpenFileDialog();
            dialogue.Title = "Open File";
            if (dialogue.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(dialogue.FileName, FileMode.Open);
                int input;
                for (int i = 0; (input = fs.ReadByte()) != -1; i++)
                {
                    File2.Add(input); // add endian support later
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogue = new OpenFileDialog();
            dialogue.Title = "Open File";
            if (dialogue.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(dialogue.FileName, FileMode.Open);
                int input;
                for (int i = 0; (input = fs.ReadByte()) != -1; i++)
                {
                    File3.Add(input); // add endian support later
                    file3 = true;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            -addressX will always contain the string of the address that it will be attempting to read from in the file
            -offsetX will increase the address, readX is affected by this
            -TargetAddressX is the string of the address that it will try to find a pointer to. will be different for both files
            -readX will always contain the integer representation of the address that it will be attempting to read from in the file
            -File[X] is an integer list where X in the list is byte X in the file
            -pointerOffset is the integer of the pointer offset that it will be attemtping to use
             */

            textBox4.Text = "";

            String address1 = "";
            String address2 = "";
            String address3 = "";

            String TargetAddress1 = textBox1.Text;
            String TargetAddress2 = textBox2.Text;
            String TargetAddress3 = textBox5.Text;

            int targetAddress1 = Convert.ToInt32(TargetAddress1, 16);
            int targetAddress2 = Convert.ToInt32(TargetAddress2, 16);
            int targetAddress3 = Convert.ToInt32(TargetAddress3, 16);

            uint read1 = 0;
            uint read2 = 0;
            uint read3 = 0;

            for (int offset1 = 0; offset1 < File1.Count; offset1 += 4)
            {
                address1 = File1[offset1].ToString("X").PadLeft(2, '0') + File1[offset1 + 1].ToString("X").PadLeft(2, '0') + File1[offset1 + 2].ToString("X").PadLeft(2, '0') + File1[offset1 + 3].ToString("X").PadLeft(2, '0');
                read1 = Convert.ToUInt32(address1, 16);
                for (int offset2 = 0; offset2 < File2.Count; offset2 += 4)
                {
                    address2 = File2[offset2].ToString("X").PadLeft(2, '0') + File2[offset2 + 1].ToString("X").PadLeft(2, '0') + File2[offset2 + 2].ToString("X").PadLeft(2, '0') + File2[offset2 + 3].ToString("X").PadLeft(2, '0');
                    read2 = Convert.ToUInt32(address2, 16);
                    if (file3)
                    {
                        for (int offset3 = 0; offset3 < File3.Count; offset3 += 4)
                        {
                            address3 = File3[offset3].ToString("X").PadLeft(2, '0') + File3[offset3 + 1].ToString("X").PadLeft(2, '0') + File3[offset3 + 2].ToString("X").PadLeft(2, '0') + File3[offset3 + 3].ToString("X").PadLeft(2, '0');
                            read3 = Convert.ToUInt32(address3, 16);
                            for (int pointerOffset = 0; pointerOffset < Convert.ToInt32(textBox3.Text, 16); pointerOffset += 4)
                            {
                                if (read1 < File1.Count && read2 < File2.Count && read3 < File3.Count)
                                {
                                    if (offset1 == offset3 && offset2 == offset1 && read1 + pointerOffset == targetAddress1 && read2 + pointerOffset == targetAddress2 && read3 + pointerOffset == targetAddress3)
                                    {
                                        textBox4.Text += "[0x" + offset1.ToString("X") + "] + 0x" + pointerOffset.ToString("X") + System.Environment.NewLine;
                                        continue;
                                    }
                                    if (checkBox1.Checked && offset1 == offset3 && offset2 == offset1 && read1 - pointerOffset == targetAddress1 && read2 - pointerOffset == targetAddress2 && read3 - pointerOffset == targetAddress3)
                                    {
                                        textBox4.Text += "[0x" + offset1.ToString("X") + "] - 0x" + pointerOffset.ToString("X") + System.Environment.NewLine;
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    else if (!file3 && read1 < File1.Count && read2 < File2.Count)
                    {
                        for (int pointerOffset = 0; pointerOffset < Convert.ToInt32(textBox3.Text, 16); pointerOffset += 4)
                        {
                            if (offset2 == offset1 && read1 + pointerOffset == targetAddress1 && read2 + pointerOffset == targetAddress2)
                            {
                                textBox4.Text += "[0x" + offset1.ToString("X") + "] + 0x" + pointerOffset.ToString("X") + System.Environment.NewLine;
                                continue;
                            }
                            if (offset2 == offset1 && checkBox1.Checked && read1 - pointerOffset == targetAddress1 && read2 - pointerOffset == targetAddress2)
                            {
                                textBox4.Text += "[0x" + offset1.ToString("X") + "] - 0x" + pointerOffset.ToString("X") + System.Environment.NewLine;
                                continue;
                            }
                        }
                    }
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a pointer searcher application written in C# by DarkFlare. It is not meant to be fast, but it should be accurate.", "About");
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program by DarkFlare.", "Credits");
        }

        private void storeSettingsToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
