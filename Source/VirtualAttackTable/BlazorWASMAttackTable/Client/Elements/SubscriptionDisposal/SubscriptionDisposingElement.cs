using CallbackList;
using Microsoft.AspNetCore.Components;

namespace BlazorWASMAttackTable.Client.Elements.SubscriptionDisposal
{
    public class SubscriptionDisposingElement : ComponentBase, ISubscriptionDisposingElement
    {
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            (this as ISubscriptionDisposingElement).DropSubscriptions();

            await base.SetParametersAsync(parameters);
        }

        protected ISubscriptionHandle Subscribe<TDelegate>(ICallbackListManager<TDelegate> callbackListManager, TDelegate @delegate)
            where TDelegate : Delegate
        {
            return (this as ISubscriptionDisposingElement).Subscribe(callbackListManager, @delegate);
        }

        protected ISubscriptionHandle WeakSubscribe<TDelegate>(ICallbackListManager<TDelegate> callbackListManager, WeakSubscriptionStorage weakSubscriptionStorage, TDelegate @delegate)
            where TDelegate : Delegate
        {
            return (this as ISubscriptionDisposingElement).WeakSubscribe(callbackListManager, weakSubscriptionStorage, @delegate);
        }
    }
}
