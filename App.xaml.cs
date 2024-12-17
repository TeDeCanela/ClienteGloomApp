using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ClienteGloomApp.Properties;


namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {

        public void cambiarIdioma(string idioma)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(idioma);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(idioma);

                foreach (Window window in Current.Windows)
                {
                    window.Language = System.Windows.Markup.XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentUICulture.IetfLanguageTag);

                    var resources = window.Resources.MergedDictionaries;
                    if (resources.Count > 0)
                    {
                        foreach (var dictionary in resources)
                        {
                            dictionary.Source = dictionary.Source;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el idioma: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




    }
}