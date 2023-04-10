namespace SpargoTest.Services
{
    /// <summary>
    /// Transient IoC контейнер
    /// </summary>
    public class IoCContainer
    {
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();

        /// <summary>
        /// Регистрация зависимости
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса</typeparam>
        /// <typeparam name="TImplementation">Тип реализации</typeparam>
        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
            => _typeMap[typeof(TInterface)] = typeof(TImplementation);

        /// <summary>
        /// Разрешение зависимости
        /// </summary>
        /// <typeparam name="T">Тип зависимости</typeparam>
        /// <returns>Экземпляр зависимости</returns>
        public T Resolve<T>() => (T)Resolve(typeof(T));

        private object Resolve(Type type)
        {
            if (!_typeMap.ContainsKey(type))
                throw new InvalidOperationException($"Type {type.Name} is not registered.");

            var implementationType = _typeMap[type];

            var constructor = implementationType
                .GetConstructors()
                .First();

            var parameters = constructor
                .GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            return constructor.Invoke(parameters);
        }
    }
}
