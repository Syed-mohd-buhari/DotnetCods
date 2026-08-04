using EntityFramework.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleModels.Extensions
{
    public static class OracleFunctions
    {
        [Function(FunctionType.BuiltInFunction, "TO_NCHAR")]
        public static string ToNChar(this string value) => Function.CallNotSupported<string>();
    }
}
