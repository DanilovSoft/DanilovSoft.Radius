namespace System.Diagnostics
{
    internal static class DebugOnly
    {
        [Conditional("DEBUG")]
        [DebuggerNonUserCode]
        public static void Break()
        {
            if(Debugger.IsAttached)
                Debugger.Break();
        }
    }
}
