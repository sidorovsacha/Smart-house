using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Web;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ArduinoSP
{
     
    public partial class Form1 : Form
    {
        SerialPort serialPort1 = new SerialPort();
       Thread Server = new Thread(SocketServer.Socket);
      
       public static string[] mas;
      public Form1()
       {
          
            InitializeComponent();
              
            
          
          
        }

      
        string temper1, davl, temper2, temper3, vlaga, dver, rip, signal, svet, sostoyanie = "C", udpData;
        int SvetAvto = 0, Svet = 0, Signal = 0, UDPtimeNarodmon = 60;
        byte SvetOnOff = 0, SignalOnOff = 0, datatimedvr = 0, datatimerip = 0, smsopoveschenie = 0, narodmonOnOFF = 1, narodmonButton = 1, smsButton = 0;
     public static bool StatusServer = false;
        private void button3_Click(object sender, EventArgs e)
        {
            if (SignalOnOff == 0) {
                if (Svet == 1 && SvetAvto == 0) {
                    serialPort1.Write("A");
                    label20.Text = "A";
                    sostoyanie = "A";

                }
                if (Svet == 0 && SvetAvto == 0) { 
                    serialPort1.Write("B");
                    label20.Text = "B";
                    sostoyanie = "B";
                }
                if (Svet == 0 && SvetAvto == 1) {
                    serialPort1.Write("E");
                    label20.Text = "E";
                    sostoyanie = "E";
                }
                SignalOnOff = 1;
                Signal = 1;
                button3.Text = "Сигнализация Выкл";
            }
            else {
                if (Svet == 0 && SvetAvto == 0)
                {
                    serialPort1.Write("C");
                    label20.Text = "C";
                    sostoyanie = "C";

                }
                if (Svet == 1 && SvetAvto == 0)
                {
                    serialPort1.Write("D");
                    label20.Text = "D";
                    sostoyanie = "D";
                }
                if (Svet == 0 && SvetAvto == 1)
                {
                    serialPort1.Write("F");
                    label20.Text = "F";
                    sostoyanie = "F";
                }
                Signal = 0;
                SignalOnOff = 0;
                button3.Text = "Сигнализация Вкл";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           if (Signal == 1){
               serialPort1.Write("E");
               label20.Text = "E";
               sostoyanie = "E";
                          }  
            if (Signal == 0){
            serialPort1.Write("F");
            label20.Text = "F";
            sostoyanie = "F";
                              
            }
            button1.Enabled = false;
            SvetAvto = 1;
            Svet = 0;
            SvetOnOff = 1;
            button2.Text = "Свет Выкл";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (SvetOnOff == 0)
            {
                if (Signal == 1)
                {
                    serialPort1.Write("A");
                    label20.Text = "A";
                    sostoyanie = "A";
                    
                }
                if (Signal == 0)
                {
                    serialPort1.Write("D");
                    label20.Text = "D";
                    sostoyanie = "D";
                 }
                SvetAvto = 0;
                Svet = 1;
                SvetOnOff = 1;
                button1.Enabled = true;
                button2.Text = "Свет Выкл";
            }
            else if (SvetOnOff==1)
            {
                if (Signal == 1)
                {
                    serialPort1.Write("B");
                    label20.Text = "B";
                    sostoyanie = "B";
                    
                }
                if(Signal== 0)
                {
                    serialPort1.Write("C");
                    label20.Text = "C";
                    sostoyanie = "C";
                    
                }
                button1.Enabled = true;
                SvetAvto = 0;
                Svet = 0;
                SvetOnOff = 0;
               button2.Text = "Свет Вкл";
            }

            
        }

        public void timer1_Tick(object sender, EventArgs e) //arduino timer
        {
            label39.Text = SocketServer.data;
          
            string a;
            a = serialPort1.ReadExisting();
         
             

          label19.Text = a;
            if (!string.IsNullOrEmpty(a))
            { 
                mas = a.Split(new char[] { ' ' });

                if (mas.Contains("*") && mas.Contains("#"))
                {
                    if (mas[1] == "*")
                    {
                        label1.Text = mas[2];
                        temper1= mas[2];
                        label2.Text = mas[3];
                        davl=mas[3];
                        label3.Text = mas[4];
                        temper2=mas[4];
                        label4.Text = mas[5];
                        vlaga=mas[5];
                        label5.Text = mas[6];
                        temper3 = mas[6];
                        dver = mas[7];
                        if (mas[7] == "open")
                        {
                            mas[7] = "Открыто";
                            // dver=mas[7];
                            if (datatimedvr == 0)
                            {
                                label22.Text = System.DateTime.Now.ToLongTimeString(); datatimedvr = 1;
                            }
                        }
                        if(mas[7]=="closed")
                        {
                            mas[7] = "Закрыто";
                            
                              }
                        
                        label6.Text = mas[7];
                        rip = mas[8];
                        if (mas[8] == "Motion")
                        {
                            mas[8] = "Движение";
                           // rip=mas[8];
                            if (datatimerip==0)
                            {  label21.Text = System.DateTime.Now.ToLongTimeString(); datatimerip = 1;  }
                        }
                        if(mas[8]=="NoMotion")
                        {
                            mas[8] = "Нет Движения";
                            
                        }
                        label7.Text = mas[8];
                        label8.Text = mas[9];
                        signal=mas[9];
                        label9.Text = mas[10];
                        svet=mas[10];
                        udpData = String.Format(@"#58-2C-80-13-92-63
                            #280123456789ABCD#{0}
                            #245743893472DCAB#{1}
                            #533467362764ABCD#{2}
                            #00000001211140C2#{3}
                            #00000001211140C3#{4}
                            
##", mas[2],mas[3],mas[4],mas[5],mas[6]);
                        if (mas[7] == "Открыто" && smsopoveschenie == 0 && smsButton==1 || mas[8] == "Движение" && smsopoveschenie == 0 && smsButton ==1) {
                          SMSC smsc = new SMSC();
                          string[] r = smsc.send_sms("375333118139", "Srabotka signalizacii",0,"",0,0,"DOM","");
                          smsopoveschenie = 1;
                        } 
                        
                    }
                }
            }

          //serialPort1.Close();
        }
        string result, result1;
        private void timer2_Tick(object sender, EventArgs e) // MySQL timer
        {
            MySqlCommand command = new MySqlCommand(); ;
            string connectionString, commandString, commandStringData, commandStringResetFlag;
            connectionString = "Data source=127.0.0.1;UserId=root;Password=8836353;database=data;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            commandStringData = "UPDATE `my_sql_table` SET `temper1`='" + temper1 + "',`davlenie`='" + davl + "',`temper2`='" + temper2 + "',`temper3`='" + temper3 + "',`Vlaga`='" + vlaga + "',`dver`='" + dver + "',`rip`='" + rip + "',`signal`='" + signal + "',`svet`='" + svet + "',`otpravkaArduino`='"+sostoyanie+"'";
            commandStringResetFlag = "UPDATE `my_sql_table` SET `sostoyanie`='0'";

            command.Connection = connection;
            commandString = "SELECT `otpravkaArduino`, `sostoyanie` FROM `my_sql_table`";
           //command.CommandText = commandString;
           MySqlDataReader reader;
            try
            {
                label23.Text = "11";
                connection.Open();  //select
                command.CommandText = commandString;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(0);
                    result1 = reader.GetString(1);

                    
               }
                connection.Close();
                label25.Text = result;
               label26.Text = result1;
               if (result1 != "0")
               {
                   serialPort1.Write(result);
                   if (result=="A"){
                       SvetAvto = 0;
                       Svet = 1;
                       SvetOnOff = 1;
                       button1.Enabled = true;
                       button2.Text = "Свет Выкл";
                       SignalOnOff = 1;
                       Signal = 1;
                       button3.Text = "Сигнализация Выкл";
                       sostoyanie = "A";
                   }
                   if (result == "B") {
                       button1.Enabled = true;
                       SvetAvto = 0;
                       Svet = 0;
                       SvetOnOff = 0;
                       button2.Text = "Свет Вкл";
                       SignalOnOff = 1;
                       Signal = 1;
                       button3.Text = "Сигнализация Выкл";
                       sostoyanie = "B";
                   }
                   if (result == "C") {
                       SvetAvto = 0;
                       Svet = 0;
                       SvetOnOff = 0;
                       button2.Text = "Свет Вкл";
                       Signal = 0;
                       SignalOnOff = 0;
                       button3.Text = "Сигнализация Вкл";
                       sostoyanie = "C";
                   }
                   if (result == "D") {
                       SvetAvto = 0;
                       Svet = 1;
                       SvetOnOff = 1;
                       button1.Enabled = true;
                       button2.Text = "Свет Выкл";
                       Signal = 0;
                       SignalOnOff = 0;
                       button3.Text = "Сигнализация Вкл";
                       sostoyanie = "D";
                   
                   }
                   if (result == "E") {
                       button1.Enabled = false;
                       SvetAvto = 1;
                       Svet = 0;
                       SvetOnOff = 1;
                       button2.Text = "Свет Выкл";
                       SignalOnOff = 1;
                       Signal = 1;
                       button3.Text = "Сигнализация Выкл";
                       sostoyanie = "E";
                   
                   }
                   if (result == "F") {
                       button1.Enabled = false;
                       SvetAvto = 1;
                       Svet = 0;
                       SvetOnOff = 1;
                       button2.Text = "Свет Выкл";
                       Signal = 0;
                       SignalOnOff = 0;
                       button3.Text = "Сигнализация Вкл";
                       sostoyanie = "F";
                   }
                   connection.Open(); //Update reset flag
                   command.CommandText = commandStringResetFlag;
                   int Resetflag = command.ExecuteNonQuery();
                   connection.Close();

               }
               
               connection.Open();  // Update data
                label23.Text = "22";
                label24.Text = commandStringData;
                command.CommandText = commandStringData;
                int UspeshnoeIzmenenie = command.ExecuteNonQuery();
                label23.Text = "33";
                if (UspeshnoeIzmenenie != 0)
                {
                    label23.Text = "vneseno";
                }
                else { label23.Text = "NEvneseno"; }
                connection.Close();
            }
            catch (MySqlException)
            {
                
            }
            finally
            {
                command.Connection.Close();
            }
            // UDP Narodmon.ru
            progressBar1.Value = UDPtimeNarodmon;
            if (UDPtimeNarodmon >= 60 && narodmonOnOFF == 1)
            {

                UdpClient udp = new UdpClient(8283);

                // Указываем адрес отправки сообщения
                IPAddress ipaddress = IPAddress.Parse("94.19.113.221");
                IPEndPoint ipendpoint = new IPEndPoint(ipaddress, 8283);

                // Формирование оправляемого сообщения и его отправка.
                byte[] message = Encoding.Default.GetBytes(udpData);
                int sended = udp.Send(message, message.Length, ipendpoint);


                // После окончания попытки отправки закрываем UDP соединение,
                // и освобождаем занятые объектом UdpClient ресурсы.
                udp.Close();
                UDPtimeNarodmon = 0;
                label38.Text = DateTime.Now.ToString();
            }
            
            if (narodmonOnOFF == 0) { UDPtimeNarodmon = 0; }
            else {UDPtimeNarodmon++; label34.Text = Convert.ToString(UDPtimeNarodmon);}
            if (UDPtimeNarodmon >= 60) { UDPtimeNarodmon = 60; }
            
        }

        private void button4_Click(object sender, EventArgs e) // narodmonOnOff
        {
            

            if (narodmonButton == 0) { 
                narodmonOnOFF = 1;
                button4.Text = "Отключить передачу данных";
                narodmonButton = 1; }
            else 
            {
                narodmonOnOFF = 0;
                button4.Text = "Включить передачу данных";
                narodmonButton = 0;
            }
        }

        private void button5_Click(object sender, EventArgs e) // smsOnOff
        {
            if (smsButton == 0) { smsButton = 1; button5.Text = "Отключить SMS оповещение"; label36.Text = "ON"; smsopoveschenie = 0; }
            else { smsButton = 0; button5.Text = "Включить SMS оповещение"; label36.Text = "OFF"; }

        }

       

        private void button6_Click(object sender, EventArgs e)
        {
            if (StatusServer == false)
            {
                Server.Start();
                label41.Text = "ON";
                button6.Text = "Отключить сервер";
                StatusServer = true;
                
            }
            else {
                
                Server.Interrupt();
               label41.Text = "OFF";
                button6.Text = "Запустить сервер";
                StatusServer = false;
                
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //serverstop = 1;
            //  textBox1.Text = serialPort1.ReadLine();
            //label1.Text = serialPort1.ReadLine();
            // При закрытии программы, закрываем порт
            if (serialPort1.IsOpen) serialPort1.Close();
            Server.Abort();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 57600;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 60;
            serialPort1.Open();
            if (!serialPort1.IsOpen)
            {


                 MessageBox.Show(String.Format("Невозможно найти устройство"),
                   "Ошибка USB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Server.Abort();
                this.Close();
            }
            else
            {
                
                
                Arduino.Start();
                MySQLDatabase.Start();
            }
          
        }

       

      

       

        
           
        

     
       
        }

     
    }

