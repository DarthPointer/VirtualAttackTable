using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallbackList
{
    public interface ISubscribableCallbackListManager<in TAction>
        where TAction : Delegate
    {
        ISubscriptionHandle Subscribe(TAction @delegate);

        ISubscriptionHandle WeakSubscribe(WeakSubscriptionStorage storage, TAction @delegate);
    }

    public interface ICallbackListManager<TAction> : ISubscribableCallbackListManager<TAction>
        where TAction : Delegate
    {
        /// <summary>
        /// Gets an invokable for firing the current state of the callback manager. The delegate returned will get outdated whenever the manager gets changed.
        /// The delegate returned should not be stored to avoid holding hard references to weak subscribers.
        /// </summary>
        /// <returns></returns>
        TAction? CreateFireCall();
    }

    public abstract class AbstractCallbackListManager<TAction> : ICallbackListManager<TAction>
        where TAction : Delegate
    {
        #region Properties
        private List<ISubscriptionHandle<TAction>> Callbacks
        {
            get;
            init;
        } = new();
        #endregion

        #region Methods
        public TAction? CreateFireCall()
        {
            Callbacks.RemoveAll(callback => callback.AssignedAction == null);

            if (Callbacks.Count == 0)
            {
                return null;
            }

            IActionWrap<TAction> firstActionWrap = CreateWrap(Callbacks.First().AssignedAction!);
            return Callbacks.Skip(1).Aggregate(firstActionWrap, (wrap, handle) => wrap.Add(handle.AssignedAction!)).StoredAction;
        }

        public ISubscriptionHandle Subscribe(TAction @delegate)
        {
            HardReferenceSubscriptionHandle<TAction> handle = new(this, @delegate);
            Callbacks.Add(handle);

            return handle;
        }

        public ISubscriptionHandle WeakSubscribe(WeakSubscriptionStorage storage, TAction @delegate)
        {
            WeakReferenceSubscriptionHandle<TAction> handle = new(this, storage, @delegate);
            Callbacks.Add(handle);

            return handle;
        }

        internal void Unsubscribe(ISubscriptionHandle<TAction> handle)
        {
            Callbacks.Remove(handle);
        }

        internal abstract IActionWrap<TAction> CreateWrap(TAction action);
        #endregion
    }

    public class CallbackListManager : AbstractCallbackListManager<Action>
    {
        internal override IActionWrap<Action> CreateWrap(Action action)
        {
            return new ActionWrap(action);
        }
    }

    public class CallbackListManager<T1> : AbstractCallbackListManager<Action<T1>>
    {
        internal override IActionWrap<Action<T1>> CreateWrap(Action<T1> action)
        {
            return new ActionWrap<T1>(action);
        }
    }

    public class CallbackListManager<T1, T2> : AbstractCallbackListManager<Action<T1, T2>>
    {
        internal override IActionWrap<Action<T1, T2>> CreateWrap(Action<T1, T2> action)
        {
            return new ActionWrap<T1, T2>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3> : AbstractCallbackListManager<Action<T1, T2, T3>>
    {
        internal override IActionWrap<Action<T1, T2, T3>> CreateWrap(Action<T1, T2, T3> action)
        {
            return new ActionWrap<T1, T2, T3>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4> : AbstractCallbackListManager<Action<T1, T2, T3, T4>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4>> CreateWrap(Action<T1, T2, T3, T4> action)
        {
            return new ActionWrap<T1, T2, T3, T4>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5>> CreateWrap(Action<T1, T2, T3, T4, T5> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6>> CreateWrap(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(action);
        }
    }

    public class CallbackListManager<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : AbstractCallbackListManager<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
    {
        internal override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> CreateWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(action);
        }
    }
}
