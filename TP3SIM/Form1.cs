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
        int semanas = 0;

        double random_demanda;
        int demanda=0;
        double random_falla1;
        double random_falla2;
        double random_falla3;
        double random_falla4;
        double random_falla5;
        double random_falla6;

        int stock_Inicial = 7;
        int stock_Final;
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
        int costoTenencia;
        int costoPedido;
        int costoAgotamiento;
        int agotamiento;
        int costoTotal;
        int costoAcumulado;

        // PARA ABAJO ES EL TP ANTERIOR
        int pedidoNeto;

        int stockReal;
        int stockRemanenteAnterior;
        int stockRemanenteActual;

        // variables para costo
        int costoMantenimiento;
        //int costoPedido;
        int costoSobrepaso;
        //int costoTotal;

        // variables adicionales
        int excedenteStock;
        int porcentajeCosto;
        int porcentajeMinActual;
        int porcentajeMaxActual;

        private void btn_simular_Click(object sender, EventArgs e)
        {
            if (txt_simulacion.Text != "" && txt_desde.Text != "")
            {
                simulaciones = Convert.ToInt32(txt_simulacion.Text);
                desde = Convert.ToInt32(txt_desde.Text);

                semanas = 0;
                dgv_simulaciones.Rows.Clear();
                //limpiarVariables();
                

                if (desde < simulaciones)
                {
                    hasta = desde + 400; //Consultar si el hasta se ingresa por parametro
                    txt_hasta.Text = Convert.ToString(desde + 400);

                    for (int i = 0; i < desde; i++)
                    {
                        simulacion();
                    }
                    cargarGrilla();


                    if (hasta > simulaciones)
                    {
                        for (int i = 0; i < (simulaciones - desde); i++)
                        {
                            simulacion();
                            cargarGrilla();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 400; i++)
                        {
                            simulacion();
                            cargarGrilla();
                        }

                        for (int i = 0; i < (simulaciones - hasta - 1); i++)
                        {
                            simulacion();
                        }

                        if (hasta != simulaciones)
                        {
                            simulacion();
                            cargarGrilla();
                        }

                    }
                   




                }
                else
                {
                    MessageBox.Show("Por Favor seleccione un DESDE menor a la cantidad de simulaciones");
                }
            }
            else
            {
                MessageBox.Show("Por Favor complete todos los campos");
            }
        }


        public void simulacion()
        {
            pide = "NO";
            
            semanas++;
            random_demanda = rnd.NextDouble();
            BuscarDemanda();

            if (/*pide == "SI" &&*/ llegada == semanas)
            {

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
                if (stock_Final < 3)
                {
                    if (stock_Final < 0) { agotamiento = stock_Final * -1; stock_Final = 0; }
                    pide = "SI";
                    random_demora = rnd.NextDouble();
                    BuscarDemora();
                    llegada = semanas + demora;

                }
            }
            stock_Inicial = stock_Final;
            //random_pedido = rnd.NextDouble();

            /*
            if (stockRemanenteAnterior >= 0)
            {
                BuscarPedido();
                pedidoNeto = pedido;
                stockRemanenteActual = stockRemanenteAnterior - demanda + pedidoNeto;
                stockRemanenteAnterior = stockRemanenteActual;

            }
            else
            {
                BuscarPedido();
                pedidoNeto = pedido + stockRemanenteAnterior;
                stockRemanenteActual = pedidoNeto - demanda;
                stockRemanenteAnterior = stockRemanenteActual;
                stockReal = 0;
            }
            if (stockRemanenteActual < 0)
            {
                stockReal = 0;
            }
            else
            {
                stockReal = stockRemanenteActual;
            }

            if (stockReal <= 20000)
            {
                costoMantenimiento = stockReal * 2500; // el costo es de 2500 por m2 y el stock no supera los 20000
                costoSobrepaso = 0;
                excedenteStock = 0;
            }
            else
            {
                costoMantenimiento = 20000 * 2500; // el costo es de 2500 por m2 y el stock supera los 20000
                excedenteStock = (stockReal - 20000);
                costoSobrepaso = excedenteStock * 5000; // el costo de sobrepaso es de 5000 por m2 que se exceda el stock de 20000.
            }

            costoPedido = 25500;
            costoTotal = costoMantenimiento + costoPedido + costoSobrepaso;
            porcentajeCosto = costoTotal / semanas;


            if (porcentajeCosto > porcentajeMaxActual)
            {
                porcentajeMaxActual = porcentajeCosto;
            }
            if (semanas > 1)
            {
                if (porcentajeCosto < porcentajeMinActual)
                {
                    porcentajeMinActual = porcentajeCosto;
                }
            }
            else
            {
                porcentajeMinActual = porcentajeCosto;
            }*/
        }

        public void BuscarDemora()
        {
            if (random_demora < 0.3)
            {
                demora = 1;
            }
            else
            {
                if (random_demora < 0.7)
                {
                    demora = 2;
                }
                else
                {
                    if (random_demora < 1)
                    {
                        demora = 3;
                    }
                    
                }
            }
        }

        public void BuscarFalla(double f1, double f2, double f3, double f4, double f5, double f6)
        {
            if (random_falla1 <= 0.70) { fallada1 = "NO"; } else { fallada1 = "SI"; cantidadFalladas++; }
            if (random_falla2 <= 0.70) { fallada2 = "NO"; } else { fallada2 = "SI"; cantidadFalladas++; }
            if (random_falla3 <= 0.70) { fallada3 = "NO"; } else { fallada3 = "SI"; cantidadFalladas++; }
            if (random_falla4 <= 0.70) { fallada4 = "NO"; } else { fallada4 = "SI"; cantidadFalladas++; }
            if (random_falla5 <= 0.70) { fallada5 = "NO"; } else { fallada5 = "SI"; cantidadFalladas++; }
            if (random_falla6 <= 0.70) { fallada6 = "NO"; } else { fallada6 = "SI"; cantidadFalladas++; }
        }

        public void BuscarDemanda()
        {
            if (random_demanda < 0.5)
            {
                demanda = 0;
            }
            else
            {
                if (random_demanda < 0.65)
                {
                    demanda = 1;
                }
                else
                {
                    if (random_demanda < 0.9)
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

        public void cargarGrilla()
        {
            if (semanas == llegada)
            {
                dgv_simulaciones.Rows.Add(Convert.ToString(semanas), Math.Round(random_demanda, 4),
                demanda, stock_Inicial, Math.Round(random_falla1, 4), fallada1, Math.Round(random_falla2, 4), fallada2, Math.Round(random_falla3, 4), fallada3, Math.Round(random_falla4, 4), fallada4, Math.Round(random_falla5, 4), fallada5, Math.Round(random_falla6, 4), fallada6, cantidadFalladas, stock_Final, pide, random_demora, demora, llegada);

            }
            else
            {
                dgv_simulaciones.Rows.Add(Convert.ToString(semanas), Math.Round(random_demanda, 4),
                demanda, stock_Inicial, "", "","","","","","","","" ,"","","",cantidadFalladas, stock_Final, pide, random_demora, demora, llegada);
            }

        }
    }
}
