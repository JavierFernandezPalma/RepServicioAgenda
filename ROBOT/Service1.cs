using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using MODELO;
using System.Timers;
using System.Configuration;
using SERVICIO;

namespace ROBOT
{
    public partial class Service1 : ServiceBase
    {
        agendaEntities bd_agenda = new agendaEntities();
        Timer tmr = null;
        ServicioCSJ servicio = new ServicioCSJ();
        LogErrores log = new LogErrores();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                log.EscribaLog("OnStart()", "Inicia el metodo OnStart()");
                tmr = new Timer();
                tmr.Interval = 60000 * Convert.ToDouble(ConfigurationManager.AppSettings["intervalo_tiempo"]);
                tmr.Elapsed += new ElapsedEventHandler(metodo_elapsed);
                tmr.Enabled = true;
                tmr.Start();
                log.EscribaLog("OnStart()", "Termino de ejecutarse el metodo OnStart()");
            }
            catch (Exception ex)
            {
                log.EscribaLog("OnStart()_Error", "El error es : " + ex.ToString());
            }
        }

        protected override void OnStop()
        {
            log.EscribaLog("OnStop()", "Finalizo el servicio Windows AgendaCSJ.");
            EventLog.WriteEntry("Finalizo el servicio Windows AgendaCSJ.");
            this.tmr.Stop();
            this.tmr = null;

        }

        private void metodo_elapsed(object sender, ElapsedEventArgs e)
        {

            try
            {
                tmr.Enabled = false;
                tmr.Stop();
                verificarAgendamientos();
                //verificarPlazoAgendamiento();
                tmr.Enabled = true;
                tmr.Start();
                EventLog.WriteEntry("Servicio AgendaCSJ completo el ciclo correctamente.");
                log.EscribaLog("ServicioAgendaCSJ", "Servicio AgendaCSJ completo el ciclo correctamente.");

            }
            catch (Exception ex)
            {
                log.EscribaLog("metodo_elapsed()_Error", "El error es : " + ex.ToString());
            }
        }

        private void EjecutarAgendamiento(List<t_formulario> idNoAgendados)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var fechaAgendamiento = Convert.ToString(fecha);
                log.EscribaLog("EjecutarAgendamiento()", "Iniciando el metodo EjecutarAgendamiento()");
                bool control = false;

                foreach (var filaAgendar in idNoAgendados)
                {

                    if (filaAgendar.EstadoAgendamiento == 4)
                    {

                        bd_agenda.t_formulario.Attach(filaAgendar);
                        filaAgendar.EstadoAgendamiento = 1;
                        bd_agenda.Configuration.ValidateOnSaveEnabled = false;
                        bd_agenda.SaveChanges();

                        var credenciales = (from db in bd_agenda.t_login_rp1cloud where db.CodigoSala == filaAgendar.Sala select db).First();
                        string usuario = credenciales.Usuario_Rp1Cloud;
                        string clave = credenciales.Clave;

                        var difDiasAgendamiento = Convert.ToInt32(Convert.ToDateTime(filaAgendar.FechaFinalizacion).Day - Convert.ToDateTime(filaAgendar.FechaRealizacion).Day);

                        for (int i = 0; i <= difDiasAgendamiento; i++)
                        {
                            bd_agenda.t_formulario.Attach(filaAgendar);
                            filaAgendar.FechaRealizacion = Convert.ToDateTime(filaAgendar.FechaRealizacion).AddDays(i).ToString();
                            control = servicio.ServicioAgenda(filaAgendar, usuario, clave);

                            if (i == difDiasAgendamiento)
                            {
                                filaAgendar.FechaRealizacion = Convert.ToDateTime(filaAgendar.FechaRealizacion).AddDays(-difDiasAgendamiento).ToString();
                            }
                        }

                        bd_agenda.SP_REGISTRO_RP1CLUOD(filaAgendar.Id, filaAgendar.Num_Consecutivo, filaAgendar.Codigo, fechaAgendamiento);

                        if (control == true)
                        {
                            filaAgendar.EstadoAgendamiento = 5;
                            log.EscribaLog("EjecutarAgendamiento()", "Registro con error N° " + filaAgendar.Id);
                        }
                        else
                        {
                            filaAgendar.EstadoAgendamiento = 2;
                            log.EscribaLog("EjecutarAgendamiento()", "Registro agendado N° " + filaAgendar.Id);
                        }

                        bd_agenda.Configuration.ValidateOnSaveEnabled = false;
                        bd_agenda.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                log.EscribaLog("EjecutarAgendamiento()_Error", "El error es : " + ex.ToString());
            }

        }


        public void verificarAgendamientos()
        {
            //var listaNoAgendados = bd_agenda.SP_LISTA_AGENDA().ToList();

            try
            {
                var listaNoAgendados = (from db in bd_agenda.t_formulario where !(from db2 in bd_agenda.t_contador select db2.IdFormulario).Contains(db.Id) select db).Take(100).ToList();

                if (listaNoAgendados.Count != 0)
                {
                    EjecutarAgendamiento(listaNoAgendados);
                }
            }
            catch (Exception ex)
            {
                log.EscribaLog("verificarAgendamientos()_Error", "El error es : " + ex.ToString());
            }
        }

        public void verificarPlazoAgendamiento()
        {
            //var registrosFechaVencida = bd_agenda.SP_LISTA_AGENDAVENCIDOS().ToList();
            DateTime fechaActual = DateTime.Now;

            try
            {
                var registrosFechaVencida = (from db in bd_agenda.t_formulario where db.Estado == "AGENDADA" && db.EstadoAgendamiento == 2 select db).Take(100).ToList();

                if (registrosFechaVencida.Count != 0)
                {
                    foreach (var filaVencido in registrosFechaVencida)
                    {
                        var agendaVencido = (from db in bd_agenda.t_formulario where db.Id == filaVencido.Id select db).First();

                        if (System.Convert.ToDateTime(agendaVencido.FechaFinalizacion).AddDays(2) <= fechaActual)
                        {
                            bd_agenda.t_formulario.Attach(agendaVencido);
                            agendaVencido.Estado = "NO REALIZADA";
                            agendaVencido.EstadoAgendamiento = 3;
                            bd_agenda.Configuration.ValidateOnSaveEnabled = false;
                            bd_agenda.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.EscribaLog("verificarPlazoAgendamiento()_Error", "El error es : " + ex.ToString());
            }
        }

        public void ServiceWindowsTest()
        {

            try
            {

                tmr = new Timer();
                tmr.Interval = 5000 * Convert.ToDouble(ConfigurationManager.AppSettings["intervalo_tiempo"]);
                tmr.Elapsed += new ElapsedEventHandler(metodo_elapsed);
                tmr.Enabled = true;
                tmr.Start();
            }
            catch (Exception ex)
            {
                log.EscribaLog("OnStart()_Error", "El error es : " + ex.ToString());
            }
        }
    }
}
