using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallbackList
{
    /// <summary>
    /// A container to hold a set of subscriptions and cancel all of them in one call. Initially created to drop all subscriptions of an object when disposing of it.
    /// </summary>
    public class SubscriptionContainer
    {
        private List<ISubscriptionHandle> Handles { get; } = new();

        /// <summary>
        /// Track the <see cref="ISubscriptionHandle"/> with this <see cref="SubscriptionContainer"/>.
        /// </summary>
        /// <param name="handle">The handle to track.</param>
        public void AddHandle(ISubscriptionHandle handle)
        {
            if (!Handles.Contains(handle))
            {
                Handles.Add(handle);
                handle.OnAddedToContainer(this);
            }
        }

        /// <summary>
        /// Stop tracking the <see cref="ISubscriptionHandle"/> with this <see cref="SubscriptionContainer"/>.
        /// Subscriptions canceled manually or via other containers call this implicitly.
        /// </summary>
        /// <param name="handle">The handle to stop tracking.</param>
        public void RemoveHandle(ISubscriptionHandle handle)
        {
            Handles.Remove(handle);
            handle.OnRemovedFromContainer(this);
        }

        /// <summary>
        /// Calls <see cref="ISubscriptionHandle.Unsubscribe"/> for all tracked handles. All tracked handles are removed implicitly on calling this method.
        /// </summary>
        public void UnsubscribeAll()
        {
            List<ISubscriptionHandle> allHandles = Handles.ToList();

            foreach (ISubscriptionHandle handle in allHandles)
            {
                handle.Unsubscribe();
            }
        }
    }
}
