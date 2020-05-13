namespace MyNotes
{
    public interface IAndroidVersion
    {
        /// <summary>
        /// Метод оценки версии операционной системы
        /// </summary>
        /// <returns>Индикатор проверки</returns>
        bool IsHigherOreo();
    }
}
