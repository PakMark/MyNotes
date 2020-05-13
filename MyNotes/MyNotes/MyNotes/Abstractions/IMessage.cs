namespace MyNotes
{
    public interface IMessage
    {
        /// <summary>
        /// Вызов длительного Toast уведомления
        /// </summary>
        /// <param name="message">Сообщение</param>
        void LongAlert(string message);

        /// <summary>
        /// Вызов быстрого Toast уведомления
        /// </summary>
        /// <param name="message">Сообщение</param>
        void ShortAlert(string message);
    }
}
