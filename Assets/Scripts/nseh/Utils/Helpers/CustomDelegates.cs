namespace nseh.Utils.Helpers
{
    /// <summary>
    /// Custom delegate that receives a ref type as parameter
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="arg1"></param>
    public delegate void FuncRef<T1>(ref T1 arg1);

    /// <summary>
    /// Custom delegate that receives two different ref types as parameters
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    public delegate void FuncRef<T1, T2>(ref T1 arg1, ref T2 arg2);

    /// <summary>
    /// Custom delegate that receive three different ref types as parameters
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <param name="arg3"></param>
    public delegate void FuncRef<T1, T2, T3>(ref T1 arg1, ref T2 arg2, ref T3 arg3);


    /// <summary>
    /// Custom delegate that receive four different ref types as parameters
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <param name="arg3"></param>
    /// <param name="arg4"></param>
    public delegate void FuncRef<T1, T2, T3, T4>(ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4);
}
