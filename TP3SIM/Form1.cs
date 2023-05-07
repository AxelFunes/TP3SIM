using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP3SIM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int simulaciones = 0;
        int desde = 0;
        int hasta = 0;

        Random rnd = new Random();
        //Variables para el procedimiento de simulaciones
        //int semanas = 0;

        double random_demanda;
        int demanda=0;
        double random_falla1;
        double random_falla2;
        double random_falla3;
        double random_falla4;
        double random_falla5;
        double random_falla6;

        double stock_Inicial;
        double stock_Final;
        string fallada1="";
        string fallada2 = "";
        string fallada3 = "";
        string fallada4 = "";
        string fallada5 = "";
        string fallada6 = "";

        string pide;
        double random_demora;
        int demora;
        int cantidadFalladas;
        int llegada;
        double costoTenencia;
        double costoPedido = 0;
        double costoAgotamiento;
        int agotamiento;
        double costoTotal;
        double costoAcumulado;
        

        bool yaPidio;
        

        private void btn_simular_Click(object sender, EventArgs e)
        {
            costoAcumulado = 0;
            stock_Inicial = Convert.ToDouble(txt_StockInicial.Text);
            
            yaPidio = false;
            llegada = 0; //Para que no muestre el ultimo valor que quedo guardado cuando se simula nuevamente

            if (txt_simulacion.Text != "" && txt_desde.Text != "" && txt_hasta.Text != "" && txtDemora0.Text != "" && txtDemora1.Text != "" && txtDemora2.Text != "" && txtFallaNo.Text != "" && txtFallaSi.Text != "" && txtProb0.Text != "" && txtProb1.Text != "" && txtProb2.Text != "" && txtProb3.Text != "" && txtTenencia.Text != "" && txtAgotamiento.Text != "" && txtPedido.Text != "")
            {
                double p0 = Convert.ToDouble(txtProb0.Text);
                double p1 = Convert.ToDouble(txtProb1.Text);
                double p2 = Convert.ToDouble(txtProb2.Text);
                double p3 = Convert.ToDouble(txtProb3.Text);
                double sumaP = p0 + p1 + p2 + p3;

                double cTenencia = double.Parse(txtTenencia.Text.ToString());
                double cAgotamiento = double.Parse(txtAgotamiento.Text.ToString());
                double cPedido = double.Parse(txtPedido.Text.ToString());

                double d0 = double.Parse(txtDemora0.Text.ToString());
                double d1 = double.Parse(txtDemora1.Text.ToString());
                double d2 = double.Parse(txtDemora2.Text.ToString());
                double sumaD = d0 + d1 + d2;

                double f0 = Convert.ToDouble(txtFallaNo.Text);
                double f1 = Convert.ToDouble(txtFallaSi.Text);
                double sumaF = f0 + f1;

                if ((sumaP == 100) && (p0 > 0 && p1 > 0 && p2 > 0 && p3 > 0)) //Valida que las probabilidades sumen 100% y no sean menor a 0
                {
                    if ((sumaD == 1.0) && (d0 > 0 && d1 > 0 && d2 > 0)) // Valida que las probabilidades sumen 100% y no sean menor a 0
                    {
                        if ((sumaF == 100) && (f0 > 0 && f1 > 0)) //Valida que las probabilidades sumen 100% y no sean menor a 0
                        {
                            if (cTenencia>=0 && cAgotamiento>=0 && cPedido>=0) //Valida que los costos no sean negativos
                            {
                                simulaciones = Convert.ToInt32(txt_simulacion.Text.ToString());
                                desde = Convert.ToInt32(txt_desde.Text.ToString());
                                hasta = Convert.ToInt32(txt_hasta.Text.ToString()) + desde;

                                //semanas = 0;
                                dgv_simulaciones.Rows.Clear();
                                //limpiarVariables();


                                if (desde < simulaciones && hasta > desde && hasta <= simulaciones)
                                {
                                    simulacion(simulaciones, desde, hasta);

                                }
                                else
                                {
                                    MessageBox.Show("Por Favor seleccione un DESDE menor a la cantidad de simulaciones o un Hasta mayor que Desde");
                                    //limpiarTxt();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Los costos no pueden ser negativos.");
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("La suma de las probabilidades de Falla es distinto de 100%. Ingrese correctamente los %.");
                            //limpiarTxt();
                        }
                    }
                    else
                    {
                        MessageBox.Show("La suma de las probabilidades de Demora es distinto de 1. Ingrese correctamente la frecuencia.");
                        //limpiarTxt();
                    }
                }
                else
                {
                    MessageBox.Show("La suma de las probabilidades de Demanda es distinto de 100%. Ingrese correctamente los %.");
                    //limpiarTxt();
                }
            }
            else
            {
                if (txt_simulacion.Text == "" || txt_desde.Text == "" || txt_hasta.Text == "") //valida el box de simulaciones
                {
                    if (txt_simulacion.Text == "")
                    {
                        MessageBox.Show("Por favor, ingrese la cantidad de semanas.");
                    }
                    else
                    {
                        if (txt_desde.Text == "")
                        {
                            MessageBox.Show("Ingrese el valor DESDE.");

                        }
                        else
                        {
                            MessageBox.Show("Ingrese la cantidad de iteraciones.");
                        }
                    }
                    
                }
                if(txtDemora0.Text == "" || txtDemora1.Text == "" || txtDemora2.Text == "")
                {
                    MessageBox.Show("Verifique que todas las frecuencias de tiempo de entrega esten cargadas. ");
                    
                }
                if (txtProb0.Text == "" || txtProb1.Text == "" || txtProb2.Text == "" || txtProb3.Text != "")
                {
                    MessageBox.Show("Verifique que todas las probabilidades de demanda esten cargadas. ");
                    
                }
                if(txtTenencia.Text == "" || txtAgotamiento.Text == "" || txtPedido.Text != "")
                {
                    MessageBox.Show("Verifique que todos los costos esten cargados. ");
                }
            }
        }

        public void limpiarTxt()
        {
            txt_simulacion.Clear();
            txt_desde.Clear();
            txt_hasta.Clear();
            txtProb0.Clear();
            txtProb1.Clear();
            txtProb2.Clear();
            txtProb3.Clear();
            txtFallaSi.Clear();
            txtFallaNo.Clear();
            txtDemora0.Clear();
            txtDemora1.Clear();
            txtDemora2.Clear();
            txtAgotamiento.Clear();
            txtPedido.Clear();
            txtTenencia.Clear();
            dgv_simulaciones.Rows.Clear();
        }
        public void simulacion(int experimentos, int desde, int hasta)
        {
            
            int semanas = 0;
            double cTenencia = Convert.ToDouble(txtTenencia.Text.ToString());
            double cAgotamiento = Convert.ToDouble(txtAgotamiento.Text.ToString());
            double cPedido = Convert.ToDouble(txtPedido.Text.ToString());


            for (int i = semanas; i <= experimentos; i++)
            {
                pide = "NO";

                random_demanda = rnd.NextDouble();
                BuscarDemanda();

                if ( llegada == i && semanas!= 0)
                {
                    yaPidio = false;
                    cantidadFalladas = 0;

                    random_falla1 = rnd.NextDouble();
                    random_falla2 = rnd.NextDouble();
                    random_falla3 = rnd.NextDouble();
                    random_falla4 = rnd.NextDouble();
                    random_falla5 = rnd.NextDouble();
                    random_falla6 = rnd.NextDouble();
                    BuscarFalla(random_falla1, random_falla2, random_falla3, random_falla4, random_falla5, random_falla6);
                    stock_Final = stock_Inicial + 6 - cantidadFalladas;

                }
                else
                {
                    stock_Final = stock_Inicial - demanda;
                    if (stock_Final < 3 && yaPidio == false)
                    {
                        if (stock_Final < 0)
                        {
                            agotamiento = Math.Abs(stock_Final);
                            stock_Final = 0;
                            costoAgotamiento = agotamiento * cAgotamiento;
                        }
                        else { costoAgotamiento = 0; } //reseteo para el acumulado
                        pide = "SI";
                        costoPedido = cPedido;

                        yaPidio = true;
                        random_demora = rnd.NextDouble();
                        BuscarDemora();
                        llegada = i + demora;

                    }
                    else //VEER
                    {
                        costoPedido = 0; //reseteo para el acumulado
                        if (stock_Final < 0)
                        {
                            agotamiento = Math.Abs(stock_Final);
                            stock_Final = 0;
                            costoAgotamiento = agotamiento * cAgotamiento;
                        }
                        else { costoAgotamiento = 0; } //reseteo para el acumulado
                    }

                    cantidadFalladas=0; //reseteo para el acumulado
                }
                semanas = i;
                
                costoTenencia = stock_Final * cTenencia ;
                costoTotal = costoTenencia + costoPedido + costoAgotamiento;
                costoAcumulado += costoTotal;
                
                if (semanas >= desde && semanas <= hasta)
                {
                    cargarGrilla(semanas);
                    
                }
                if (semanas == experimentos && hasta!=experimentos)
                {
                    cargarGrilla(semanas);
                }
                stock_Inicial = stock_Final;
            }
            
        }

        public void BuscarDemora()
        {
            double d0 = Convert.ToDouble(txtDemora0.Text.ToString());
            double d1 = Convert.ToDouble(txtDemora1.Text.ToString());

            if (random_demora < d0)
            {
                demora = 1;
            }
            else
            {
                if (random_demora < d1)
                {
                    demora = 2;
                }
                else
                {
                    demora = 3;
                }
            }
        }

        public void BuscarFalla(double f1, double f2, double f3, double f4, double f5, double f6)
        {
            double probNoFalla= Convert.ToDouble(txtFallaNo.Text)/100;

            if (random_falla1 <= probNoFalla) { fallada1 = "NO"; } else { fallada1 = "SI"; cantidadFalladas++; }
            if (random_falla2 <= probNoFalla) { fallada2 = "NO"; } else { fallada2 = "SI"; cantidadFalladas++; }
            if (random_falla3 <= probNoFalla) { fallada3 = "NO"; } else { fallada3 = "SI"; cantidadFalladas++; }
            if (random_falla4 <= probNoFalla) { fallada4 = "NO"; } else { fallada4 = "SI"; cantidadFalladas++; }
            if (random_falla5 <= probNoFalla) { fallada5 = "NO"; } else { fallada5 = "SI"; cantidadFalladas++; }
            if (random_falla6 <= probNoFalla) { fallada6 = "NO"; } else { fallada6 = "SI"; cantidadFalladas++; }
        }

        public void BuscarDemanda()
        {
            double prob0 = Convert.ToDouble(txtProb0.Text)/100;
            double prob1 = Convert.ToDouble(txtProb1.Text)/100;
            double prob2 = Convert.ToDouble(txtProb2.Text)/100;


            if (random_demanda < prob0)
            {
                demanda = 0;
            }
            else
            {
                if (random_demanda < (prob0+prob1))
                {
                    demanda = 1;
                }
                else
                {
                    if (random_demanda < (prob0+prob1+prob2))
                    {
                        demanda = 2;
                    }
                    else
                    {
                        if (random_demanda < 1)
                        {
                            demanda = 3;
                        }
                        
                    }
                }
            }

            //return demanda;
        }

        public void cargarGrilla(int semanas)
        {
            if (semanas==0)
            {
                dgv_simulaciones.Rows.Add(semanas, Math.Round(random_demanda, 4),
                    demanda, stock_Inicial, "", "", "", "", "", "", "", "", "", "", "", "", "", stock_Final, pide, "" /*Math.Round(random_demora, 4)*/, "", "", costoTenencia, "", costoAgotamiento, costoTotal, costoAcumulado);
            }
            else
            {
                if (pide == "SI")
                {
                    dgv_simulaciones.Rows.Add(semanas, Math.Round(random_demanda, 4),
                    demanda, stock_Inicial, "", "", "", "", "", "", "", "", "", "", "", "", "", stock_Final, pide, Math.Round(random_demora, 4), demora, llegada, costoTenencia, costoPedido, costoAgotamiento, costoTotal, costoAcumulado);

                }
                else
                {
                    if (semanas == llegada)
                    {
                        dgv_simulaciones.Rows.Add(semanas, Math.Round(random_demanda, 4),
                        demanda, stock_Inicial, Math.Round(random_falla1, 4), fallada1, Math.Round(random_falla2, 4), fallada2, Math.Round(random_falla3, 4), fallada3, Math.Round(random_falla4, 4), fallada4, Math.Round(random_falla5, 4), fallada5, Math.Round(random_falla6, 4), fallada6, cantidadFalladas, stock_Final, pide, "", ""/* Math.Round(random_demora, 4), demora*/, llegada, costoTenencia, "", costoAgotamiento, costoTotal, costoAcumulado);

                    }
                    else
                    {
                        if (llegada < semanas)
                        {
                            dgv_simulaciones.Rows.Add(semanas, Math.Round(random_demanda, 4),
                        demanda, stock_Inicial, "", "", "", "", "", "", "", "", "", "", "", "", "", stock_Final, pide, "" /*Math.Round(random_demora, 4)*/, "", "", costoTenencia, "", costoAgotamiento, costoTotal, costoAcumulado);
                        }
                        else
                        {
                            dgv_simulaciones.Rows.Add(semanas, Math.Round(random_demanda, 4),
                        demanda, stock_Inicial, "", "", "", "", "", "", "", "", "", "", "", "", "", stock_Final, pide, "" /* Math.Round(random_demora, 4)*/, "", llegada, costoTenencia, "", costoAgotamiento, costoTotal, costoAcumulado);
                        }

                    }
                }
            }
            
            


        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            limpiarTxt();

        }

        
    }
}
