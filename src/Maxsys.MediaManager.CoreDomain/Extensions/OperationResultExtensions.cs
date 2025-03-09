using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxsys.Core;

// TODO: Mover para Core (atual: v14.1)
public static class OperationResultExtensions
{
    public static void AddException(this OperationResult result, Exception exception, ResultTypes resultType = ResultTypes.Error)
    {
        result.AddNotification(new Notification(exception, resultType));
    }

    public static void AddException(this OperationResult result, Exception exception, string message, ResultTypes resultType = ResultTypes.Error)
    {
        result.AddNotification(new Notification(exception, message, resultType));
    }
}
