using System;
using System.IO;
using Newtonsoft.Json;
using NLog;
using OracleWebCenterExporter.Infrastructure;
using OracleWebCenterExporter.Model;

namespace OracleWebCenterExporter.ConsoleApp
{
    public class Program
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        
        public static void Main(string[] args)
        {
            try
            {
                _logger.Info("Start - Processing");
                Run();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
            }
            finally
            {
                _logger.Info("End - Processing");
            }
        }

        private static void Run()
        {
            var source = new OracleWebCenterSourceEndpoint(new WebCenterConfiguration());

            var sourceModel = source.Execute();
            
            File.WriteAllText($@"d:\temp\oracle-export-{DateTime.Now.Ticks}.json", JsonConvert.SerializeObject(sourceModel, CustomJsonSerializerSettings.Instance.Settings));
        }
    }
}
