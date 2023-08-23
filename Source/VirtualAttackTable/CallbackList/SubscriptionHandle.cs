using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallbackList
{
    public interface ISubscriptionHandle
    {
        void Unsubscribe();

        internal void OnAddedToContainer(SubscriptionContainer container);

        internal void OnRemovedFromContainer(SubscriptionContainer container);
    }

    internal interface ISubscriptionHandle<TAction> : ISubscriptionHandle
        where TAction : Delegate
    {
        TAction? AssignedAction { get; }
    }

    internal abstract class AbstractSubscriptionHandle<TAction> : ISubscriptionHandle<TAction>
        where TAction : Delegate
    {
        private List<WeakReference<SubscriptionContainer>> TrackingContainers
        {
            get;
        } = new();

        private WeakReference<AbstractCallbackListManager<TAction>>? OwningManager
        {
            get;
            set;
        }

        TAction? ISubscriptionHandle<TAction>.AssignedAction => AssignedAction;

        protected abstract TAction? AssignedAction { get; }

        protected AbstractSubscriptionHandle(AbstractCallbackListManager<TAction> owningManager)
        {
            OwningManager = new(owningManager);
        }

        public virtual void Unsubscribe()
        {
            if (OwningManager?.TryGetTarget(out var manager) == true)
            {
                manager?.Unsubscribe(this);
            }

            OwningManager = null;

            TrackingContainers.RemoveAll(x => x.TryGetTarget(out var _) == false);

            foreach (WeakReference<SubscriptionContainer> containerRef in TrackingContainers.ToList())
            {
                containerRef.TryGetTarget(out SubscriptionContainer? container);
                container!.RemoveHandle(this);
            }
        }

        void ISubscriptionHandle.OnAddedToContainer(SubscriptionContainer container)
        {
            TrackingContainers.Add(new(container));
        }

        void ISubscriptionHandle.OnRemovedFromContainer(SubscriptionContainer container)
        {
            TrackingContainers.RemoveAll(weakRef =>
            {
                if (weakRef.TryGetTarget(out SubscriptionContainer? checkContainer))
                    return container == checkContainer;

                return false;
            });
        }
    }

    internal class HardReferenceSubscriptionHandle<TAction> : AbstractSubscriptionHandle<TAction>
        where TAction : Delegate
    {
        private TAction? ActionReference
        {
            get;
            set;
        }

        protected override TAction? AssignedAction => ActionReference;

        internal HardReferenceSubscriptionHandle(AbstractCallbackListManager<TAction> owningManager, TAction actionReference) :
            base(owningManager)
        {
            ActionReference = actionReference;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            ActionReference = null;
        }
    }

    internal class WeakReferenceSubscriptionHandle<TAction> : AbstractSubscriptionHandle<TAction>
        where TAction : Delegate
    {
        private WeakReference<TAction>? WeakAcionReference
        {
            get;
            set;
        }

        private WeakReference<WeakSubscriptionStorage>? OwningStorage
        {
            get;
            set;
        }

        protected override TAction? AssignedAction
        {
            get
            {
                if (WeakAcionReference == null) return null;

                if (WeakAcionReference.TryGetTarget(out TAction? reference))
                    return reference;

                return null;
            }
        }

        internal WeakReferenceSubscriptionHandle(AbstractCallbackListManager<TAction> owningManager, WeakSubscriptionStorage owningSubscriptionStorage, TAction actionReference) :
            base(owningManager)
        {
            OwningStorage = new(owningSubscriptionStorage);
            WeakAcionReference = new(actionReference);

            owningSubscriptionStorage.StoreSubscription(this, actionReference);
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            if (OwningStorage?.TryGetTarget(out var storage) == true)
            {
                storage.RemoveSubscription(this);
                OwningStorage = null;
            }

            WeakAcionReference = null;
        }
    }
}
