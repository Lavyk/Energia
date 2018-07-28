using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace Energia
{
    public partial class Form1 : Form
    {

        DateTime DataHoraAbertura;
        int seg, min, horas, dias;
        double mins;
        String strSeg, strMin, strHoras, strDias;

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000;  
            timer1.Tick += new EventHandler(timer1_Tick); 
            timer1.Enabled = true;
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope scope = new ManagementScope();
            scope.Connect();

            ObjectQuery query = new ObjectQuery(
            "SELECT * FROM Win32_CurrentProbe");
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                // Display the remote computer information
                Console.WriteLine("NormalMax : {0}",
                    m["NormalMax"]);
                Console.WriteLine("CurrentReading: {0}",
                    m["CurrentReading"]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataHoraAbertura = DateTime.Now;
            timer1.Start();
        }

        /* private void timer1_Tick(object sender, EventArgs e)
        {
            DiferencaDataHora = (DateTime.Now).Subtract(DataHoraAbertura);
            this.label1.Text = DiferencaDataHora.ToString();
            //Pode ser feito assim também

            this.label1.Text = String.Format("Dias: {0}, Horas: {1}, Minutos: {2}, Segundos: {3}", DiferencaDataHora.Days, DiferencaDataHora.Hours, DiferencaDataHora.Minutes, DiferencaDataHora.Seconds);
            this.label1.Text = this.DataHoraAbertura.ToString();
        } */

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Contador de tempo

            if (seg < 60)
            {
                seg++;
            }
            else
            {
                min++;
                seg = 0;
            }

            if (min == 60)
            {
                horas++;
                min = 0;
            }

            if (horas == 24)
            {
                dias++;
                horas = 0;
            }


            if (seg <= 9)
            {
                this.strSeg = "0" + seg;
            }
            else
            {
                this.strSeg = seg.ToString();
            }

            if (min <= 9)
            {
                strMin = "0" + min;
            }
            else
            {
                strMin = min.ToString();
            }

            if (horas <= 9)
            {
                strHoras = "0" + horas;
            }
            else
            {
                strHoras = horas.ToString();
            }

            if (dias <= 9)
            {
                strDias = "0" + dias;
            }
            else
            {
                strDias = dias.ToString();
            }

            this.label1.Text = String.Format("{0}:{1}:{2}:{3}", this.strDias, this.strHoras, this.strMin, this.strSeg);
            this.labelKvhAtual.Text = this.getKvHoras().ToString();
            this.labelReaisAtual.Text = this.getCusto().ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private double getKvHoras()
        {
            mins = (this.min / 6);
            double tempo = this.horas + (mins/10);
            double kvh = (260 * tempo);
            return Math.Round(kvh / 1000, 2);
        }

        private double getCusto()
        {
            double preco = this.getKvHoras() * 0.49885;
            return Math.Round(preco, 2); ;
        }


    }
}
