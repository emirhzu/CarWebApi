using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace CarWebApi.Utilities.Formatters
{
    public class CsvOutPutFormatter : TextOutputFormatter
    {
        public CsvOutPutFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(CarDto).IsAssignableFrom(type) || typeof(IEnumerable<CarDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }           
        private static void FormatCsv(StringBuilder buffer, CarDto car)
        {
            buffer.AppendLine($"{car.id}, {car.Brand}, {car.Model}, {car.Years}, {car.Price}");
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if(context.Object is IEnumerable<CarDto>)
            {
                foreach(var car in (IEnumerable<CarDto>)context.Object)
                {
                    FormatCsv(buffer, car);
                }
            }
            else
            {
                FormatCsv(buffer, (CarDto)context.Object);
            }
            await response.WriteAsync(buffer.ToString());
        }
    }
}
