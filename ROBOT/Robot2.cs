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
    //    DateTime HoyAhora = DateTime.Now; // Extrae la facha y hora actual
    //    agendaEntities tablaFormulario = new agendaEntities();
    //    LogErrores log = new LogErrores();
    //    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(ConfigurationManager.AppSettings["rutadriver"]);


    //    public void ServicioAgenda(int idregistro_, string usuario_, string clave_)
    //    {
    //        service.FirefoxBinaryPath = ConfigurationManager.AppSettings["rutanavegador"];
    //        IWebDriver driver_ = new FirefoxDriver(service);

    //        try
    //        {
    //            IniciarNavegador(driver_, usuario_, clave_);
    //            SeleccionarCalendario(driver_);
    //            ProgramarReunion(driver_);
    //            LlenarCampos(driver_, idregistro_);
    //            SeleccionarDia(driver_, SeleccionarAnoMes(driver_, idregistro_));
    //            AdicionarCorreo(driver_, idregistro_);
    //            EscribirMensaje(driver_, idregistro_);
    //            Cerrar(driver_);


    //        }
    //        catch (Exception ex)
    //        {
    //            Cerrar(driver_);
    //            log.EscribaLog("ServicioAgenda_Error", "El error es : " + ex.ToString());

    //        }


    //    }


    //    private void IniciarNavegador(IWebDriver driver, string usuario, string clave)
    //    {

    //        try
    //        {

    //            driver.Navigate().GoToUrl("http://my.rp1cloud.com/");
    //            //driver.FindElement(By.Name("accountLogin")).SendKeys(usuario);
    //            //driver.FindElement(By.Name("accountPassword")).SendKeys(clave);
    //            driver.FindElement(By.Name("accountLogin")).SendKeys("david.mendez@axede.com.co");
    //            driver.FindElement(By.Name("accountPassword")).SendKeys("123456");
    //            driver.FindElement(By.ClassName("submit")).Click();
    //            driver.Manage().Window.Maximize(); //Minimixar ventana del navegador
    //        }
    //        catch (Exception ex)
    //        {
    //            log.EscribaLog("IniciarNavegador_Error", "El error es : " + ex.ToString());

    //        }


    //    }

    //    public void SeleccionarCalendario(IWebDriver driver)
    //    {

    //        try
    //        {
    //            //Seleccionar calendario
    //            driver.Navigate().GoToUrl("https://my.rp1cloud.com/meetings/schedule");
    //            //Seleccionar calendario
    //        }
    //        catch (Exception e)
    //        {
    //            log.EscribaLog("SeleccionarCalendario_Error", " El error es : " + e.ToString());
    //        }
    //    }


    //    public void ProgramarReunion(IWebDriver driver)
    //    {
    //        try
    //        {
    //            //Programar reunion
    //            driver.FindElement(By.XPath("//*[@id='meeting-controls']/div/div[2]/div/div/button")).Click();
    //            //Programar reunion
    //        }
    //        catch (Exception e)
    //        {
    //            log.EscribaLog("ProgramarReunion_Error", " El error es : " + e.ToString());
    //        }
    //    }


    //    public void LlenarCampos(IWebDriver driver, int idregistro)
    //    {
    //        try
    //        {
    //            //Seleccionar VMR
    //            Actions action = new Actions(driver);
    //            action.MoveByOffset(298, 657).Click().Build().Perform();
    //            //Fin Seleccionar VMR

    //            //Llenar campos
    //            var tipo_solicitud = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Codigo).First();
    //            var hora = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.HoraDeInicio).First();

    //            string horaInicio;
    //            string tipoDia = "am";

    //            string[] separarHora;
    //            separarHora = hora.Split(' ');

    //            var separar = separarHora;
    //            string[] separarNum = separar[0].Split(':');
    //            int horaMeridiano = int.Parse(separarNum[0]);

    //            if (int.Parse(separarNum[0]) >= 12)
    //            {
    //                tipoDia = "pm";
    //                horaMeridiano = int.Parse(separarNum[0]) - 12;
    //            }

    //            horaInicio = Convert.ToString(horaMeridiano) + ":" + separarNum[1] + tipoDia;

    //            driver.FindElement(By.Id("name")).SendKeys(tipo_solicitud);
    //            driver.FindElement(By.Name("startTime")).Clear();
    //            driver.FindElement(By.Name("startTime")).SendKeys(horaInicio);
    //            driver.FindElement(By.Name("endTime")).Clear();
    //            driver.FindElement(By.Name("endTime")).SendKeys("11:59pm");
    //            //Fin Llenar campos

    //            //Seleccionar zona
    //            var option = driver.FindElement(By.Name("timeZone"));
    //            var selectElement = new SelectElement(option);
    //            selectElement.SelectByText("America/Bogota");
    //            //Seleccionar zona

    //        }
    //        catch (Exception ex)
    //        {
    //            log.EscribaLog("LlenarCampos_Error", " El error es : " + ex.ToString());
    //        }
    //    }

    //    public void AdicionarCorreo(IWebDriver driver, int idregistro)
    //    {

    //        try
    //        {
    //            var correos = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.CorreoDeSolicitante).First();

    //            //Aqui va el checkbox

    //            string[] separarCorreos;
    //            separarCorreos = correos.Split('|');
    //            var contarCorreos = separarCorreos.Count();

    //            IWebElement webElement;

    //            for (int i = 0; i < separarCorreos.Count(); i++)
    //            {
    //                webElement = driver.FindElement(By.Name("email"));
    //                webElement.Click();
    //                webElement.SendKeys(separarCorreos[i].ToLower());

    //                webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[7]/div[2]/div/a"));
    //                webElement.Click();


    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            log.EscribaLog("AdicionarCorreo_Error", " El error es : " + ex.ToString());
    //        }


    //    }


    //    public void EscribirMensaje(IWebDriver driver, int idregistro)
    //    {
    //        try
    //        {
    //            var consecutivo = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Num_Consecutivo).First();
    //            var observaciones = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Observaciones).First();
    //            var nomSala = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Sala).First();

    //            var salnom = (from db in tablaFormulario.t_salas where db.CodigoSala == nomSala select db.NombreCompletoSalaVirtual).First();

    //            var procesado = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Procesado).First();
    //            var declarante = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.Declarante).First();

    //            driver.FindElement(By.Name("message")).SendKeys("Recuerde que el número del consecutivo de esta audiencia es: " + consecutivo + ". \nEl nombre de la sala es: " + salnom + ". " + observaciones + ". "); //Nombre sala y observaciones

    //            if (procesado != null && procesado != "")
    //            {
    //                driver.FindElement(By.Name("message")).SendKeys("\nProcesado: " + procesado); //Nombre sala, observaciones, procesado y declarante

    //            }

    //            if (declarante != null && procesado != "")
    //            {
    //                driver.FindElement(By.Name("message")).SendKeys("\nDeclarante: " + declarante); //Nombre sala, observaciones, procesado y declarante

    //            }

    //            //driver.FindElement(By.Id("addsend")).Click();
    //            driver.FindElement(By.Id("addcancel")).Click();  //Para pruebas

    //        }
    //        catch (Exception ex)
    //        {
    //            log.EscribaLog("EscribirMensaje_Error", " El error es : " + ex.ToString());
    //        }

    //    }


    //    public void Cerrar(IWebDriver driver)
    //    {

    //        try
    //        {
    //            driver.Close();
    //            driver.Quit();
    //        }
    //        catch (Exception ex)
    //        {
    //            log.EscribaLog("CerrarOcultar_Error", " El error es : " + ex.ToString());
    //        }

    //    }

    //    public int SeleccionarAnoMes(IWebDriver driver, int idregistro)
    //    {
    //        try
    //        {
    //            int diferenciaMes, diferenciaAno;

    //            //var idregistro = (from db in tablaFormulario.t_formulario select db.Id).Max();
    //            var fecha_realizacion = (from db in tablaFormulario.t_formulario where db.Id == idregistro select db.FechaRealizacion).First();

    //            Console.WriteLine("Fecha y hora: " + fecha_realizacion);

    //            int mesHoy = int.Parse(DateTime.Now.Month.ToString());
    //            int anoHoy = int.Parse(DateTime.Now.Year.ToString());
    //            int mesRegistro = int.Parse(Convert.ToDateTime(fecha_realizacion).Month.ToString());
    //            int anoRegistro = int.Parse(Convert.ToDateTime(fecha_realizacion).Year.ToString());
    //            int diaRegistro = int.Parse(Convert.ToDateTime(fecha_realizacion).Day.ToString());

    //            diferenciaMes = mesRegistro - mesHoy;
    //            diferenciaAno = anoRegistro - anoHoy;

    //            if (diferenciaAno == 0)
    //            {

    //                IWebElement webElement = driver.FindElement(By.Name("startDate"));
    //                webElement.Click();

    //                webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[3]")); //Seleccionar Mes Incremental

    //                for (int i = 0; i < diferenciaMes; i++)
    //                {
    //                    webElement.Click();
    //                }
    //            }
    //            else if (diferenciaAno > 0)
    //            {
    //                IWebElement webElement = driver.FindElement(By.Name("startDate"));
    //                webElement.Click();

    //                webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[3]")); //Seleccionar Mes Incremental

    //                for (int i = 0; i < (diferenciaAno * 12) - mesHoy; i++)
    //                {
    //                    webElement.Click();
    //                }

    //                webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[3]")); //Seleccionar Mes Incremental

    //                for (int i = 0; i < mesRegistro; i++)
    //                {
    //                    webElement.Click();
    //                }

    //            }

    //            //webElement = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/table/thead/tr/th[1]")); //Seleccionar Mes Decremental 
    //            ////List<IWebElement> list = driver.FindElements(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody")).ToList();
    //            return diaRegistro;
    //        }
    //        catch (Exception ex)
    //        {
    //            log.EscribaLog("SeleccionarAnoMes_Error", " El error es : " + ex.ToString());
    //            return 0;
    //        }
    //    }

    //    public void SeleccionarDia(IWebDriver driver, int dia)
    //    {
    //        try
    //        {
    //            int diaCalendario, j;


    //            if (dia < 8)
    //            {

    //                for (int i = 1; i < 8; i++)
    //                {
    //                    var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[1]/td[" + i + "]"));
    //                    diaCalendario = int.Parse(numDia.Text);
    //                    Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

    //                    if ((diaCalendario == dia))
    //                    {
    //                        numDia.Click();
    //                        Console.WriteLine("Encontrado");
    //                        return;
    //                    }

    //                }

    //                j = 2;
    //                do
    //                {
    //                    for (int i = 1; i < 8; i++)
    //                    {
    //                        var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[" + j + "]/td[" + i + "]"));
    //                        diaCalendario = int.Parse(numDia.Text);
    //                        Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

    //                        if ((diaCalendario == dia))
    //                        {
    //                            numDia.Click();
    //                            Console.WriteLine("Encontrado");
    //                            return;
    //                        }

    //                    }

    //                    j = j + 1;

    //                } while (j != 4);

    //            }

    //            else if (dia > 1 && dia < 22)
    //            {
    //                j = 2;
    //                do
    //                {
    //                    for (int i = 1; i < 8; i++)
    //                    {
    //                        var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[" + j + "]/td[" + i + "]"));
    //                        diaCalendario = int.Parse(numDia.Text);
    //                        Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

    //                        if ((diaCalendario == dia))
    //                        {
    //                            numDia.Click();
    //                            Console.WriteLine("Encontrado");
    //                            return;
    //                        }

    //                    }

    //                    j = j + 1;

    //                } while (j != 4);

    //            }

    //            if (dia > 15 && dia < 32)
    //            {
    //                j = 4;
    //                do
    //                {
    //                    for (int i = 1; i < 8; i++)
    //                    {
    //                        var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[" + j + "]/td[" + i + "]"));
    //                        diaCalendario = int.Parse(numDia.Text);
    //                        Console.WriteLine("DiaCalendario: " + diaCalendario + " dia: " + dia);

    //                        if ((diaCalendario == dia))
    //                        {
    //                            numDia.Click();
    //                            Console.WriteLine("Encontrado");
    //                            return;
    //                        }

    //                    }

    //                    j = j + 1;

    //                } while (j != 6);

    //            }

    //            if (dia > 29)
    //            {
    //                for (int i = 1; i < 8; i++)
    //                {
    //                    var numDia = driver.FindElement(By.XPath("//*[@id='addMeetingForm']/div[2]/div[1]/div/div/div/div/table/tbody/tr[6]/td[" + i + "]"));
    //                    diaCalendario = int.Parse(numDia.Text);
    //                    Console.WriteLine("DiaCalendario: " + diaCalendario + "dia: " + dia);

    //                    if ((diaCalendario == dia))
    //                    {
    //                        numDia.Click();
    //                        Console.WriteLine("Encontrado");
    //                        return;
    //                    }

    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            log.EscribaLog("SeleccionarDia_Error", " El error es : " + ex.ToString());
    //        }

    //    }

    }
}
