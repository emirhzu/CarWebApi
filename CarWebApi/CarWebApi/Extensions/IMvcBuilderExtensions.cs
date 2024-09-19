using CarWebApi.Utilities.Formatters;

namespace CarWebApi.Extensions
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config =>
                config.OutputFormatters
                    .Add(new CsvOutPutFormatter()));
    }
}
