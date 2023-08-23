
namespace CallbackList
{
    internal interface IActionWrap<TAction>
    {
        public TAction StoredAction { get; }

        /// <summary>
        /// Creates a new wrap with consecutive calls of own and appended actions
        /// </summary>
        /// <param name="anotherAction"></param>
        /// <returns></returns>
        IActionWrap<TAction> Add(TAction anotherAction);
    }

    internal abstract class GenericActionWrap<TAction>: IActionWrap<TAction>
    {
        #region Properties
        TAction IActionWrap<TAction>.StoredAction => StoredAction;

        public TAction StoredAction { get; private init; }
        #endregion

        #region Constructors
        public GenericActionWrap(TAction action)
        {
            StoredAction = action;
        }
        #endregion

        #region Methods
        public abstract IActionWrap<TAction> Add(TAction anotherAction);
        #endregion
    }

    internal class ActionWrap : GenericActionWrap<Action>
    {
        public ActionWrap(Action action) : base(action)
        { }

        public override IActionWrap<Action> Add(Action anotherAction)
        {
            return new ActionWrap(() =>
            {
                StoredAction();
                anotherAction();
            });
        }
    }

    internal class ActionWrap<T1> : GenericActionWrap<Action<T1>>
    {
        public ActionWrap(Action<T1> action) : base(action)
        { }

        public override IActionWrap<Action<T1>> Add(Action<T1> anotherAction)
        {
            return new ActionWrap<T1>((arg1) =>
            {
                StoredAction(arg1);
                anotherAction(arg1);
            });
        }
    }

    internal class ActionWrap<T1, T2> : GenericActionWrap<Action<T1, T2>>
    {
        public ActionWrap(Action<T1, T2> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2>> Add(Action<T1, T2> anotherAction)
        {
            return new ActionWrap<T1, T2>((arg1, arg2) =>
            {
                StoredAction(arg1, arg2);
                anotherAction(arg1, arg2);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3> : GenericActionWrap<Action<T1, T2, T3>>
    {
        public ActionWrap(Action<T1, T2, T3> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3>> Add(Action<T1, T2, T3> anotherAction)
        {
            return new ActionWrap<T1, T2, T3>((arg1, arg2, arg3) =>
            {
                StoredAction(arg1, arg2, arg3);
                anotherAction(arg1, arg2, arg3);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4> : GenericActionWrap<Action<T1, T2, T3, T4>>
    {
        public ActionWrap(Action<T1, T2, T3, T4> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4>> Add(Action<T1, T2, T3, T4> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4>((arg1, arg2, arg3, arg4) =>
            {
                StoredAction(arg1, arg2, arg3, arg4);
                anotherAction(arg1, arg2, arg3, arg4);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5> : GenericActionWrap<Action<T1, T2, T3, T4, T5>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5>> Add(Action<T1, T2, T3, T4, T5> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5>((arg1, arg2, arg3, arg4, arg5) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5);
                anotherAction(arg1, arg2, arg3, arg4, arg5);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6>> Add(Action<T1, T2, T3, T4, T5, T6> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6>((arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7>> Add(Action<T1, T2, T3, T4, T5, T6, T7> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
            });
        }
    }

    internal class ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : GenericActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
    {
        public ActionWrap(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action) : base(action)
        { }

        public override IActionWrap<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> Add(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> anotherAction)
        {
            return new ActionWrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            {
                StoredAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
                anotherAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
            });
        }
    }
}
