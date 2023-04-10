namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Интерфейс IoC контейнера
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Регистрация зависимости
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса</typeparam>
        /// <typeparam name="TImplementation">Тип реализации</typeparam>
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;

        /// <summary>
        /// Разрешение зависимости
        /// </summary>
        /// <typeparam name="T">Тип зависимости</typeparam>
        /// <returns>Экземпляр зависимости</returns>
        T Resolve<T>();
    }
}