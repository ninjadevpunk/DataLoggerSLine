using Core.Models.App_Models;
using ReasonableRTF;
using ReasonableRTF.Enums;
using ReasonableRTF.Models;

namespace Data.Utilities
{
    internal class RtfConverter
    {
        public static (string Text, DataError? Error) ToPlainText(string rtfContent)
        {
            if (string.IsNullOrWhiteSpace(rtfContent))
                return (string.Empty, null);

            try
            {
                RtfResult result = ConvertRtf.ToText(rtfContent);

                if (result.Error == RtfError.OK)
                    return (result.Text, null);

                return (
                    string.Empty,
                    new DataError
                    {
                        PublicMessage = "The text could not be read.",
                        TechnicalMessage = result.Exception?.Message ?? result.Error.ToString(),
                        MethodName = nameof(ToPlainText),
                        ExceptionType = result.Exception?.GetType().Name ?? nameof(RtfError)
                    });
            }
            catch (Exception ex)
            {
                return (
                    string.Empty,
                    new DataError
                    {
                        PublicMessage = "The text could not be read.",
                        TechnicalMessage = ex.Message,
                        MethodName = nameof(ToPlainText),
                        ExceptionType = ex.GetType().Name
                    });
            }
        }
    }
}
