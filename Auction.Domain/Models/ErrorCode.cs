

using System.ComponentModel;
using System.Reflection;

namespace Auction.Application.Common.Models.Enum
{
    public enum ErrorCode
    {
        [Description("Пользователь уже существует")]
        UserAlreadyExist,

        [Description("Пользователь с этим email, уже зарегестрирован")]
        EmailIsBusy
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this ErrorCode value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
