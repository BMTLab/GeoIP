/* 
    This file was generated automatically, do not make changes to it manually
*/

using Localization.Attributes;


// ReSharper disable All
namespace Localization
{
    public interface ILocalization
    {
        public string Lang { get; }
        /// <summary> 
        /// <para> en: Error </para>
        /// <para> ru: Ошибка </para>
        /// </summary>
       public string Error { get; } 
       
        /// <summary> 
        /// <para> en: Internal error </para>
        /// <para> ru: Внутреняя ошибка </para>
        /// </summary>
       public string ErrorInternal { get; } 
       
        /// <summary> 
        /// <para> en: Lost connection </para>
        /// <para> ru: Потеряно соединение </para>
        /// </summary>
       public string ErrorLostConnection { get; } 
       
        /// <summary> 
        /// <para> en: IP is not correct </para>
        /// <para> ru: IP не корректный </para>
        /// </summary>
       public string ErrorIpIsNotCorrect { get; } 
       
        /// <summary> 
        /// <para> en: Location not found </para>
        /// <para> ru: Не найдено местоположение </para>
        /// </summary>
       public string ErrorNotFoundLocation { get; } 
       
        /// <summary> 
        /// <para> en: Hello </para>
        /// <para> ru: Привет </para>
        /// </summary>
       public string Hello { get; } 
       
    }
    
    [Language(@"en")]
    public sealed class LocalizationEn : ILocalization
    {
        public string Lang => string.Intern(@"en");

        public string Error { get; } = "Error";
        public string ErrorInternal { get; } = "Internal error";
        public string ErrorLostConnection { get; } = "Lost connection";
        public string ErrorIpIsNotCorrect { get; } = "IP is not correct";
        public string ErrorNotFoundLocation { get; } = "Location not found";
        public string Hello { get; } = "Hello";
    }
    
    [Language(@"ru")]
    public sealed class LocalizationRu : ILocalization
    {
        public string Lang => string.Intern(@"ru");

        public string Error { get; } = "Ошибка";
        public string ErrorInternal { get; } = "Внутреняя ошибка";
        public string ErrorLostConnection { get; } = "Потеряно соединение";
        public string ErrorIpIsNotCorrect { get; } = "IP не корректный";
        public string ErrorNotFoundLocation { get; } = "Не найдено местоположение";
        public string Hello { get; } = "Привет";
    }
}

