using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using CapaAccesoDatos;


namespace CapaLogicaNegocios
{
    //Recuerda que una aplicacion que ejecuta este codigo debe correr con PERMISOS DE ADMINISTRADOR
    //Esta clase obtiene informacion del App.config de la capa de presentacion
    //Despues de encriptar, desencriptar ó cambiar algun valor dentro del App.config  usando codigo
    //, NO llames a ConfigurationManager.RefreshSection()
    //porque genera una exception, esto se debe a que ConfigurationManager no se sincroniza fisicamente 
    //con el App.config.  Una solución es primero hacer los cambios, luego terminar la app y
    //volver a correr la aplicacion para que ConfigurationManager vuelva a cargarse.
    public class ClsEnlaceToAppConfig
    {
        //-----------------Constructor
        public ClsEnlaceToAppConfig()
        {

        }//parameterless constructor


        //-------------------Methods
        public string ObtenerCadenaConexionAppConfig()
        {
            EncryptAppSettings("appSettings");

            SqlConnectionStringBuilder str = new SqlConnectionStringBuilder();
            str["Data Source"] = ConfigurationManager.AppSettings["servidor"];
            str["Initial Catalog"] = ConfigurationManager.AppSettings["nombreBd"]; 
            str["User Id"] = ConfigurationManager.AppSettings["username"];
            str["password"] = ConfigurationManager.AppSettings["password"];

            return (str.ConnectionString);
        }

        public void ModificarContenidoAppConfig(string cadena)
        {
            SqlConnectionStringBuilder str = new SqlConnectionStringBuilder(cadena);

            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = GetAppPath() + "CapaPresentacion.exe" + ".config";  

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            config.AppSettings.Settings["password"].Value = str.Password;
            config.AppSettings.Settings["username"].Value = str.UserID;
            config.AppSettings.Settings["servidor"].Value = str.DataSource;
            config.AppSettings.Settings["nombreBd"].Value = str.InitialCatalog;

            config.Save();
        }


        public string ObtenerCadenaConexionDeCapaAccesoDatos()
        {
            ClsManejador cls = new ClsManejador();
            
            return (cls.ObtenerCadenaConexionDeCapaAccesoDatos());
        }


        
        private static string GetAppPath()
        {
            System.Reflection.Module[] modules = System.Reflection.Assembly.GetExecutingAssembly().GetModules();
            string location = System.IO.Path.GetDirectoryName(modules[0].FullyQualifiedName);
            if ((location != "") && (location[location.Length - 1] != '\\'))
                location += '\\';
            return location;
        }


        /// <summary>
        /// Encripta el App.Config ubicado en la capa de presentacion,en caso de no estarlo
        /// </summary>
        /// <param name="section">Es el nombre de la seccion a encriptar en el App.Config</param>
        private void EncryptAppSettings(string section)
        {
            Configuration objConfig = ConfigurationManager.OpenExeConfiguration(GetAppPath() + "CapaPresentacion.exe");
            //System.Configuration.Configuration objConfig =
            //ConfigurationManager.OpenExeConfiguration(
            //ConfigurationUserLevel.None);

            //AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection(section);  //funciona con COnfigurationUserLevel.None    y con GetAppPath() + "WindowsFormsApp10.exe"
            //ConfigurationSection objAppsettings = objConfig.GetSection(section);   //funciona con COnfigurationUserLevel.None 
            ConfigurationSection objAppsettings = objConfig.AppSettings;   //funciona con COnfigurationUserLevel.None  y con GetAppPath() + "WindowsFormsApp10.exe"

            if (!objAppsettings.SectionInformation.IsProtected)
            {
                objAppsettings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                objAppsettings.SectionInformation.ForceSave = true;

                objConfig.Save(ConfigurationSaveMode.Modified);  //original
            }
        }

        public void QuitarEncriptacion()
        {
            Configuration objConfig = ConfigurationManager.OpenExeConfiguration(GetAppPath() + "CapaPresentacion.exe");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
            if (objAppsettings.SectionInformation.IsProtected)
            {
                objAppsettings.SectionInformation.UnprotectSection();
                objAppsettings.SectionInformation.ForceSave = true;
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

    }

}
