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
        int demanda;
        double random_falla;
        
        int stock_Inicial = 7;
        int stock_Final;
        string fallada;
        string pide;
        double random_demora;
        int demora;
        int llegada;
        int costoTenencia;
        int costoPedido;
        int costoAgotamiento;
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
            semanas++;
            random_demanda = rnd.NextDouble();
            BuscarDemanda();
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
            dgv_simulaciones.Rows.Add(Convert.ToString(semanas), Math.Round(random_demanda, 4),
                demanda); /*, Math.Round(random_pedido, 4), pedido, pedidoNeto, stockRemanenteActual, stockReal,
                excedenteStock, costoMantenimiento, costoSobrepaso, costoPedido, costoTotal,
                porcentajeCosto, porcentajeMinActual, porcentajeMaxActual);*/
        }

    }
}
