namespace BlazorWASMAttackTable.Client.AsyncLoading
{
    public class AsyncLoadedItem<T>
    {
        #region Fields
        private T _value = default!;
        #endregion

        #region Properties
        public bool IsLoaded { get; private set; } = false;

        public Task<T> ItemTask { get; }
        #endregion

        #region Constructors
        public AsyncLoadedItem()
        {
            ItemTask = new(() => _value!);
        }
        #endregion

        #region Methods
        public void SetValue(T value)
        {
            if (IsLoaded)
                throw new InvalidOperationException($"{typeof(AsyncLoadedItem<T>)} already has its value loaded.");

            IsLoaded = true;
            _value = value;
            ItemTask.RunSynchronously();
        }
        #endregion
    }
}
