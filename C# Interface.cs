using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
namespace yöntusları

{
    public partial class Form1 : Form
    {
        public delegate void uptadeui();
        public delegate void uptadeui2(string text);
        bool state2 = false;
        bool state = false;
        bool state3 = false;
        bool state4 = false;
        string[] portlar = SerialPort.GetPortNames();
        public delegate void testDelegate();
        public Form1()
        {

            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            button2.BackColor = Color.Red;

            groupBox2.Enabled = false;
            button5.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            foreach (string ports in portlar)
            {
                comboBox2.Items.Add(ports);
                comboBox2.SelectedIndex = 0;
            }
            comboBox1.Items.Add("4800");
            comboBox1.Items.Add("9600");
            comboBox1.SelectedIndex = 1;
        }
        public void button3_Click(object sender, EventArgs e)
        {
      
            try
            {
                serialPort1.PortName = comboBox2.Text;
                //serialPort1.BaudRate = Convert.ToInt16((comboBox2.Text));
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("PortNotFound", "Hata",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (serialPort1.IsOpen == true)
            {
                groupBox2.Enabled = true;

                button3.BackColor = Color.DarkGray;
                button4.BackColor = Color.White;
                button3.Text = "Connected";
                button4.Text = "Disconnect";
            }      
        }
            
        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
            }
            if (serialPort1.IsOpen == false)
            {

                button4.Text = "Disconnected";
                button4.BackColor = Color.Red;
                button3.BackColor = Color.White;

                button3.Text = "Connect";


            }
        }
   
        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
       
            button5.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
        }

        public void uilabel(string text)
        {
            label5.Text = text + "";
          
           
        }
        public void uilabel2(string text)
        {
            label6.Text = text + "";


        }
        public void uilabel3(string text)
        {
            label7.Text = text + "";


        }
        public void uilabelz(string text)
        {
            label11.Text = text + "";
        }
        public void uilabelx(string text)
        {
            label9.Text = text + "";
        }
        public void uilabely(string text)
        {
            label10.Text = text + "";
        }
        public void button5_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(SendCode);
            t.Start();
        }
   
        public void button7_Click(object sender, EventArgs e)
        {

            state = !state;
        }

        public void reset()
        {
            button5.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
        }
        public void buton8ui()
        {
            button5.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button1.Enabled = true;
            textBox1.Text = "";
         

        }
        public void butonreset()
        {
            label5.Text = "0";
            label6.Text = "0";
            label7.Text = "0";
            label9.Text = "0";
            label10.Text = "0";
            label11.Text = "0";
        }
    public void buton7r()
        {
            button7.Text = "Resume";
            button5.Enabled = false;
            button1.Enabled = true;
        }
        public void buton7p()
        {
            button7.Text = "Pause";
            button5.Enabled = false;
            button1.Enabled = false;
        }
        public void button8_Click(object sender, EventArgs e) {
            state2 = !state2;
  
        }
  
        public void SendCode()
        {
            try
            {
            
                List<Class1> ercans = new List<Class1>();
                string filepath = textBox1.Text;
                List<string> text = File.ReadAllLines(filepath).ToList();
                state2 = false;
                state = true;
                
                button5.Enabled = true;
                foreach (var line in text)
                {
                   
                    string[] entries = line.Split(' ', 'Y');
                    Class1 eksen = new Class1();
                    state4 = false;
                    button2.BackColor = Color.Green;
                    if (entries.Length == 1)
                    {
                      
                        while (!state)
                        {

                            button7.Invoke(new uptadeui(buton7r));
                            if (state4)
                            {
                                button1.Invoke(new uptadeui(butonreset)); //reset
                            }
                            if (state2)
                            {   
                                break;
                            }
                        }
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        button7.Invoke(new uptadeui(buton7p));
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        eksen.a = entries[0];
                        serialPort1.Write(eksen.a);
                        while (true)
                        {
                            if ((serialPort1.ReadLine())!= "D")                             
                                break;
                        }
                    }
                    if (entries.Length == 2)
                    {
                        while (!state)
                        {
                                
                            button7.Invoke(new uptadeui(buton7r));  //pause
                            if (state4)
                            {
                                button1.Invoke(new uptadeui(butonreset));
                            }
                            if (state2)
                            {
                                break;
                            }
                        }
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui)); //cancel
                            break;
                        }
                        button7.Invoke(new uptadeui(buton7p)); //resume
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui)); // cancel
                   
                            break;
                        }
                        eksen.a = entries[0];

                        serialPort1.Write(eksen.a);
                       
                        while (true)
                        {
                            if ((serialPort1.ReadLine()) != "D")
                                break;

                        }
                        while (!state)
                        {

                            button7.Invoke(new uptadeui(buton7r));
                            if (state4)
                            {
                                button1.Invoke(new uptadeui(butonreset));
                            }
                            if (state2)
                            {
                                break;
                            }
                        }
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        button7.Invoke(new uptadeui(buton7p));
                        if (state2)
                        {

                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }


                        eksen.b = entries[1];
                        string z = entries[1].Substring(1);
                            label7.Invoke(new uptadeui2(uilabel3), z);
                        serialPort1.Write(eksen.b);
                        
                        while (true)
                        {
                            if ((serialPort1.ReadLine()) != "D")
                            break;
                        }
                        label11.Invoke(new uptadeui2(uilabelz), z);
                    }


                    if (entries.Length == 3)
                    {
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }

                        while (!state)
                        {

                            button7.Invoke(new uptadeui(buton7r));
                            if (state4)
                            {
                                button1.Invoke(new uptadeui(butonreset));
                            }
                            if (state2)
                            {
                                break;
                            }
                        }
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        button7.Invoke(new uptadeui(buton7p));
                        eksen.a = entries[0];

                        serialPort1.Write(eksen.a);
                        while (true)
                        {
                            if ((serialPort1.ReadLine()) != "D")
                                break;

                        }
                        while (!state)
                        {

                            button7.Invoke(new uptadeui(buton7r));
                            if (state4)
                            {
                                button1.Invoke(new uptadeui(butonreset)); //reset
                            }
                            if (state2)
                            {
                                break;
                            }
                        }
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        button7.Invoke(new uptadeui(buton7p));
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }

                        eksen.b = entries[1];

                        string x = entries[1].Substring(1);
                        label5.Invoke(new uptadeui2(uilabel), x);
                        serialPort1.Write(eksen.b);
                        while (true)
                        {
                            if ((serialPort1.ReadLine()) != "D")
                                break;
                        }
                        label9.Invoke(new uptadeui2(uilabelx), x);
                        while (!state)
                        {

                            button7.Invoke(new uptadeui(buton7r));
                            if (state4)
                            {
                                button1.Invoke(new uptadeui(butonreset)); //reset
                            }
                            if (state2)
                            {
                                break;
                            }
                        }
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        button7.Invoke(new uptadeui(buton7p));
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        eksen.c = entries[2];
                        serialPort1.Write(eksen.c);
                        string y = entries[2];
                        label6.Invoke(new uptadeui2(uilabel2), y);
                        while (true)
                        { 
                                if ((serialPort1.ReadLine()) != "D")
                                    break;
                        }
                        label10.Invoke(new uptadeui2(uilabely), y);
                        while (!state)
                        {
                            button7.Invoke(new uptadeui(buton7r));
                            if (state4)
                            {
                                button1.Invoke(new uptadeui(butonreset)); //reset
                            }
                            if (state2)
                            {

                                break;
                            }
                        }
                        if (state2)
                        {
                            button8.Invoke(new uptadeui(buton8ui));
                            break;
                        }
                        button7.Invoke(new uptadeui(buton7p));
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void button1_Click_1(object sender, EventArgs e)
        {
            state4 = !state4;
            serialPort1.Write("R");
            label5.Text = "0";
            label6.Text = "0";
            label7.Text = "0";
            label9.Text = "0";
            label10.Text = "0";
            label11.Text = "0";

        }
    }
    }


    


