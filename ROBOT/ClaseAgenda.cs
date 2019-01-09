using MODELO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ROBOT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SERVICIO
{
    public class ServicioCSJ
    {
        DateTime HoyAhora = DateTime.Now; // Extrae la facha y hora actual
        agendaEntities tablaFormulario = new agendaEntities();
        LogErrores log = new LogErrores();
        FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(ConfigurationManager.AppSettings["rutadriver"]);


        public bool ServicioAgenda(t_formulario filaRegistro, string usuario_, string clave_)
        {
            service.FirefoxBinaryPath = ConfigurationManager.AppSettings["rutanavegador"];
            IWebDriver driver_ = new FirefoxDriver(service);

            try
            {
                int a = IniciarNavegador(driver_, filaRegistro.Id, usuario_, clave_);
                int b = SeleccionarCalendario(driver_, filaRegistro.Id);
                int c = ProgramarReunion(driver_, filaRegistro.Id);
                int d = LlenarCampos(driver_, filaRegistro.Id);
                int e = SeleccionarDia(driver_, filaRegistro.Id, SeleccionarAnoMes(driver_, filaRegistro));
                int f = AdicionarCorreo(driver_, filaRegistro.Id);
                int g = EscribirMensaje(driver_, filaRegistro.Id);
                Cerrar(driver_, filaRegistro.Id);

                if(a+b+c+d+e+f+g > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Cerrar(driver_, filaRegistro.Id);
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + filaRegistro + ". El error es " + ex.ToString() + "/nEn <<ServicioAgenda_Error>>");
                return true;
            }


        }

        private int IniciarNavegador(IWebDriver driver, int idregistro, string usuario, string clave)
        {

            try
            {

                driver.Navigate().GoToUrl("http://my.rp1cloud.com/");
                //driver.FindElement(By.Name("accountLogin")).SendKeys(usuario);
                //driver.FindElement(By.Name("accountPassword")).SendKeys(clave);
                driver.FindElement(By.Name("accountLogin")).SendKeys("david.mendez@axede.com.co");
                driver.FindElement(By.Name("accountPassword")).SendKeys("123456");
                driver.FindElement(By.ClassName("submit")).Click();
                driver.Manage().Window.Maximize(); //Minimixar ventana del navegador

                return 0;
            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\nEn <<IniciarNavegador_Error>>");
                return 1;
            }


        }

        public int SeleccionarCalendario(IWebDriver driver, int idregistro)
        {

            try
            {
                //Seleccionar calendario
                driver.Navigate().GoToUrl("https://my.rp1cloud.com/meetings/schedule");
                return 0;
                //Seleccionar calendario
            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<SeleccionarCalendario_Error>>");
                return 1;
            }
        }


        public int ProgramarReunion(IWebDriver driver, int idregistro)
        {
            try
            {
                //Programar reunion
                driver.FindElement(By.XPath("//*[@id='meeting-controls']/div/div[2]/div/div/button")).Click();
                return 0;
                //Programar reunion
            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<ProgramarReunion_Error>>");
                return 1;
            }
        }


        public int LlenarCampos(IWebDriver driver, int idregistro)
        {
            try
            {
                //Seleccionar VMR
                Actions action = new Actions(driver);
                action.MoveByOffset(298, 657).Click().Build().Perform();
                //Fin Seleccionar VMR

                //Llenar campos
                var tipo_solicitud = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Codigo).First();
                var hora = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.HoraDeInicio).First();

                string horaInicio;
                string tipoDia = "am";

                string[] separarHora;
                separarHora = hora.Split(' ');

                var separar = separarHora;
                string[] separarNum = separar[0].Split(':');
                int horaMeridiano = int.Parse(separarNum[0]);

                if (int.Parse(separarNum[0]) >= 12)
                {
                    tipoDia = "pm";
                    horaMeridiano = int.Parse(separarNum[0]) - 12;
                }

                horaInicio = Convert.ToString(horaMeridiano) + ":" + separarNum[1] + tipoDia;

                driver.FindElement(By.Id("name")).SendKeys(tipo_solicitud);
                driver.FindElement(By.Name("startTime")).Clear();
                driver.FindElement(By.Name("startTime")).SendKeys(horaInicio);
                driver.FindElement(By.Name("endTime")).Clear();
                driver.FindElement(By.Name("endTime")).SendKeys("11:59pm");
                //Fin Llenar campos

                //Seleccionar zona
                var option = driver.FindElement(By.Name("timeZone"));
                var selectElement = new SelectElement(option);
                selectElement.SelectByText("America/Bogota");
                //Seleccionar zona

                return 0;

            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<LlenarCampos_Error>>");
                return 1;
            }
        }

        public int AdicionarCorreo(IWebDriver driver, int idregistro)
        {

            try
            {
                var correos = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.CorreoDeSolicitante).First();

                //Aqui va el checkbox

                string[] separarCorreos;
                separarCorreos = correos.Split('|');
                var contarCorreos = separarCorreos.Count();

                IWebElement webElement;

                for (int i = 0; i < separarCorreos.Count(); i++)
                {
                    webElement = driver.FindElement(By.Name("email"));
                    webElement.Click();
                    webElement.SendKeys(separarCorreos[i].ToLower());

                    webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[7]/div[2]/div/a"));
                    webElement.Click();
                }

                return 0;

            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<AdicionarCorreo_Error>>");
                return 1;
            }


        }


        public int EscribirMensaje(IWebDriver driver, int idregistro)
        {
            try
            {
                var consecutivo = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Num_Consecutivo).First();
                var observaciones = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Observaciones).First();
                var nomSala = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Sala).First();

                var salnom = (from db in tablaFormulario.t_salas where db.CodigoSala == nomSala select db.NombreCompletoSalaVirtual).First();

                var procesado = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Procesado).First();
                var declarante = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Declarante).First();

                driver.FindElement(By.Name("message")).SendKeys("Recuerde que el número del consecutivo de esta audiencia es: " + consecutivo + ". <br/> \nEl nombre de la sala es: " + salnom + ". " + observaciones + ". <br/>"); //Nombre sala y observaciones

                if (procesado != null && procesado != "")
                {
                    driver.FindElement(By.Name("message")).SendKeys("\nProcesado: " + procesado + " <br/>"); //Nombre sala, observaciones, procesado y declarante

                }

                if (declarante != null && procesado != "")
                {
                    driver.FindElement(By.Name("message")).SendKeys("\nDeclarante: " + declarante + " <br/>"); //Nombre sala, observaciones, procesado y declarante

                }

                driver.FindElement(By.Name("message")).SendKeys("\n<br/>Lo invitamos a visitar nuestra página: " + "<a href='https://www.ramajudicial.gov.co/'>ramajudicial</a>");


                driver.FindElement(By.Id("addsend")).Click();
                //driver.FindElement(By.Id("addcancel")).Click();  //Para pruebas

                return 0;
            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<EscribirMensaje_Error>>");
                return 1;
            }

        }


        public void Cerrar(IWebDriver driver, int idregistro)
        {

            try
            {
                driver.Close();
                driver.Quit();
            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<CerrarOcultar_Error>>");
            }

        }

        public int SeleccionarAnoMes(IWebDriver driver, t_formulario idregistro)
        {
            try
            {
                int diferenciaMes, diferenciaAno;

                var fecha_realizacion = idregistro.FechaRealizacion;
                //var idregistro = (from db in tablaFormulario.t_formulario select db.Id).Max();
                //var fecha_realizacion = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.FechaRealizacion).First();

                Console.WriteLine("Fecha y hora: " + fecha_realizacion);

                int mesHoy = int.Parse(DateTime.Now.Month.ToString());
                int anoHoy = int.Parse(DateTime.Now.Year.ToString());
                int mesRegistro = int.Parse(Convert.ToDateTime(fecha_realizacion).Month.ToString());
                int anoRegistro = int.Parse(Convert.ToDateTime(fecha_realizacion).Year.ToString());
                int diaRegistro = int.Parse(Convert.ToDateTime(fecha_realizacion).Day.ToString());

                diferenciaMes = mesRegistro - mesHoy;
                diferenciaAno = anoRegistro - anoHoy;

                if (diferenciaAno == 0)
                {

                    IWebElement webElement = driver.FindElement(By.Name("startDate"));
                    webElement.Click();

                    webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[3]")); //Seleccionar Mes Incremental

                    for (int i = 0; i < diferenciaMes; i++)
                    {
                        webElement.Click();
                    }
                }
                else if (diferenciaAno > 0)
                {
                    IWebElement webElement = driver.FindElement(By.Name("startDate"));
                    webElement.Click();

                    webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[3]")); //Seleccionar Mes Incremental

                    for (int i = 0; i < (diferenciaAno * 12) - mesHoy; i++)
                    {
                        webElement.Click();
                    }

                    webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[3]")); //Seleccionar Mes Incremental

                    for (int i = 0; i < mesRegistro; i++)
                    {
                        webElement.Click();
                    }

                }

                //webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[1]")); //Seleccionar Mes Decremental 
                ////List<IWebElement> list = driver.FindElements(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody")).ToList();
                return diaRegistro;
            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<SeleccionarAnoMes_Error>>");
                return 1;
            }
        }

        public int SeleccionarDia(IWebDriver driver, int idregistro, int dia)
        {
            try
            {
                int diaCalendario, j;


                if (dia < 8)
                {

                    for (int i = 1; i < 8; i++)
                    {
                        var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[1]/td[" + i + "]"));
                        diaCalendario = int.Parse(numDia.Text);
                        Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

                        if ((diaCalendario == dia))
                        {
                            numDia.Click();
                            Console.WriteLine("Encontrado");
                            return 0;
                        }

                    }

                    j = 2;
                    do
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[" + j + "]/td[" + i + "]"));
                            diaCalendario = int.Parse(numDia.Text);
                            Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

                            if ((diaCalendario == dia))
                            {
                                numDia.Click();
                                Console.WriteLine("Encontrado");
                                return 0;
                            }

                        }

                        j = j + 1;

                    } while (j != 4);

                }

                else if (dia > 1 && dia < 22)
                {
                    j = 2;
                    do
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[" + j + "]/td[" + i + "]"));
                            diaCalendario = int.Parse(numDia.Text);
                            Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

                            if ((diaCalendario == dia))
                            {
                                numDia.Click();
                                Console.WriteLine("Encontrado");
                                return 0;
                            }

                        }

                        j = j + 1;

                    } while (j != 4);

                }

                if (dia > 15 && dia < 32)
                {
                    j = 4;
                    do
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[" + j + "]/td[" + i + "]"));
                            diaCalendario = int.Parse(numDia.Text);
                            Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

                            if ((diaCalendario == dia))
                            {
                                numDia.Click();
                                Console.WriteLine("Encontrado");
                                return 0;
                            }

                        }

                        j = j + 1;

                    } while (j != 6);

                }

                if (dia > 29)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[6]/td[" + i + "]"));
                        diaCalendario = int.Parse(numDia.Text);
                        Console.WriteLine("DiaCalendario: " + diaCalendario + "dia: " + dia);

                        if ((diaCalendario == dia))
                        {
                            numDia.Click();
                            Console.WriteLine("Encontrado");
                            return 0;
                        }

                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                log.EscribaLog("ErrorUsuarios", "Se genera el siguiente error para el id de formulario " + idregistro + ". El error es " + ex.ToString() + "\n En <<SeleccionarDia_Error>>");
                return 1;
            }

        }

    }
}
