using Microsoft.CodeAnalysis.Sarif;
using MilkyWare.Sarif.Converter.Enums;

namespace MilkyWare.Sarif.Converter.Converters
{
    public interface ISarifConverter
    {
        public OutputType OutputType { get; }

        Task<string> ConvertAsync(SarifLog sarif);
    }
}
