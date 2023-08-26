using CallbackList;
using System.Runtime.CompilerServices;

namespace BlazorWASMAttackTable.Client.Elements.SubscriptionDisposal
{
    public interface ISubscriptionDisposingElement : IDisposable
    {
        private static ConditionalWeakTable<ISubscriptionDisposingElement, SubscriptionContainer> Containers = new();

        public ISubscriptionHandle Subscribe<TDelegate>(ISubscribableCallbackListManager<TDelegate> callbackListManager, TDelegate @delegate)
            where TDelegate : Delegate
        {
            ISubscriptionHandle result = callbackListManager.Subscribe(@delegate);
            Containers.GetOrCreateValue(this).AddHandle(result);
            return result;
        }

        public ISubscriptionHandle WeakSubscribe<TDelegate>(ISubscribableCallbackListManager<TDelegate> callbackListManager, WeakSubscriptionStorage weakSubscriptionStorage, TDelegate @delegate)
            where TDelegate : Delegate
        {
            ISubscriptionHandle result = callbackListManager.WeakSubscribe(weakSubscriptionStorage, @delegate);
            Containers.GetOrCreateValue(this).AddHandle(result);
            return result;
        }

        public void DropSubscriptions()
        {
            Containers.GetOrCreateValue(this).UnsubscribeAll();
        }

        void IDisposable.Dispose()
        {
            DropSubscriptions();
        }
    }
}
