using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alura.ListaLeitura.App.HTML
{
  public  class HtmlLUtios
    {
        public static string CarregandoArquivoHTML(string nomedoArquivo)
        {
            using (var arquivo = File.OpenText($"HTML/{nomedoArquivo}.html"))
            {
                return arquivo.ReadToEnd();
            }
        }
    }
}
