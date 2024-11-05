using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace ClienteGloomApp
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void CambiarIdioma(string idioma)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(idioma);

            foreach (Window window in Current.Windows)
            {
                window.Language = System.Windows.Markup.XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentUICulture.IetfLanguageTag);
            }
        }

        


    }
}
