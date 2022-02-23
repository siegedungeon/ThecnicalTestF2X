using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using System.Text;

namespace ApiBackendF2X.Utility
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(List<ReporteMesDTO> data)
        {
            var countEstaciones = data.FirstOrDefault().Estaciones.Count();

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Reporte!</h1></div>
                                <table align='center'>
                                    <tr>");

            for (int i = 0; i < countEstaciones; i++)
            {
                sb.Append($"<th>Estacion{i + 1}</th>");
            }

            sb.Append($"</tr>");

            foreach (var emp in data)
            {
                for (int i = 0; i < emp.Estaciones.Count; i++)
                {
                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                  </tr>", $"{emp.Estaciones[i].Cantidad} | {emp.Estaciones[i].Valor}");
                }
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
