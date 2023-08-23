using CallbackList;
using System.Collections;

namespace BlazorWASMAttackTable.Client.Interactions.Common
{
    public class ListInteraction<T> : IList<T>
    {
        #region Backend
        #region Fields
        private List<T> _list;
        #endregion

        #region Properties
        private List<T> List
        {
            get
            {
                return _list;
            }
            set
            {
                if (value != _list)
                {
                    _list = value;
                    NotifyListChanged();
                }
            }
        }

        public CallbackListManager ListChanged
        {
            get;
        } = new();
        //public event Action? ListChanged;
        #endregion

        #region Constructors
        public ListInteraction()
        {
            _list = new List<T>();
        }

        public ListInteraction(IEnumerable<T> initialElements)
        {
            _list = initialElements.ToList();
        }
        #endregion

        #region Methods
        private void NotifyListChanged()
        {
            ListChanged.CreateFireCall()?.Invoke();
        }

        /// <summary>
        /// Completely replaces the internal list with a new one with elements specified.
        /// </summary>
        /// <param name="newElements"></param>
        public void Overwrite(IEnumerable<T> newElements)
        {
            List = newElements.ToList();
        }
        #endregion
        #endregion

        #region IList Implementation
        public T this[int index]
        {
            get
            {
                return List[index];
            }
            set
            {
                List[index] = value;
                NotifyListChanged();
            }
        }

        public int Count => List.Count;

        /// <summary>
        /// Should always be <see langword="false"/> because it always is backed by <see cref="List{T}"./>
        /// </summary>
        bool ICollection<T>.IsReadOnly => (List as ICollection<T>).IsReadOnly;

        public void Add(T item)
        {
            List.Add(item);
            NotifyListChanged();
        }

        public void Clear()
        {
            List.Clear();
            NotifyListChanged();
        }

        public bool Contains(T item)
        {
            return List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            List.Insert(index, item);
            NotifyListChanged();
        }

        public bool Remove(T item)
        {
            if (List.Remove(item))
            {
                NotifyListChanged();
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
            NotifyListChanged();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
