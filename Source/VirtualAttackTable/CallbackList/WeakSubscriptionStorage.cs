using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallbackList
{
    public class WeakSubscriptionStorage
    {
        #region Properties
        private Dictionary<ISubscriptionHandle, Delegate> StoredDelegates
        {
            get;
            init;
        } = new();
        #endregion

        #region Methods
        internal void StoreSubscription(ISubscriptionHandle subscription, Delegate @delegate)
        {
            StoredDelegates.Add(subscription, @delegate);
        }

        internal void RemoveSubscription(ISubscriptionHandle subscription)
        {
            StoredDelegates.Remove(subscription);
        }
        #endregion
    }
}
